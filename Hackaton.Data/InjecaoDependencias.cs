using Hackaton.Application.Contracts.Repositories;
using Hackaton.Data.DataContext;
using Hackaton.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Hackaton.Data
{
    public static class InjecaoDependencias
    {
        public static IServiceCollection AddDataServices(this IServiceCollection services)
        {

            services.AddDbContext<DatabaseContext>(options =>
                options.UseSqlite("Data source=app.db"));

            services.AddScoped<IUsuarioRepository, UsuarioRepository>();
            services.AddScoped<IAgendaRepository, AgendaRepository>();  

            return services;
        }
    }
}
