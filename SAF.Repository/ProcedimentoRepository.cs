using GISA.Domain.Model;
using GISA.Domain.Repository;

using System;
using System.Collections.Generic;
using System.Linq;

namespace SAF.Repository
{
    public class ProcedimentoRepository : IProcedimentoRepository
    {
        private readonly SAFDbContext _context;

        public ProcedimentoRepository(SAFDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public IList<Procedimento> RecuperarTodos()
        {
            return _context.Procedimentos.ToList();
        }
    }
}
