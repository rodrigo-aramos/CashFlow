using AutoMapper;
using CashFlow.Domain.DTO.Response.Financial;
using CashFlow.Domain.DTO.Request.Create.Financial;
using CashFlow.Domain.DTO.Request.Update.Financial;
using CashFlow.Domain.Entity.Financial;

namespace CashFlow.Domain.Profiles
{
    public class CashMovimentProfile : Profile
    {
        public CashMovimentProfile()
        {
            CreateMap<CashMoviment, CashMovimentGetDtoResponse>().ReverseMap();
            CreateMap<CashMoviment, CashMovimentCreateDtoRequest>().ReverseMap();
            CreateMap<CashMoviment, CashMovimentUpdateDtoRequest>().ReverseMap();
        }
    }
}

