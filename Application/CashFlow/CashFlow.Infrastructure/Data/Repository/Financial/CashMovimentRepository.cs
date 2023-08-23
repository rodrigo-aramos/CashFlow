using CashFlow.Domain.Entity.Financial;
using CashFlow.Domain.Interface.Repository.Financial;
using CashFlow.Infrastructure.Data.Context;
using System.Collections.Generic;
using System;
using System.Linq;


namespace CashFlow.Infrastructure.Data.Repository.Financial
{
    public class CashMovimentRepository : Repository<CashMoviment>, ICashMovimentRepository
    {
        public CashMovimentRepository(FinancialDbContext context) : base(context) { }
		
        public  CashMoviment GetCashMovimentById(long id)
        {
            return FindById(id);
        }

        public  CashMoviment CreateCashMoviment(CashMoviment cashMoviment)
        {
            Add(cashMoviment);
            Save();
            return cashMoviment;
        }

        public CashMoviment UpdateCashMoviment(CashMoviment cashMoviment)
        {
            Update(cashMoviment);
            Save();
            return cashMoviment;
        }

        public void CreateCashMovimentList(List<CashMoviment> list)
        {
            Add(list);
            Save();
        }

        public CashMoviment DeleteCashMoviment(CashMoviment cashMoviment)
        {
            Remove(cashMoviment);
            Save();
            return cashMoviment;
        }

        public IList<CashMoviment> FindCashMovimentsByBetweenDates(DateTime startDate, DateTime endDate)
        {
            return ReturnAll()
                .Where(x => x.CreateAt >= startDate && x.CreateAt <= endDate)
                .GroupBy(x => new { CreateAt = x.CreateAt, Nature = x.Nature })
                .Select(x => new CashMoviment {
                    CreateAt = new DateTime(x.Key.CreateAt.Year, x.Key.CreateAt.Month, x.Key.CreateAt.Day),
                    Nature = x.Key.Nature,
                    Value = x.Sum(p => p.Value)
                    // Value = x.Sum(p => p.Nature == (int)NatureCashMoviment.InFlow ? p.Value : (p.Value * -1))
                })
                .ToList();
        }
    }
}

