using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using pos_covid_api.Models;

namespace pos_covid_api.Data.Mapping;

public class PsicologoMap : IEntityTypeConfiguration<Psicologo>
{
    public void Configure(EntityTypeBuilder<Psicologo> builder)
    {
        builder.ToTable("Psicologo");
        builder.HasKey(x => x.Id);
        builder.HasIndex(x => x.CRP);

        builder.HasOne(x => x.Usuario)
            .WithOne(x => x.Psicologo)
            .HasForeignKey<Psicologo>(x => x.UsuarioId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}