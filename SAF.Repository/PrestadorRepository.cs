using GISA.Domain.Model;
using GISA.Domain.Repository;

using Microsoft.EntityFrameworkCore;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SAF.Repository
{
    public class PrestadorRepository : IPrestadorRepository
    {
        private readonly SAFDbContext _context;

        public PrestadorRepository(SAFDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public Task<Prestador> RecuperarPorId(long prestadorId)
        {
            return _context.Prestadores.FirstOrDefaultAsync(a => a.Id == prestadorId);
        }

        public IList<Prestador> RecuperarPorEspecialidade(long especialidadeId)
        {
            return _context.Prestadores.Where(p => p.Especialidades.Any(e => e.Id == especialidadeId)).ToList();
        }

        public IList<Prestador> RecuperarPorEstado(string estadoPrestador)
        {
            return _context.Prestadores.Where(p => p.Endereco.Estado == estadoPrestador).ToList();
        }
    }
}
