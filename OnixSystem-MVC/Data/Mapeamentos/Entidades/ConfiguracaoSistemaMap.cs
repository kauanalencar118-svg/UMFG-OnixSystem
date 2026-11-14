
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnixSystem_MVC.Models.Entidades;

namespace OnixSystem_MVC.Data.Mapeamentos.Entidades
{
    public class ConfiguracaoSistemaMap : IEntityTypeConfiguration<ConfiguracaoSistema>
    {
        public void Configure(EntityTypeBuilder<ConfiguracaoSistema> builder)
        {
            builder.ToTable("CONFIGURACAO_SISTEMA");

            builder.HasKey(c => c.Id);

            builder.Property(c => c.NomeLoja)
                .HasColumnName("NOME_LOJA")
                .HasMaxLength(150)
                .IsRequired();

            builder.Property(c => c.CNPJ)
                .HasColumnName("CNPJ")
                .HasMaxLength(18)
                .IsRequired();

            builder.Property(c => c.Endereco)
                .HasColumnName("ENDERECO")
                .HasMaxLength(250)
                .IsRequired();

            builder.Property(c => c.Telefone)
                .HasColumnName("TELEFONE")
                .HasMaxLength(20)
                .IsRequired();

            builder.Property(c => c.Email)
                .HasColumnName("EMAIL")
                .HasMaxLength(150)
                .IsRequired();
           
            builder.Property(c => c.SenhaHash)
            .HasColumnName("SENHA_HASH")
            .HasMaxLength(255)
            .IsRequired();
        }
    }
}

