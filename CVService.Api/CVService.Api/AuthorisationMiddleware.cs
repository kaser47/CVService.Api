using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;

namespace CVService.Api
{
    public sealed class AuthorisationMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly SecurityModule _securityModule;

        public AuthorisationMiddleware(RequestDelegate next, SecurityModule securityModule)
        {
            this._next = next;
            _securityModule = securityModule;
        }

        public async Task Invoke(HttpContext context)
        {
            //Check if going to maintenance request and then don't authorise.
            if (context.Request.Path.Value.ToLower().Contains("/maintenance/")) { 
                await _next(context);
                return;
            }

            bool authorisationSuccess;
            StringValues requestToken = context.Request.Headers["PrivateAccessToken"];
            if (requestToken.Count == 0)
            {
                requestToken = context.Request.Query["PrivateAccessToken"];
                authorisationSuccess = requestToken.Count != 0 && _securityModule.AuthoriseToken(requestToken);
            }
            else
            {
                authorisationSuccess = _securityModule.AuthoriseToken(requestToken);
            }

            if (authorisationSuccess)
            {
                await _next(context);
            }
            else
            {
                context.Response.Clear();
                context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                await context.Response.WriteAsync("Unauthorized");
            }
        }
    }
}
