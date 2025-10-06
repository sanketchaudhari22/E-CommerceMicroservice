using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Text;

namespace E_CommerceSharedLibrary.DependencyInjecation
{
    public static class JwtAuthenticationSchem
    {
        public static IServiceCollection AddJwtAutheticationschem(this IServiceCollection services, IConfiguration configuration)
        {
            var key = Encoding.UTF8.GetBytes(configuration["Authentication:Key"]!);
            var issuer = configuration["Authentication:Issuer"]!;
            var audience = configuration["Authentication:Audience"]!;

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.RequireHttpsMetadata = false;
                    options.SaveToken = true;
                    options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(key),
                        ValidateIssuer = true,
                        ValidIssuer = issuer,
                        ValidateAudience = true,
                        ValidAudience = audience,
                        RequireExpirationTime = true,
                        ValidateLifetime = true,
                        ClockSkew = System.TimeSpan.Zero
                    };
                });

            return services;
        }
    }
}
