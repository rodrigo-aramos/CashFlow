using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using CashFlow.Data.Infrastructure.Context;

namespace CashFlow.API.Extensions
{
    public static class DataExtension
    {
        public static IApplicationBuilder ApplyMigrations(this IApplicationBuilder app)
        {
            using var scope = app.ApplicationServices.CreateScope();
            var dataContext = scope.ServiceProvider.GetRequiredService<FinancialDbContext>();
            dataContext.Database.Migrate();
            return app;
        }
    }
}
