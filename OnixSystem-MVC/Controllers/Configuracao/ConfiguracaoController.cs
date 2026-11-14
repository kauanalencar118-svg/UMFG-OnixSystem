
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using OnixSystem_MVC.Data.Repositorios.Configuracoes;
using OnixSystem_MVC.Models.Entidades;

namespace OnixSystem_MVC.Controllers
{
    public class ConfiguracaoController : Controller
    {
        private readonly IRepositorioConfiguracaoSistema _repositorio;

        private readonly PasswordHasher<ConfiguracaoSistema> _passwordHasher = new();

        public ConfiguracaoController(IRepositorioConfiguracaoSistema repositorio)
        {
            _repositorio = repositorio;
        }

        public async Task<IActionResult> Index()
        {
            var configuracao = await _repositorio.PegarConfiguracao();

            ViewBag.TemConfiguracao = configuracao != null;

            if (configuracao == null)
            {
                configuracao = new ConfiguracaoSistema();
            }

            return View("~/Views/Pages/Configuracao/Index.cshtml", configuracao);
        }

        public async Task<IActionResult> Editar()
        {
            var configuracao = await _repositorio.PegarConfiguracao();

            if (configuracao == null)
            {
                return RedirectToAction(nameof(Index));
            }

            ViewBag.TemConfiguracao = true;
            ViewBag.ModoEdicao = true;

            return View("~/Views/Pages/Configuracao/Index.cshtml", configuracao);
        }

        [HttpPost]
        public async Task<IActionResult> Salvar(ConfiguracaoSistema configuracao)
        {
            var configuracaoExistente = await _repositorio.PegarConfiguracao();

            if (!ModelState.IsValid)
            {
                ViewBag.TemConfiguracao = configuracaoExistente != null;
                ViewBag.ModoEdicao = configuracaoExistente != null;

                return View("~/Views/Pages/Configuracao/Index.cshtml", configuracao);
            }

            if (configuracaoExistente == null)
            {
                if (string.IsNullOrWhiteSpace(configuracao.Senha))
                {
                    ModelState.AddModelError("Senha", "Informe uma senha de segurança.");

                    ViewBag.TemConfiguracao = false;

                    return View("~/Views/Pages/Configuracao/Index.cshtml", configuracao);
                }

                configuracao.SenhaHash = _passwordHasher.HashPassword(
                    configuracao,
                    configuracao.Senha
                );

                await _repositorio.CriarConfiguracao(configuracao);
            }
            else
            {
                configuracaoExistente.NomeLoja = configuracao.NomeLoja;
                configuracaoExistente.CNPJ = configuracao.CNPJ;
                configuracaoExistente.Endereco = configuracao.Endereco;
                configuracaoExistente.Telefone = configuracao.Telefone;
                configuracaoExistente.Email = configuracao.Email;

                if (string.IsNullOrWhiteSpace(configuracaoExistente.SenhaHash))
                {
                    if (string.IsNullOrWhiteSpace(configuracao.Senha))
                    {
                        ModelState.AddModelError("Senha", "Defina uma senha de segurança.");

                        ViewBag.TemConfiguracao = true;
                        ViewBag.ModoEdicao = true;

                        return View("~/Views/Pages/Configuracao/Index.cshtml", configuracao);
                    }

                    configuracaoExistente.SenhaHash = _passwordHasher.HashPassword(
                        configuracaoExistente,
                        configuracao.Senha
                    );
                }

                await _repositorio.AtualizarConfiguracao(configuracaoExistente);
            }

            TempData["MensagemSucesso"] = "Configurações salvas com sucesso!";

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Excluir(string senha)
        {
            var configuracao = await _repositorio.PegarConfiguracao();

            if (configuracao == null)
            {
                return RedirectToAction(nameof(Index));
            }

            var resultado = _passwordHasher.VerifyHashedPassword(
                configuracao,
                configuracao.SenhaHash,
                senha ?? string.Empty
            );

            if (resultado == PasswordVerificationResult.Failed)
            {
                TempData["MensagemErro"] = "Senha incorreta! A loja não foi excluída.";

                return RedirectToAction(nameof(Index));
            }

            await _repositorio.DesativarConfiguracao(configuracao);

            TempData["MensagemSucesso"] = "Dados da loja excluídos com sucesso!";

            return RedirectToAction(nameof(Index));
        }
    }
}
