namespace OnixSystem_MVC.Models.Abstracts
{
    //abstract -> herança, reutilizar
    //sealed -> não pode ser herdade;
    //void -> passa todos os tipos;
    public abstract class AbstracaoEntidade
    {
        public Guid Id { get; set; }
        public DateTime DataCriacao { get; set; } = DateTime.UtcNow;
        public DateTime DataAtualizacao { get; set; } = DateTime.UtcNow;
        public bool IsAtivo { get; set; } = true;

        public void DataAtualizar()
        {
            DataAtualizacao = DateTime.UtcNow;
        }
        public void Desativar()
        {
            IsAtivo = false;
            DataAtualizar();
        }
    }
}
