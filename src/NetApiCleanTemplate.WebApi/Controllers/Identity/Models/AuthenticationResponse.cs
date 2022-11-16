using NetApiCleanTemplate.WebApi.Models;

namespace NetApiCleanTemplate.WebApi.Controllers.Identity.Models;

public class RegistrationResponse : BaseResponse
{
    public bool Result { get; set; } = false; 
    public bool RequiresEmailConfirmation { get; set; } = false;

    public RegistrationResponse(Guid correlationId) : base(correlationId) { }
    public RegistrationResponse() { }
}

