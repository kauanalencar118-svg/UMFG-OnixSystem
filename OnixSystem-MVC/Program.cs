using Microsoft.EntityFrameworkCore;
using OnixSystem_MVC.Data.Contexts;
using OnixSystem_MVC.Data.Repositorios.Produtos;

namespace OnixSystem_MVC
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseMySQL(
                    builder.Configuration.GetConnectionString("Database") ?? throw new ArgumentNullException("Nao pode nao")

                )
            );
            builder.Services.AddScoped<IRepositorioProduto, RepositorioProduto>();
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
