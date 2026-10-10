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

        public async Task<IActionResult> Index()
        {

            var produtos = await _repositoryProduto.ListarProdutos();

            return View(produtos);
        }
        public IActionResult Criar()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CriarPost(Produto produto)
        {
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
        public async Task<IActionResult> DeleteSoftPost(Produto produto)
        {

            //var produto = await _repositoryProduto.PegarProdutoPorId(id);
            await _repositoryProduto.RemoverProduto(produto);
            return RedirectToAction("Index");
        }
    }
}
