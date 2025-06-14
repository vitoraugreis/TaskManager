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

        public void AddTask(string title, string? description = null, DateTime? completionDate = null, bool hasCompletionTime = false)
        {
            using var context = new AppDbContext();
            var user = context.Users.First(u => u.Id == CurrentUser.Id);
            var newTask = new UserTask(user, title, description, completionDate, hasCompletionTime);

            context.Tasks.Add(newTask);
            context.SaveChanges();

            Tasks.Add(newTask);
        }

        public void RemoveTask(UserTask taskToRemove)
        {
            using var context = new AppDbContext();
            var taskInDb = context.Tasks.Find(taskToRemove.Id);
            var taskInCollection = Tasks.FirstOrDefault(t => t.Id == taskToRemove.Id);
            if (taskInDb != null)
            {
                context.Tasks.Remove(taskInDb);
                context.SaveChanges();
                if (taskInCollection != null)
                    Tasks.Remove(taskInCollection);
            }
            else
            {
                if (taskInCollection != null)
                    Tasks.Remove(taskInCollection); ;
            }
        }

        public void UpdateTask(UserTask updatedTask)
        {
            if (updatedTask == null) return;
            using var context = new AppDbContext();

            // Como updatedTask � a mesma inst�ncia que est� no context, apenas SaveChanges � necess�rio.
            context.SaveChanges();
        }
    }
}