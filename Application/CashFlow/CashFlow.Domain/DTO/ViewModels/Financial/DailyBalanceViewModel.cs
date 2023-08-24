using CashFlow.Domain.Enum;
using System;
using System.Collections.Generic;

namespace CashFlow.Domain.DTO.ViewModels.Financial
{
    public class DailyBalanceViewModel
    {
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public decimal InFlowSum { get; set; } = 0L;
        public decimal OutFlowSum { get; set; } = 0L;
        public decimal Balance { get; set; } = 0L;
        public string BalanceNatureDescription { get; set; } = "";
        public int BalanceNature { get; set; } = (int)NatureCashMovimentEnum.InFlow;
        public IList<DailyBalanceItemViewModel> BalanceItems { get; set; }
    }
}