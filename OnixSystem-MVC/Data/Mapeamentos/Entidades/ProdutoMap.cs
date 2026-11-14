using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnixSystem_MVC.Data.Mapeamentos.Abstracao;
using OnixSystem_MVC.Models.Abstracts;
using OnixSystem_MVC.Models.Entidades;

namespace OnixSystem_MVC.Data.Mapeamentos.Entidades
{
    public sealed class ProdutoMap : AbstracaoEntidadeMap<Produto>
    {
        public override void Configure(EntityTypeBuilder<Produto> builder)
        {
            builder.ToTable("PRODUTO");

            builder
                .Property(x => x.Nome)
                .HasColumnName("NOME")
                .IsRequired();

            builder
             .Property(x => x.Tamanho)
             .HasColumnName("TAMANHO")
             .IsRequired();

            builder
             .Property(x => x.Detalhes)
             .HasColumnName("DETALHES")
             .IsRequired(false);


            builder
                .Property(x => x.Preco)
                .HasColumnName("PRECO")
                .HasColumnType("decimal(10,2)")
                .IsRequired();

            builder
                 .Property(x => x.CodigoBarras)
                 .HasColumnName("CODIGO_BARRAS")
                 .IsRequired();


        }
    }
}
