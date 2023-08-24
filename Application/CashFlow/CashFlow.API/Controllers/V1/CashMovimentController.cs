using CashFlow.Domain.Interface.Service.Financial;
using CashFlow.Domain.DTO.Models.Financial;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Net;


namespace CashFlow.API.Controllers.V1
{

    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class CashMovimentController : ControllerCustomBase
    {
        private readonly ICashMovimentService _cashMovimentService;

        public CashMovimentController(ICashMovimentService cashMovimentService)
        {
            _cashMovimentService = cashMovimentService;
        }

        [HttpGet]
        [Route("{id}")]
        public IActionResult Get(long? id)
        {
            if (!id.HasValue)
            {
                return StatusCode((int)HttpStatusCode.BadRequest, default);
            }
            return ResponseCustom(_cashMovimentService.GetCashMovimentById(id ?? 0));
        }

        [HttpPost]
        public IActionResult Post([FromBody] CashMovimentModel model)
        {
            return ResponseCustom(_cashMovimentService.SaveCashMoviment(model));
        }

        [HttpPatch]
        public IActionResult Patch([FromBody] CashMovimentModel model)
        {
            return ResponseCustom(_cashMovimentService.UpdateCashMoviment(model));
        }

        [HttpDelete]
        [Route("{id}")]
        public IActionResult Delete(long id)
        {
            return ResponseCustom(_cashMovimentService.DeleteCashMoviment(id));
        }
        
        [HttpGet]
        [Route("balance")]
        public IActionResult GetBalance([FromQuery] string start, [FromQuery] string end)
        {
            return ResponseCustom(_cashMovimentService.ListDailyBalance(start, end));
        }
    }
}