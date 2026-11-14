
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OnixSystem_MVC.Migrations
{
    public partial class AlterarDescricaoParaTamanho : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                "UPDATE PRODUTO SET DESCRICAO = '' WHERE DESCRICAO IS NULL;");

            migrationBuilder.Sql(
                "ALTER TABLE PRODUTO CHANGE DESCRICAO TAMANHO LONGTEXT NOT NULL;");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                "ALTER TABLE PRODUTO CHANGE TAMANHO DESCRICAO LONGTEXT NULL;");
        }
    }
}
