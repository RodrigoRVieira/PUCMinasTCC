using GISA.Domain.Model;

using System.Collections.Generic;

namespace GISA.Domain.Repository
{
    public interface IProcedimentoRepository
    {
        IList<Procedimento> RecuperarTodos();
    }
}
