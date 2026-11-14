
using Microsoft.AspNetCore.Mvc;
using OnixSystem_MVC.Data.Repositorios.Vendas;
using OnixSystem_MVC.Data.Repositorios.Estoques;
using System.Text.Json;
using OnixSystem_MVC.Models.ViewModels;
using OnixSystem_MVC.Models.Entidades;

namespace OnixSystem_MVC.Controllers.Vendas
{
    public sealed class VendaController : Controller
    {
        private readonly IRepositorioVenda _repositoryVenda;
        private readonly IRepositorioEstoque _repositoryEstoque;

        public VendaController(
            IRepositorioVenda repositoryVenda,
            IRepositorioEstoque repositoryEstoque)
        {
            _repositoryVenda = repositoryVenda;
            _repositoryEstoque = repositoryEstoque;
        }

        public async Task<IActionResult> Index()
        {
            var estoques = await _repositoryEstoque.ListarEstoques();

            var carrinhoJson = HttpContext.Session.GetString("Carrinho");

            var carrinho = string.IsNullOrEmpty(carrinhoJson)
                ? new List<ItemCarrinhoViewModel>()
                : JsonSerializer.Deserialize<List<ItemCarrinhoViewModel>>(carrinhoJson)!;

            ViewBag.Carrinho = carrinho;

            return View(estoques);
        }

        [HttpGet]
        public IActionResult Pagamento()
        {
            var carrinhoJson = HttpContext.Session.GetString("Carrinho");

            if (string.IsNullOrEmpty(carrinhoJson))
            {
                TempData["Erro"] = "O carrinho está vazio.";
                return RedirectToAction("Index");
            }

            var carrinho = JsonSerializer.Deserialize<List<ItemCarrinhoViewModel>>(carrinhoJson)
                ?? new List<ItemCarrinhoViewModel>();

            if (!carrinho.Any())
            {
                TempData["Erro"] = "O carrinho está vazio.";
                return RedirectToAction("Index");
            }

            var pagamento = new PagamentoViewModel
            {
                Subtotal = carrinho.Sum(item => item.Preco * item.Quantidade),
                ValorTotal = carrinho.Sum(item => item.Preco * item.Quantidade)
            };

            return View(pagamento);
        }

