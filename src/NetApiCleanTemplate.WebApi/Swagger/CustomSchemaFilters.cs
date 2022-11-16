using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace NetApiCleanTemplate.WebApi.Swagger;

public class CustomSchemaFilters : ISchemaFilter
{
    public void Apply(OpenApiSchema schema, SchemaFilterContext context)
    {
        // Exclude CorrelationId from BaseMessage class
        var excludeProperties = new[] { "CorrelationId" };

        foreach (var prop in excludeProperties)
            if (schema.Properties.ContainsKey(prop))
                schema.Properties.Remove(prop);
    }
}

