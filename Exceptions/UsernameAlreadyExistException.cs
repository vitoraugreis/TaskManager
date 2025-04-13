namespace TaskManager.Exceptions
{
    public class UsernameAlreadyExistException : Exception
    {
        public UsernameAlreadyExistException(string username)
            : base($"O nome de usu�rio j� foi criado anteriormente.") { }
    }
}