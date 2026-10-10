using OnixSystem_MVC.Models.Abstracts;

namespace OnixSystem_MVC.Models.Entidades
{
    public class Produto : AbstracaoEntidade
    {
        public string Nome { get; set; } = string.Empty;
        public string? Descricao {  get; set; } = string.Empty;
        public string Preco {  get; set; } = string.Empty;
        public string CodigoBarras { get; set; } = string.Empty;
    }
}
