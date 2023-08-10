using System;
using System.IO;
using System.Xml.Linq;
using ChatAppYasir.Models;
using Newtonsoft.Json;

namespace ChatAppYasir.Services
{
    public class LogService
    {
        private readonly string _logFilePath = Directory.GetCurrentDirectory() + "/LogFiles";

        public void LogWrite(string name)
        {
            
            //Users user1 = new Users { Name = name };
            Users user = new Users();
            user.Name = name;
            var json = JsonConvert.SerializeObject(user);
            string fileName = "/logUser.txt";
            string logFilePath = _logFilePath + fileName;
            Console.Write(logFilePath);
            if (!File.Exists(logFilePath))
                throw new FileNotFoundException(fileName + " isn't found!");
            using (var writer = new StreamWriter(logFilePath, true))
            {
                writer.WriteLine(json);
            }
        }
        public void LogWrite(string sender, string message,DateTime time)
        {
            ChatMessage logData = new ChatMessage();
            logData.Sender = sender;
            logData.Message = message;
            logData.SentTime =DateTime.UtcNow;


            /* ChatMessage logData = new ChatMessage
             {
                 SentTime = time,
                 Sender = sender,
                  Message= message
             };*/


            var json = JsonConvert.SerializeObject(logData);
            string fileName = "/logPublic.txt";
            string logFilePath = _logFilePath + fileName;
            if (!File.Exists(logFilePath))
                throw new FileNotFoundException(fileName + " isn't found!");
            using (var writer = new StreamWriter(logFilePath, true))
            {
                writer.WriteLine(json);
            }
        }

        public void LogWrite(string sender, string receiver, string message,DateTime time)
        {

            string fileName = "/logPrivate_"+sender+receiver +".txt";
            if (!File.Exists(_logFilePath + fileName))
            {
                fileName = "/logPrivate_" + sender + receiver + ".txt";

            }

            ChatMessage logData = new ChatMessage
            {
                SentTime = DateTime.UtcNow,
                Sender = sender,
                Message = message
            };

            var json = JsonConvert.SerializeObject(logData);

            using (var writer = new StreamWriter(_logFilePath + fileName, true))
            {
                writer.WriteLine(json);
            }
        }
        public List<string> LogRead(string fileName)
        {

            string filePath = _logFilePath + "/" + fileName; 
            if(!File.Exists(filePath))
                throw new FileNotFoundException(fileName+" isn't found!");
            List<string> lines = new List<string>();

            using (StreamReader reader = new StreamReader(filePath))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                   
                    lines.Add(line);                    
                }
            }
            return lines;
        }
    }

}

