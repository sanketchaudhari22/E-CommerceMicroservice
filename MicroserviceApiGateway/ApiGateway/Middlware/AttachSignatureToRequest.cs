using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;



namespace ApiGateway.Middleware
{
    public class AttachSignatureToRequest
    {
        private readonly RequestDelegate _next;

        public AttachSignatureToRequest(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            httpContext.Request.Headers["Api-Gateway"] = "Signed";
            Console.WriteLine("Header set: " + httpContext.Request.Headers["Api-Gateway"]);
            await _next(httpContext);
        }

    }
}
