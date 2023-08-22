using CashFlow.Domain.Interface.Service.Financial;
using CashFlow.Domain.DTO.Request.Create.Financial;
using CashFlow.Domain.DTO.Request.Update.Financial;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace CashFlow.API.Controllers.V1
{
    // [Authorize]
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
        //[Authorize(Roles = "Administrator")]
        [Route("{id}")]
        public IActionResult Get(long id)
        {
            return ResponseCustom(_cashMovimentService.GetCashMovimentById(id));
        }

        [HttpPost]
        //[Authorize(Roles = "Administrator")]
        public IActionResult Post([FromBody] CashMovimentCreateDtoRequest model)
        {
            return ResponseCustom(_cashMovimentService.SaveCashMoviment(model));
        }

        [HttpPatch]
        //[Authorize(Roles = "Administrator")]
        public IActionResult Patch([FromBody] CashMovimentUpdateDtoRequest model)
        {
            return ResponseCustom(_cashMovimentService.UpdateCashMoviment(model));
        }

        [HttpDelete]
        //[Authorize(Roles = "Administrator")]
        [Route("{id}")]
        public IActionResult Delete(long id)
        {
            return ResponseCustom(_cashMovimentService.DeleteCashMoviment(id));
        }
        
        [HttpGet]
        [Route("balance")]
        //[Authorize(Roles = "Administrator")]
        public IActionResult GetBalance([FromQuery] string start, [FromQuery] string end)
        {
            return ResponseCustom(_cashMovimentService.ListDailyBalance(start, end));
        }

    }
}