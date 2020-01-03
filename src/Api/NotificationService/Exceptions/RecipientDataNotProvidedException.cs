using System;

namespace NotificationService.Exceptions
{
    class RecipientDataNotProvidedException : Exception
    {
        public RecipientDataNotProvidedException() : this("User data not provided")
        {

        }

        public RecipientDataNotProvidedException(string message) : base(message)
        {

        }
    }
}
