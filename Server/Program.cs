global using LouiseTieDyeStore.Shared;
global using Microsoft.EntityFrameworkCore;
global using LouiseTieDyeStore.Server.Data;
global using LouiseTieDyeStore.Server.Services.ProductService;
global using LouiseTieDyeStore.Server.Services.ProductTypeService;
global using LouiseTieDyeStore.Server.Services.CategoryService;
global using LouiseTieDyeStore.Server.Services.AuthService;
global using LouiseTieDyeStore.Server.Services.CartService;
global using LouiseTieDyeStore.Server.Services.ShippingService;
global using LouiseTieDyeStore.Server.Services.SalesTaxService;
global using LouiseTieDyeStore.Server.Services.PaymentService;
global using LouiseTieDyeStore.Server.Services.OrderService;
global using LouiseTieDyeStore.Server.Services.MessageService;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.OpenApi.Models;
using Stripe;
using Microsoft.EntityFrameworkCore.Migrations;
using LouiseTieDyeStore.Server.PostgreSQL;

namespace LouiseTieDyeStore
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddDbContext<DataContext>(options =>

                //options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));

                options.UseNpgsql(Environment.GetEnvironmentVariable("ConnectionStrings_RenderConnection") ?? builder.Configuration.GetConnectionString("RenderConnection"),                
                x => x.MigrationsHistoryTable("__efmigrationshistory", "public"))
                .ReplaceService<IHistoryRepository, LoweredCaseMigrationHistoryRepository>());


            builder.Services.AddControllersWithViews();
            builder.Services.AddRazorPages();

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(option =>
            {
                option.SwaggerDoc("v1", new OpenApiInfo { Title = "Demo API", Version = "v1" });
                option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please enter a valid token",
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JWT",
                    Scheme = "Bearer"
                });
                option.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type=ReferenceType.SecurityScheme,
                                Id="Bearer"
                            }
                        },
                        new string[]{}
                    }
                });
            });

            builder.Services.AddScoped<IProductService, Server.Services.ProductService.ProductService>();
            builder.Services.AddScoped<IProductTypeService, ProductTypeService>();
            builder.Services.AddScoped<ICategoryService, CategoryService>();
            builder.Services.AddScoped<IAuthService, AuthService>();
            builder.Services.AddScoped<ICartService, CartService>();
            builder.Services.AddScoped<IShippingService, FedExShippingService>();
            builder.Services.AddScoped<ISalesTaxService, SalesTaxService>();
            builder.Services.AddScoped<IPaymentService, SquareService>();
            builder.Services.AddScoped<IOrderService, OrderService>();
            builder.Services.AddScoped<IMessageService, MessageService>();

            builder.Services.AddHttpClient();
            builder.Services.AddHttpClient("fedExApi", client =>
            {
                client.BaseAddress = new Uri("https://apis.fedex.com");
                client.DefaultRequestHeaders.Add("X-locale", "en_US");
            });

            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, c =>
                {
                    c.Authority = $"{builder.Configuration["Auth0:Domain"]}";
                    c.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                    {
                        ValidAudience = builder.Configuration["Auth0:Audience"],
                        ValidIssuer = $"{builder.Configuration["Auth0:Domain"]}"
                    };
                });

            builder.Services.AddHttpContextAccessor();

            var app = builder.Build();

            app.UseSwaggerUI();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseWebAssemblyDebugging();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseSwagger();
            //app.UseHttpsRedirection(); // TODO: Uncomment

            app.UseBlazorFrameworkFiles();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapRazorPages();
            app.MapControllers();
            app.MapFallbackToFile("index.html");

            app.Run();
        }
    }
}
