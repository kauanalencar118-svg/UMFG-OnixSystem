using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using OnixSystem_MVC.Models.ViewModels;
using System.Globalization;

namespace OnixSystem_MVC.Services.Relatorios
{
    public class PdfRelatorioEstoque
    {
        public byte[] Gerar(RelatorioEstoqueViewModel model)
        {
            var cultura = new CultureInfo("pt-BR");

            return Document.Create(document =>
            {
                document.Page(page =>
                {
                    page.Size(PageSizes.A4.Landscape());
                    page.Margin(30);
                    page.DefaultTextStyle(x => x.FontSize(10));

                    page.Header().Column(column =>
                    {
                        column.Item()
                            .Text("ONIXSYSTEM")
                            .FontSize(22)
                            .Bold()
                            .FontColor("#1E3A8A");

                        column.Item()
                            .Text("Relatório de Estoque")
                            .FontSize(16)
                            .Bold();

                        column.Item()
                            .Text($"Gerado em: {DateTime.Now:dd/MM/yyyy HH:mm}");

                        column.Item()
                            .Text(model.EstoqueBaixo
                                ? "Filtro: Produtos com estoque baixo"
                                : "Filtro: Estoque completo");

                        column.Item().PaddingBottom(10);
                    });

                    page.Content().Column(column =>
                    {
                        column.Item()
                            .PaddingBottom(10)
                            .Text($"Total de produtos: {model.TotalProdutos} | Total de unidades: {model.TotalUnidades}")
                            .Bold();

                        column.Item().Table(table =>
                        {
                            table.ColumnsDefinition(columns =>
                            {
                                columns.RelativeColumn(3);
                                columns.RelativeColumn(1);
                                columns.RelativeColumn(3);
                                columns.RelativeColumn(1.5f);
                                columns.RelativeColumn(1);
                                columns.RelativeColumn(1);
                                columns.RelativeColumn(1.5f);
                            });

                            table.Header(header =>
                            {
                                header.Cell().Element(Cabecalho).Text("Produto");
                                header.Cell().Element(Cabecalho).Text("Tamanho");
                                header.Cell().Element(Cabecalho).Text("Detalhes");
                                header.Cell().Element(Cabecalho).Text("Preço");
                                header.Cell().Element(Cabecalho).Text("Qtd.");
                                header.Cell().Element(Cabecalho).Text("Mínimo");
                                header.Cell().Element(Cabecalho).Text("Situação");
                            });

                            foreach (var estoque in model.Estoques)
                            {
                                table.Cell().Element(Celula)
                                    .Text(estoque.Produto?.Nome ?? "Não encontrado");

                                table.Cell().Element(Celula)
                                    .Text(estoque.Produto?.Tamanho ?? "-");

                                table.Cell().Element(Celula)
                                    .Text(estoque.Produto?.Detalhes ?? "-");

                                table.Cell().Element(Celula)
                                    .Text(estoque.Produto != null
                                        ? estoque.Produto.Preco.ToString("C", cultura)
                                        : "-");

                                table.Cell().Element(Celula)
                                    .Text(estoque.Quantidade.ToString());

                                table.Cell().Element(Celula)
                                    .Text(estoque.EstoqueMinimo.ToString());

                                table.Cell().Element(Celula)
                                    .Text(estoque.Quantidade <= estoque.EstoqueMinimo
                                        ? "Estoque Baixo"
                                        : "Normal");
                            }
                        });
                    });

                    page.Footer().AlignCenter().Text(text =>
                    {
                        text.Span("OnixSystem | Página ");
                        text.CurrentPageNumber();
                        text.Span(" de ");
                        text.TotalPages();
                    });
                });
            }).GeneratePdf();
        }

        private static IContainer Cabecalho(IContainer container)
        {
            return container
                .Background("#1E3A8A")
                .Padding(5)
                .DefaultTextStyle(x => x
                    .FontColor("#FFFFFF")
                    .Bold());
        }

        private static IContainer Celula(IContainer container)
        {
            return container
                .BorderBottom(1)
                .BorderColor("#D1D5DB")
                .Padding(5);
        }
    }
}
