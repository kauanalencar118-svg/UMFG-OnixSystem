using OnixSystem_MVC.Models.Entidades;

namespace OnixSystem_MVC.Models.ViewModels
{
    public class RelatorioFaturamentoViewModel
    {
        public List<Venda> Vendas { get; set; } = new();

        public DateTime? DataInicial { get; set; }

        public DateTime? DataFinal { get; set; }

        public decimal TotalFaturado { get; set; }

        public decimal TotalDescontos { get; set; }

        public decimal TotalBruto { get; set; }

        public int TotalVendas { get; set; }
    }
}