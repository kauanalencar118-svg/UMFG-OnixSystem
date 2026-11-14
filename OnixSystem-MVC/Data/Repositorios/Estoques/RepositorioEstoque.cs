using Microsoft.EntityFrameworkCore;
using OnixSystem_MVC.Data.Contexts;
using OnixSystem_MVC.Models.Entidades;

namespace OnixSystem_MVC.Data.Repositorios.Estoques
{
    public class RepositorioEstoque : IRepositorioEstoque
    {
        private readonly AppDbContext _context;

        public RepositorioEstoque(AppDbContext context)
        {
            _context = context;
        }

        public async Task CriarEstoque(Estoque estoque)
        {
            await _context.Estoques.AddAsync(estoque);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Estoque>> ListarEstoques()
        {
            return await _context.Estoques
                .Include(e => e.Produto)
                .Where(e => e.IsAtivo == true)
                .ToListAsync();
        }

        public async Task<Estoque?> PegarEstoquePorId(Guid id)
        {
            return await _context.Estoques
                .Include(e => e.Produto)
                .FirstOrDefaultAsync(e => e.Id == id && e.IsAtivo == true);
        }

        public async Task<Estoque> AtualizarEstoque(Estoque estoque)
        {
            estoque.DataAtualizar();

            _context.Estoques.Update(estoque);
            await _context.SaveChangesAsync();

            return estoque;
        }

        public async Task RemoverEstoque(Estoque estoque)
        {
            estoque.Desativar();

            _context.Estoques.Update(estoque);
            await _context.SaveChangesAsync();
        }


        public async Task<Estoque?> PegarEstoquePorProdutoId(Guid produtoId)
        {
            return await _context.Estoques
                .Include(e => e.Produto)
                .FirstOrDefaultAsync(e =>
                    e.ProdutoId == produtoId &&
                    e.IsAtivo == true);
        }


        public async Task ReativarEstoque(Estoque estoque)
        {
            estoque.IsAtivo = true;
            estoque.DataAtualizar();

            _context.Estoques.Update(estoque);

            await _context.SaveChangesAsync();
        }

        public async Task<List<Estoque>> ListarEstoquesInativos()
        {
            return await _context.Estoques
                .Include(e => e.Produto)
                .Where(e => e.IsAtivo == false)
                .ToListAsync();
        }

        public async Task<Estoque?> PegarEstoquePorIdIncluindoInativos(Guid id)
        {
            return await _context.Estoques
                .Include(e => e.Produto)
                .FirstOrDefaultAsync(e => e.Id == id);
        }
    }
}