
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using OnixSystem_MVC.Models.ViewModels;
using System.Globalization;

namespace OnixSystem_MVC.Services.Relatorios
{
    public class PdfRelatorioFaturamento
    {
        public byte[] Gerar(RelatorioFaturamentoViewModel model)
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
                        column.Item().Text("ONIXSYSTEM")
                            .FontSize(22)
                            .Bold()
                            .FontColor("#1E3A8A");

                        column.Item().Text("RELATÓRIO DE FATURAMENTO")
                            .FontSize(14)
                            .Bold();

                        column.Item().Text(
                            $"Período: {(model.DataInicial.HasValue ? model.DataInicial.Value.ToString("dd/MM/yyyy") : "Início")} até {(model.DataFinal.HasValue ? model.DataFinal.Value.ToString("dd/MM/yyyy") : "Hoje")}")
                            .FontSize(10)
                            .FontColor("#555555");

                        column.Item().PaddingBottom(10).LineHorizontal(1)
                            .LineColor("#1E3A8A");
                    });

                    // CONTEÚDO
                    page.Content().PaddingVertical(10).Column(column =>
                    {
                        column.Spacing(15);

                        // RESUMO FINANCEIRO
                        column.Item().Row(row =>
                        {
                            row.RelativeItem().Border(1).BorderColor("#198754")
                                .Padding(12).Column(card =>
                                {
                                    card.Item().Text("FATURAMENTO LÍQUIDO")
                                        .FontSize(10).Bold();

                                    card.Item().Text(
                                        model.TotalFaturado.ToString("C", cultura))
                                        .FontSize(18).Bold()
                                        .FontColor("#198754");
                                });

                            row.RelativeItem().Border(1).BorderColor("#1E3A8A")
                                .Padding(12).Column(card =>
                                {
                                    card.Item().Text("VENDAS REALIZADAS")
                                        .FontSize(10).Bold();

                                    card.Item().Text(model.TotalVendas.ToString())
                                        .FontSize(18).Bold()
                                        .FontColor("#1E3A8A");
                                });

                            row.RelativeItem().Border(1).BorderColor("#DC3545")
                                .Padding(12).Column(card =>
                                {
                                    card.Item().Text("TOTAL DE DESCONTOS")
                                        .FontSize(10).Bold();

                                    card.Item().Text(
                                        model.TotalDescontos.ToString("C", cultura))
                                        .FontSize(18).Bold()
                                        .FontColor("#DC3545");
                                });
                        });

                        // TABELA
                        column.Item().Table(table =>
                        {
                            table.ColumnsDefinition(columns =>
                            {
                                columns.RelativeColumn(2);
                                columns.RelativeColumn(2);
                                columns.RelativeColumn(2);
                                columns.RelativeColumn(2);
                                columns.RelativeColumn(2);
                            });

                            table.Header(header =>
                            {
                                header.Cell().Element(CellHeader).Text("Data");
                                header.Cell().Element(CellHeader).Text("Subtotal");
                                header.Cell().Element(CellHeader).Text("Desconto");
                                header.Cell().Element(CellHeader).Text("Valor Total");
                                header.Cell().Element(CellHeader).Text("Pagamento");
                            });

                            foreach (var venda in model.Vendas.OrderByDescending(v => v.DataVenda))
                            {
                                table.Cell().Element(CellBody)
                                    .Text(venda.DataVenda.ToString("dd/MM/yyyy HH:mm"));

                                table.Cell().Element(CellBody)
                                    .Text(venda.Subtotal.ToString("C", cultura));

                                table.Cell().Element(CellBody)
                                    .Text(venda.Desconto.ToString("C", cultura));

                                table.Cell().Element(CellBody)
                                    .Text(venda.ValorTotal.ToString("C", cultura));

                                table.Cell().Element(CellBody)
                                    .Text(venda.FormaPagamento);
                            }

                            // TOTALIZADOR
                            table.Cell().Element(CellTotal).Text("TOTAL");
                            table.Cell().Element(CellTotal)
                                .Text(model.Vendas.Sum(v => v.Subtotal).ToString("C", cultura));

                            table.Cell().Element(CellTotal)
                                .Text(model.TotalDescontos.ToString("C", cultura));

                            table.Cell().Element(CellTotal)
                                .Text(model.TotalFaturado.ToString("C", cultura));

                            table.Cell().Element(CellTotal).Text("");
                        });
                    });

                    // RODAPÉ
                    page.Footer().AlignCenter().Text(text =>
                    {
                        text.Span("ONIXSYSTEM  |  Relatório de Faturamento  |  Página ");
                        text.CurrentPageNumber();
                        text.Span(" de ");
                        text.TotalPages();
                    });
                });
            }).GeneratePdf();
        }

        // ESTILO DO CABEÇALHO DA TABELA
        static IContainer CellHeader(IContainer container)
        {
            return container
                .Background("#1E3A8A")
                .Padding(8)
                .DefaultTextStyle(x => x.FontColor("#FFFFFF").Bold())
                .Border(1)
                .BorderColor("#FFFFFF");
        }

        // ESTILO DAS CÉLULAS
        static IContainer CellBody(IContainer container)
        {
            return container
                .BorderBottom(1)
                .BorderColor("#DDDDDD")
                .Padding(8);
        }

        // ESTILO DO TOTAL
        static IContainer CellTotal(IContainer container)
        {
            return container
                .Background("#E9ECEF")
                .Border(1)
                .BorderColor("#CCCCCC")
                .Padding(8)
                .DefaultTextStyle(x => x.Bold());
        }
    }
}
