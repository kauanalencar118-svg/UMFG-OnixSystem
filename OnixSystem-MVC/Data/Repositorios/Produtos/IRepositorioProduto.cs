using OnixSystem_MVC.Models.Entidades;

namespace OnixSystem_MVC.Data.Repositorios.Produtos
{
    public interface IRepositorioProduto
    {
        Task CriarProduto(Produto produto);
        Task<IEnumerable<Produto>> ListarProdutos();
        Task<Produto> AtualizarProduto(Produto produto);
        Task<Produto?> PegarProdutoPorId(Guid Id);
        Task RemoverProduto(Produto produto);

    }
}
