
using OnixSystem_MVC.Models.Entidades;

namespace OnixSystem_MVC.Data.Repositorios.Configuracoes
{
    public interface IRepositorioConfiguracaoSistema
    {
        Task<ConfiguracaoSistema?> PegarConfiguracao();

        Task CriarConfiguracao(ConfiguracaoSistema configuracao);

        Task AtualizarConfiguracao(ConfiguracaoSistema configuracao);
        Task DesativarConfiguracao(ConfiguracaoSistema configuracao);
    }
}

