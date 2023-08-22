using CashFlow.Domain.Enum;
using System;

namespace CashFlow.Domain.DTO.Response.Financial
{
    public class DailyBalanceItemGetDtoResponse
    {
        public string BalanceDate { get; set; } = "";
        public decimal SumValueIn { get; set; } = 0L;
        public decimal SumValueOut { get; set; } = 0L;
        public decimal BalanceValue { get; set; } = 0L;
        public int Nature { get; set; } = (int)NatureCashMoviment.InFlow;
        public string NatureDescription { get; set; } = "";
        
    }
}