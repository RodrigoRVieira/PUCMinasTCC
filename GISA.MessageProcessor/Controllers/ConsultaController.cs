using Dapr;
using Dapr.Client;

using GISA.Domain.Model;
using GISA.Domain.Model.DTO;
using GISA.Domain.Repository;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

using System.Collections.Generic;
using System.Threading.Tasks;

namespace GISA.MessageProcessor.Controllers
{
    [ApiController]
    [Route("")]
    public class ConsultaController : ControllerBase
    {
        DaprClient _daprClient;

        IAssociadoRepository _associadoRepository;
        IConsultaRepository _consultaRepository;
        IEspecialidadeRepository _especialidadeRepository;
        IPrestadorRepository _prestadorRepository;

        ILogger<ConsultaController> _logger;

        public ConsultaController(DaprClient daprClient,
            ILogger<ConsultaController> logger,
            IAssociadoRepository associadoRepository,
            IConsultaRepository consultaRepository,
            IEspecialidadeRepository especialidadeRepository,
            IPrestadorRepository prestadorRepository)
        {
            _daprClient = daprClient;

            _logger = logger;

            _associadoRepository = associadoRepository;
            _consultaRepository = consultaRepository;
            _especialidadeRepository = especialidadeRepository;
            _prestadorRepository = prestadorRepository;
        }

        [Topic("pubsub", "consultassolicitadas")]
        [Route("solicitarconsulta")]
        [HttpPost()]
        public async Task<ActionResult> ConsultaSolicitar(ConsultaDTO consulta)
        {
            _logger.LogInformation(Newtonsoft.Json.JsonConvert.SerializeObject(consulta));

            Associado associado = null;
            Prestador prestador = null;

            try
            {
                associado = await _associadoRepository.RecuperarPorId(consulta.AssociadoId);
                _logger.LogInformation("Associado: " + Newtonsoft.Json.JsonConvert.SerializeObject(associado));

                var especialidade = await _especialidadeRepository.RecuperarPorId(consulta.EspecialidadeId);
                _logger.LogInformation("Especialidade: " + Newtonsoft.Json.JsonConvert.SerializeObject(especialidade));

                prestador = await _prestadorRepository.RecuperarPorId(consulta.PrestadorId);
                _logger.LogInformation("Prestador: " + Newtonsoft.Json.JsonConvert.SerializeObject(prestador));

                var consultaAgendada = await _consultaRepository.Incluir(new Consulta(consulta));

                var detalhesEmail = new Dictionary<string, string>
                {
                    ["emailFrom"] = "atendimento@boasaude.com.br",
                    ["emailTo"] = associado.Email.Endereco,
                    ["subject"] = $"[Boa Saúde] Sua Consulta em {consulta.Data.ToString("dd/MM")} às {consulta.Data.ToString("HH:mm")} foi agendada"
                };

                await _daprClient.PublishEventAsync("pubsub", "consultasagendadas",
                    new
                    {
                        metadata = new
                        {
                            emailFrom = "atendimento@boasaude.com.br",
                            emailTo = associado.Email.Endereco,
                            subject = $"[Boa Saúde] Sua Consulta em {consulta.Data.ToString("dd/MM")} às {consulta.Data.ToString("HH:mm")} foi agendada"
                        },
                        data = $"<span style='font-family: Verdana'><b>{associado.Nome.Split(' ')[0]}</b>,</br></br>Sua consulta com o(a) Dr(a) {prestador.Nome}, especialidade {especialidade.Nome}, às {consulta.Data.ToString("HH:mm")} do dia {consulta.Data.ToString("dd/MM")} está marcada.</br></br>Até lá!</br></br>Equipe Boa Saúde</span>"
                    });
            }
            catch (System.Exception)
            {
                await _daprClient.PublishEventAsync("pubsub", "consultasrejeitadas",
                    new
                    {
                        metadata = new
                        {
                            emailFrom = "atendimento@boasaude.com.br",
                            emailTo = associado.Email.Endereco,
                            subject = $"[Boa Saúde] Ops! Não foi possível agendar a sua Consulta :("
                        },
                        data = $"<span style='font-family: Verdana'><b>{associado.Nome.Split(' ')[0]}</b>,</br></br>Sua consulta com o(a) Dr(a) {prestador.Nome} às {consulta.Data.ToString("HH:mm")} do dia {consulta.Data.ToString("dd/MM")} NÃO FOI AGENDADA.</br></br>Equipe Boa Saúde</span>"
                    });
            }

            return Ok();
        }
    }
}
