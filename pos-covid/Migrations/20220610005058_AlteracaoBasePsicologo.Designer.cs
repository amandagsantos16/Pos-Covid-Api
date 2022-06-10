﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using pos_covid_api.Data;

#nullable disable

namespace pos_covid_api.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20220610005058_AlteracaoBasePsicologo")]
    partial class AlteracaoBasePsicologo
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

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

            modelBuilder.Entity("pos_covid_api.Models.Usuario", b =>
                {
                    b.Navigation("Paciente");

                    b.Navigation("Psicologo");
                });
#pragma warning restore 612, 618
        }
    }
}
