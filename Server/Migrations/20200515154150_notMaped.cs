using Microsoft.EntityFrameworkCore.Migrations;

namespace Apoyo.Server.Migrations
{
    public partial class notMaped : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IdGrupoString",
                table: "dbActividades");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "IdGrupoString",
                table: "dbActividades",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
