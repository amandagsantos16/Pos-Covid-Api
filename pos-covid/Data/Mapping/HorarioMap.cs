using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using pos_covid_api.Models;

namespace pos_covid_api.Data.Mapping;

public class HorarioMap : IEntityTypeConfiguration<Horario>
{
    public void Configure(EntityTypeBuilder<Horario> builder)
    {
        builder.ToTable("Horario");
        builder.HasKey(x => x.Id);

        builder.HasOne(x => x.Psicologo)
            .WithMany(x => x.Horarios)
            .HasForeignKey(x => x.PsicologoId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}