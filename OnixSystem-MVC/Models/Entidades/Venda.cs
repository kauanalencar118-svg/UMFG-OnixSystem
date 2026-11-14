
using OnixSystem_MVC.Models.Abstracts;

namespace OnixSystem_MVC.Models.Entidades
{
    public class Venda : AbstracaoEntidade
    {
        public DateTime DataVenda { get; set; } = DateTime.Now;

        // Valor da venda antes do desconto
        public decimal Subtotal { get; set; }

        // Valor do desconto aplicado em reais
        public decimal Desconto { get; set; }

        // Valor final após aplicar o desconto
        public decimal ValorTotal { get; set; }

        public string FormaPagamento { get; set; } = string.Empty;

        public ICollection<ItemVenda> Itens { get; set; } = new List<ItemVenda>();
    }
}
