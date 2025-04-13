namespace TaskManager.Exceptions
{
    public class UsernameNotExistException : Exception
    {
        public UsernameNotExistException() : base("O nome de usuário não foi criado anteriormente.") { }
    }
}