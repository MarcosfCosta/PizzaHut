using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using OrderService.Repository;
using System;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using HealthChecks.System;
using HealthChecks.UI.Configuration;

namespace OrderService
{
    public class Startup
    {
        private const string ApiVersion = "v1";
        private const string ApiName = "OrderAPI";
        private const string HealthURL = "/health";
        private const string SQLConnectionName = "SQLConnection";
        private const string MySqlConnection = "MySqlConnection";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc(ApiVersion, new OpenApiInfo { Title = ApiName, Version = ApiVersion });


                // Use the controller comments to display directly on the swagger
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });


            //Health checks
            //**NOTE** In order to healthchecks work it is necessary to add the following dependencies
            // - AspNetCore.HealthChecks.SqlServer (in case sqlserver)
            // - Microsoft.EntityFrameworkCore.SqlServer
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString(MySqlConnection)));

            // Redis cache
            //services.AddDistributedRedisCache(options =>
            //{
            //    options.Configuration =
            //        Configuration.GetConnectionString("CacheRedis");
            //    options.InstanceName = "Cache-APIIndicadores-";
            //});

            services.AddHealthChecks()
                .AddMySql(Configuration.GetConnectionString(MySqlConnection), name: MySqlConnection)
                //.AddSqlServer(Configuration.GetConnectionString(SQLConnectionName), name: SQLConnectionName)
                .AddDiskStorageHealthCheck(delegate (DiskStorageOptions diskStorageOptions)
                {
                    diskStorageOptions.AddDrive(@"C:\", 100);
                }, name: "My Drive", HealthStatus.Unhealthy);
            //.AddRedis(Configuration.GetConnectionString("CacheRedis"), name: "cacheRedis");
            services.AddHealthChecksUI();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/" + ApiVersion + "/swagger.json", ApiName);
                //c.RoutePrefix = string.Empty;
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            //Enable HealthChecks
            app.UseHealthChecks("/status",
               new HealthCheckOptions()
               {
                   ResponseWriter = async (context, report) =>
                   {
                       var result = JsonConvert.SerializeObject(
                           new
                           {
                               statusApplication = report.Status.ToString(),
                               healthChecks = report.Entries.Select(e => new
                               {
                                   check = e.Key,
                                   ErrorMessage = e.Value.Exception?.Message,
                                   status = Enum.GetName(typeof(HealthStatus), e.Value.Status)
                               })
                           });
                       context.Response.ContentType = MediaTypeNames.Application.Json;
                       await context.Response.WriteAsync(result);
                   }
               });

            // Generate health checks endpoint
            app.UseHealthChecks("/healthchecks-data-ui", new HealthCheckOptions()
            {
                Predicate = _ => true,
                ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
            });

            app.UseHealthChecksUI(delegate (Options options)
            {
                options.UIPath = HealthURL;
            });
        }
    }
}
