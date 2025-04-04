namespace TaskManager.Models
{
    public class User
    {
        public int Id { get; private set; }
        public string Name { get; set;}

        public User(string name)
        {
            Name = name;
        }
    }
}