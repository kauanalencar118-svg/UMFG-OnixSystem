using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnixSystem_MVC.Data.Mapeamentos.Abstracao;
using OnixSystem_MVC.Models.Entidades;

namespace OnixSystem_MVC.Data.Mapeamentos.Entidades
{
    public sealed class ItemVendaMap : AbstracaoEntidadeMap<ItemVenda>
    {
        public override void Configure(EntityTypeBuilder<ItemVenda> builder)
        {
            base.Configure(builder);

            builder.ToTable("ITEM_VENDA");

            builder.Property(x => x.VendaId)
                .HasColumnName("VENDA_ID")
                .IsRequired();

            builder.Property(x => x.ProdutoId)
                .HasColumnName("PRODUTO_ID")
                .IsRequired();

            builder.Property(x => x.Quantidade)
               .HasColumnName("QUANTIDADE")
               .IsRequired();

            builder.Property(x => x.PrecoUnitario)
                .HasColumnName("PRECO_UNITARIO")
                .HasColumnType("decimal(10,2)")
                .IsRequired();

            builder.HasOne(x => x.Venda)
                .WithMany(x => x.Itens)
                .HasForeignKey(x => x.VendaId);

            builder.HasOne(x => x.Produto)
                .WithMany()
                .HasForeignKey(x => x.ProdutoId);
        }
    }

}