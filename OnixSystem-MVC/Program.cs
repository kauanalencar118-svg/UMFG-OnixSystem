using Microsoft.EntityFrameworkCore;
using OnixSystem_MVC.Data.Contexts;
using OnixSystem_MVC.Data.Repositorios.Clientes;
using OnixSystem_MVC.Data.Repositorios.Estoques;
using OnixSystem_MVC.Data.Repositorios.Fornecedores;
using OnixSystem_MVC.Data.Repositorios.Produtos;
using OnixSystem_MVC.Data.Repositorios.Configuracoes;
using OnixSystem_MVC.Data.Repositorios.Vendas;

namespace OnixSystem_MVC
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddDistributedMemoryCache();

            builder.Services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(30);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });

            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseMySQL(
                    builder.Configuration.GetConnectionString("Database") ?? throw new ArgumentNullException("Nao pode nao")

                )
            );
            builder.Services.AddScoped<IRepositorioProduto, RepositorioProduto>();
            builder.Services.AddScoped<IRepositorioCliente, RepositorioCliente>();
            builder.Services.AddScoped<IRepositorioEstoque, RepositorioEstoque>();
            builder.Services.AddScoped<IRepositorioFornecedor, RepositorioFornecedor>();
            builder.Services.AddScoped<IRepositorioConfiguracaoSistema, RepositorioConfiguracaoSistema>();
            builder.Services.AddScoped<IRepositorioVenda, RepositorioVenda>();
            // Add services to the container.
            builder.Services
                .AddControllersWithViews()
                .AddRazorOptions(options =>
                {
                    options.ViewLocationFormats.Add("/Views/Pages/{1}/{0}.cshtml");
                });
            var app = builder.Build();


            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseRouting();

            app.UseSession();

            app.UseAuthorization();

            app.MapStaticAssets();
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}")
                .WithStaticAssets();

            app.Run();
        }
    }
}
