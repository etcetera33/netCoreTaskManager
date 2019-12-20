using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Models
{
    public class Project
    {
        public int ProjectId { get; set; }
        public string ProjectName { get; set; }
        public string Description { get; set; }

        [ForeignKey(nameof(User))]
        public int OwnerId { get; set; }
        public User Owner { get; set; }

        public ICollection<WorkItem> WorkItems { get; set; }
    }
}
