using CashFlow.Domain.Entity.Financial;
using System;
using System.Collections.Generic;

namespace CashFlow.Domain.Interface.Repository.Financial
{
    public interface ICashMovimentRepository
    {
        CashMoviment GetCashMovimentById(long id);
        CashMoviment CreateCashMoviment(CashMoviment cashMoviment);
        CashMoviment UpdateCashMoviment(CashMoviment cashMoviment);
        void CreateCashMovimentList(List<CashMoviment> list);
        CashMoviment DeleteCashMoviment(CashMoviment cashMoviment);

        IList<CashMoviment> FindCashMovimentsByBetweenDates(DateTime startDate, DateTime endDate);
	}
}