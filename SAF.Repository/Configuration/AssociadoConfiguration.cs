using GISA.Domain.Model;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace SAF.Repository
{
    public class AssociadoConfiguration : BaseConfiguration, IEntityTypeConfiguration<Associado>
    {
        public AssociadoConfiguration() : base(TipoPessoa.Associado) { }

        public void Configure(EntityTypeBuilder<Associado> builder)
        {
            builder.OwnsOne(e => e.Email).HasData(Emails.ToArray());
            builder.OwnsOne(e => e.Endereco).HasData(Enderecos.ToArray());
        }
    }
}
