namespace Dues.Infrastructure.Exceptions
{
    public class ResidentException : Exception
    {
        public ResidentException() { }
        public ResidentException(string message) : base(message) { }
        public ResidentException(string message, Exception innerException) : base(message, innerException) { }
    }
}
