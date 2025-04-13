namespace TaskManager.Exceptions
{
    public class InvalidUsernameException : Exception
    {
        public InvalidUsernameException() : base("O nome de usu�rio � inv�lido.") { }
        public InvalidUsernameException(string message) : base(message) { }
    }
}