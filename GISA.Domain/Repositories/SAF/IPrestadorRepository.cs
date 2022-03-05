using GISA.Domain.Model;

using System.Collections.Generic;
using System.Threading.Tasks;

namespace GISA.Domain.Repository
{
    public interface IPrestadorRepository
    {
        Task<Prestador> RecuperarPorId(long prestadorId);

        IList<Prestador> RecuperarPorEspecialidade(long especialidadeId);

        IList<Prestador> RecuperarPorEstado(string estadoPrestador);
    }
}
