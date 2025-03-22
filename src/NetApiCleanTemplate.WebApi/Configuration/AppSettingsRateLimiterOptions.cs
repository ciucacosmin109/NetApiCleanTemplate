namespace NetApiCleanTemplate.WebApi.Configuration;

public class AppSettingsRateLimiterOptions
{
    public bool Enabled { get; set; }
    public bool LimitByUser { get; set; }
    public string? Type { get; set; }

    public int PermitLimit { get; set; }
    public int Window { get; set; }
    public int QueueLimit { get; set; }

    public int SegmentsPerWindow { get; set; }
}
