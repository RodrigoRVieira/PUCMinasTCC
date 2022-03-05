using GISA.Domain.Model;
using GISA.Domain.Repository;

using Microsoft.EntityFrameworkCore;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SAF.Repository
{
    public class ConsultaRepository : IConsultaRepository
    {
        private readonly SAFDbContext _context;

        public ConsultaRepository(SAFDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<Consulta> Incluir(Consulta consulta)
        {
            consulta.CriadoEm = DateTime.UtcNow;
            consulta.Status = StatusConsulta.Criada;

            var result = (await _context.Consultas.AddAsync(consulta)).Entity;

            await _context.SaveChangesAsync();

            return result;
        }

        public Task<Consulta> RecuperarPorId(long consultaId)
        {
            return _context.Consultas.FirstOrDefaultAsync(a => a.Id == consultaId);
        }

        public IList<Consulta> RecuperarPorStatusPorPrestador(string statusConsulta, long prestadorId)
        {
            return _context.Consultas.Where(a => 
            a.Status == Enum.Parse<StatusConsulta>(statusConsulta) &&
            a.PrestadorId == prestadorId).ToList();
        }
    }
}
