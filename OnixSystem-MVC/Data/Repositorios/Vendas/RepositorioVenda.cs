
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

            // Calcula o valor total dos produtos antes do desconto
            venda.Subtotal = venda.Itens.Sum(
                item => item.Quantidade * item.PrecoUnitario);

            // Garante que o desconto tenha no máximo duas casas decimais
            venda.Desconto = Math.Round(
                venda.Desconto, 2, MidpointRounding.AwayFromZero);

            // Impede descontos inválidos
            if (venda.Desconto < 0 || venda.Desconto > venda.Subtotal)
            {
                throw new InvalidOperationException(
                    "O desconto informado é inválido.");
            }

            // Calcula o valor final da venda
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
    }
}

