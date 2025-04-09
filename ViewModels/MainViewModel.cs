using TaskManager.Models;
using TaskManager.Data;

namespace TaskManager.ViewModels
{
    public class MainViewModel
    {
        public void CreateUser(string name)
        {
            using var context = new AppDbContext();
            var userExist = context.Users.FirstOrDefault(u => u.Name == name);

            if (userExist != null)
                return;

            var newUser = new User(name);
            context.Users.Add(newUser);
            context.SaveChanges();
        }

        public void CreateTask(int userId, string title, string? description, DateTime? conclusionDate)
        {
            using var context = new AppDbContext();
            User user = context.Users.FirstOrDefault(u => u.Id == userId);
            if (user == null)
                return;

            var newTask = new UserTask(user, title, description, conclusionDate);
            context.Tasks.Add(newTask);
            context.SaveChanges();
        }

        public List<UserTask> GetTasksByUser(int userId)
        {
            using var context = new AppDbContext();
            return context.Tasks.Where(t => t.User.Id == userId).ToList();
        }
    }
}