        [HttpPost]
        public async Task<IActionResult> ConfirmarVenda(PagamentoViewModel pagamento)
        {
            var carrinhoJson = HttpContext.Session.GetString("Carrinho");

            if (string.IsNullOrEmpty(carrinhoJson))
            {
                TempData["Erro"] = "O carrinho está vazio.";
                return RedirectToAction("Index");
            }

            var carrinho = JsonSerializer.Deserialize<List<ItemCarrinhoViewModel>>(carrinhoJson)
                ?? new List<ItemCarrinhoViewModel>();

            if (!carrinho.Any())
            {
                TempData["Erro"] = "O carrinho está vazio.";
                return RedirectToAction("Index");
            }

            if (string.IsNullOrWhiteSpace(pagamento.FormaPagamento))
            {
                TempData["Erro"] = "Selecione uma forma de pagamento.";
                return RedirectToAction("Pagamento");
            }

            var subtotal = carrinho.Sum(i => i.Preco * i.Quantidade);

            var valorInformado = Math.Round(
                pagamento.ValorDesconto,
                2,
                MidpointRounding.AwayFromZero);

            if (valorInformado < 0)
            {
                TempData["Erro"] = "O desconto não pode ser negativo.";
                return RedirectToAction("Pagamento");
            }

            decimal desconto;

            if (pagamento.TipoDesconto == "Porcentagem")
            {
                if (valorInformado > 100)
                {
                    TempData["Erro"] = "O desconto não pode ultrapassar 100%.";
                    return RedirectToAction("Pagamento");
                }

                desconto = Math.Round(
                    subtotal * valorInformado / 100,
                    2,
                    MidpointRounding.AwayFromZero);
            }
            else if (pagamento.TipoDesconto == "Valor")
            {
                desconto = valorInformado;
            }
            else
            {
                TempData["Erro"] = "Tipo de desconto inválido.";
                return RedirectToAction("Pagamento");
            }

            if (desconto > subtotal)
            {
                TempData["Erro"] = "O desconto não pode ser maior que o subtotal.";
                return RedirectToAction("Pagamento");
            }

            var valorTotal = subtotal - desconto;

            if (pagamento.FormaPagamento == "Dinheiro" &&
                pagamento.ValorRecebido < valorTotal)
            {
                TempData["Erro"] = "O valor recebido é insuficiente.";
                return RedirectToAction("Pagamento");
            }

            if (pagamento.FormaPagamento != "Dinheiro" &&
                pagamento.FormaPagamento != "Pix" &&
                pagamento.FormaPagamento != "CartaoCredito" &&
                pagamento.FormaPagamento != "CartaoDebito")
            {
                TempData["Erro"] = "Forma de pagamento inválida.";
                return RedirectToAction("Pagamento");
            }

            var venda = new Venda
            {
                Subtotal = subtotal,
                Desconto = desconto,
                ValorTotal = valorTotal,
                FormaPagamento = pagamento.FormaPagamento,

                Itens = carrinho.Select(item => new ItemVenda
                {
                    ProdutoId = item.ProdutoId,
                    Quantidade = item.Quantidade,
                    PrecoUnitario = item.Preco
                }).ToList()
            };

            try
            {
                await _repositoryVenda.CriarVenda(venda);

                HttpContext.Session.Remove("Carrinho");

                TempData["Sucesso"] = "Venda registrada com sucesso!";
                return RedirectToAction("Index");
            }
            catch (InvalidOperationException ex)
            {
                TempData["Erro"] = ex.Message;
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Adicionar(Guid produtoId)
        {
            var estoque = await _repositoryEstoque
                .PegarEstoquePorProdutoId(produtoId);

            if (estoque == null || estoque.Produto == null || estoque.Quantidade <= 0)
            {
                TempData["Erro"] = "Produto indisponível ou sem estoque.";
                return RedirectToAction("Index");
            }

            var carrinhoJson = HttpContext.Session.GetString("Carrinho");

            var carrinho = string.IsNullOrEmpty(carrinhoJson)
                ? new List<ItemCarrinhoViewModel>()
                : JsonSerializer.Deserialize<List<ItemCarrinhoViewModel>>(carrinhoJson)!;

            var itemExistente = carrinho
                .FirstOrDefault(i => i.ProdutoId == produtoId);

            if (itemExistente != null)
            {
                if (itemExistente.Quantidade >= estoque.Quantidade)
                {
                    TempData["Erro"] = "Quantidade maior que o estoque disponível.";
                    return RedirectToAction("Index");
                }

                itemExistente.Quantidade++;
            }
            else
            {
                carrinho.Add(new ItemCarrinhoViewModel
                {
                    ProdutoId = estoque.ProdutoId,
                    Nome = estoque.Produto.Nome,
                    Tamanho = estoque.Produto.Tamanho,
                    Preco = estoque.Produto.Preco,
                    Quantidade = 1,
                    EstoqueDisponivel = estoque.Quantidade
                });
            }

            HttpContext.Session.SetString(
                "Carrinho",
                JsonSerializer.Serialize(carrinho)
            );

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> AlterarQuantidade(Guid produtoId, int alteracao)
        {
            var carrinhoJson = HttpContext.Session.GetString("Carrinho");

            if (string.IsNullOrEmpty(carrinhoJson))
                return RedirectToAction("Index");

            var carrinho = JsonSerializer.Deserialize<List<ItemCarrinhoViewModel>>(carrinhoJson)
                ?? new List<ItemCarrinhoViewModel>();

            var item = carrinho.FirstOrDefault(i => i.ProdutoId == produtoId);

            if (item == null || (alteracao != 1 && alteracao != -1))
                return RedirectToAction("Index");

            if (alteracao == 1)
            {
                var estoque = await _repositoryEstoque
                    .PegarEstoquePorProdutoId(produtoId);

                if (estoque == null || item.Quantidade >= estoque.Quantidade)
                {
                    TempData["Erro"] = "Quantidade maior que o estoque disponível.";
                    return RedirectToAction("Index");
                }

                item.Quantidade++;
            }
            else if (item.Quantidade > 1)
            {
                item.Quantidade--;
            }

            HttpContext.Session.SetString(
                "Carrinho",
                JsonSerializer.Serialize(carrinho)
            );

            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Remover(Guid produtoId)
        {
            var carrinhoJson = HttpContext.Session.GetString("Carrinho");

            if (!string.IsNullOrEmpty(carrinhoJson))
            {
                var carrinho = JsonSerializer.Deserialize<List<ItemCarrinhoViewModel>>(carrinhoJson)
                    ?? new List<ItemCarrinhoViewModel>();

                carrinho.RemoveAll(i => i.ProdutoId == produtoId);

                HttpContext.Session.SetString(
                    "Carrinho",
                    JsonSerializer.Serialize(carrinho)
                );
            }

            return RedirectToAction("Index");
        }
    }
}
