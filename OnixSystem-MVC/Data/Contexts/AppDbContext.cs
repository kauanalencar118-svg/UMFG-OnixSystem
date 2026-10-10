using Microsoft.EntityFrameworkCore;
using OnixSystem_MVC.Data.Mapeamentos.Entidades;
using OnixSystem_MVC.Models.Entidades;

namespace OnixSystem_MVC.Data.Contexts
{
    public sealed class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }
        public DbSet<Produto> Produtos { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new ProdutoMap());
        }
    }
}