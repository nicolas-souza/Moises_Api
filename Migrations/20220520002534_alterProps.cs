using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace api.Migrations
{
    public partial class alterProps : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Jwt",
                table: "Usuarios",
                newName: "ChaveDeAcesso");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ChaveDeAcesso",
                table: "Usuarios",
                newName: "Jwt");
        }
    }
}
