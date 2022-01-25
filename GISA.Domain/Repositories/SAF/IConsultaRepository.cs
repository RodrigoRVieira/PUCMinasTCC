using GISA.Domain.Model;

using System.Collections.Generic;

using System.Threading.Tasks;

namespace GISA.Domain.Repository
{
    public interface IConsultaRepository
    {
        Task<Consulta> Incluir(Consulta consulta);

        Task<Consulta> RecuperarPorId(long consultaId);

        IList<Consulta> RecuperarPorStatusPorPrestador(string statusConsulta, long prestadorId);
    }
}
