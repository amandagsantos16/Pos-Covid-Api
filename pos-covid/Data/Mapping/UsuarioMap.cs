using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using pos_covid_api.Models;

namespace pos_covid_api.Data.Mapping;

public class UsuarioMap : IEntityTypeConfiguration<Usuario>
{
    public void Configure(EntityTypeBuilder<Usuario> builder)
    {
        builder.ToTable("Usuario");
        builder.HasKey(x => x.Id);
        builder.HasIndex(x => x.Email);
    }
}