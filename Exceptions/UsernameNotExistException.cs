namespace TaskManager.Exceptions
{
    public class UsernameNotExistException : Exception
    {
        public UsernameNotExistException() : base("O nome de usu�rio n�o foi criado anteriormente.") { }
    }
}