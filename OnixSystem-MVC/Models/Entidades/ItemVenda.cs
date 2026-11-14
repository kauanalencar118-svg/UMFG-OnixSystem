
using OnixSystem_MVC.Models.Abstracts;

namespace OnixSystem_MVC.Models.Entidades
{
    public class ItemVenda : AbstracaoEntidade
    {
        public Guid VendaId { get; set; }

        public Venda? Venda { get; set; }

        public Guid ProdutoId { get; set; }

        public Produto? Produto { get; set; }

        public int Quantidade { get; set; }

        public decimal PrecoUnitario { get; set; }
    }
}
