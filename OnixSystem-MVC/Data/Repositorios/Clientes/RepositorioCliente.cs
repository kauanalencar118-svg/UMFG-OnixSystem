using OnixSystem_MVC.Data.Contexts;
using OnixSystem_MVC.Models.Entidades;
using Microsoft.EntityFrameworkCore;

namespace OnixSystem_MVC.Data.Repositorios.Clientes
{
    public class RepositorioCliente : IRepositorioCliente
    {
        private readonly AppDbContext _context;

        public RepositorioCliente(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Cliente> AtualizarCliente(Cliente cliente)
        {
            _context.Clientes.Update(cliente);
            await _context.SaveChangesAsync();
            return cliente;
        }

        public async Task CriarCliente(Cliente cliente)
        {
            await _context.AddAsync(cliente);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Cliente>> ListarClientes()
        {
            return await _context.Clientes.Where(c => c.IsAtivo == true)
                .ToListAsync();
        }

        public async Task<Cliente?> PegarClientePorId(Guid id)
        {
            return await _context.Set<Cliente>()
                .FirstOrDefaultAsync(c => c.Id == id);
        }
        public async Task RemoverCliente(Cliente cliente)
        {
            cliente.Desativar();
            _context.Clientes.Update(cliente);
            await _context.SaveChangesAsync();
        }

        public async Task<Cliente?> PegarClientePorIdIncluindoInativos(Guid id)
        {
            return await _context.Clientes
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task ReativarCliente(Cliente cliente)
        {
            cliente.IsAtivo = true;
            cliente.DataAtualizar();

            _context.Clientes.Update(cliente);

            await _context.SaveChangesAsync();
        }
        public async Task<IEnumerable<Cliente>> ListarClientesInativos()
        {
            return await _context.Clientes
                .Where(c => c.IsAtivo == false)
                .ToListAsync();
        }
    }

}
