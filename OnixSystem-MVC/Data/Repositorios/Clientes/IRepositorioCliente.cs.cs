
using OnixSystem_MVC.Models.Entidades;

namespace OnixSystem_MVC.Data.Repositorios.Clientes
{
    public interface IRepositorioCliente
    {
        Task CriarCliente(Cliente cliente);
        Task<IEnumerable<Cliente>> ListarClientes();
        Task<Cliente> AtualizarCliente(Cliente cliente);
        Task<Cliente?> PegarClientePorId(Guid id);
        Task RemoverCliente(Cliente cliente);

        Task<Cliente?> PegarClientePorIdIncluindoInativos(Guid id);
        Task ReativarCliente(Cliente cliente);
        Task<IEnumerable<Cliente>> ListarClientesInativos();
    }
}
