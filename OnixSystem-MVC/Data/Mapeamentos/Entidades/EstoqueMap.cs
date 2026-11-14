using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnixSystem_MVC.Data.Mapeamentos.Abstracao;
using OnixSystem_MVC.Models.Entidades;

namespace OnixSystem_MVC.Data.Mapeamentos.Entidades
{
    public sealed class EstoqueMap : AbstracaoEntidadeMap<Estoque>
    {
        public override void Configure(EntityTypeBuilder<Estoque> builder)
        {
            builder.ToTable("ESTOQUE");

            builder
                .Property(x => x.ProdutoId)
                .HasColumnName("PRODUTO_ID")
                .IsRequired();

            builder
                .Property(x => x.Quantidade)
                .HasColumnName("QUANTIDADE")
                .IsRequired();

            builder
                .Property(x => x.EstoqueMinimo)
                .HasColumnName("ESTOQUE_MINIMO")
                .IsRequired();

            builder
                .HasOne(x => x.Produto)
                .WithOne()
                .HasForeignKey<Estoque>(x => x.ProdutoId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
