using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NetApiCleanTemplate.Core.Exceptions;

namespace NetApiCleanTemplate.WebApi.Controllers.Identity.Exceptions;

public class InvalidUserDetailsException : DomainException {
    public InvalidUserDetailsException() : base("Invalid user details") { }
}

