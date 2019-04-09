using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyModel;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Swashbuckle.AspNetCore.Swagger;
using WebApplication4.Models;

namespace WebApplication4
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddMvc()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_1)
                .AddJsonOptions(
                    options => options.SerializerSettings
                        .ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
                );

            services.AddDbContext<LibraryContext>(options => options.UseSqlServer(
                "Server=.\\TEW_SQLEXPRESS;Database=Library;Trusted_Connection=True;ConnectRetryCount=0"));

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc(
                    "LibraryAPI",
                    new Info
                    {
                        Title = "Library API",
                        Version = "v1"
                    });
                var basePath = System.AppContext.BaseDirectory;
                var xmlPath = Path.Combine(basePath, "WebApplication4.xml");
                c.IncludeXmlComments(xmlPath);
            });

            services.AddAuthentication(options => {
                options.DefaultAuthenticateScheme = "JwtBearer";
                options.DefaultChallengeScheme = "JwtBearer";
            })
                 .AddJwtBearer("JwtBearer", jwtBearerOptions =>
                 {
                     jwtBearerOptions.TokenValidationParameters = new TokenValidationParameters
                     {
                         ValidateIssuerSigningKey = true,
                         IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes("une phrase secrète d'une longueur > 16 caractères")),
                         ValidateLifetime = true,
                         ValidateAudience = false,
                         ValidateIssuer = false,
                         ClockSkew = TimeSpan.FromMinutes(5)
                     };
                 });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseAuthentication();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint(
                    "/swagger/LibraryAPI/swagger.json",
                    "Library API V1");
            });
            app.UseMvc();
        }
    }
}
