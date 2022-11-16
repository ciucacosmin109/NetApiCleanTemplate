namespace NetApiCleanTemplate.WebApi.Models;

/// <summary>
/// Base class model used by API requests
/// </summary>
public abstract class BaseMessage
{
    /// <summary>
    /// Unique Identifier used by logging
    /// </summary>
    protected Guid _correlationId = Guid.NewGuid();
     
    // Constructors
    public BaseMessage() { }
    public BaseMessage(Guid correlationId) { _correlationId = correlationId; }

    // Methods
    public Guid CorrelationId()
    {
        return _correlationId;
    }
}

