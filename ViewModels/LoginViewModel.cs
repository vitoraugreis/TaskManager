using TaskManager.Models;
using TaskManager.Data;
using TaskManager.Exceptions;

namespace TaskManager.ViewModels
{
    public class LoginViewModel
    {
        public void CreateUser(string username)
        {
            if (string.IsNullOrEmpty(username))
                throw new InvalidUsernameException("O campo de usuário está vazio.");
            if (username.Contains(" "))
                throw new InvalidUsernameException("O nome de usuário não pode conter espaços.");

            username = username.ToLower();
            using var context = new AppDbContext();
            if (context.Users.Any(u => u.Username == username))
                throw new UsernameAlreadyExistException(username);

            var newUser = new User(username);
            context.Users.Add(newUser);
            context.SaveChanges();
        }

        public User LoginUser(string username)
        {
            if (string.IsNullOrEmpty(username))
                throw new InvalidUsernameException("O campo de usuário está vazio.");
            if (username.Contains(" "))
                throw new InvalidUsernameException("O nome de usuário não pode conter espaços.");

            username = username.ToLower();
            using var context = new AppDbContext();
            var user = context.Users.FirstOrDefault(u => u.Username == username);
            if (user == null)
                throw new UsernameNotExistException();

            return user;
        }
    }
}
