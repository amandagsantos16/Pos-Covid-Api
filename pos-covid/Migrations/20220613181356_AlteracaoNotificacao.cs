using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace pos_covid_api.Migrations
{
    public partial class AlteracaoNotificacao : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "AgendamentoId",
                table: "Notificacao",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Notificacao_AgendamentoId",
                table: "Notificacao",
                column: "AgendamentoId");

            migrationBuilder.AddForeignKey(
                name: "FK_Notificacao_Agenda_AgendamentoId",
                table: "Notificacao",
                column: "AgendamentoId",
                principalTable: "Agenda",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Notificacao_Agenda_AgendamentoId",
                table: "Notificacao");

            migrationBuilder.DropIndex(
                name: "IX_Notificacao_AgendamentoId",
                table: "Notificacao");

            migrationBuilder.DropColumn(
                name: "AgendamentoId",
                table: "Notificacao");
        }
    }
}
