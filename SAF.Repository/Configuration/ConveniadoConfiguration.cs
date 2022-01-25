using GISA.Domain.Model;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace SAF.Repository
{
    public class ConveniadoConfiguration : IEntityTypeConfiguration<Conveniado>
    {
        public void Configure(EntityTypeBuilder<Conveniado> builder)
        {
            builder.OwnsOne(e => e.Email);
            builder.OwnsOne(e => e.Endereco);
        }
    }
}
