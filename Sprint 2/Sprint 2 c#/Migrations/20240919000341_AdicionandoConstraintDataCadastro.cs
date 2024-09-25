using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GestaoSimples.Migrations
{
    public partial class AdicionandoConstraintDataCadastro : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("ALTER TABLE Fornecedores ADD CONSTRAINT DF_DataCadastro DEFAULT GETDATE() FOR DataCadastro");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("ALTER TABLE Fornecedores DROP CONSTRAINT DF_DataCadastro");
        }
    }
}