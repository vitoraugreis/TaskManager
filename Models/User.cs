using System.ComponentModel.DataAnnotations;

namespace TaskManager.Models
{
    public class User
    {
        public int Id { get; private set; }

        [Required]
        [MaxLength(20)]
        public string Username { get; set; }

        public ICollection<UserTask> Tasks { get; set; } = new List<UserTask>();

        public User() { 
            Username = String.Empty;
        }

        public User(string username)
        {
            Username = username;
        }
    }
}