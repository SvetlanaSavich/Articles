using System;

namespace Articles.Data
{
    public class ResourceNotFoundException : Exception
    {
        public ResourceNotFoundException()
        {

        }

        public ResourceNotFoundException(string message)
            : base(message)
        {

        }
    }
}