using System;

namespace TodoApp.Dependency.Exceptions
{
    internal class DependencyResolutionFailedException : Exception
    {
        public DependencyResolutionFailedException()
        {
        }

        public DependencyResolutionFailedException(string message) 
            : base(message)
        {
        }

        public DependencyResolutionFailedException(string message, Exception innerException) 
            : base(message, innerException)
        {
        }
    }
}
