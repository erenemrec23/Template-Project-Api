using Microsoft.AspNetCore.Http;
using QrAssignment.Application.Interfaces;

namespace QrAssignment.Presentation.Middlewares
{
    public sealed class PermissionChangeContextMiddleware
    {
        private readonly RequestDelegate _next;
        public PermissionChangeContextMiddleware(RequestDelegate next) => _next = next;

        public async Task Invoke(HttpContext ctx, IPermissionChangeContext context)
        {
            if (ctx.Request.Headers.TryGetValue("X-Source-Page", out var v))
                context.SourcePage = v.ToString();

            await _next(ctx);
        }
    }
}