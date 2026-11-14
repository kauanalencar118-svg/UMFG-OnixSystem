
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using OnixSystem_MVC.Data.Repositorios.Clientes;
using OnixSystem_MVC.Data.Repositorios.Configuracoes;
using OnixSystem_MVC.Models.Entidades;

namespace OnixSystem_MVC.Controllers
{
    public class ClienteController : Controller
    {
        private readonly IRepositorioCliente _repositorio;
        private readonly IRepositorioConfiguracaoSistema _repositorioConfiguracao;

        private readonly PasswordHasher<ConfiguracaoSistema> _passwordHasher = new();

        public ClienteController(
            IRepositorioCliente repositorio,
            IRepositorioConfiguracaoSistema repositorioConfiguracao)
        {
            _repositorio = repositorio;
            _repositorioConfiguracao = repositorioConfiguracao;
        }

        public async Task<IActionResult> Index(string? pesquisa)
        {
            var clientes = await _repositorio.ListarClientes();

            if (!string.IsNullOrWhiteSpace(pesquisa))
            {
                clientes = clientes.Where(c =>
                    c.Nome.Contains(pesquisa, StringComparison.OrdinalIgnoreCase) ||
                    c.CPF.Contains(pesquisa, StringComparison.OrdinalIgnoreCase) ||
                    c.Telefone.Contains(pesquisa, StringComparison.OrdinalIgnoreCase) ||
                    c.Email.Contains(pesquisa, StringComparison.OrdinalIgnoreCase));
            }

            ViewBag.Pesquisa = pesquisa;

            return View(clientes);
        }

        public IActionResult Criar()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CriarPost(Cliente cliente)
        {
            if (ModelState.IsValid)
            {
                await _repositorio.CriarCliente(cliente);
                return RedirectToAction("Index");
            }

            return View("Criar", cliente);
        }

        public async Task<IActionResult> Atualizar(Guid id)
        {
            var cliente = await _repositorio.PegarClientePorId(id);

            if (cliente == null)
                return NotFound();

            return View(cliente);
        }

        [HttpPost]
        public async Task<IActionResult> AtualizarPost(Cliente cliente)
        {
            if (ModelState.IsValid)
            {
                await _repositorio.AtualizarCliente(cliente);
                return RedirectToAction("Index");
            }

            return View("Atualizar", cliente);
        }

        public async Task<IActionResult> DeleteSoft(Guid id)
        {
            var cliente = await _repositorio.PegarClientePorId(id);

            if (cliente == null)
                return NotFound();

            return View(cliente);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteSoftPost(Cliente cliente, string senha)
        {
            if (!await ValidarSenhaLoja(senha))
            {
                TempData["Erro"] = "Senha incorreta! Não foi possível desativar o cliente.";
                return RedirectToAction("DeleteSoft", new { id = cliente.Id });
            }

            var clienteBanco = await _repositorio.PegarClientePorId(cliente.Id);

            if (clienteBanco == null)
                return NotFound();

            await _repositorio.RemoverCliente(clienteBanco);

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Reativar(Guid id, string senha)
        {
            if (!await ValidarSenhaLoja(senha))
            {
                TempData["Erro"] = "Senha incorreta! Não foi possível reativar o cliente.";
                return RedirectToAction("Inativos");
            }

            var cliente = await _repositorio.PegarClientePorIdIncluindoInativos(id);

            if (cliente == null)
                return NotFound();

            await _repositorio.ReativarCliente(cliente);

            return RedirectToAction("Inativos");
        }

        public async Task<IActionResult> Inativos()
        {
            var clientes = await _repositorio.ListarClientesInativos();

            return View(clientes);
        }

        private async Task<bool> ValidarSenhaLoja(string senha)
        {
            var configuracao = await _repositorioConfiguracao.PegarConfiguracao();

            if (configuracao == null ||
                string.IsNullOrWhiteSpace(configuracao.SenhaHash))
            {
                return false;
            }

            var resultado = _passwordHasher.VerifyHashedPassword(
                configuracao,
                configuracao.SenhaHash,
                senha ?? string.Empty
            );

            return resultado != PasswordVerificationResult.Failed;
        }
    }
}
