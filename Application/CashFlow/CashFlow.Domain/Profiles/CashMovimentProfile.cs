using AutoMapper;
using CashFlow.Domain.DTO.ViewModels;
using CashFlow.Domain.DTO.ViewModels.Financial;
using CashFlow.Domain.DTO.Models.Financial;
using CashFlow.Domain.Entity.Financial;

namespace CashFlow.Domain.Profiles
{
    public class CashMovimentProfile : Profile
    {
        public CashMovimentProfile()
        {
            CreateMap<CashMoviment, CashMovimentViewModel>().ReverseMap();
            CreateMap<CashMoviment, CashMovimentModel>().ReverseMap();
        }
    }
}

