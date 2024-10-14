namespace LongRunningApp.Application.Models;
public class AppLayerSettings
{
    public int DelayEmulationMaxSec { get; set; } = 0;
    public bool UseRedisCache { get; set; }
}
