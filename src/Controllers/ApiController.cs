using Microsoft.AspNetCore.Mvc;

using config.map.test.Service;

namespace config.map.test.V1
{
    [ApiController]
    [Route("api/v1")]
    public class ApiController : Controller
    {
        private IService _service;

        public class Fault { public string Message; public string StackTrace; }

        private ObjectResult LoggedFailureCode(int statusCode, Exception exception)
        {
            return StatusCode(statusCode, new Fault { Message = exception.Message, StackTrace = exception.StackTrace ?? "" });
        }

        public ApiController(IService service)
        {
            _service = service;
        }

        [HttpGet("GetContentFromConfigMap")]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(Fault))]
        public async Task<ActionResult> GetContentFromConfigMap()
        {
            try
            {
                var content =_service.ReadFromConfigMapExample();
                return StatusCode(StatusCodes.Status200OK, content);
            }
            catch (Exception ex)
            {
                return LoggedFailureCode(400, ex);
            }
        }
    }
}
