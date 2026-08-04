namespace Dues.Infrastructure.Exceptions
{
    public class DueException : Exception
    {
        public DueException() { }
        public DueException(string message) : base(message) { }
        public DueException(string message, Exception innerException) : base(message, innerException) { }
    }
}
