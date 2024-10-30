using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GestaoSimples.Migrations
{
    public partial class CPFusuario : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CPF",
                table: "Usuarios",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CPF",
                table: "Usuarios");
        }
    }
}
