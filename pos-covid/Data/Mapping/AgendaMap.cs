using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using pos_covid_api.Models;

namespace pos_covid_api.Data.Mapping;

public class AgendaMap :IEntityTypeConfiguration<Agenda>
{
    public void Configure(EntityTypeBuilder<Agenda> builder)
    {
        builder.ToTable("Agenda");
        builder.HasKey(x => x.Id);

        builder.HasOne(x => x.Psicologo)
            .WithMany(x => x.Agendamentos)
            .HasForeignKey(x => x.PsicologoId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasOne(x => x.Paciente)
            .WithMany(x => x.Agendamentos)
            .HasForeignKey(x => x.PacienteId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasOne(x => x.Horario)
            .WithMany(x => x.Agendamentos)
            .HasForeignKey(x => x.HorarioId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}