
using Microsoft.AspNetCore.Mvc;
using OnixSystem_MVC.Data.Repositorios.Vendas;
using OnixSystem_MVC.Models.ViewModels;
using OnixSystem_MVC.Services.Relatorios;

namespace OnixSystem_MVC.Controllers.Faturamentos
{
    public class FaturamentoController : Controller
    {
        private readonly IRepositorioVenda _repositorioVenda;

        public FaturamentoController(IRepositorioVenda repositorioVenda)
        {
            _repositorioVenda = repositorioVenda;
        }

        // RELATÓRIO DE FATURAMENTO
        public async Task<IActionResult> Relatorio(DateTime? dataInicial, DateTime? dataFinal)
        {
            var vendas = await _repositorioVenda.ListarVendas();

            if (dataInicial.HasValue)
            {
                vendas = vendas
                    .Where(v => v.DataVenda >= dataInicial.Value.Date)
                    .ToList();
            }

            if (dataFinal.HasValue)
            {
                vendas = vendas
                    .Where(v => v.DataVenda < dataFinal.Value.Date.AddDays(1))
                    .ToList();
            }

            var viewModel = new RelatorioFaturamentoViewModel
            {
                Vendas = vendas.ToList(),
                DataInicial = dataInicial,
                DataFinal = dataFinal,
                TotalVendas = vendas.Count(),
                TotalFaturado = vendas.Sum(v => v.ValorTotal),
                TotalDescontos = vendas.Sum(v => v.Desconto)
            };

            return View("/Views/Pages/Faturamento/Relatorio.cshtml", viewModel);
        }

        // PDF DO RELATÓRIO DE FATURAMENTO
        public async Task<IActionResult> BaixarPdf(DateTime? dataInicial, DateTime? dataFinal)
        {
            var vendas = await _repositorioVenda.ListarVendas();

            if (dataInicial.HasValue)
            {
                vendas = vendas
                    .Where(v => v.DataVenda >= dataInicial.Value.Date)
                    .ToList();
            }

            if (dataFinal.HasValue)
            {
                vendas = vendas
                    .Where(v => v.DataVenda < dataFinal.Value.Date.AddDays(1))
                    .ToList();
            }

            var viewModel = new RelatorioFaturamentoViewModel
            {
                Vendas = vendas.ToList(),
                DataInicial = dataInicial,
                DataFinal = dataFinal,
                TotalVendas = vendas.Count(),
                TotalFaturado = vendas.Sum(v => v.ValorTotal),
                TotalDescontos = vendas.Sum(v => v.Desconto)
            };

            var pdf = new PdfRelatorioFaturamento().Gerar(viewModel);

            return File(pdf, "application/pdf", "RelatorioFaturamento.pdf");
        }
    }
}
