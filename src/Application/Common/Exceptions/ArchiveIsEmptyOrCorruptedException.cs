namespace Application.Common.Exceptions
{
    public class ArchiveIsEmptyOrCorruptedException : CustomExceptionBase
    {
        public ArchiveIsEmptyOrCorruptedException(string message): base(message) { }
    }
}