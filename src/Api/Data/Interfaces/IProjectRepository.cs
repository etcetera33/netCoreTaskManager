using Data.Models;
using System.Collections.Generic;

namespace Data.Interfaces
{
    internal interface IProjectRepository
    {
        IEnumerable<Project> GetAll();
    }
}
