using ContactManager.Common;
using ContactManager.Data;
using ContactManager.Model;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace ContactManager.Services
{
    public static class ServiceExtensions
    {
        public static void ConfigureIdentity(this IServiceCollection services)
        {
            var builder = services.AddIdentityCore<User>(o => { o.User.RequireUniqueEmail = true; });
            builder = new IdentityBuilder(builder.UserType, typeof(IdentityRole), services);
            builder.AddEntityFrameworkStores<ContactDbContext>().AddDefaultTokenProviders();
        }

        public static void ConfigureJWT(this IServiceCollection services, IConfiguration Configuration)
        {
            var jwtSettings = Configuration.GetSection("Jwt");
            var key = jwtSettings.GetSection("key").Value;
            //var key  = Environment.GetEnvironmentVariables("");
            services.AddAuthentication(o =>
            {
                o.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                o.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(o =>
               {
                   o.TokenValidationParameters = new TokenValidationParameters
                   {
                       ValidateIssuer = true,
                       ValidateAudience = false,
                       ValidateLifetime = true,
                       ValidateIssuerSigningKey = true,
                       ValidIssuer = jwtSettings.GetSection("Issuer").Value,
                       IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)),
                       
                   };
               });
        }
        public static void ConfigureExceptionHandler (this IApplicationBuilder app)
        {
            app.UseExceptionHandler(error =>
           {
               error.Run(
                   async context =>
                   {
                       context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                       context.Response.ContentType = "application/json";
                       var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
                       if (contextFeature != null)
                       {
                           await context.Response.WriteAsync(new Error
                           {
                               StatusCode = context.Response.StatusCode,
                               Message = $"Internal Server Error, Please Try Again later : {contextFeature.Error}"
                           }.ToString());
                       }
                   });
           });
        }
    }
}