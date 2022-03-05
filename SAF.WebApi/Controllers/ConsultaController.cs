using Dapr.Client;

using GISA.Domain.Model;
using GISA.Domain.Model.DTO;
using GISA.Domain.Repository;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;

using SAF.WebApi.Filters;

using System.Collections.Generic;
using System.Threading.Tasks;

namespace SAF.WebApi.Controllers
{
    [ApiVersion("1.0")]
    [Route("v{v:apiVersion}/[controller]")]
    [ApiController]
    public class ConsultaController : BaseController
    {
        DaprClient _daprClient;
        IConsultaRepository _consultaRepository;
        ILogger<ConsultaController> _logger;

        public ConsultaController(
            IDistributedCache cache,
            IConsultaRepository consultaRepository,
            DaprClient daprClient,
            ILogger<ConsultaController> logger) : base(cache)
        {
            _consultaRepository = consultaRepository;

            _daprClient = daprClient;

            _logger = logger;
        }

        [HttpPost]
        [Authorize]
        [UserAttributeFilter(new object [] {"Associado"})]
        public async Task<ActionResult> Post(ConsultaDTO consulta)
        {
            consulta.CriadoPor = consulta.AssociadoId = base.PessoaId;
            
            if (!string.IsNullOrEmpty(System.Environment.GetEnvironmentVariable("DAPR")))
                await _daprClient.PublishEventAsync("pubsub", "consultassolicitadas", consulta);
            else
                await _consultaRepository.Incluir(new Consulta(consulta));

            return Ok();
        }

        [HttpGet]
        [Route("PorStatus/{statusConsulta}")]
        [Authorize]
        [UserAttributeFilter(new object [] {"Prestador"})]
        public ActionResult<IList<Consulta>> RecuperarPorStatus(string statusConsulta)
        {
            var repositoryData = _consultaRepository.RecuperarPorStatusPorPrestador(statusConsulta, base.PessoaId);

            return repositoryData?.Count > 0 ?
            (ActionResult)Ok(repositoryData) :
            NoContent();
        }
    }
}