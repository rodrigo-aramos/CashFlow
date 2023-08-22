using System;
using System.Collections.Generic;
using System.Text;

namespace CashFlow.Domain.DTO.Request.Create.Financial
{
    public class CashMovimentCreateDtoRequest
    {
        public long Id { get; set; }
        public DateTime CreateAt { get; set; }
        public string Historic { get; set; }
        public decimal Value { get; set; }
        public int Nature { get; set; }
        
    }
}