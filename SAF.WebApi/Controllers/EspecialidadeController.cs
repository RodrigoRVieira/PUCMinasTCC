using GISA.Domain.Model;
using GISA.Domain.Repository;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;

using Newtonsoft.Json;

using System.Collections.Generic;
using System.Threading.Tasks;

namespace SAF.WebApi.Controllers
{
    [ApiVersion("1.0")]
    [Route("v{v:apiVersion}/[controller]")]
    [ApiController]
    public class EspecialidadeController : BaseController
    {
        IEspecialidadeRepository _especialidadeRepository;

        public EspecialidadeController(
            IDistributedCache cache,
            IEspecialidadeRepository especialidadeRepository) : base(cache)
        {
            _especialidadeRepository = especialidadeRepository;
        }

        [HttpGet]
        [Route("{especialidadeId}")]
        public async Task<ActionResult<IList<Prestador>>> Get(long especialidadeId)
        {
            var repositoryData = await _especialidadeRepository.RecuperarPorId(especialidadeId);

            return repositoryData != null ?
            (ActionResult)Ok(repositoryData) :
            NoContent();
        }

        [HttpGet]
        public async Task<ActionResult<IList<Especialidade>>> EspecialidadeRecuperarTodas()
        {
            var cachedData = GetCacheData("Especialidades");

            if (cachedData == null)
            {
                var repositoryData = _especialidadeRepository.RecuperarTodas();

                await SetCacheData("Especialidades",
                    JsonConvert.SerializeObject(repositoryData,
                    new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore }));

                return repositoryData?.Count > 0 ?
                (ActionResult)Ok(repositoryData) :
                NoContent();
            }

            return cachedData != null ?
                (ActionResult)Ok(JsonConvert.DeserializeObject(cachedData)) :
                NoContent();
        }
    }
}
