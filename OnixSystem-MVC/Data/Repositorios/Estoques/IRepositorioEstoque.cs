using OnixSystem_MVC.Models.Entidades;

namespace OnixSystem_MVC.Data.Repositorios.Estoques
{
    public interface IRepositorioEstoque
    {
        Task CriarEstoque(Estoque estoque);

        Task<IEnumerable<Estoque>> ListarEstoques();

        Task<Estoque?> PegarEstoquePorId(Guid id);

        Task<Estoque> AtualizarEstoque(Estoque estoque);

        Task RemoverEstoque(Estoque estoque);

        Task<Estoque?> PegarEstoquePorProdutoId(Guid produtoId);

        Task ReativarEstoque(Estoque estoque);

        Task<List<Estoque>> ListarEstoquesInativos();

        Task<Estoque?> PegarEstoquePorIdIncluindoInativos(Guid id);
    }
}