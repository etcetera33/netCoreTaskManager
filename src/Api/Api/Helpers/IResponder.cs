using Microsoft.AspNetCore.Mvc;

namespace Api.Helpers
{
    interface IResponder
    {
        IActionResult Ok(object response);
        IActionResult Created(object response);
        IActionResult NoContent(object response);
        IActionResult NotFound(object response);
    }
}
