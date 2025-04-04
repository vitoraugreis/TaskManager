namespace TaskManager.Models
{
    public class UserTask
    {
        public int Id { get; private set; }
        public User User { get; private set; }
        public string? Description { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime? CompletionDate { get; set; }
        public bool IsComplete { get; set; }

        public UserTask(User user, string? description, DateTime? completionDate)
        {
            User = user;
            Description = description;
            CreationDate = DateTime.Now;
            CompletionDate = completionDate;
            IsComplete = false;
        }

        public void MarkAsComplete()
        {
            IsComplete = true;
        }
    }
}