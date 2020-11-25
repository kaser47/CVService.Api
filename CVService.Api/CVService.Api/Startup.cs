using System;
using System.IO;
using System.Reflection;
using CVService.Api.BusinessLogicLayer;
using CVService.Api.BusinessLogicLayer.Abstracts;
using CVService.Api.CommonLayer;
using CVService.Api.DataLayer;
using CVService.Api.DataLayer.Abstracts;
using CVService.Api.DataLayer.Models;
using CVService.Api.DataLayer.Repositories;
using CVService.Api.WebLayer;
using CVService.Api.WebLayer.Abstracts;
using CVService.Api.WebLayer.Filters;
using CVService.Api.WebLayer.Middleware;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace CVService.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            //TODO: Tech Test - In production this wouldn't be using an in-memory DB. Its easy enough to change using the DBContextOptionBuilder, opt.UseSqlServer()
            //It would also be good to implement caching, which could be wrapped around the repositories as a decorator.
            services.AddDbContext<ApiContext>(opt => opt.UseInMemoryDatabase("CVDatabase"));
            
            SetupDependancies(services);

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "CV API",
                    Description = "A simple web service to store and allow manipulation of parts of users' CVs in a structured form." +
                                  " A CV can have multiple skills and company histories. All schemas should be well documented below." +
                                  "Every endpoint needs an authourisation token, it is set by default for the demo.",
                    Contact = new OpenApiContact
                    {
                        Name = "Ash Rhodes",
                        Email = "kaser47@hotmail.com",
                        Url = new Uri("https://www.linkedin.com/in/ash-rhodes-b4168948/"),
                    }
                });

                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.XML";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
                c.OperationFilter<AddRequiredHeaderParameter>();
            });

            services.AddControllers();
        }

        private static void SetupDependancies(IServiceCollection services)
        {
            services.AddTransient<ICvBusinessLogic, CvBusinessLogic>();
            services.AddTransient<ICrudBusinessLogic<Skill>, SkillBusinessLogic>();
            services.AddTransient<ICrudBusinessLogic<CompanyHistory>, CompanyHistoryBusinessLogic>();

            services.AddTransient<ICvRepository, CvRepository>();
            services.AddTransient<IRepositoryBase<Skill>, SkillRepository>();
            services.AddTransient<IRepositoryBase<CompanyHistory>, CompanyHistoryRepository>();
            
            services.AddTransient<SecurityModule>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider serviceProvider)
        {
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.DocumentTitle = "CV API V1";
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "CV API V1");
            });

            var context = serviceProvider.GetService<ApiContext>();

            //TODO: Tech Test - In a real world app this would not be needed, this is just dummy data.
            SeedData.AddTestData(context);

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseMiddleware<RedirectionMiddleware>();
            app.UseMiddleware<AuthorisationMiddleware>();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}

