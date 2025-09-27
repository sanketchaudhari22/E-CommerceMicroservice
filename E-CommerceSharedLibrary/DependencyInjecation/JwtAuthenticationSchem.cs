using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_CommerceSharedLibrary.DependencyInjecation
{
    public static class JwtAuthenticationSchem
    {
        public static IServiceCollection AddJwtAutheticationschem(this IServiceCollection services , IConfiguration configuration )
        {
            // add jwt authentication services here

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)

                .AddJwtBearer("bearer", options =>
                {
                    var key = Encoding.UTF8.GetBytes(configuration.GetSection("Authentication : key").Value!);
                    string issuer = configuration.GetSection("Authentication : issuer").Value!;
                    string audience = configuration.GetSection("Authentication : audience").Value!;

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
                        ClockSkew = TimeSpan.Zero
                    };
                });
            return services;
        }

    }
}
