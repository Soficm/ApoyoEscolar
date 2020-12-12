using Microsoft.EntityFrameworkCore.Migrations;

namespace Apoyo.Server.Migrations
{
    public partial class materias : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IdMateria",
                table: "dbActividades");

            migrationBuilder.AlterColumn<string>(
                name: "Actividad",
                table: "dbActividades",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(
                name: "IdGrupoString",
                table: "dbActividades",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "IdMateriaId",
                table: "dbActividades",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_dbActividades_IdMateriaId",
                table: "dbActividades",
                column: "IdMateriaId");

            migrationBuilder.AddForeignKey(
                name: "FK_dbActividades_dbMaterias_IdMateriaId",
                table: "dbActividades",
                column: "IdMateriaId",
                principalTable: "dbMaterias",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_dbActividades_dbMaterias_IdMateriaId",
                table: "dbActividades");

            migrationBuilder.DropIndex(
                name: "IX_dbActividades_IdMateriaId",
                table: "dbActividades");

            migrationBuilder.DropColumn(
                name: "IdGrupoString",
                table: "dbActividades");

            migrationBuilder.DropColumn(
                name: "IdMateriaId",
                table: "dbActividades");

            migrationBuilder.AlterColumn<string>(
                name: "Actividad",
                table: "dbActividades",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "IdMateria",
                table: "dbActividades",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
