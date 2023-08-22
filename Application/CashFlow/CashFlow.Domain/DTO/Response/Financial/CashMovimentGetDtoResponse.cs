using CashFlow.Domain.Enum;
using System;

namespace CashFlow.Domain.DTO.Response.Financial
{
    public class CashMovimentGetDtoResponse
    {
        public long Id { get; set; }
        public DateTime CreateAt { get; set; } = DateTime.Now;
        public string Historic { get; set; } = "";
        public decimal Value { get; set; } = 0L;
        public int Nature { get; set; } = (int)NatureCashMoviment.InFlow;
        
    }
}