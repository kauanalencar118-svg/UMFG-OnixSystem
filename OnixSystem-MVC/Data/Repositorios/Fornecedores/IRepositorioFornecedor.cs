using OnixSystem_MVC.Models.Entidades;

namespace OnixSystem_MVC.Data.Repositorios.Fornecedores
{
    public interface IRepositorioFornecedor
    {
        Task CriarFornecedor(Fornecedor fornecedor);

        Task<IEnumerable<Fornecedor>> ListarFornecedores();

        Task<Fornecedor?> PegarFornecedorPorId(Guid id);

        Task AtualizarFornecedor(Fornecedor fornecedor);

        Task RemoverFornecedor(Fornecedor fornecedor);
        Task<Fornecedor?> PegarFornecedorPorIdIncluindoInativos(Guid id);

        Task<IEnumerable<Fornecedor>> ListarFornecedoresInativos();

        Task ReativarFornecedor(Fornecedor fornecedor);
    }
}