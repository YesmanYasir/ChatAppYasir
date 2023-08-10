using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using ChatAppYasir.Models;
using Newtonsoft.Json;
using ChatAppYasir.Services;
using System.Xml.Linq;

namespace ChatAppYasir.Controllers
{
    public class ChatController : Controller
    {
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly string _logFilePath;
        private readonly LogService _logService;

        public ChatController(IWebHostEnvironment hostingEnvironment, LogService logService)
        {
            _hostingEnvironment = hostingEnvironment;
            _logService = logService;
            _logFilePath = Path.Combine(_hostingEnvironment.ContentRootPath, "LogFiles");
        }
        [HttpGet]
        public IActionResult PrivateChat(string nickname, string clickedUser)
        {
            if (string.IsNullOrEmpty(nickname )&& string.IsNullOrEmpty(clickedUser))
            {
                // Handle the case when the user parameter is empty or null
                // You can choose to redirect to an error page or handle it differently
                return RedirectToAction("Index");
            }

            // Perform any necessary logic for the private chat

            // Pass the user parameter to the view
            ViewBag.Nickname = nickname; // Use ViewBag.Nickname instead of ViewBag.User

            // Set the clickedUser value if you have it
            ViewBag.ClickedUser = clickedUser; // Replace with the actual value you want to pass

            return View();
        }


        private void LogWrite(string fileName, string data)
        {
            string filePath = Path.Combine(_logFilePath, fileName);

            if (!Directory.Exists(_logFilePath))
                Directory.CreateDirectory(_logFilePath);

            using (var writer = new StreamWriter(filePath, true))
            {
                writer.WriteLine(data);
            }
        }

      /*  private List<string> LogRead(string fileName)
        {
            return _logService.LogRead(fileName);

        }*/

        [HttpPost]
        public ActionResult CreateUser(string name)
        {
            try
            {
                _logService.LogWrite(name);

                
                //die return bevat geen name via get proberen te doen 
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, error = ex.Message });
            }
        }

        [HttpPost]
        public ActionResult PostMessage(string sender, string message, DateTime time)
        {
            try
            {
                _logService.LogWrite(sender, message, time);

                return Json(new { success = true});
            }
            catch (Exception ex)
            {
                return Json(new { success = false, error = ex.Message });
            }
        }
        [HttpPost]
        public ActionResult PostMessagePrivate(string sender, string message, DateTime time,string receiver)
        {
            try
            {
                _logService.LogWrite(sender,receiver, message, time) ;

                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, error = ex.Message });
            }
        }

        [HttpGet]
        public ActionResult<IEnumerable<ChatMessage>> GetMessagesPublic()
        {
            try
            {

                var lines = _logService.LogRead("logPublic.txt");
                var chatMessages = new List<ChatMessage>();

                foreach (var line in lines)
                {
                    var logData = JsonConvert.DeserializeObject<ChatMessage>(line);
                    chatMessages.Add(logData) ;
                }

                return Json(new { success = true,  chatMessages });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, error = ex.Message });
            }
        }
        [HttpGet]
        public IActionResult GetChatMessagesPrivate(string sender, string receiver)
        {
            try
            {

                string fileName = "logPrivate_" + sender + receiver + ".txt";
                var lines = _logService.LogRead(fileName);
                var chatMessages = new List<ChatMessage>();

                foreach (var line in lines)
                {
                    var logData = JsonConvert.DeserializeObject<ChatMessage>(line);
                    chatMessages.Add(logData);
                }

                return Json(new { success = true, chatMessages });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, error = ex.Message });
            }
        }

        public ActionResult GetUsers()
        {
            List<string> userNames = new List<string>();

            var lines = _logService.LogRead("logUser.txt");

            foreach (var line in lines)
            {
                var user = JsonConvert.DeserializeObject<Users>(line);
                userNames.Add(user.Name);
            }

            return Json(new { success = true, userNames });
        }
    }
}
