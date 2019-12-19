using System.Collections.Generic;

namespace Data.Models
{
    public class Project
    {
        public int ProjectId { get; set; }
        public string ProjectName { get; set; }
        //owner
        //descr

        public ICollection<WorkItem> WorkItems { get; set; }
    }
}
