using Microsoft.EntityFrameworkCore;
using CashFlow.Infrastructure.Data.Context;
using CashFlow.Domain.Interface.Repository.Financial;
using CashFlow.Infrastructure.Data.Repository.Financial;
using CashFlow.Domain.Enum;
using CashFlow.Domain.Entity.Financial;
using System;
using System.Globalization;

namespace CashFlow.Tests.Initializer;

public class FinancialDataDBInitializer
{
    
    private const string CONST_DATE_TIME_FORMAT = "yyyy/MM/dd HH:mm:ss";

    public void Seed(FinancialDbContext context)
    {
        context.Database.EnsureDeleted();
        context.Database.EnsureCreated();

        context.Moviments.AddRange(
            new CashMoviment {
                CreateAt = GetDate("2023-08-21 09:32:12"),
                Historic = "Recebimento fornecedor cod.: 8993",
                Value = 564.98M,
                Nature = (int)NatureCashMoviment.InFlow
            },
            new CashMoviment {
				CreateAt = GetDate("2023-08-21 11:15:23"),
				Historic = "Pagamento despesa correio",
				Value = 21.73M,
				Nature = (int)NatureCashMoviment.OutFlow
            },
            new CashMoviment {
				CreateAt = GetDate("2023-08-21 15:09:11"),
				Historic = "Pagamento despesa com frete",
				Value = 50.00M,
				Nature = (int)NatureCashMoviment.OutFlow
            },
            new CashMoviment {
				CreateAt = GetDate("2023-08-22 08:22:43"),
				Historic = "Recebimento fornecedor cod.: 12312",
				Value = 1200.00M,
                Nature = (int)NatureCashMoviment.InFlow
            },
            new CashMoviment {
				CreateAt = GetDate("2023-08-22 10:45:02"),
				Historic = "Pagamento despesa com lavagem",
				Value = 90.00M,
				Nature = (int)NatureCashMoviment.OutFlow
            }
        );
        
        context.SaveChanges();
    }

    private DateTime GetDate(string strdate)
    {
        DateTime date = DateTime.Now;
        date = DateTime.TryParseExact(strdate, CONST_DATE_TIME_FORMAT, CultureInfo.InvariantCulture, DateTimeStyles.None, out date) ? date : DateTime.Now;
        return date;
    }
}