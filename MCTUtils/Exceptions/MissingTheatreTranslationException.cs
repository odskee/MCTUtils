using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCTUtils.Exceptions
{
    public class MissingTheatreTranslationException : Exception
    {
        public MissingTheatreTranslationException()
        {
        }

        public MissingTheatreTranslationException(string message)
            : base(message)
        {
        }

        public MissingTheatreTranslationException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
