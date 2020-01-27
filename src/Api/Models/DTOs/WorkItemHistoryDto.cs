namespace Models.DTOs
{
    public class WorkItemHistoryDto
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public int Priority { get; set; }
        public int Progress { get; set; }
        public int WorkItemTypeId { get; set; }
        public int StatusId { get; set; }
        public int AssigneeId { get; set; }
        public int AuthorId { get; set; }
        public int ProjectId { get; set; }

        public static bool operator ==(WorkItemHistoryDto obj1, WorkItemHistoryDto obj2)
        {
            if (obj1 is null)
            {
                return false;
            }
            if (obj2 is null)
            {
                return false;
            }

            return (obj1.Title == obj2.Title
                    && obj1.Description == obj2.Description
                    && obj1.Priority == obj2.Priority
                    && obj1.Progress == obj2.Progress
                    && obj1.Priority == obj2.Priority
                    && obj1.WorkItemTypeId == obj2.WorkItemTypeId
                    && obj1.StatusId == obj2.StatusId
                    && obj1.AssigneeId == obj2.AssigneeId
                    && obj1.AuthorId == obj2.AuthorId
                    && obj1.ProjectId == obj2.ProjectId

            );
        }

        public static bool operator !=(WorkItemHistoryDto obj1, WorkItemHistoryDto obj2)
        {
            return !(obj1 == obj2);
        }
    }
}
