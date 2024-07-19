using System;

namespace SOS.Core
{
    /// <summary>
    /// Thrown when the user does not have permission to execute an action
    /// </summary>
    public class SecurityException : Exception
    {
        public SecurityException()
            : base()
        {
        }

        public SecurityException(string message)
            : base(message)
        {
        }
    }
}
