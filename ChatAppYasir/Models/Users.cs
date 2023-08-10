using System.ComponentModel.DataAnnotations;

namespace ChatAppYasir.Models
{
    public class Users
    {
        [Key]
        public int Id { get; set; }
        public  string Name { get; set; }
    }
}
   