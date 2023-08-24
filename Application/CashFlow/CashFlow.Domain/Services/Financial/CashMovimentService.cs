using CashFlow.Domain.Entity.Financial;
using CashFlow.Domain.Interface.Repository.Financial;
using CashFlow.Domain.Interface.Service.Financial;
using CashFlow.Domain.DTO.ViewModels;
using CashFlow.Domain.DTO.ViewModels.Financial;
using CashFlow.Domain.DTO.Models.Financial;
using Microsoft.Extensions.Logging;
using System.Net;
using AutoMapper;
using System.Collections.Generic;
using System;
using System.Globalization;
using System.Linq;
using CashFlow.Domain.Enum;


namespace CashFlow.Domain.Services.Financial
{
    public class CashMovimentService : ICashMovimentService
    {
        public const string CONST_DATE_FORMAT = "dd/MM/yyyy";
        public const string CONST_DATE_TIME_FORMAT = "dd/MM/yyyy HH:mm:ss";

        private readonly ICashMovimentRepository _cashMovimentRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<CashMovimentService> _logger;

        public CashMovimentService(
            ICashMovimentRepository cashMovimentRepository,
            IMapper mapper,
            ILogger<CashMovimentService> logger
        )
        {
            _cashMovimentRepository = cashMovimentRepository;
            _mapper = mapper;
            _logger = logger;
        }
        
        public ResultViewModel<DailyBalanceViewModel> ListDailyBalance(string startDay, string endDay)
        {
            bool isValidStart = DateTime.TryParseExact(startDay, CONST_DATE_FORMAT, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime startDate);
            bool isValidEnd = DateTime.TryParseExact(endDay, CONST_DATE_FORMAT, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime endDate);

            startDate = isValidStart ? startDate : DateTime.Now;
            endDate = isValidEnd ? endDate : DateTime.Now;
            endDate = DateTime.TryParseExact($"{endDate.ToString(CONST_DATE_FORMAT)} 23:59:59", CONST_DATE_TIME_FORMAT, CultureInfo.InvariantCulture, DateTimeStyles.None, out endDate) ? endDate : DateTime.Now;
            
            var listItems = _cashMovimentRepository.FindCashMovimentsByBetweenDates(startDate, endDate);

            var itemsGrouping = listItems
                .GroupBy(x => x.CreateAt)
                .Select(x => new DailyBalanceItemViewModel
                {
                    BalanceDate = x.Key.ToString(CONST_DATE_FORMAT),
                    SumValueIn = x.Sum(s => s.Nature == (int)NatureCashMovimentEnum.InFlow ? Math.Abs(s.Value) : 0),
                    SumValueOut = x.Sum(s => s.Nature == (int)NatureCashMovimentEnum.OutFlow ? Math.Abs(s.Value) : 0),
                    BalanceValue = x.Sum(s => s.Nature == (int)NatureCashMovimentEnum.InFlow ? s.Value : (s.Value * -1)),
                    Nature = x.Min(s => s.Nature),
                    NatureDescription = x.Min(s => s.Nature == (int)NatureCashMovimentEnum.InFlow ? "Entrada" : "Saída")
                })
                .ToList();
            
            var balance = new DailyBalanceViewModel
            {
                StartDate = startDate.ToString(CONST_DATE_FORMAT),
                EndDate = endDate.ToString(CONST_DATE_FORMAT),
                InFlowSum = itemsGrouping.Sum(s => s.SumValueIn),
                OutFlowSum = itemsGrouping.Sum(s => s.SumValueOut),
                Balance = itemsGrouping.Sum(s => s.SumValueIn - s.SumValueOut),
                BalanceNature = itemsGrouping.Sum(s => s.SumValueIn - s.SumValueOut) >= 0 ? (int)NatureCashMovimentEnum.InFlow : (int)NatureCashMovimentEnum.OutFlow,
                BalanceNatureDescription = itemsGrouping.Sum(s => s.SumValueIn - s.SumValueOut) >= 0 ? "Entrada" : "Saída",
                BalanceItems = itemsGrouping
            };
            
            return new ResultViewModel<DailyBalanceViewModel>(HttpStatusCode.OK, balance);
        }

        public ResultViewModel<CashMovimentViewModel> GetCashMovimentById(long id)
        {
            try
            {
                var cashMoviment = _cashMovimentRepository.GetCashMovimentById(id);

                if (cashMoviment == null)
                {
                    _logger.LogError("CashMoviment was not found.");
                    return new ResultViewModel<CashMovimentViewModel>(HttpStatusCode.NotFound, null);
                }

                var result = _mapper.Map<CashMovimentViewModel>(cashMoviment);

                _logger.LogInformation("CashMoviment found.", result);

                return new ResultViewModel<CashMovimentViewModel>(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex.Message, ex);
                return new ResultViewModel<CashMovimentViewModel>(HttpStatusCode.InternalServerError, null);
            }
        }

        public ResultViewModel<CashMovimentViewModel> SaveCashMoviment(CashMovimentModel model)
        {
            try
            {
                var cashMoviment = new CashMoviment()
                {
                    CreateAt = model.CreateAt,
                    Historic = model.Historic,
                    Value = model.Value,
                    Nature = model.Nature,
                };

                cashMoviment = _cashMovimentRepository.CreateCashMoviment(cashMoviment);

                var result = _mapper.Map<CashMovimentViewModel>(cashMoviment);

                _logger.LogInformation("CashMoviment created.", result);

                return new ResultViewModel<CashMovimentViewModel>(HttpStatusCode.Created, result);
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex.Message, ex);
                return new ResultViewModel<CashMovimentViewModel>(HttpStatusCode.InternalServerError, null);
            }
        }

        public ResultViewModel<CashMovimentViewModel> UpdateCashMoviment(CashMovimentModel model)
        {
            try
            {
                var current = _cashMovimentRepository.GetCashMovimentById(model.Id);

                if (current == null)
                {
                    _logger.LogError("CashMoviment was not found.");
                    return new ResultViewModel<CashMovimentViewModel>(HttpStatusCode.BadRequest, null);
                }

                var cashMoviment = new CashMoviment()
                {
                    Id = model.Id,
                    CreateAt = model.CreateAt,
                    Historic = model.Historic,
                    Value = model.Value,
                    Nature = model.Nature,

                };

                cashMoviment = _cashMovimentRepository.UpdateCashMoviment(cashMoviment);
                var result = _mapper.Map<CashMovimentViewModel>(cashMoviment);

                _logger.LogInformation("CashMoviment updated successfully.", result);

                return new ResultViewModel<CashMovimentViewModel>(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex.Message, ex);
                return new ResultViewModel<CashMovimentViewModel>(HttpStatusCode.InternalServerError, null);
            }
        }

        public ResultViewModel<bool> DeleteCashMoviment(long id)
        {
            try
            {
                var cashMoviment = _cashMovimentRepository.GetCashMovimentById(id);

                if (cashMoviment == null)
                {
                    _logger.LogError("CashMoviment was not found.");
                    return new ResultViewModel<bool>(HttpStatusCode.BadRequest, false);
                }

                _cashMovimentRepository.DeleteCashMoviment(cashMoviment);

                _logger.LogInformation("CashMoviment was deleted successfully.");

                return new ResultViewModel<bool>(HttpStatusCode.OK, true);
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex.Message, ex);
                return new ResultViewModel<bool>(HttpStatusCode.InternalServerError, false);
            }
        }
    }
}