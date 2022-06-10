using Microsoft.EntityFrameworkCore;
using pos_covid_api.Data.Mapping;
using pos_covid_api.Models;

namespace pos_covid_api.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) 
        : base(options)
    { }

    public DbSet<Usuario> Usuarios { get; set; }
    public DbSet<Paciente> Pacientes { get; set; }
    public DbSet<Psicologo> Psicologos { get; set; }
    public DbSet<Notificacao> Notificacoes { get; set; }
    public DbSet<Horario> Horarios { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfiguration(new UsuarioMap());
        modelBuilder.ApplyConfiguration(new PacienteMap());
        modelBuilder.ApplyConfiguration(new PsicologoMap());
        modelBuilder.ApplyConfiguration(new NotificacaoMap());
        modelBuilder.ApplyConfiguration(new HorarioMap());
    }
}