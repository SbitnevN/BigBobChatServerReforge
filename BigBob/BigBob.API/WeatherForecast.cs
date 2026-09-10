using BigBob.Core;
using BigBob.DI;

namespace BigBob.API;

public partial class WeatherForecast
{
    [Service]
    public DateOnly Date { get; set; }

    public int TemperatureC { get; set; }

    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);

    public string? Summary { get; set; }

    [Startup]
    public void Start()
    {

    }

    [Startup]
    public void Start3()
    {

    }
}

[Singleton(typeof(Some))]
public partial class Some
{
    [Service]
    public DateOnly Date { get; set; }
}
