﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using pos_covid_api.Data;

#nullable disable

namespace pos_covid_api.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("pos_covid_api.Models.Agenda", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("Data")
                        .HasColumnType("datetime2");

                    b.Property<Guid?>("HorarioId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("PacienteId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("PsicologoId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("StatusAgendamento")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("HorarioId");

                    b.HasIndex("PacienteId");

                    b.HasIndex("PsicologoId");

                    b.ToTable("Agenda", (string)null);
                });

            modelBuilder.Entity("pos_covid_api.Models.Horario", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("DiaDaSemana")
                        .HasColumnType("int");

                    b.Property<DateTime>("Hora")
                        .HasColumnType("datetime2");

                    b.Property<Guid?>("PsicologoId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("PsicologoId");

                    b.ToTable("Horario", (string)null);
                });

            modelBuilder.Entity("pos_covid_api.Models.Notificacao", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("AgendamentoId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Mensagem")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("AgendamentoId");

                    b.ToTable("Notificacao", (string)null);
                });

            modelBuilder.Entity("pos_covid_api.Models.Paciente", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Nome")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid?>("UsuarioId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("UsuarioId")
                        .IsUnique()
                        .HasFilter("[UsuarioId] IS NOT NULL");

                    b.ToTable("Paciente", (string)null);
                });

            modelBuilder.Entity("pos_covid_api.Models.Psicologo", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("CRP")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime>("DataNascimento")
                        .HasColumnType("datetime2");

                    b.Property<string>("Especializacoes")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Nome")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("RegistroValido")
                        .HasColumnType("bit");

                    b.Property<string>("Resumo")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid?>("UsuarioId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("CRP");

                    b.HasIndex("UsuarioId")
                        .IsUnique()
                        .HasFilter("[UsuarioId] IS NOT NULL");

                    b.ToTable("Psicologo", (string)null);
                });

            modelBuilder.Entity("pos_covid_api.Models.Usuario", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Senha")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("Email");

                    b.ToTable("Usuario", (string)null);
                });

            modelBuilder.Entity("pos_covid_api.Models.Agenda", b =>
                {
                    b.HasOne("pos_covid_api.Models.Horario", "Horario")
                        .WithMany("Agendamentos")
                        .HasForeignKey("HorarioId")
                        .OnDelete(DeleteBehavior.SetNull);

                    b.HasOne("pos_covid_api.Models.Paciente", "Paciente")
                        .WithMany("Agendamentos")
                        .HasForeignKey("PacienteId")
                        .OnDelete(DeleteBehavior.SetNull);

                    b.HasOne("pos_covid_api.Models.Psicologo", "Psicologo")
                        .WithMany("Agendamentos")
                        .HasForeignKey("PsicologoId")
                        .OnDelete(DeleteBehavior.SetNull);

                    b.Navigation("Horario");

                    b.Navigation("Paciente");

                    b.Navigation("Psicologo");
                });

            modelBuilder.Entity("pos_covid_api.Models.Horario", b =>
                {
                    b.HasOne("pos_covid_api.Models.Psicologo", "Psicologo")
                        .WithMany("Horarios")
                        .HasForeignKey("PsicologoId")
                        .OnDelete(DeleteBehavior.SetNull);

                    b.Navigation("Psicologo");
                });

            modelBuilder.Entity("pos_covid_api.Models.Notificacao", b =>
                {
                    b.HasOne("pos_covid_api.Models.Agenda", "Agendamento")
                        .WithMany("Notificacoes")
                        .HasForeignKey("AgendamentoId")
                        .OnDelete(DeleteBehavior.SetNull);

                    b.Navigation("Agendamento");
                });

            modelBuilder.Entity("pos_covid_api.Models.Paciente", b =>
                {
                    b.HasOne("pos_covid_api.Models.Usuario", "Usuario")
                        .WithOne("Paciente")
                        .HasForeignKey("pos_covid_api.Models.Paciente", "UsuarioId")
                        .OnDelete(DeleteBehavior.SetNull);

                    b.Navigation("Usuario");
                });

            modelBuilder.Entity("pos_covid_api.Models.Psicologo", b =>
                {
                    b.HasOne("pos_covid_api.Models.Usuario", "Usuario")
                        .WithOne("Psicologo")
                        .HasForeignKey("pos_covid_api.Models.Psicologo", "UsuarioId")
                        .OnDelete(DeleteBehavior.SetNull);

                    b.Navigation("Usuario");
                });

            modelBuilder.Entity("pos_covid_api.Models.Agenda", b =>
                {
                    b.Navigation("Notificacoes");
                });

            modelBuilder.Entity("pos_covid_api.Models.Horario", b =>
                {
                    b.Navigation("Agendamentos");
                });

            modelBuilder.Entity("pos_covid_api.Models.Paciente", b =>
                {
                    b.Navigation("Agendamentos");
                });

            modelBuilder.Entity("pos_covid_api.Models.Psicologo", b =>
                {
                    b.Navigation("Agendamentos");

                    b.Navigation("Horarios");
                });

            modelBuilder.Entity("pos_covid_api.Models.Usuario", b =>
                {
                    b.Navigation("Paciente");

                    b.Navigation("Psicologo");
                });
#pragma warning restore 612, 618
        }
    }
}
