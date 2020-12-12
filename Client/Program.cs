using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Apoyo.Services;
using System.Threading.Tasks;
using System.Net.Http;
using System;
using Apoyo.Client.Auth;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Options;
using Toolbelt.Blazor.Extensions.DependencyInjection;
using Apoyo.Shared.Models;

namespace Apoyo.Client
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("app");
            builder.Services.AddSingleton(new HttpClient 
            {
                BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) 
            });
            builder.Services.AddAuthorizationCore();
            builder.Services.AddOptions();
            builder.Services.AddScoped<JWTAuthenticationProvider>();
            builder.Services.AddScoped<AuthenticationStateProvider, JWTAuthenticationProvider>(provider => provider.GetRequiredService<JWTAuthenticationProvider>());
            builder.Services.AddScoped<ILoginServices, JWTAuthenticationProvider>(provider => provider.GetRequiredService<JWTAuthenticationProvider>());
            builder.Services.AddI18nText(options => options.PersistanceLevel = Toolbelt.Blazor.I18nText.PersistanceLevel.SessionAndLocal);
            builder.Services.AddScoped<CuentaService>();
            builder.Services.AddScoped<MateriaService>();
            builder.Services.AddScoped<GruposService>();
            builder.Services.AddScoped<ActividadesService>();
            builder.Services.AddScoped<WeatherForecastService>();

            builder.Services.AddTelerikBlazor();

            await builder.Build().RunAsync();
        }
    }
}
