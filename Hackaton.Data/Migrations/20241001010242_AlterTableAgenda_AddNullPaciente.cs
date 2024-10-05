using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hackaton.Data.Migrations
{
    /// <inheritdoc />
    public partial class AlterTableAgenda_AddNullPaciente : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Agendas_Usuarios_MedicoId",
                table: "Agendas");

            migrationBuilder.DropForeignKey(
                name: "FK_Agendas_Usuarios_PacienteId",
                table: "Agendas");

            migrationBuilder.DropIndex(
                name: "IX_Agendas_MedicoId",
                table: "Agendas");

            migrationBuilder.DropIndex(
                name: "IX_Agendas_PacienteId",
                table: "Agendas");

            migrationBuilder.AlterColumn<int>(
                name: "PacienteId",
                table: "Agendas",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AddColumn<int>(
                name: "UsuarioEntityId",
                table: "Agendas",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Agendas_UsuarioEntityId",
                table: "Agendas",
                column: "UsuarioEntityId");

            migrationBuilder.AddForeignKey(
                name: "FK_Agendas_Usuarios_UsuarioEntityId",
                table: "Agendas",
                column: "UsuarioEntityId",
                principalTable: "Usuarios",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Agendas_Usuarios_UsuarioEntityId",
                table: "Agendas");

            migrationBuilder.DropIndex(
                name: "IX_Agendas_UsuarioEntityId",
                table: "Agendas");

            migrationBuilder.DropColumn(
                name: "UsuarioEntityId",
                table: "Agendas");

            migrationBuilder.AlterColumn<int>(
                name: "PacienteId",
                table: "Agendas",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Agendas_MedicoId",
                table: "Agendas",
                column: "MedicoId");

            migrationBuilder.CreateIndex(
                name: "IX_Agendas_PacienteId",
                table: "Agendas",
                column: "PacienteId");

            migrationBuilder.AddForeignKey(
                name: "FK_Agendas_Usuarios_MedicoId",
                table: "Agendas",
                column: "MedicoId",
                principalTable: "Usuarios",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Agendas_Usuarios_PacienteId",
                table: "Agendas",
                column: "PacienteId",
                principalTable: "Usuarios",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
