using System;

namespace SOS.Core
{   
    /// <summary>
    /// Thrown when there is not an existing session
    /// </summary>
    public class SessionException : Exception
    {
        public SessionException()
            : base()
        {
        }

        public SessionException(string message)
            : base(message)
        {
        }
    }
}
