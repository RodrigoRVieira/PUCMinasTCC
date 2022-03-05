using GISA.Domain.Authorization;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

using System.Collections.Generic;
using System.Threading.Tasks;

namespace GISA.AuthorizationService.Controllers
{
    [ApiVersion("1.0")]
    [Route("v{v:apiVersion}/[controller]")]
    [ApiController]
    public class AuthorizationController : Controller
    {
        readonly IConfiguration _configuration;

        readonly ILogger<AuthorizationController> _logger;

        public AuthorizationController(
            IConfiguration configuration,
            ILogger<AuthorizationController> logger)
        {
            _configuration = configuration;

            _logger = logger;
        }
        
        [HttpGet]
        public ActionResult Index()
        {
            return Ok(System.Environment.GetEnvironmentVariable("SOURCE_VERSION") ?? System.Net.Dns.GetHostName());
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> Post(User user)
        {
            if (!user.Password.Equals("bo@saud3"))
                return await Task.FromResult(BadRequest("Usuário ou senha inválida."));

            var jwtToken = JWTTokenHelper.GenerateToken(_configuration.GetSection("Authorization").GetValue<string>("Secret"),
                _configuration.GetSection("Authorization").GetValue<double>("TokenDuration"),
                user.Login);

            return await Task.FromResult(Ok(jwtToken));
        }
    }
}