using System;
using System.Collections.Generic;
using System.Text;

namespace Services.Helpers
{
    public interface IPasswordHasher
    {
        string Hash(string password);
    }
}
