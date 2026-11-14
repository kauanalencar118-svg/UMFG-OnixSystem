using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnixSystem_MVC.Data.Mapeamentos.Abstracao;
using OnixSystem_MVC.Models.Entidades;

namespace OnixSystem_MVC.Data.Mapeamentos.Entidades
{
    public sealed class ClienteMap : AbstracaoEntidadeMap<Cliente>
    {
        public override void Configure(EntityTypeBuilder<Cliente> builder)
        {
            builder.ToTable("CLIENTE");

            builder
                .Property(x => x.Nome)
                .HasColumnName("NOME")
                .IsRequired();

            builder
                .Property(x => x.CPF)
                .HasColumnName("CPF")
                .IsRequired();

            builder
                .Property(x => x.Telefone)
                .HasColumnName("TELEFONE")
                .IsRequired();

            builder
                .Property(x => x.Email)
                .HasColumnName("EMAIL")
                .IsRequired();

            builder
                .Property(x => x.Endereco)
                .HasColumnName("ENDERECO")
                .IsRequired();
        }
    }
}
