
using Microsoft.AspNetCore.Mvc;
using OnixSystem_MVC.Data.Repositorios.Fornecedores;
using OnixSystem_MVC.Models.Entidades;

namespace OnixSystem_MVC.Controllers
{
    public class FornecedorController : Controller
    {
        private readonly IRepositorioFornecedor _repositorio;

        public FornecedorController(IRepositorioFornecedor repositorio)
        {
            _repositorio = repositorio;
        }

        public async Task<IActionResult> Index(string? pesquisa)
        {
            var fornecedores = await _repositorio.ListarFornecedores();

            if (!string.IsNullOrWhiteSpace(pesquisa))
            {
                fornecedores = fornecedores.Where(f =>
                    f.RazaoSocial.Contains(pesquisa, StringComparison.OrdinalIgnoreCase) ||
                    f.CNPJ.Contains(pesquisa, StringComparison.OrdinalIgnoreCase) ||
                    f.Telefone.Contains(pesquisa, StringComparison.OrdinalIgnoreCase) ||
                    f.Email.Contains(pesquisa, StringComparison.OrdinalIgnoreCase));
            }

            ViewBag.Pesquisa = pesquisa;

            return View(fornecedores);
        }

        public IActionResult Criar()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CriarPost(Fornecedor fornecedor)
        {
            if (ModelState.IsValid)
            {
                await _repositorio.CriarFornecedor(fornecedor);
                return RedirectToAction("Index");
            }

            return View("Criar", fornecedor);
        }
        public async Task<IActionResult> Atualizar(Guid id)
        {
            var fornecedor = await _repositorio.PegarFornecedorPorId(id);

            if (fornecedor == null)
                return NotFound();

            return View(fornecedor);
        }

        [HttpPost]
        public async Task<IActionResult> AtualizarPost(Fornecedor fornecedor)
        {
            if (ModelState.IsValid)
            {
                await _repositorio.AtualizarFornecedor(fornecedor);
                return RedirectToAction("Index");
            }

            return View("Atualizar", fornecedor);
        }
        public async Task<IActionResult> DeleteSoft(Guid id)
        {
            var fornecedor = await _repositorio.PegarFornecedorPorId(id);

            if (fornecedor == null)
                return NotFound();

            return View(fornecedor);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteSoftPost(Fornecedor fornecedor)
        {
            var fornecedorBanco = await _repositorio.PegarFornecedorPorId(fornecedor.Id);

            if (fornecedorBanco == null)
                return NotFound();

            await _repositorio.RemoverFornecedor(fornecedorBanco);

            return RedirectToAction("Index");
        }
        [HttpPost]
        public async Task<IActionResult> Reativar(Guid id)
        {
            var fornecedor = await _repositorio.PegarFornecedorPorIdIncluindoInativos(id);

            if (fornecedor == null)
            {
                return NotFound();
            }

            await _repositorio.ReativarFornecedor(fornecedor);

            return RedirectToAction("Inativos");
        }

        public async Task<IActionResult> Inativos()
        {
            var fornecedores = await _repositorio.ListarFornecedoresInativos();

            return View(fornecedores);
        }
    }
}
