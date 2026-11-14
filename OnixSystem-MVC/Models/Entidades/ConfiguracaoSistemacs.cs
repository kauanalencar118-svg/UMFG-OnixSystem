
using System.ComponentModel.DataAnnotations.Schema;
using OnixSystem_MVC.Models.Abstracts;

namespace OnixSystem_MVC.Models.Entidades
{
    public class ConfiguracaoSistema : AbstracaoEntidade
    {
        public string NomeLoja { get; set; } = string.Empty;
        public string CNPJ { get; set; } = string.Empty;
        public string Endereco { get; set; } = string.Empty;
        public string Telefone { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;

        public string SenhaHash { get; set; } = string.Empty;

        [NotMapped]
        public string Senha { get; set; } = string.Empty;
    }
}
