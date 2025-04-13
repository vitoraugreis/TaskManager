namespace TaskManager.Exceptions
{
    public class UsernameAlreadyExistException : Exception
    {
        public UsernameAlreadyExistException(string username)
            : base($"O nome de usuário já foi criado anteriormente.") { }
    }
}