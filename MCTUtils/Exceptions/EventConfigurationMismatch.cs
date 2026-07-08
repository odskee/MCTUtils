using System;

namespace MCTUtils.Exceptions
{
    public class EventConfigurationMismatch : Exception
    {
        public EventConfigurationMismatch()
        {
        }

        public EventConfigurationMismatch(string message)
            : base(message)
        {
        }

        public EventConfigurationMismatch(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
