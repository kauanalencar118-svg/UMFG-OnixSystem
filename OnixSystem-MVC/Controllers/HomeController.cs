
using Microsoft.AspNetCore.Mvc;
using OnixSystem_MVC.Models;
using OnixSystem_MVC.Data.Repositorios.Configuracoes;
using System.Diagnostics;

namespace OnixSystem_MVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly IRepositorioConfiguracaoSistema _repositorio;

        public HomeController(IRepositorioConfiguracaoSistema repositorio)
        {
            _repositorio = repositorio;
        }

        public async Task<IActionResult> Index()
        {
            var configuracao = await _repositorio.PegarConfiguracao();

            ViewBag.NomeLoja = configuracao?.NomeLoja;

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel
            {
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
            });
        }
    }
}
