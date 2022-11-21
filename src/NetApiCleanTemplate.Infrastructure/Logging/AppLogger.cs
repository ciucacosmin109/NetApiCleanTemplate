using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NetApiCleanTemplate.SharedKernel.Exceptions;
using NetApiCleanTemplate.SharedKernel.Interfaces;

namespace NetApiCleanTemplate.Infrastructure.Logging;

public class AppLogger<T> : IAppLogger<T>
{
    private readonly ILogger<T> _logger;
    public AppLogger(ILoggerFactory loggerFactory)
    {
        _logger = loggerFactory.CreateLogger<T>();
    }

    public void LogInformation(string message, params object[] args)
    {
        _logger.LogInformation(message, args);
    }
    public void LogWarning(string message, params object[] args)
    {
        _logger.LogWarning(message, args);
    }
    public void LogError(string message, params object[] args)
    {
        _logger.LogError(message, args);
    }
    
    public void LogException(Exception ex)
    {
        _logger.LogCritical($"{ex.Message}{Environment.NewLine}{ex.StackTrace}");
    }
    public void LogException(DomainException ex)
    {
        LogError(ex.Message);
    }
}
