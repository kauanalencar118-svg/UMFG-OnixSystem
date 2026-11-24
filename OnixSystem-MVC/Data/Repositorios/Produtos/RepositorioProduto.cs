using Microsoft.EntityFrameworkCore;
using OnixSystem_MVC.Data.Contexts;
using OnixSystem_MVC.Models.Entidades;

namespace OnixSystem_MVC.Data.Repositorios.Produtos
{
    public class RepositorioProduto : IRepositorioProduto
    {
        private readonly AppDbContext _context;

        public RepositorioProduto(AppDbContext context)
        {
            _context = context;
        }


        public async Task<Produto> AtualizarProduto(Produto produto)
        {
            _context.Produtos.Update(produto);
            await _context.SaveChangesAsync();
            return produto;
        }


        public async Task CriarProduto(Produto produto)
        {
            await _context.AddAsync(produto);
            await _context.SaveChangesAsync();
        }


        public async Task<IEnumerable<Produto>> ListarProdutos()
        {
            return await _context.Produtos
                .Where(p => p.IsAtivo == true)
                .ToListAsync();
        }


        public async Task<Produto?> PegarProdutoPorId(Guid id)
        {
            return await _context.Set<Produto>()
                .FirstOrDefaultAsync(p => p.Id == id);
        }


        public async Task RemoverProduto(Produto produto)
        {
            produto.Desativar();
            _context.Produtos.Update(produto);
            await _context.SaveChangesAsync();
        }


        public async Task<bool> ExisteProdutoComMesmoNomeETamanho(string nome, string tamanho)
        {
            return await _context.Produtos.AnyAsync(p =>
                p.IsAtivo == true &&
                p.Nome.ToLower() == nome.ToLower() &&
                p.Tamanho.ToLower() == tamanho.ToLower());
        }


        public async Task<bool> ExisteCodigoBarras(string codigoBarras)
        {
            return await _context.Produtos.AnyAsync(p =>
                p.IsAtivo == true &&
                p.CodigoBarras == codigoBarras);
        }
    }
}