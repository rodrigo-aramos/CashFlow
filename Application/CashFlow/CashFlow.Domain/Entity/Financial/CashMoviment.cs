using System;
using System.Collections.Generic;
using System.Text;
using CashFlow.Domain.Enum;
using CashFlow.Domain.Entity;

namespace CashFlow.Domain.Entity.Financial;

public class CashMoviment : BaseEntity
{
    public string Historic { get; set; } = "";
    public decimal Value { get; set; } = 0;
    public int Nature { get; set; } = (int)NatureCashMoviment.InFlow;
    public DateTime CreateAt { get; set; } = DateTime.Now;
    
}