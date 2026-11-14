
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnixSystem_MVC.Data.Mapeamentos.Abstracao;
using OnixSystem_MVC.Models.Entidades;

namespace OnixSystem_MVC.Data.Mapeamentos.Entidades
{
    public sealed class VendaMap : AbstracaoEntidadeMap<Venda>
    {
        public override void Configure(EntityTypeBuilder<Venda> builder)
        {
            base.Configure(builder);

            builder.ToTable("VENDA");

            builder.Property(x => x.DataVenda)
                .HasColumnName("DATA_VENDA")
                .IsRequired();

            builder.Property(x => x.Subtotal)
                .HasColumnName("SUBTOTAL")
                .HasColumnType("decimal(10,2)")
                .IsRequired();

            builder.Property(x => x.Desconto)
                .HasColumnName("DESCONTO")
                .HasColumnType("decimal(10,2)")
                .IsRequired();

            builder.Property(x => x.ValorTotal)
                .HasColumnName("VALOR_TOTAL")
                .HasColumnType("decimal(10,2)")
                .IsRequired();

            builder.Property(x => x.FormaPagamento)
                .HasColumnName("FORMA_PAGAMENTO")
                .IsRequired();
        }
    }
}
