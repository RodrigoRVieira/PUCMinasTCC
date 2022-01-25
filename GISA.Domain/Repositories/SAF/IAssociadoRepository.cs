using GISA.Domain.Model;

using System.Threading.Tasks;

namespace GISA.Domain.Repository
{
    public interface IAssociadoRepository
    {
        Task<Associado> Incluir(Associado associado);

        Task<Associado> RecuperarPorId(long associadoId);

        Task<Associado> RecuperarPorCPF(string cpfAssociado);

        Task<Associado> RecuperarPorCarteirinha(string numeroCarteirinha);

        Associado Alterar(Associado associado);

        void Excluir(Associado associado);
    }
}
