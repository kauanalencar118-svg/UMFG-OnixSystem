
using OnixSystem_MVC.Models.Abstracts;
using System.ComponentModel.DataAnnotations;

namespace OnixSystem_MVC.Models.Entidades
{
    public class Produto : AbstracaoEntidade
    {
        [Required(ErrorMessage = "O nome do produto é obrigatório.")]
        [StringLength(100, MinimumLength = 2,
            ErrorMessage = "O nome deve ter entre 2 e 100 caracteres.")]
        public string Nome { get; set; } = string.Empty;


        [Required(ErrorMessage = "O tamanho do produto é obrigatório.")]
        [StringLength(20,
            ErrorMessage = "O tamanho não pode ultrapassar 20 caracteres.")]
        public string Tamanho { get; set; } = string.Empty;


        [StringLength(200,
            ErrorMessage = "Os detalhes não podem ultrapassar 200 caracteres.")]
        public string? Detalhes { get; set; }


        [Required(ErrorMessage = "O preço é obrigatório.")]
        [Range(0.01, 99999999.99,
            ErrorMessage = "O preço deve ser maior que zero.")]
        public decimal Preco { get; set; }


        [Required(ErrorMessage = "O código de barras é obrigatório.")]
        [RegularExpression(@"^[0-9]{13}$",
    ErrorMessage = "O código de barras deve conter exatamente 13 números.")]
        public string CodigoBarras { get; set; } = string.Empty;
    }
}
