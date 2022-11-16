using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NetApiCleanTemplate.SharedKernel.Exceptions;

namespace NetApiCleanTemplate.WebApi.Controllers.Identity.Exceptions;

public class UserAlreadyExistsException : DomainException {
    public UserAlreadyExistsException() : base("The user already exists") { }
}

