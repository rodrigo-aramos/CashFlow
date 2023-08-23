using CashFlow.Domain.Entity.Financial;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace CashFlow.Infrastructure.Data.Context;

public class FinancialDbContext : DbContext
{
    public FinancialDbContext(DbContextOptions<FinancialDbContext> options) : base(options) 
    {
        
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }

    public virtual DbSet<CashMoviment> Moviments { get; set; }
}