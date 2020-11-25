using System.Collections.Generic;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace CVService.Api.WebLayer.Filters
{
    public class AddRequiredHeaderParameter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            if (operation.Parameters == null)
                operation.Parameters = new List<OpenApiParameter>();

            operation.Parameters.Add(new OpenApiParameter
            {
                Name = "PrivateAccessToken",
                In = ParameterLocation.Header,
                Required = true,
                Example = new OpenApiString("84157CEC-965E-4680-BDD8-AFFD81AD0D2A", false)
            });
        }
    }
}