using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeworkApp.Bll.Services
{
    public class RateLimitingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly RateLimiterService _rateLimiter;

        public RateLimitingMiddleware(RequestDelegate next, RateLimiterService rateLimiter)
        {
            _next = next;
            _rateLimiter = rateLimiter;
        }

        public async Task Invoke(HttpContext context)
        {
            var ipAddress = context.Request.Headers["X-R256-USER-IP"].ToString();

            if (!await _rateLimiter.IsRequestAllowed(ipAddress))
            {
                context.Response.StatusCode = 429;
                await context.Response.WriteAsync("Rate limit exceeded.");
                return;
            }

            await _next(context);
        }
    }
}
