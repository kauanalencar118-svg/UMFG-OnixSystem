using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OnixSystem_MVC.Migrations
{
    /// <inheritdoc />
    public partial class CriarTabelasVenda : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "VENDA",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "char(36)", nullable: false),
                    DATA_VENDA = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    VALOR_TOTAL = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    FORMA_PAGAMENTO = table.Column<string>(type: "longtext", nullable: false),
                    DATA_CRIACAO = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    DATA_ATUALIZACAO = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    IS_ATIVO = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VENDA", x => x.ID);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ITEM_VENDA",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "char(36)", nullable: false),
                    VENDA_ID = table.Column<Guid>(type: "char(36)", nullable: false),
                    PRODUTO_ID = table.Column<Guid>(type: "char(36)", nullable: false),
                    QUANTIDADE = table.Column<int>(type: "int", nullable: false),
                    PRECO_UNITARIO = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    DATA_CRIACAO = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    DATA_ATUALIZACAO = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    IS_ATIVO = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ITEM_VENDA", x => x.ID);
                    table.ForeignKey(
                        name: "FK_ITEM_VENDA_PRODUTO_PRODUTO_ID",
                        column: x => x.PRODUTO_ID,
                        principalTable: "PRODUTO",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ITEM_VENDA_VENDA_VENDA_ID",
                        column: x => x.VENDA_ID,
                        principalTable: "VENDA",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_ITEM_VENDA_PRODUTO_ID",
                table: "ITEM_VENDA",
                column: "PRODUTO_ID");

            migrationBuilder.CreateIndex(
                name: "IX_ITEM_VENDA_VENDA_ID",
                table: "ITEM_VENDA",
                column: "VENDA_ID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ITEM_VENDA");

            migrationBuilder.DropTable(
                name: "VENDA");
        }
    }
}
