using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using TD1BlazorApp.Models;
using TD1BlazorApp.Services;
using TD1BlazorApp.ViewModels;

namespace TD1BlazorApp
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");
            builder.RootComponents.Add<HeadOutlet>("head::after");

            //builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
            builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri( "https://localhost:7111/api/") });

            builder.Services.AddScoped<IService<Produit>>(provider =>
                new WSService<Produit>(provider.GetRequiredService<HttpClient>(), "Produits"));
            builder.Services.AddScoped<IService<Marque>>(provider =>
                new WSService<Marque>(provider.GetRequiredService<HttpClient>(), "Marques"));
            builder.Services.AddScoped<IService<TypeProduit>>(provider =>
                new WSService<TypeProduit>(provider.GetRequiredService<HttpClient>(), "TypeProduits"));

            builder.Services.AddScoped<ProduitViewModel>();

            builder.Services.AddBlazorBootstrap();

            await builder.Build().RunAsync();
        }
    }
}
