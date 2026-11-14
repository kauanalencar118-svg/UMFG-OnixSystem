using OnixSystem_MVC.Models.Abstracts;

namespace OnixSystem_MVC.Models.Entidades
{
    public class Fornecedor : AbstracaoEntidade
    {
        public string RazaoSocial { get; set; } = string.Empty;
        public string CNPJ { get; set; } = string.Empty;
        public string Telefone { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Endereco { get; set; } = string.Empty;

    }

}