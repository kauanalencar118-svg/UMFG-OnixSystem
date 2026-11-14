
using Microsoft.EntityFrameworkCore;
using OnixSystem_MVC.Data.Contexts;
using OnixSystem_MVC.Models.Entidades;

namespace OnixSystem_MVC.Data.Repositorios.Configuracoes
{
    public class RepositorioConfiguracaoSistema : IRepositorioConfiguracaoSistema
    {
        private readonly AppDbContext _context;

        public RepositorioConfiguracaoSistema(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ConfiguracaoSistema?> PegarConfiguracao()
        {
            return await _context.ConfiguracoesSistema
                .FirstOrDefaultAsync(c => c.IsAtivo == true);
        }

        public async Task CriarConfiguracao(ConfiguracaoSistema configuracao)
        {
            await _context.ConfiguracoesSistema.AddAsync(configuracao);
            await _context.SaveChangesAsync();
        }

        public async Task AtualizarConfiguracao(ConfiguracaoSistema configuracao)
        {
            configuracao.DataAtualizacao = DateTime.Now;

            await _context.SaveChangesAsync();
        }
        public async Task DesativarConfiguracao(ConfiguracaoSistema configuracao)
        {
            configuracao.IsAtivo = false;
            configuracao.DataAtualizacao = DateTime.Now;

            await _context.SaveChangesAsync();
        }
    }
}
