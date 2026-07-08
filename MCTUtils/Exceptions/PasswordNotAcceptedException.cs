using System;

namespace MCTUtils.Exceptions
{
    public class PasswordNotAcceptedException : Exception
    {
        public PasswordNotAcceptedException()
        {
        }

        public PasswordNotAcceptedException(string message)
            : base(message)
        {
        }

        public PasswordNotAcceptedException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
