using System;

namespace EntitiesObserver.Exceptions
{
    class WorkItemNotFoundException : Exception
    {
        public WorkItemNotFoundException() : this("Host not provided")
        {

        }
        public WorkItemNotFoundException(string message) : base(message)
        {

        }
    }
}
