using System;

namespace NotificationService.Exceptions
{
    class DataNotProvidedException : Exception
    {
        public DataNotProvidedException() : this("Work item not provided")
        {

        }

        public DataNotProvidedException(string message) : base(message)
        {

        }
    }
}
