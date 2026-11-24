using OnixSystem_MVC.Models.Entidades;

namespace OnixSystem_MVC.Models.ViewModels
{
    public class RelatorioEstoqueViewModel
    {
        public List<Estoque> Estoques { get; set; } = new();

        public bool EstoqueBaixo { get; set; }

        public int TotalProdutos { get; set; }

        public int TotalUnidades { get; set; }
    }
}