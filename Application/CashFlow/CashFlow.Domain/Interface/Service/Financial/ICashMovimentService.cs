using CashFlow.Domain.DTO.ViewModels;
using CashFlow.Domain.DTO.ViewModels.Financial;
using CashFlow.Domain.DTO.Models.Financial;
using System.Collections.Generic;

namespace CashFlow.Domain.Interface.Service.Financial
{
    public interface ICashMovimentService
    {
        ResultViewModel<DailyBalanceViewModel> ListDailyBalance(string startDay, string endDay);
        ResultViewModel<CashMovimentViewModel> GetCashMovimentById(long id);
		ResultViewModel<CashMovimentViewModel> SaveCashMoviment(CashMovimentModel model);
        ResultViewModel<CashMovimentViewModel> UpdateCashMoviment(CashMovimentModel model);
        ResultViewModel<bool> DeleteCashMoviment(long id);
	}
}