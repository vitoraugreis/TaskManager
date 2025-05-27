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
                // var existingTask = _dbContext.UserTasks.Find(updatedTask.Id);
                // if (existingTask != null) {
                //     _dbContext.Entry(existingTask).CurrentValues.SetValues(updatedTask);
                // } else {
                //    _dbContext.UserTasks.Update(updatedTask); // Ou _dbContext.Entry(updatedTask).State = EntityState.Modified;
                // }
                // Se 'updatedTask' é a mesma instância que está no DbContext e foi modificada pela UI,
                // apenas SaveChanges() é necessário.

            context.SaveChanges();
        }
    }
}