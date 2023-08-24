using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Moq;
using FluentAssertions;
using Xunit;
using Xunit.Abstractions;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc;
using System;
using Microsoft.Extensions.DependencyInjection;
using CashFlow.Infrastructure.Data.Context;
using CashFlow.Domain.Interface.Repository.Financial;
using CashFlow.Infrastructure.Data.Repository.Financial;
using CashFlow.Tests.Initializer;
using CashFlow.Domain.Services.Financial;
using CashFlow.Domain.Profiles;
using CashFlow.Domain.Interface.Service.Financial;
using CashFlow.API.Controllers.V1;
using CashFlow.Domain.DTO.Response;
using CashFlow.Domain.DTO.Response.Financial;

namespace CashFlow.Tests.Api.Controller;

public class TestCashMovimentController
{
    private ICashMovimentRepository repository;
    private ICashMovimentService service;

    public static DbContextOptions<FinancialDbContext> dbContextOptions { get; }
    public static string connectionString = "Host=localhost;Port=5432;Pooling=true;Database=dbcashflow;User Id=postgres;Password=5dacb9130fb44377ad519eb2b479741f;sslmode=Prefer;Trust Server Certificate=true";
    
    static TestCashMovimentController()
    {
        dbContextOptions = new DbContextOptionsBuilder<FinancialDbContext>()
            .UseNpgsql(connectionString)
            .Options;
    }

    private ITestOutputHelper OutputHelper { get; }

    private const string CONST_DATE_FORMAT = "dd/MM/yyyy";

    public TestCashMovimentController (ITestOutputHelper outputHelper)
    {
        OutputHelper = outputHelper;
        var context = new FinancialDbContext(dbContextOptions);
        FinancialDataDBInitializer db = new FinancialDataDBInitializer();
        db.Seed(context);
        
        repository = new CashMovimentRepository(context);
        
        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile(new CashMovimentProfile());
        });
        var mapper = config.CreateMapper();

        var services = new ServiceCollection()
            .AddLogging((builder) => builder.AddConsole())
            .AddDbContext<FinancialDbContext>(
                options => options.UseNpgsql(connectionString)
            )
            .AddSingleton(mapper)
            .AddScoped<ICashMovimentRepository, CashMovimentRepository>()
            .AddScoped<ICashMovimentService, CashMovimentService>();
        
        service = services
            .BuildServiceProvider()
            .GetRequiredService<ICashMovimentService>();
    }

    [Fact]
    public void Task_GetCashMovimentById_Return_OkResult()
    {
        //Arrange
        var controller = new CashMovimentController(service);
        long postId = 2;

        //Act
        var actionResult = controller.Get(postId);

        //Assert
        var result = (actionResult as ObjectResult);
        Assert.NotNull(result);
        Assert.Equal(result.StatusCode, 200);
    }

    [Fact]
    public void Task_GetCashMovimentById_Return_NotFoundResult()
    {
        //Arrange
        var controller = new CashMovimentController(service);
        long postId = 9999;

        //Act
        var actionResult = controller.Get(postId);

        //Assert
        var result = (actionResult as ObjectResult);
        Assert.NotNull(result);
        Assert.Equal(result.StatusCode, 404);
    }
    
    [Fact]
    public void GetCashMovimentById_Return_BadRequestResult()
    {
        //Arrange
        var controller = new CashMovimentController(service);
        long? postId = null;

        //Act
        var actionResult = controller.Get(postId);

        //Assert
        var result = (actionResult as ObjectResult);
        Assert.NotNull(result);
        Assert.Equal(result.StatusCode, 400);
    }

    [Fact]
    public void GetCashMovimentById_MatchResult()
    {
        //Arrange
        var controller = new CashMovimentController(service);
        long? postId = 1;

        //Act
        var actionResult = controller.Get(postId);

        //Assert
        var result = (actionResult as ObjectResult);
        Assert.Equal(result.StatusCode, 200);

        var dtoResponse = result.Value.Should().BeAssignableTo<ResultViewModel<CashMovimentViewModel>>().Subject;

        Assert.Equal("Recebimento fornecedor cod.: 8993", dtoResponse.Result.Historic);
    }
    
    [Fact]
    public void Task_GetBalance_Return_OkResult()
    {
        //Arrange
        var controller = new CashMovimentController(service);

        //Act
        string start, end;
        start = "21/08/2023";
        end = "22/08/2023";
        var actionResult = controller.GetBalance(start, end);

        //Assert
        var result = (actionResult as ObjectResult);
        Assert.NotNull(result);
        Assert.Equal(result.StatusCode, 200);
    }

    [Fact]
    public void Task_GetBalance_Return_MatchResult()
    {
        //Arrange
        var controller = new CashMovimentController(service);

        //Act
        string start, end;
        start = "21/08/2023";
        end = "22/08/2023";
        var actionResult = controller.GetBalance(start, end);

        //Assert
        var result = (actionResult as ObjectResult);
        Assert.Equal(result.StatusCode, 200);

        var dtoResponse = result.Value.Should().BeAssignableTo<ResultViewModel<DailyBalanceViewModel>>().Subject;

        Assert.Equal("21/08/2023", dtoResponse.Result.StartDate);
    }
    
    [Fact]
    public void Task_GetBalance_Empty_Return_MatchResult()
    {
        //Arrange
        var controller = new CashMovimentController(service);

        //Act
        string start, end;
        start = "";
        end = "";
        var actionResult = controller.GetBalance(start, end);

        //Assert
        var result = (actionResult as ObjectResult);
        Assert.Equal(result.StatusCode, 200);

        var dtoResponse = result.Value.Should().BeAssignableTo<ResultViewModel<DailyBalanceViewModel>>().Subject;

        Assert.Equal(DateTime.Now.ToString(CONST_DATE_FORMAT), dtoResponse.Result.StartDate);
    }
}