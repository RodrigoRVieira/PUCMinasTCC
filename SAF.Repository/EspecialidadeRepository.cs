using GISA.Domain.Model;
using GISA.Domain.Repository;

using Microsoft.EntityFrameworkCore;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SAF.Repository
{
    public class EspecialidadeRepository : IEspecialidadeRepository
    {
        private readonly SAFDbContext _context;

        public EspecialidadeRepository(SAFDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public Task<Especialidade> RecuperarPorId(long especialidadeId)
        {
            return _context.Especialidades.FirstOrDefaultAsync(e => e.Id == especialidadeId);
        }

        public IList<Especialidade> RecuperarTodas()
        {
            return _context.Especialidades.ToList();
        }
    }
}
