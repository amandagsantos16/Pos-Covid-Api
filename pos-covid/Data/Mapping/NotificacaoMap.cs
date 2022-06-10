using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using pos_covid_api.Models;

namespace pos_covid_api.Data.Mapping;

public class NotificacaoMap : IEntityTypeConfiguration<Notificacao>
{
    public void Configure(EntityTypeBuilder<Notificacao> builder)
    {
        builder.ToTable("Notificacao");
        builder.HasKey(x => x.Id);
    }
}