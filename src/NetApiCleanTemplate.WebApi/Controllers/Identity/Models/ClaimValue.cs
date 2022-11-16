namespace NetApiCleanTemplate.WebApi.Controllers.Identity.Models;

public class ClaimValue
{
    public string Type { get; set; }
    public string Value { get; set; }

    public ClaimValue(string type, string value)
    {
        Type = type;
        Value = value;
    }

}

