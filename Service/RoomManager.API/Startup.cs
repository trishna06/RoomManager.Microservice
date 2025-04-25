using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using Autofac;
using EventBusUtility.Helper;
using EventBusUtility.MassTransit;
using IdentityManager.API.Helper;
using IdentityManager.API.Helper.Models;
using RoomManager.API.Configurations;
using RoomManager.API.Helpers;
using RoomManager.API.Middlewares;
using RoomManager.API.Services;
using RoomManager.Application;
using RoomManager.Application.Helpers;
using RoomManager.Infrastructure;
using Microservice.Utility.Domain.SeedWork;
using Microservice.Utility.Exception;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json.Converters;
using PermissionManager.API.Helper;
using Serilog;

namespace RoomManager.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            Version = Configuration.GetValue<string>("Version");

            DatabaseOptions databaseOptions = configuration.GetSection(nameof(DatabaseOptions)).Get<DatabaseOptions>();
            string connectionString = $"Server={databaseOptions.Server};Database={databaseOptions.Name};User Id={databaseOptions.Username};Password={databaseOptions.Password};";
            DatabaseConnectionString = connectionString;
        }

        public IConfiguration Configuration { get; }
        public string DatabaseConnectionString { get; }
        public string Version { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddHealthChecks();

            services.AddControllers(o => { o.Filters.Add(new ProducesResponseTypeAttribute(typeof(ArcstoneErrorModel), (int)HttpStatusCode.BadRequest)); })
                    .ConfigureApiBehaviorOptions(options =>
                    {
                        options.InvalidModelStateResponseFactory = actionContext =>
                        {
                            IEnumerable<string> allErrors = actionContext.ModelState.Values.SelectMany(v => v.Errors.Select(b => b.ErrorMessage));
                            string message = "Invalid Request Body: " + string.Join("; ", allErrors);
                            return new BadRequestObjectResult(new ArcstoneErrorModel(message, actionContext.HttpContext.Request.Path));
                        };
                    }).AddNewtonsoftJson(options => options.SerializerSettings.Converters.Add(new StringEnumConverter()));

            services.AddSwaggerGenNewtonsoftSupport();

            services.AddSwaggerGen(setup =>
            {
                setup.SwaggerDoc(
                    "v1",
                    new OpenApiInfo { Title = ApplicationHelper.Namespace, Version = Version });
            });

            services.AddCors(action =>
            {
                action.AddPolicy("corspolicy", builder =>
                {
                    builder.AllowAnyMethod()
                           .AllowCredentials()
                           .AllowAnyHeader()
                           .WithOrigins(Configuration.GetSection("AllowedSpecificOrigins").Get<string[]>());
                });
            });

            services.AddHttpContextAccessor()
                    .AddScoped<IUserService, UserService>();

            EndpointOptions endpointOptions = Configuration.GetSection(nameof(EndpointOptions)).Get<EndpointOptions>();

            services.AddIdentityManagerService<TokenManager>(endpointOptions.GetEndpointUrl("IdentityManager"), IdentityManagerExtension.ApplicationType.API);
            services.AddPermissionManagerService(endpointOptions.GetEndpointUrl("PermissionManager"), PermissionManagerExtension.ApplicationType.API);

            services.AddRoomApplication();
            services.AddRoomInfrastructure(DatabaseConnectionString);
        }

        public void ConfigureContainer(ContainerBuilder builder)
        {
            builder.RegisterModule(new ApplicationModule());
            builder.RegisterModule(new MediatorModule());
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMiddleware<ExceptionMiddleware>();

            app.UseSerilogRequestLogging(opts =>
            {
                opts.EnrichDiagnosticContext = LogHelper.EnrichFromRequest;
                opts.GetLevel = LogHelper.ExcludeHealthChecks; // Use custom level function
            });

            app.UseRouting();

            app.UseCors("corspolicy");

            app.UseSwagger(c =>
            {
#if !DEBUG
                c.RouteTemplate = "swagger/{documentName}/swagger.json";
                c.PreSerializeFilters.Add((swaggerDoc, httpReq) => swaggerDoc.Servers = new System.Collections.Generic.List<OpenApiServer>
                {
                  new OpenApiServer { Url = $"{httpReq.Scheme}://{httpReq.Host.Value}/{Configuration.GetValue<string>(ServiceHelper.SERVICE_PATH)}" }
                });
#endif
            });
            app.UseSwaggerUI(x => { x.SwaggerEndpoint("../swagger/v1/swagger.json", $"{ApplicationHelper.Namespace} {Version}"); });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();

                // The readiness check uses all registered checks with the 'ready' tag.
                endpoints.MapHealthChecks("/health/ready", new HealthCheckOptions() { Predicate = (check) => check.Tags.Contains("ready"), });

                endpoints.MapHealthChecks("/health/live", new HealthCheckOptions()
                {
                    // Exclude all checks and return a 200-Ok.
                    Predicate = (_) => false
                });
            });
        }
    }
}
