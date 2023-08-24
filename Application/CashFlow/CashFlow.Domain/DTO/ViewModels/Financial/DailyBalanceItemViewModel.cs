using CashFlow.Domain.Enum;
using System;

namespace CashFlow.Domain.DTO.ViewModels.Financial
{
    public class DailyBalanceItemViewModel
    {
        public string BalanceDate { get; set; } = "";
        public decimal SumValueIn { get; set; } = 0L;
        public decimal SumValueOut { get; set; } = 0L;
        public decimal BalanceValue { get; set; } = 0L;
        public int Nature { get; set; } = (int)NatureCashMovimentEnum.InFlow;
        public string NatureDescription { get; set; } = "";
        
    }
}