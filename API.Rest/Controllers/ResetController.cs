using Microsoft.AspNetCore.Mvc;

namespace App.Controllers;

[ApiController]
[Route("[controller]")]
public class ResetController : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult> PostReset()
    {
        await Db.Handler.Reset();

        return Content("OK");
    }
}
