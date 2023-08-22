using CashFlow.Domain.DTO.Response.Financial;
using CashFlow.Domain.DTO.Request.Create.Financial;
using CashFlow.Domain.DTO.Request.Update.Financial;
using CashFlow.Domain.DTO.Response;
using System.Collections.Generic;

namespace CashFlow.Domain.Interface.Service.Financial
{
    public interface ICashMovimentService
    {
        DefaultDtoResponse<DailyBalanceGetDtoResponse> ListDailyBalance(string startDay, string endDay);
        DefaultDtoResponse<CashMovimentGetDtoResponse> GetCashMovimentById(long id);
		DefaultDtoResponse<CashMovimentGetDtoResponse> SaveCashMoviment(CashMovimentCreateDtoRequest model);
        DefaultDtoResponse<CashMovimentGetDtoResponse> UpdateCashMoviment(CashMovimentUpdateDtoRequest model);
        DefaultDtoResponse<bool> DeleteCashMoviment(long id);
	}
}