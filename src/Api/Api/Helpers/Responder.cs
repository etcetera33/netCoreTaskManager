using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Api.Helpers
{
    public class Responder : IResponder
    {
        public IActionResult Ok(object response)
        {
           return Invoke(response, new { StatusCodeResult = HttpStatusCode.OK });
        }
        public IActionResult Created(object response)
        {
            return Invoke(response, new { StatusCodeResult = HttpStatusCode.Created });
        }

        public IActionResult NoContent(object response)
        {
            return Invoke(response, new { StatusCodeResult = HttpStatusCode.NoContent });
        }

        public IActionResult NotFound(object response)
        {
            return Invoke(response, new { StatusCodeResult = HttpStatusCode.NotFound });
        }

        private IActionResult Invoke(object response, object serializerSettings)
        {
            return new JsonResult(response, serializerSettings);
        }
    }
}
