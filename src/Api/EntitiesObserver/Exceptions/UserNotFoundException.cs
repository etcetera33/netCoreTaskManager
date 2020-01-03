using System;

namespace EntitiesObserver.Exceptions
{
    class UserNotFoundException : Exception
    {
        public UserNotFoundException() : this("User not found")
        {

        }
        public UserNotFoundException(string message) : base(message)
        {

        }
    }
}
