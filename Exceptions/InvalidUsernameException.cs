namespace TaskManager.Exceptions
{
    public class InvalidUsernameException : Exception
    {
        public InvalidUsernameException() : base("O nome de usuário é inválido.") { }
        public InvalidUsernameException(string message) : base(message) { }
    }
}