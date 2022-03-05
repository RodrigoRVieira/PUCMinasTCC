using Microsoft.AspNetCore.Mvc;

namespace SAF.WebApi.Controllers
{
    [ApiVersionNeutral]
    [ApiController]
    [Route("/")]
    public class HomeController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            return Ok(System.Environment.GetEnvironmentVariable("SOURCE_VERSION") ?? System.Net.Dns.GetHostName());
        }
    }
}