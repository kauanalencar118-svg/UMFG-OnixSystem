using OnixSystem_MVC.Models.Abstracts;
using System.ComponentModel.DataAnnotations;

namespace OnixSystem_MVC.Models.Entidades
{
    public class Cliente : AbstracaoEntidade
    {
        [Required(ErrorMessage = "O nome é obrigatório.")]
        public string Nome { get; set; } = string.Empty;

        [Required(ErrorMessage = "O CPF é obrigatório.")]
        [StringLength(11, MinimumLength = 11, ErrorMessage = "O CPF deve conter os 11 números.")]
        [RegularExpression(@"^[0-9]{11}$", ErrorMessage = "O CPF deve conter somente números.")]
        public string CPF { get; set; } = string.Empty;

        [Required(ErrorMessage = "O telefone é obrigatório.")]
        [StringLength(11, MinimumLength = 10, ErrorMessage = "O telefone deve conter entre 10 e 11 números.")]
        [RegularExpression(@"^[0-9]{10,11}$", ErrorMessage = "O telefone deve conter somente números.")]
        public string Telefone { get; set; } = string.Empty;

        [Required(ErrorMessage = "O e-mail é obrigatório.")]
        [EmailAddress(ErrorMessage = "Digite um e-mail válido.")]
        [StringLength(254, ErrorMessage = "O e-mail não pode ultrapassar 254 caracteres.")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "O endereço é obrigatório.")]
        public string Endereco { get; set; } = string.Empty;

        [Required(ErrorMessage = "O número é obrigatório.")]
        public string Numero { get; set; } = string.Empty;
    }
}