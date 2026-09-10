using Microsoft.AspNetCore.Mvc;

namespace BigBob.API.Controllers;

[ApiController]
[Route("[controller]")]
public partial class WeatherForecastController : ControllerBase
{
    private static readonly string[] Summaries =
    [
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    ];
}
