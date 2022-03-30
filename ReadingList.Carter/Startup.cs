using System;
using System.IO;
using System.Linq;
using System.Net.Mime;
using Carter;
using Carter.Response;
using LightInject;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using ReadingList.Carter.Trello;
using ReadingList.Trello.Models;

namespace ReadingList.Carter
{
    public class Startup
    {
        readonly string AllowSpecificOrigins = "_allowSpecificOrigins";
        private string HostName => Configuration.GetValue<string>("HostUrl");
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<TrelloAuthSettings>(Configuration.GetSection(nameof(TrelloAuthSettings)));
            services.AddSingleton<ITrelloAuthModel>(sp => sp.GetRequiredService<IOptions<TrelloAuthSettings>>().Value);
            services.AddCarter(
                configurator: configurator =>
                {
                    configurator.WithResponseNegotiator<NewtonsoftJsonResponseNegotiatorWithEnumConverter>();
                });
            services.AddCors(options =>
            {
                options.AddPolicy(AllowSpecificOrigins,
                    builder =>
                    {
                        builder.WithOrigins("*")
                            .AllowAnyHeader()
                            .AllowAnyMethod();
                    });
            });
            services.AddHealthChecks().AddCheck<TrelloHealthCheck>(nameof(TrelloHealthCheck));
        }
        
        public void Configure(IApplicationBuilder app)
        {
            app.UseRouting();
            app.UseCors(AllowSpecificOrigins);
            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });

            app.UseSwaggerUI(opt =>
            {
                opt.RoutePrefix = "openapi/ui";
                opt.SwaggerEndpoint($"{HostName}/openapi", "ReadingList");
                opt.DocumentTitle = "ReadingList";
            });
            app.UseEndpoints(builder =>
            {
                builder.MapCarter();
                builder.MapHealthChecks("/health", CreateHealthCheckOptions());
            });
        }

        private static HealthCheckOptions CreateHealthCheckOptions()
        {
            var options = new HealthCheckOptions
            {
                ResponseWriter = async (ctx, rpt) =>
                {
                    var result = JsonConvert.SerializeObject(new
                    {
                        status = rpt.Status.ToString(),
                        checks = rpt.Entries.Select(e => new
                        {
                            service = e.Key,
                            status = Enum.GetName(typeof(HealthStatus),
                            e.Value.Status),
                            error = e.Value.Exception?.Message,
                            incidents = e.Value.Data
                        }),
                    },
                    Formatting.None, new JsonSerializerSettings()
                    {
                        NullValueHandling = NullValueHandling.Ignore
                    });

                    ctx.Response.ContentType = MediaTypeNames.Application.Json;
                    await ctx.Response.WriteAsync(result);
                }
            };
            return options;
        }
    }
}