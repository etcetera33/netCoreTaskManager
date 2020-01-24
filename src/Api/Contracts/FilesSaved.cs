using Core.Enums;
using Models.DTOs;
using System.Collections.Generic;

namespace Contracts
{
    public class FilesSaved
    {
        public IEnumerable<FileDto> Files { get; set; }
        public FileAddedAction Action { get; set; }
        public int? EntityId { get; set; }
    }
}
