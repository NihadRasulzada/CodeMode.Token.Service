using CodeMode.Token.Service.Configurations;
using CodeMode.Token.Service.Services.Abstract;
using CodeMode.Token.Service.Services.Concreate;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Security.Claims;

namespace CodeMode.Token.Service.DependencyResolver
{
    public static class TokenRegistration
    {
        public static void AddTokenService(this WebApplicationBuilder builder,IConfigurationSection tokenOption)
        {
            builder.Services.AddScoped(typeof(ITokenService<>), typeof(TokenService<>));
            builder.Services.Configure<CustomTokenOptions>(tokenOption);
        }
    }
}
