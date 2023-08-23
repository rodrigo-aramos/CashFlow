using Microsoft.EntityFrameworkCore;
using CashFlow.Infrastructure.Data.Context;
using CashFlow.Domain.Interface.Repository.Financial;
using CashFlow.Infrastructure.Data.Repository.Financial;
using CashFlow.Tests.Initializer;

namespace CashFlow.Tests.Api.Controller;

public class TestCashMovimentController
{
    
    private ICashMovimentRepository repository;
    public static DbContextOptions<FinancialDbContext> dbContextOptions { get; }
    public static string connectionString = "Host=localhost;Port=5432;Pooling=true;Database=dbcashflow;User Id=postgres;Password=5dacb9130fb44377ad519eb2b479741f;sslmode=Prefer;Trust Server Certificate=true";
    
    static TestCashMovimentController()
    {
        dbContextOptions = new DbContextOptionsBuilder<FinancialDbContext>()
            .UseNpgsql(connectionString)
            .Options;
    }

    public TestCashMovimentController ()
    {
        var context = new FinancialDbContext(dbContextOptions);
        FinancialDataDBInitializer db = new FinancialDataDBInitializer();
        db.Seed(context);

        repository = new CashMovimentRepository(context);
    }

    [Fact]
    public void Test1()
    {

    }
}