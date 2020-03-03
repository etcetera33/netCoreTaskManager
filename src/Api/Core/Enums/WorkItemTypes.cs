using System.ComponentModel.DataAnnotations;

namespace Core.Enums
{
    public enum WorkItemTypes
    {
        [Display(Name = "Bug")]
        Bug = 1,
        [Display(Name = "Feature")]
        Feature = 2
    }
}
