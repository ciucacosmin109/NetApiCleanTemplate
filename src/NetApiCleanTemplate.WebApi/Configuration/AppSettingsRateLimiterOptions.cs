namespace NetApiCleanTemplate.WebApi.Configuration;

public class AppSettingsRateLimiterOptions
{
    // Settings
    public bool Enabled { get; set; }
    public bool LimitByUser { get; set; }
    public string? Type { get; set; }

    // Common (all)
    public int QueueLimit { get; set; }

    // Common (fixed window + sliding window + concurrency)
    public int PermitLimit { get; set; }

    // Common (fixed window + sliding window)
    public int Window { get; set; }

    // Sliding window
    public int SegmentsPerWindow { get; set; }

    // Token bucket
    public int TokenLimit { get; set; }
    public int ReplenishmentPeriod { get; set; }
    public int TokensPerPeriod { get; set; }
    public bool AutoReplenishment { get; set; }

}
