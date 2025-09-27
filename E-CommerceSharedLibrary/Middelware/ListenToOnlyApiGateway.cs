using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace E_CommerceSharedLibrary.Middleware
{
    public class ListenToOnlyApiGateway
    {
        private readonly RequestDelegate _next;

        public ListenToOnlyApiGateway(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // Header cha value ghe
            var signedHeader = context.Request.Headers["Api-Gateway"].FirstOrDefault();

            if (string.IsNullOrEmpty(signedHeader))
            {
                context.Response.StatusCode = StatusCodes.Status503ServiceUnavailable;
                await context.Response.WriteAsync("Service Unavailable: Missing Api-Gateway header");
                return;
            }

            // jar header milala, pipeline madhe pudhe pathav
            await _next(context);
        }
    }
}
