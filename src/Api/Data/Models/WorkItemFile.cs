using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Models
{
    public class WorkItemFile
    {
        public int WorkItemFileId { get; set; }

        [ForeignKey(nameof(WorkItem))]
        public int WorkItemId { get; set; }
        public WorkItem WorkItem { get; set; }

        [ForeignKey(nameof(File))]
        public int FileId { get; set; }
        public File File { get; set; }
    }
}
