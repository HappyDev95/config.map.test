using System.Reflection;
using Microsoft.AspNetCore.Mvc;

using config.map.test.Service;

namespace config.map.test.Diagnostic
{
    [ApiController]
    [Produces("application/json")]
    [Route("diagnostic")]
    public class DiagnosticController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly IService _service;
        private ObjectResult LoggedFailureCode(int statusCode, Exception exception) { return StatusCode(statusCode, $"{exception.Message}\n{exception?.StackTrace?.ToString()}"); }

        public DiagnosticController( IConfiguration configuration, IService service)
        {
            _configuration = configuration; 
            _service = service;
        }

        //GET diagnostic/version
        [HttpGet("version")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<string> GetVersions()
        {
            var version = $"{Assembly.GetEntryAssembly().GetName().Version} [{Assembly.GetExecutingAssembly().GetCustomAttribute<AssemblyInformationalVersionAttribute>()?.InformationalVersion ?? "UNKNOWN"}]";
            return StatusCode(StatusCodes.Status200OK, version);
        }

        //GET diagnostic/configuration
        [HttpGet("configuration")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status502BadGateway)]
        public ActionResult<string> GetConfiguration()
        {
            try
            {
                return StatusCode(StatusCodes.Status200OK, _configuration.ToString());
            }
            catch(Exception ex)
            {
                return LoggedFailureCode(StatusCodes.Status502BadGateway, ex);
            }
            
        }

        //GET diagnostic/insights
        [HttpGet("insights")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<string> GetInsights() 
        {
            return StatusCode(StatusCodes.Status200OK, "SOME INSIGHTS");
        }

        //GET diagnostic/stats
        [HttpGet("stats")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<string> GetStats()
        {
            return StatusCode(StatusCodes.Status200OK, "SOME DATA");
        }
    }
}
