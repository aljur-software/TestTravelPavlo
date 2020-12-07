namespace Application.Common.Exceptions
{
    public class InvalidFileFormatException : CustomExceptionBase
    {
        public InvalidFileFormatException(string message) : base(message) { }
    }
}