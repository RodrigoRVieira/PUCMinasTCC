using GISA.Domain.Model;
using GISA.Domain.Repository;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;

using System.Collections.Generic;
using System.Threading.Tasks;

namespace SAF.WebApi.Controllers
{
    [ApiVersion("1.0")]
    [Route("v{v:apiVersion}/[controller]")]
    [ApiController]
    public class PrestadorController : BaseController
    {
        IPrestadorRepository _prestadorRepository;

        public PrestadorController(
            IDistributedCache cache,
            IPrestadorRepository prestadorRepository) : base(cache)
        {
            _prestadorRepository = prestadorRepository;
        }

        [HttpGet]
        [Route("{prestadorId}")]
        public async Task<ActionResult<IList<Prestador>>> Get(long prestadorId)
        {
            var repositoryData = await _prestadorRepository.RecuperarPorId(prestadorId);

            return repositoryData != null ?
            (ActionResult)Ok(repositoryData) :
            NoContent();
        }

        [HttpGet]
        [Route("PorEspecialidade/{especialidadeId}")]
        public ActionResult<IList<Prestador>> RecuperarPorEspecialidade(long especialidadeId)
        {
            var repositoryData = _prestadorRepository.RecuperarPorEspecialidade(especialidadeId);

            return repositoryData?.Count > 0 ?
            (ActionResult)Ok(repositoryData) :
            NoContent();
        }

        [HttpGet]
        [Route("PorEstado/{estadoPrestador}")]
        public ActionResult<IList<Prestador>> RecuperarPorEstado(string estadoPrestador)
        {
            var repositoryData = _prestadorRepository.RecuperarPorEstado(estadoPrestador);

            return repositoryData?.Count > 0 ?
            (ActionResult)Ok(repositoryData) :
            NoContent();
        }
    }
}
