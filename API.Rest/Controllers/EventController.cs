using Microsoft.AspNetCore.Mvc;

using Model;

namespace App.Controllers;

[ApiController]
[Route("[controller]")]
public class EventController : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> PostEvent([FromBody] Event data)
    {
        try
        {
            Transaction result = await Operation.Bank.Handler(data);
            
            return new ObjectResult(result) { StatusCode = StatusCodes.Status201Created };
        }
        catch
        {
            return NotFound(0);
        }
    }
}
