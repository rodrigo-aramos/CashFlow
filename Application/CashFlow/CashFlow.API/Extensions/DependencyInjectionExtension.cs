using CashFlow.Domain.Interface.Service.Financial;
using CashFlow.Domain.Services.Financial;
using CashFlow.Domain.Interface.Repository.Financial;
using CashFlow.Infrastructure.Data.Repository.Financial;
using Microsoft.Extensions.DependencyInjection;

namespace CashFlow.API.Extensions
{
    public static class DependencyInjectionExtension
    {
        public static void AddDependencies(this IServiceCollection services)
        {
            SetServices(services);
            SetRepositories(services);
        }

        private static void SetServices(IServiceCollection services)
        {
            services.AddScoped<ICashMovimentService, CashMovimentService>();
        }

        private static void SetRepositories(IServiceCollection services)
        {
            services.AddScoped<ICashMovimentRepository, CashMovimentRepository>();
        }
    }
}
