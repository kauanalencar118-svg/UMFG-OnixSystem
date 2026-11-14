using OnixSystem_MVC.Models.Abstracts;

namespace OnixSystem_MVC.Models.Entidades
{
    public class Estoque : AbstracaoEntidade
    {
        public Guid ProdutoId { get; set; }

        public Produto? Produto { get; set; }

        public int Quantidade { get; set; }

        public int EstoqueMinimo { get; set; }
    }
}
