
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using OnixSystem_MVC.Models.ViewModels;
using System.Globalization;

namespace OnixSystem_MVC.Services.Relatorios
{
    public class PdfRelatorioVendas
    {
        public byte[] Gerar(RelatorioVendasViewModel model)
        {
            var cultura = new CultureInfo("pt-BR");

            return Document.Create(document =>
            {
                document.Page(page =>
                {
                    page.Size(PageSizes.A4.Landscape());
                    page.Margin(30);
                    page.DefaultTextStyle(x => x.FontSize(10));

                    // CABEÇALHO
                    page.Header().Column(column =>
                    {
                        column.Item()
                            .Text("ONIXSYSTEM")
                            .FontSize(22)
                            .Bold()
                            .FontColor("#1E3A8A");

                        column.Item()
                            .Text("Relatório de Vendas")
                            .FontSize(16)
                            .Bold();

                        column.Item()
                            .Text($"Gerado em: {DateTime.Now:dd/MM/yyyy HH:mm}");

                        column.Item()
                            .Text($"Período: {(model.DataInicial.HasValue ? model.DataInicial.Value.ToString("dd/MM/yyyy") : "Inicial")} até {(model.DataFinal.HasValue ? model.DataFinal.Value.ToString("dd/MM/yyyy") : "Atual")}");

                        column.Item().PaddingBottom(10);
                    });

                    // CONTEÚDO
                    page.Content().Column(column =>
                    {
                        column.Item()
                            .PaddingBottom(10)
                            .Text($"Total de vendas: {model.TotalVendas} | Total faturado: {model.TotalFaturado.ToString("C", cultura)} | Total de descontos: {model.TotalDescontos.ToString("C", cultura)}")
                            .Bold();

                        column.Item().Table(table =>
                        {
                            table.ColumnsDefinition(columns =>
                            {
                                columns.RelativeColumn(1.5f);
                                columns.RelativeColumn(3);
                                columns.RelativeColumn(1.5f);
                                columns.RelativeColumn(1.5f);
                                columns.RelativeColumn(1.5f);
                                columns.RelativeColumn(1.5f);
                            });

                            // CABEÇALHO DA TABELA
                            table.Header(header =>
                            {
                                header.Cell().Element(Cabecalho).Text("Data");
                                header.Cell().Element(Cabecalho).Text("Produtos");
                                header.Cell().Element(Cabecalho).Text("Pagamento");
                                header.Cell().Element(Cabecalho).Text("Subtotal");
                                header.Cell().Element(Cabecalho).Text("Desconto");
                                header.Cell().Element(Cabecalho).Text("Total");
                            });

                            // VENDAS
                            foreach (var venda in model.Vendas.OrderByDescending(v => v.DataVenda))
                            {
                                var produtos = string.Join(", ",
                                    venda.Itens.Select(item =>
                                        $"{item.Produto?.Nome ?? "Produto não encontrado"} x{item.Quantidade}"));

                                table.Cell().Element(Celula)
                                    .Text(venda.DataVenda.ToString("dd/MM/yyyy HH:mm"));

                                table.Cell().Element(Celula)
                                    .Text(produtos);

                                table.Cell().Element(Celula)
                                    .Text(venda.FormaPagamento?.ToString() ?? "-");

                                table.Cell().Element(Celula)
                                    .Text(venda.Subtotal.ToString("C", cultura));

                                table.Cell().Element(Celula)
                                    .Text(venda.Desconto.ToString("C", cultura));

                                table.Cell().Element(Celula)
                                    .Text(venda.ValorTotal.ToString("C", cultura));
                            }
                        });
                    });

                    // RODAPÉ
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

        // ESTILO DO CABEÇALHO DA TABELA
        private static IContainer Cabecalho(IContainer container)
        {
            return container
                .Background("#1E3A8A")
                .Padding(5)
                .DefaultTextStyle(x => x
                    .FontColor("#FFFFFF")
                    .Bold());
        }

        // ESTILO DAS CÉLULAS
        private static IContainer Celula(IContainer container)
        {
            return container
                .BorderBottom(1)
                .BorderColor("#D1D5DB")
                .Padding(5);
        }
    }
}
