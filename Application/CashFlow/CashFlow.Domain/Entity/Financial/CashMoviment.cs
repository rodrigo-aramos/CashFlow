using System;
using CashFlow.Domain.Enum;

namespace CashFlow.Domain.Entity.Financial;

public class CashMoviment : BaseEntity
{
    public string Historic { get; set; } = "";
    public decimal Value { get; set; } = 0;
    public int Nature { get; set; } = (int)NatureCashMovimentEnum.InFlow;
    public DateTime CreateAt { get; set; } = DateTime.Now;
    
}