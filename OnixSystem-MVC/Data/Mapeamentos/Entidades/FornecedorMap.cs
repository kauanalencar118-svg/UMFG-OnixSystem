
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnixSystem_MVC.Data.Mapeamentos.Abstracao;
using OnixSystem_MVC.Models.Entidades;

namespace OnixSystem_MVC.Data.Mapeamentos.Entidades
{
    public sealed class FornecedorMap : AbstracaoEntidadeMap<Fornecedor>
    {
        public override void Configure(EntityTypeBuilder<Fornecedor> builder)
        {
            builder.ToTable("FORNECEDOR");

            builder
                .Property(x => x.RazaoSocial)
                .HasColumnName("RAZAO_SOCIAL")
                .IsRequired();

            builder
                .Property(x => x.CNPJ)
                .HasColumnName("CNPJ")
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
