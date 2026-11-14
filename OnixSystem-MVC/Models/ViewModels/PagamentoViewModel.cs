
namespace OnixSystem_MVC.Models.ViewModels
{
    public class PagamentoViewModel
    {
        public decimal Subtotal { get; set; }

        public string TipoDesconto { get; set; } = "Valor";

        public decimal ValorDesconto { get; set; }

        public decimal DescontoCalculado { get; set; }

        public decimal ValorTotal { get; set; }

        public string FormaPagamento { get; set; } = string.Empty;

        public decimal ValorRecebido { get; set; }

        public decimal Troco => ValorRecebido - ValorTotal;
    }
}
