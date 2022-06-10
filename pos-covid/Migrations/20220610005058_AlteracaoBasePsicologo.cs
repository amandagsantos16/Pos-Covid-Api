using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace pos_covid_api.Migrations
{
    public partial class AlteracaoBasePsicologo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Psicologo_Usuario_UsuarioId",
                table: "Psicologo");

            migrationBuilder.AlterColumn<string>(
                name: "CRP",
                table: "Psicologo",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_Psicologo_CRP",
                table: "Psicologo",
                column: "CRP");

            migrationBuilder.AddForeignKey(
                name: "FK_Psicologo_Usuario_UsuarioId",
                table: "Psicologo",
                column: "UsuarioId",
                principalTable: "Usuario",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Psicologo_Usuario_UsuarioId",
                table: "Psicologo");

            migrationBuilder.DropIndex(
                name: "IX_Psicologo_CRP",
                table: "Psicologo");

            migrationBuilder.AlterColumn<string>(
                name: "CRP",
                table: "Psicologo",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddForeignKey(
                name: "FK_Psicologo_Usuario_UsuarioId",
                table: "Psicologo",
                column: "UsuarioId",
                principalTable: "Usuario",
                principalColumn: "Id");
        }
    }
}
