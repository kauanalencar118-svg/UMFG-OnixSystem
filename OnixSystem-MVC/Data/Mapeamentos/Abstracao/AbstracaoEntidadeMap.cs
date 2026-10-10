using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnixSystem_MVC.Models.Abstracts;

namespace OnixSystem_MVC.Data.Mapeamentos.Abstracao
{
    public abstract class AbstracaoEntidadeMap<T> : IEntityTypeConfiguration<T> where T : AbstracaoEntidade
    {
        public virtual void Configure(EntityTypeBuilder<T> builder)
        {
            builder
                .Property(x => x.Id)
                .HasColumnName("ID")
                .IsRequired();

            builder
                .HasKey(x => x.Id);

            builder
                .Property(x => x.DataCriacao)
                .HasColumnName("DATA_CRIACAO")
                .IsRequired();

            builder
                .Property(x => x.DataAtualizacao)
                .HasColumnName("DATA_ATUALIZACAO")
                .IsRequired();

            builder
                .Property(x => x.IsAtivo)
                .HasColumnName("IS_ATIVO")
                .IsRequired();
        }
    }
}
