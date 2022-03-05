using GISA.Domain.Model;
using GISA.Domain.Repository;

using Microsoft.EntityFrameworkCore;

using System;
using System.Threading.Tasks;

namespace SAF.Repository
{
    public class AssociadoRepository : IAssociadoRepository
    {
        private readonly SAFDbContext _context;

        public AssociadoRepository(SAFDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<Associado> Incluir(Associado associado)
        {
            return (await _context.Associados.AddAsync(associado)).Entity;
        }

        public Associado Alterar(Associado associado)
        {
            return _context.Associados.Update(associado).Entity;
        }

        public Task<Associado> RecuperarPorId(long associadoId)
        {
            return _context.Associados.FirstOrDefaultAsync(a => a.Id == associadoId);
        }

        public Task<Associado> RecuperarPorCPF(string cpfAssociado)
        {
            return _context.Associados.FirstOrDefaultAsync(a => a.CPF == cpfAssociado);
        }

        public Task<Associado> RecuperarPorCarteirinha(string numeroCarteirinha)
        {
            return _context.Associados.FirstOrDefaultAsync(a => a.NumeroCarteirinha == numeroCarteirinha);
        }

        public void Excluir(Associado associado)
        {
            _context.Remove(associado);
        }
    }
}
