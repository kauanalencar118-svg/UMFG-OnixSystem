namespace OnixSystem_MVC.Models.ViewModels
{
    public class ItemCarrinhoViewModel
    {
        public Guid ProdutoId { get; set; }

        public string Nome { get; set; } = string.Empty;

        public string Tamanho { get; set; } = string.Empty;

        public decimal Preco { get; set; }

        public int Quantidade { get; set; }

        public int EstoqueDisponivel { get; set; }

        public decimal Subtotal => Preco * Quantidade;
    }
}