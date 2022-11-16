using NetApiCleanTemplate.WebApi.Models;

namespace NetApiCleanTemplate.WebApi.Controllers.Identity.Models;

public class AuthenticateResponse : BaseResponse
{
    public bool Result { get; set; } = false;
    public string Token { get; set; } = String.Empty;
    public string Username { get; set; } = String.Empty;
    public bool IsLockedOut { get; set; } = false;
    public bool IsNotAllowed { get; set; } = false;
    public bool RequiresTwoFactor { get; set; } = false;

    public AuthenticateResponse(Guid correlationId) : base(correlationId) { }
    public AuthenticateResponse() { }
}

