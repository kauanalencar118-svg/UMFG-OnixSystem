
using OnixSystem_MVC.Models.Abstracts;
using System.ComponentModel.DataAnnotations;

namespace OnixSystem_MVC.Models.Entidades
{
    public class Produto : AbstracaoEntidade
    {
        [Required(ErrorMessage = "O nome é obrigatório.")]
        public string Nome { get; set; } = string.Empty;
        public string Tamanho { get; set; } = string.Empty;
        public string? Detalhes { get; set; }
        public decimal Preco { get; set; }
        public string CodigoBarras { get; set; } = string.Empty;
    }
}