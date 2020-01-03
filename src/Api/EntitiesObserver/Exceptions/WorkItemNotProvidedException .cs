using System;

namespace EntitiesObserver.Exceptions
{
    class WorkItemNotProvidedException : Exception
    {
        public WorkItemNotProvidedException() : this("Host not provided")
        {

        }
        public WorkItemNotProvidedException(string message) : base(message)
        {

        }
    }
}
