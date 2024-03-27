global using LouiseTieDyeStore.Shared;
global using System.Net.Http.Json;
global using LouiseTieDyeStore.Client.Services.ProductService;
global using LouiseTieDyeStore.Client.Services.ProductTypeService;
global using LouiseTieDyeStore.Client.Services.CategoryService;
global using LouiseTieDyeStore.Client.Services.AuthService;
global using LouiseTieDyeStore.Client.Services.CartService;
global using LouiseTieDyeStore.Client.Services.SalesTaxService;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Blazored.LocalStorage;
using Blazored.SessionStorage;

namespace LouiseTieDyeStore.Client
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");
            builder.RootComponents.Add<HeadOutlet>("head::after");

            builder.Services.AddBlazoredLocalStorage();
            builder.Services.AddBlazoredSessionStorage();

            builder.Services.AddHttpClient<PublicClient>(client => client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress));

            builder.Services.AddHttpClient("ServerAPI",
      client => client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress))
    .AddHttpMessageHandler<BaseAddressAuthorizationMessageHandler>();

            builder.Services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>()
              .CreateClient("ServerAPI"));

            builder.Services.AddScoped<IProductService, ProductService>();
            builder.Services.AddScoped<IProductTypeService, ProductTypeService>();
            builder.Services.AddScoped<ICategoryService, CategoryService>();
            builder.Services.AddScoped<IAuthService, AuthService>();
            builder.Services.AddScoped<ICartService, CartService>();
            builder.Services.AddScoped<ISalesTaxService, SalesTaxService>();

            builder.Services.AddOidcAuthentication(options =>
            {
                builder.Configuration.Bind("Auth0", options.ProviderOptions);
                options.ProviderOptions.ResponseType = "code";
                options.ProviderOptions.AdditionalProviderParameters.Add("audience", builder.Configuration["Auth0:Audience"]);
            });

            builder.Services.AddScoped(typeof(AccountClaimsPrincipalFactory<RemoteUserAccount>),
  typeof(CustomAccountFactory));

            await builder.Build().RunAsync();
        }
    }
}
