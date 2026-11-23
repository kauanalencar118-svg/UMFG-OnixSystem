
using Microsoft.EntityFrameworkCore;
using OnixSystem_MVC.Data.Contexts;
using OnixSystem_MVC.Models.Entidades;

namespace OnixSystem_MVC.Data.Repositorios.Vendas
{
    public class RepositorioVenda : IRepositorioVenda
    {
        private readonly AppDbContext _context;

        public RepositorioVenda(AppDbContext context)
        {
            _context = context;
        }

        public async Task CriarVenda(Venda venda)
        {
            await using var transaction =
                await _context.Database.BeginTransactionAsync();

            foreach (var item in venda.Itens)
            {
                if (item.Quantidade <= 0)
                    throw new InvalidOperationException(
                        "A quantidade deve ser maior que zero.");

                var produto = await _context.Produtos
                    .FirstOrDefaultAsync(p =>
                        p.Id == item.ProdutoId && p.IsAtivo);

                if (produto == null)
                    throw new InvalidOperationException(
                        "Produto não encontrado ou inativo.");

                var estoque = await _context.Estoques
                    .FirstOrDefaultAsync(e =>
                        e.ProdutoId == item.ProdutoId && e.IsAtivo);

                if (estoque == null || estoque.Quantidade < item.Quantidade)
                    throw new InvalidOperationException(
                        $"Estoque insuficiente para o produto {produto.Nome}.");

                item.PrecoUnitario = produto.Preco;

                estoque.Quantidade -= item.Quantidade;
            }

            // Calcula subtotal
            venda.Subtotal = venda.Itens.Sum(
                item => item.Quantidade * item.PrecoUnitario);

            // Arredonda desconto
            venda.Desconto = Math.Round(
                venda.Desconto, 2, MidpointRounding.AwayFromZero);

            // Valida desconto
            if (venda.Desconto < 0 || venda.Desconto > venda.Subtotal)
            {
                throw new InvalidOperationException(
                    "O desconto informado é inválido.");
            }

            // Calcula total
            venda.ValorTotal = venda.Subtotal - venda.Desconto;

            await _context.Vendas.AddAsync(venda);

            await _context.SaveChangesAsync();

            await transaction.CommitAsync();
        }

        public async Task<IEnumerable<Venda>> ListarVendas()
        {
            return await _context.Vendas
                .Include(v => v.Itens)
                    .ThenInclude(i => i.Produto)
                .Where(v => v.IsAtivo)
                .ToListAsync();
        }

        public async Task<Venda?> PegarVendaPorId(Guid id)
        {
            return await _context.Vendas
                .Include(v => v.Itens)
                    .ThenInclude(i => i.Produto)
                .FirstOrDefaultAsync(v => v.Id == id);
        }

        // EXCLUIR VENDA (SOFT DELETE E DEVOLUÇÃO DE ESTOQUE)
        public async Task<bool> ExcluirVenda(Guid id)
        {
            await using var transaction =
                await _context.Database.BeginTransactionAsync();

            var venda = await _context.Vendas
                .Include(v => v.Itens)
                .FirstOrDefaultAsync(v => v.Id == id && v.IsAtivo);

            if (venda == null)
                return false;

            foreach (var item in venda.Itens)
            {
                var estoque = await _context.Estoques
                    .FirstOrDefaultAsync(e => e.ProdutoId == item.ProdutoId);

                if (estoque == null)
                {
                    throw new InvalidOperationException(
                        $"Estoque não encontrado para o produto ID {item.ProdutoId}.");
                }

                // Devolve a quantidade ao estoque
                estoque.Quantidade += item.Quantidade;
            }

            // Desativa a venda
            venda.IsAtivo = false;

            await _context.SaveChangesAsync();

            await transaction.CommitAsync();

            return true;
        }
    }
}
