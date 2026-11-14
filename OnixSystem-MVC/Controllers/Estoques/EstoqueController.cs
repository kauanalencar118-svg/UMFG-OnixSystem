using Microsoft.AspNetCore.Mvc;
using OnixSystem_MVC.Data.Repositorios.Estoques;
using OnixSystem_MVC.Data.Repositorios.Produtos;
using OnixSystem_MVC.Models.Entidades;

namespace OnixSystem_MVC.Controllers.Estoques
{
    public class EstoqueController : Controller
    {
        private readonly IRepositorioEstoque _repositorioEstoque;
        private readonly IRepositorioProduto _repositorioProduto;

        public EstoqueController(
            IRepositorioEstoque repositorioEstoque,
            IRepositorioProduto repositorioProduto)
        {
            _repositorioEstoque = repositorioEstoque;
            _repositorioProduto = repositorioProduto;
        }

        public async Task<IActionResult> Index()
        {
            var estoques = await _repositorioEstoque.ListarEstoques();
            return View(estoques);
        }


        public async Task<IActionResult> Criar()
        {
            var produtos = await _repositorioProduto.ListarProdutos();

            var estoques = await _repositorioEstoque.ListarEstoques();

            var produtosComEstoque = estoques
                .Select(e => e.ProdutoId)
                .ToList();

            ViewBag.Produtos = produtos
                .Where(p => !produtosComEstoque.Contains(p.Id))
                .ToList();

            return View();
        }


        [HttpPost]
        public async Task<IActionResult> CriarPost(Estoque estoque)
        {
            if (ModelState.IsValid)
            {
                var estoqueExistente = await _repositorioEstoque
                    .PegarEstoquePorProdutoId(estoque.ProdutoId);

                if (estoqueExistente != null)
                {
                    ModelState.AddModelError(
                        "ProdutoId",
                        "Este produto já possui cadastro no estoque."
                    );

                    ViewBag.Produtos = await _repositorioProduto.ListarProdutos();
                    return View("Criar", estoque);
                }

                await _repositorioEstoque.CriarEstoque(estoque);
                return RedirectToAction("Index");
            }

            ViewBag.Produtos = await _repositorioProduto.ListarProdutos();
            return View("Criar", estoque);
        }

        public async Task<IActionResult> Atualizar(Guid id)
        {
            var estoque = await _repositorioEstoque.PegarEstoquePorId(id);

            if (estoque == null)
                return NotFound();

            return View(estoque);
        }

        [HttpPost]
        public async Task<IActionResult> AtualizarPost(Estoque estoque)
        {
            if (ModelState.IsValid)
            {
                await _repositorioEstoque.AtualizarEstoque(estoque);
                return RedirectToAction("Index");
            }

            return View("Atualizar", estoque);
        }

        public async Task<IActionResult> DeleteSoft(Guid id)
        {
            var estoque = await _repositorioEstoque.PegarEstoquePorId(id);

            if (estoque == null)
                return NotFound();

            return View(estoque);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteSoftPost(Estoque estoque)
        {
            var estoqueBanco = await _repositorioEstoque.PegarEstoquePorId(estoque.Id);

            if (estoqueBanco == null)
                return NotFound();

            await _repositorioEstoque.RemoverEstoque(estoqueBanco);
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Inativos()
        {
            var estoques = await _repositorioEstoque.ListarEstoquesInativos();

            return View(estoques);
        }

        [HttpPost]
        public async Task<IActionResult> Reativar(Guid id)
        {
            var estoque = await _repositorioEstoque.PegarEstoquePorIdIncluindoInativos(id);

            if (estoque == null)
            {
                return NotFound();
            }

            await _repositorioEstoque.ReativarEstoque(estoque);

            return RedirectToAction("Inativos");
        }
    }
}
