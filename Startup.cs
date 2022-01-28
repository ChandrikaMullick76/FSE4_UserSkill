using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserSkillProfiles.BusinessLogicLayer;
using UserSkillProfiles.DBServiceLayer;
using UserSkillProfiles.Models;

namespace UserSkillProfiles
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
            services.Configure<UserSkillProfileDBDatabaseSettings>(Configuration.GetSection(nameof(UserSkillProfileDBDatabaseSettings)));

            services.AddSingleton<IUserSkillProfileDBDatabaseSettings>(sp => sp.GetRequiredService<IOptions<UserSkillProfileDBDatabaseSettings>>().Value);

            services.AddScoped<IUserSkillProfileService_BL, UserSkillProfileService_BL>();

            services.AddCors();
            services.AddControllers();

            services.AddSingleton<UserSkillProfileService>();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "User Skill Profile API",
                    Description = "User profiles create/update using this API",
                    Contact = new OpenApiContact
                    {
                        Name = "Chandrika Mullick",
                        Email = string.Empty
                    }
                });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseExceptionHandler(errorApp =>
            {
                errorApp.Run(async context =>
                {
                    context.Response.StatusCode = 500; // or another Status accordingly to Exception Type
                    context.Response.ContentType = "application/json";

                    var error = context.Features.Get<IExceptionHandlerFeature>();
                    if (error != null)
                    {
                        var ex = error.Error;

                        await context.Response.WriteAsync(new ErrorDto()
                        {
                            Code = 500,
                            Message = ex.Message // or your custom message
                            // other custom data
                        }.ToString(), Encoding.UTF8);
                    }
                });
            });

            app.UseHttpsRedirection();

            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
                c.RoutePrefix = string.Empty;
            });

            app.UseCors(
               builder => builder
                            .AllowAnyOrigin()
                            .AllowAnyMethod()
                            .AllowAnyHeader());
            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
