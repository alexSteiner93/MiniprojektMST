using System;
namespace AutoReservation.BusinessLayer.Exceptions
{
    public class AutoUnavailableException : Exception
    {
        public AutoUnavailableException()
        {
        }

        public AutoUnavailableException(string message)
            : base(message)
        {
        }

        public AutoUnavailableException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
