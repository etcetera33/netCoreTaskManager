using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;
using System.Threading.Tasks;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WorkItemAuditsController : ControllerBase
    {
        private readonly IWorkItemAuditService _workItemAuditService;

        public WorkItemAuditsController(IWorkItemAuditService workItemAuditService)
        {
            _workItemAuditService = workItemAuditService;
        }

        [HttpGet("{workItemId}")]
        [Authorize]
        public async Task<IActionResult> Get(int workItemId)
        {
            var returnValue = await _workItemAuditService.GetWorkItemsHistoryById(workItemId);

            return Ok(returnValue);
        }
    }
}