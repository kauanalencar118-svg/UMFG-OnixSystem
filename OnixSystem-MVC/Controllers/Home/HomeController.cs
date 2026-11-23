using Microsoft.AspNetCore.Mvc;
using OnixSystem_MVC.Data.Repositorios.Produtos;
using OnixSystem_MVC.Data.Repositorios.Clientes;
using OnixSystem_MVC.Data.Repositorios.Configuracoes;
using OnixSystem_MVC.Data.Repositorios.Vendas;
using OnixSystem_MVC.Data.Repositorios.Estoques;
using System.Globalization;

namespace OnixSystem_MVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly IRepositorioProduto _repositorioProduto;
        private readonly IRepositorioCliente _repositorioCliente;
        private readonly IRepositorioConfiguracaoSistema _repositorioConfiguracao;
        private readonly IRepositorioVenda _repositorioVenda;
        private readonly IRepositorioEstoque _repositorioEstoque;

        public HomeController(
            IRepositorioProduto repositorioProduto,
            IRepositorioCliente repositorioCliente,
            IRepositorioConfiguracaoSistema repositorioConfiguracao,
            IRepositorioVenda repositorioVenda,
            IRepositorioEstoque repositorioEstoque)
        {
            _repositorioProduto = repositorioProduto;
            _repositorioCliente = repositorioCliente;
            _repositorioConfiguracao = repositorioConfiguracao;
            _repositorioVenda = repositorioVenda;
            _repositorioEstoque = repositorioEstoque;
        }

        public async Task<IActionResult> Index()
        {
            var produtos = await _repositorioProduto.ListarProdutos();
            var clientes = await _repositorioCliente.ListarClientes();
            var configuracao = await _repositorioConfiguracao.PegarConfiguracao();
            var vendas = await _repositorioVenda.ListarVendas();
            var estoques = await _repositorioEstoque.ListarEstoques();

            ViewBag.TotalProdutos = produtos.Count();
            ViewBag.TotalClientes = clientes.Count();
            ViewBag.NomeLoja = configuracao?.NomeLoja;
            ViewBag.TotalVendas = vendas.Sum(v => v.ValorTotal);

            ViewBag.EstoqueBaixo = estoques.Count(e =>
                e.Quantidade <= e.EstoqueMinimo);

            // RANKING DOS 5 PRODUTOS MAIS VENDIDOS
            var rankingProdutos = vendas
                .SelectMany(v => v.Itens)
                .Where(item => item.Produto != null)
                .GroupBy(item => new
                {
                    item.ProdutoId,
                    Nome = item.Produto!.Nome,
                    Detalhes = item.Produto!.Detalhes,
                    Tamanho = item.Produto!.Tamanho
                })
                .Select(grupo => new
                {
                    Nome = grupo.Key.Nome,
                    Detalhes = grupo.Key.Detalhes,
                    Tamanho = grupo.Key.Tamanho,
                    QuantidadeVendida = grupo.Sum(item => item.Quantidade)
                })
                .OrderByDescending(item => item.QuantidadeVendida)
                .Take(5)
                .ToList();

            ViewBag.RankingProdutos = rankingProdutos;

            // GRÁFICO DA SEMANA ATUAL (SEGUNDA A DOMINGO)
            var hoje = DateTime.Today;

            // Calcula a segunda-feira da semana atual
            var diasDesdeSegunda = ((int)hoje.DayOfWeek + 6) % 7;
            var inicioSemana = hoje.AddDays(-diasDesdeSegunda);

            var fimSemana = inicioSemana.AddDays(7);

            // Busca somente vendas da semana atual
            var vendasSemana = vendas
                .Where(v => v.DataVenda.Date >= inicioSemana &&
                            v.DataVenda.Date < fimSemana)
                .GroupBy(v => v.DataVenda.Date)
                .ToDictionary(
                    grupo => grupo.Key,
                    grupo => grupo.Sum(v => v.ValorTotal)
                );

            var cultura = new CultureInfo("pt-BR");

            // Gera os 7 dias da semana atual
            var dadosSemana = Enumerable.Range(0, 7)
                .Select(i =>
                {
                    var data = inicioSemana.AddDays(i);

                    var valor = vendasSemana.ContainsKey(data)
                        ? vendasSemana[data]
                        : 0m;

                    return new
                    {
                        Data = data,
                        Dia = cultura.TextInfo.ToTitleCase(
                            data.ToString("ddd", cultura)),
                        Valor = valor
                    };
                })
                .ToList();

            // Identifica o maior faturamento da semana
            var maiorValor = dadosSemana.Max(d => d.Valor);

            // Calcula a altura proporcional das barras
            var graficoSemana = dadosSemana
                .Select(d => new
                {
                    Dia = d.Dia,
                    Valor = d.Valor,

                    Altura = maiorValor > 0
                        ? (double)(d.Valor / maiorValor) * 100
                        : 0,

                    ValorFormatado = d.Valor.ToString("C", cultura),

                    Data = d.Data,

                    Hoje = d.Data == hoje
                })
                .ToList();

            ViewBag.GraficoSemana = graficoSemana;

            // TOTAL FATURADO NA SEMANA ATUAL
            ViewBag.FaturamentoSemana = dadosSemana.Sum(d => d.Valor);

            return View("~/Views/Pages/Home/Index.cshtml");
        }
    }
}