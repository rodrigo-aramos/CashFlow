using CashFlow.Domain.DTO.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace CashFlow.API.Controllers
{
    public class ControllerCustomBase : ControllerBase
    {
        protected IActionResult ResponseCustom<T>(ResultViewModel<T> response)
        {
            return StatusCode(response.StatusCode, response);
        }
    }
}
