namespace Dues.Infrastructure.Exceptions
{
    public class ApartmentException : Exception
    {
        public ApartmentException() { }
        public ApartmentException(string message) : base(message) { }
        public ApartmentException(string message, Exception innerException) : base(message, innerException) { }
    }
}
