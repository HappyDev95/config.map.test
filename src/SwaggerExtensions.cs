using System;
using System.Linq;

using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace config.map.test
{
    public class SwaggerDescriptionAttribute : Attribute
    {
        public string Description { get; private set; }

        public SwaggerDescriptionAttribute(string description)
        {
            this.Description = description;
        }
    }

    public class SwaggerDescriptionOperationFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            var description = context.ApiDescription.CustomAttributes()
                .Where(x => x.GetType() == typeof(SwaggerDescriptionAttribute))
                .Select(x => ((SwaggerDescriptionAttribute)x).Description)
                .FirstOrDefault();
            if (!string.IsNullOrWhiteSpace(description)) { operation.Description = description; }
        }
    }
}
