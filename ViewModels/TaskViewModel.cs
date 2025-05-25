using System.Collections.ObjectModel;
using System.Linq;
using TaskManager.Models;
using TaskManager.Data;
using TaskManager.Exceptions;

namespace TaskManager.ViewModels
{
    public class TaskViewModel
    {
        public ObservableCollection<UserTask> Tasks { get; set; } = new();
        public User CurrentUser { get; }

        public TaskViewModel(User user) 
        {
            CurrentUser = user;
            LoadTasks();
        }

        private void LoadTasks()
        {
            using var context = new AppDbContext();
            var userTasks = context.Tasks
                .Where(t => t.UserId == CurrentUser.Id)
                .ToList();
            
            Tasks = new ObservableCollection<UserTask>(userTasks);
        }

        public void AddTask(string title, string? description = null, DateTime? completionDate = null)
        {

            using var context = new AppDbContext();
            var user = context.Users.First(u => u.Id == CurrentUser.Id);
            var newTask = new UserTask(user, title, description, completionDate);

            context.Tasks.Add(newTask);
            context.SaveChanges();

            Tasks.Add(newTask);
        }
    }
}