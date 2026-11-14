using Microsoft.EntityFrameworkCore;
using OnixSystem_MVC.Data.Contexts;
using OnixSystem_MVC.Models.Entidades;

namespace OnixSystem_MVC.Data.Repositorios.Fornecedores
{
    public class RepositorioFornecedor : IRepositorioFornecedor
    {
        private readonly AppDbContext _context;

        public RepositorioFornecedor(AppDbContext context)
        {
            _context = context;
        }

        public async Task CriarFornecedor(Fornecedor fornecedor)
        {
            await _context.Fornecedores.AddAsync(fornecedor);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Fornecedor>> ListarFornecedores()
        {
            return await _context.Fornecedores
                .Where(f => f.IsAtivo == true)
                .ToListAsync();
        }

        public async Task<Fornecedor?> PegarFornecedorPorId(Guid id)
        {
            return await _context.Fornecedores
                .FirstOrDefaultAsync(f => f.Id == id);
        }

        public async Task AtualizarFornecedor(Fornecedor fornecedor)
        {
            _context.Fornecedores.Update(fornecedor);
            await _context.SaveChangesAsync();
        }

        public async Task RemoverFornecedor(Fornecedor fornecedor)
        {
            fornecedor.IsAtivo = false;

            _context.Fornecedores.Update(fornecedor);
            await _context.SaveChangesAsync();
        }
        public async Task<Fornecedor?> PegarFornecedorPorIdIncluindoInativos(Guid id)
        {
            return await _context.Fornecedores
                .FirstOrDefaultAsync(f => f.Id == id);
        }

        public async Task<IEnumerable<Fornecedor>> ListarFornecedoresInativos()
        {
            return await _context.Fornecedores
                .Where(f => f.IsAtivo == false)
                .ToListAsync();
        }

        public async Task ReativarFornecedor(Fornecedor fornecedor)
        {
            fornecedor.IsAtivo = true;

            _context.Fornecedores.Update(fornecedor);
            await _context.SaveChangesAsync();
        }
    }
}
