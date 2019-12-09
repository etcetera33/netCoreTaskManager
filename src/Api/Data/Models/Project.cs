using System.Collections.Generic;

namespace Data.Models
{
    public class Project
    {
        public int ProjectId { get; set; }
        public string ProjectName { get; set; }
        public string Slug { get; set; }

        public ICollection<WorkItem> WorkItems { get; set; }
    }
}
