using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using pos_covid_api.Models;

namespace pos_covid_api.Data.Mapping;

public class PacienteMap : IEntityTypeConfiguration<Paciente>
{
    public void Configure(EntityTypeBuilder<Paciente> builder)
    {
        builder.ToTable("Paciente");
        builder.HasKey(x => x.Id);
        
        builder.HasOne(x => x.Usuario)
            .WithOne(x => x.Paciente)
            .HasForeignKey<Paciente>(x => x.UsuarioId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}