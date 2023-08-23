using CashFlow.Domain.Interface.Service.Financial;
using CashFlow.Domain.DTO.Request.Create.Financial;
using CashFlow.Domain.DTO.Request.Update.Financial;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Net;


namespace CashFlow.API.Controllers.V1
{

#if !DEBUG
    [Authorize]
#endif
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
#if !DEBUG
        [Authorize(Roles = "Administrator")]
#endif
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
#if !DEBUG
        [Authorize(Roles = "Administrator")]
#endif
        public IActionResult Post([FromBody] CashMovimentCreateDtoRequest model)
        {
            return ResponseCustom(_cashMovimentService.SaveCashMoviment(model));
        }

        [HttpPatch]
#if !DEBUG
        [Authorize(Roles = "Administrator")]
#endif
        public IActionResult Patch([FromBody] CashMovimentUpdateDtoRequest model)
        {
            return ResponseCustom(_cashMovimentService.UpdateCashMoviment(model));
        }

        [HttpDelete]
#if !DEBUG
        [Authorize(Roles = "Administrator")]
#endif
        [Route("{id}")]
        public IActionResult Delete(long id)
        {
            return ResponseCustom(_cashMovimentService.DeleteCashMoviment(id));
        }
        
        [HttpGet]
        [Route("balance")]
#if !DEBUG
        [Authorize(Roles = "Administrator")]
#endif
        public IActionResult GetBalance([FromQuery] string start, [FromQuery] string end)
        {
            return ResponseCustom(_cashMovimentService.ListDailyBalance(start, end));
        }
    }
}