using System;

namespace Articles.Services
{
    public class ResourceHasConflictException : Exception
    {
        public ResourceHasConflictException()
        {
        }

        public ResourceHasConflictException(string message)
            : base(message)
        {
        }
    }
}