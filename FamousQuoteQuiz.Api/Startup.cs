using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Threading.Tasks;
using FamousQuoteQuiz.Api.Middlewares;
using FamousQuoteQuiz.Api.Models;
using FamousQuoteQuiz.Api.Models.Common;
using FamousQuoteQuiz.Api.Models.Common.Contracts;
using FamousQuoteQuiz.Api.Models.ValidationModels;
using FamousQuoteQuiz.Api.Policies;
using FamousQuoteQuiz.Data.Entity.Context;
using FamousQuoteQuiz.Data.Entity.Repositories;
using FamousQuoteQuiz.Data.EntityContracts;
using FamousQuoteQuiz.Data.EntityModels;
using FamousQuoteQuiz.Data.ServiceContracts;
using FamousQuoteQuiz.Application;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using FamousQuoteQuiz.Api.Helpers;

namespace FamousQuoteQuiz.Api
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
            services.AddControllers();

            //var mvcBuilder = services.AddMvc(options =>
            //{
            //    // Force authorization by default.
            //    var policy = new AuthorizationPolicyBuilder()
            //        .RequireAuthenticatedUser()
            //        .Build();

            //    options.Filters.Add(new AuthorizeFilter(policy));
            //}).SetCompatibilityVersion(CompatibilityVersion.Version_3_0);

            //mvcBuilder.AddControllersAsServices();

            services.AddSwaggerGen(x =>
            {
                x.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
                {
                    Version = "v1",
                    Title = "Fqq Api V1"
                });
            });

            services.AddApiVersioning(x =>
            {
                x.DefaultApiVersion = new ApiVersion(1, 0);
                x.AssumeDefaultVersionWhenUnspecified = true;
                x.ReportApiVersions = true;
            });

            services.AddMvcCore()
                //.AddAuthorization(options =>
                //{
                //    options.AddPolicy("PermissionsPolicy", policy => policy.Requirements.Add(new PermissionRequirement()));
                //})
                ;

            //services.AddAuthentication("Bearer")
            //    .AddIdentityServerAuthentication(options =>
            //    {
            //        options.Authority = "http://localhost:33235";
            //        options.ApiName = "Api Auth";
            //    });

            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IQuoteService, QuoteService>();

            services.RegisterValidators();
            services.RegisterRepositories();

            services.AddDbContext<BaseDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("FamousQuoteQuizContext")));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();
            app.UseSwaggerUI(x =>
            {
                x.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
                x.SwaggerEndpoint("/swagger/v2/swagger.json", "v2");
            });

            app.UseMiddleware<ErrorHandlingMiddleware>();
            app.UseRouting();
            //app.UseAuthentication();
            //app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
            });
        }
    }
}
