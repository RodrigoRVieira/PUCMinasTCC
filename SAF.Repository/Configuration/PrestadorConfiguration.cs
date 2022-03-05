using GISA.Domain.Model;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace SAF.Repository
{
    public class PrestadorConfiguration : BaseConfiguration, IEntityTypeConfiguration<Prestador>
    {
        public PrestadorConfiguration() : base(TipoPessoa.Colaborador) { }

        public void Configure(EntityTypeBuilder<Prestador> builder)
        {
            builder.OwnsOne(e => e.Email).HasData(Emails.ToArray());
            builder.OwnsOne(e => e.Endereco).HasData(Enderecos.ToArray());
        }
    }
}
