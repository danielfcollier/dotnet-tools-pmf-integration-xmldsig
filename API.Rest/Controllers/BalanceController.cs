using Microsoft.AspNetCore.Mvc;

using Model;

namespace App.Controllers;

[ApiController]
[Route("[controller]")]
public class BalanceController : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult> GetBalance([FromQuery(Name = "account_id")] string id)
    {
        Account? data = await Db.Handler.Read(id);
        var result = $"{data?.Balance}" ?? "Not Found";

        if (data is null)
        {
            return NotFound(0);
        }

        return Ok(data.Balance);
    }
}
