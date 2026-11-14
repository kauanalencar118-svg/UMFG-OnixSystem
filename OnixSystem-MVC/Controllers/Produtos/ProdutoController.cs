using Microsoft.AspNetCore.Mvc;
using OnixSystem_MVC.Data.Repositorios.Produtos;
using OnixSystem_MVC.Models.Entidades;

namespace OnixSystem_MVC.Controllers.Produtos
{
    public sealed class ProdutoController : Controller
    {

        private IRepositorioProduto _repositoryProduto;
        public ProdutoController(IRepositorioProduto repositoryProduto)
        {
            _repositoryProduto = repositoryProduto;
        }

        public async Task<IActionResult> Index(string? pesquisa)
        {
            var produtos = await _repositoryProduto.ListarProdutos();

            if (!string.IsNullOrWhiteSpace(pesquisa))
            {
                produtos = produtos.Where(p =>
                    p.Nome.Contains(pesquisa, StringComparison.OrdinalIgnoreCase) ||
                    p.CodigoBarras.Contains(pesquisa, StringComparison.OrdinalIgnoreCase));
            }

            ViewBag.Pesquisa = pesquisa;

            return View(produtos);
        }
        public IActionResult Criar()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CriarPost(Produto produto)
        {
            if (!ModelState.IsValid)
            {
                return View("Criar", produto);
            }

            await _repositoryProduto.CriarProduto(produto);

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Atualizar(Guid id)
        {
            var produto = await _repositoryProduto.PegarProdutoPorId(id);
            return View(produto);
        }
        public async Task<IActionResult> AtualizarPost(Produto produto)
        {
            if (!ModelState.IsValid)
            {
                return View("Atualizar", produto);
            }

            await _repositoryProduto.AtualizarProduto(produto);

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> DeleteSoft(Guid id)
        {
            var produto = await _repositoryProduto.PegarProdutoPorId(id);
            return View(produto);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteSoftPost(Produto produto)
        {
            var produtoBanco = await _repositoryProduto.PegarProdutoPorId(produto.Id);

            if (produtoBanco == null)
            {
                return NotFound();
            }

            await _repositoryProduto.RemoverProduto(produtoBanco);

            return RedirectToAction("Index");
        }
    }
}
