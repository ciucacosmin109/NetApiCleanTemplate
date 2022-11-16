namespace NetApiCleanTemplate.WebApi.Models;

/// <summary>
/// Base class used by API responses
/// </summary>
public abstract class BaseResponse : BaseMessage
{
    public BaseResponse(Guid correlationId) : base(correlationId) { } 
    public BaseResponse() { }
}
