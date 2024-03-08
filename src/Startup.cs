using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.OpenApi.Models;

using Newtonsoft.Json.Serialization;

namespace config.map.test
{
    public class Startup
    {
        private static readonly string SERVICE_NAME = System.Reflection.Assembly.GetEntryAssembly().EntryPoint.DeclaringType.Namespace;

        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        private class ApiExplorerGroupPerVersionConvention : IControllerModelConvention
        {
            public void Apply(ControllerModel controller)
            {
                var controllerNamespace = controller.ControllerType.Namespace;
                var apiVersion = controllerNamespace?.Split('.').Last().ToLower();
                controller.ApiExplorer.GroupName = apiVersion;
            }
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<Service.IService, Service.Service>();

            services.AddControllers();

            services.AddMvc(c => c.Conventions.Add(new ApiExplorerGroupPerVersionConvention())).AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.Converters.Add(new Newtonsoft.Json.Converters.StringEnumConverter());
                options.SerializerSettings.ContractResolver = new DefaultContractResolver();
                options.SerializerSettings.DateTimeZoneHandling = Newtonsoft.Json.DateTimeZoneHandling.Utc;
                options.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
            });

            services.Configure<GzipCompressionProviderOptions>(options => options.Level = System.IO.Compression.CompressionLevel.Optimal);
            services.AddResponseCompression();

            services.AddSwaggerGen(c =>
            {
                c.EnableAnnotations();
                c.CustomSchemaIds(x => x.FullName.Replace("+", "."));
                c.OperationFilter<SwaggerDescriptionOperationFilter>();
                c.SwaggerDoc("diagnostic", new OpenApiInfo { Title = $"{SERVICE_NAME} DIAGNOSTICS", Version = "diagnostic" });
                c.SwaggerDoc("v1", new OpenApiInfo { Title = $"{SERVICE_NAME} API", Version = "v1-preview" });
            });
            services.AddSwaggerGenNewtonsoftSupport();

            //Get the current settings
            ThreadPool.GetMinThreads(out int minWorkerThreads, out int minIOThreads);
            ThreadPool.SetMinThreads(_configuration.min_worker_threads, _configuration.min_io_threads);
        }

        public void Configure(IApplicationBuilder app, IHostEnvironment env)
        {
            //THIS KICK STARTS THE DEPENDENCY INJECTION SO INSTANCE IS READY FOR FIRST REQUEST
            //also otherwise context of first request will be forever logged by internal tasks
            var service = app.ApplicationServices.GetService<Service.IService>();

            app.UseResponseCompression();
            if(env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseSwagger(c =>
            {
                c.PreSerializeFilters.Add((swaggerDoc, httpRequest) =>
                {
                    if(httpRequest.Headers.ContainsKey("X-Forwarded-Path"))
                    {
                        var serverUrl = $"{httpRequest.Headers["X-Forwarded-Proto"]}://" +
                                        $"{httpRequest.Headers["X-Forwarded-Host"]}/" +
                                        $"{httpRequest.Headers["X-Forwarded-Path"]}";
                        swaggerDoc.Servers = new List<OpenApiServer>()
                        {
                            new OpenApiServer { Url = serverUrl }
                        };
                    }
                });
            });
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/diagnostic/swagger.json", $"{SERVICE_NAME} DIAGNOSTIC");
                c.SwaggerEndpoint("/swagger/v1/swagger.json", $"{SERVICE_NAME} API - V1");
            });

            app.UseRouting();

            app.UseCors(c =>
            {
                c.AllowAnyMethod()
                .AllowAnyMethod()
                .WithOrigins("http://localhost:4200");
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
