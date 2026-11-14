
using OnixSystem_MVC.Models.Entidades;

namespace OnixSystem_MVC.Data.Repositorios.Vendas
{
    public interface IRepositorioVenda
    {
        Task CriarVenda(Venda venda);

        Task<IEnumerable<Venda>> ListarVendas();

        Task<Venda?> PegarVendaPorId(Guid id);
    }
}
