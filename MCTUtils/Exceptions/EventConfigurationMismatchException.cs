using System;

namespace MCTUtils.Exceptions
{
    public class EventConfigurationMismatchException : Exception
    {
        public EventConfigurationMismatchException()
        {
        }

        public EventConfigurationMismatchException(string message)
            : base(message)
        {
        }

        public EventConfigurationMismatchException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
