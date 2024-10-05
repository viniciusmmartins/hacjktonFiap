using Hackaton.Application.Contracts.Services.Email;
using Hackaton.Application.Contracts.Services.Token;
using Hackaton.Infrastructure.Services.Email;
using Hackaton.Infrastructure.Services.Token;
using Microsoft.Extensions.DependencyInjection;

namespace Hackaton.Infrastructure
{
    public static class InjecaoDependencias
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
        {
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IEmailService, EmailService>();

            return services;
        }
    }
}
