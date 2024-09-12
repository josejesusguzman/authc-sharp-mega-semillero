using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class KillsController : ControllerBase
{
    [HttpGet]
    public IActionResult getKills()
    {
        return Ok(new List<string> {"Halo Combat evoled", "Halo 2"});
    }
}