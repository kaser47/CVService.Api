using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace CVService.Api.WebLayer.Middleware
{
    public sealed class RedirectionMiddleware
    {
        private readonly RequestDelegate _next;

        public RedirectionMiddleware(RequestDelegate next)
        {
            this._next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            //TODO: Tech Test - Instead of creating our own middleware, we would use some other redirection package or setup Swagger redirection
            if (context.Request.Path.Value.ToLower() == "/api/v1/")
            {
                context.Response.Redirect("/swagger/index.html");
                return;
            }
            await _next(context);
        }
    }
}