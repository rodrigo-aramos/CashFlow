using CashFlow.Domain.DTO.Response;
using Microsoft.AspNetCore.Mvc;

namespace CashFlow.API.Controllers
{
    public class ControllerCustomBase : ControllerBase
    {
        protected IActionResult ResponseCustom<T>(DefaultDtoResponse<T> response)
        {
            return StatusCode(response.StatusCode, response);
        }
    }
}
