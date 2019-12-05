using System;
namespace AutoReservation.BusinessLayer.Exceptions
{
    public class InvalidDateRangeException : Exception
    {
        public InvalidDateRangeException()
        {
        }

        public InvalidDateRangeException(string message)
            : base(message)
        {
        }

        public InvalidDateRangeException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
