﻿using System.Collections;
using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi.Models;
using NetApiCleanTemplate.WebApi.Conventions;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace NetApiCleanTemplate.WebApi.Swagger;

public class AuthorizeCheckOperationFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        var hasAuthorize = context.MethodInfo.DeclaringType != null && ( false
            || AddAuthorizeFiltersControllerConvention.NeedsAuthorization(context.MethodInfo.DeclaringType.FullName ?? "") // AddAuthorizeFiltersControllerConvention adds filters to the controller ... filters are not attributes :(
            || context.MethodInfo.DeclaringType.GetCustomAttributes(true).OfType<AuthorizeAttribute>().Any()
            || context.MethodInfo.GetCustomAttributes(true).OfType<AuthorizeAttribute>().Any()
        );

        if (hasAuthorize)
        {
            operation.Responses.Add("401", new OpenApiResponse { Description = "Unauthorized" });
            operation.Responses.Add("403", new OpenApiResponse { Description = "Forbidden" });

            operation.Security = new List<OpenApiSecurityRequirement>
            {
                new OpenApiSecurityRequirement
                {
                    [
                        new OpenApiSecurityScheme {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "oauth2"
                            }
                        }
                    ] = new[] { Registration.AppApiScopeId, Registration.AdminApiScopeId }
                }
            };

        }
    }
}