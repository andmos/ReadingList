using System.Collections.Generic;
using System.IO;
using Carter;
using Carter.ModelBinding;
using Carter.Response;
using LightInject;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using ReadingList.Trello.Models;

namespace ReadingList.Carter
{
    public class Startup
    {
        readonly string AllowSpecificOrigins = "_allowSpecificOrigins";
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
                options: options =>
                {
                    options.OpenApi.ServerUrls = new[]
                    {
                        Configuration.GetValue<string>("HostUrl")
                    };
                    options.OpenApi.Securities = new Dictionary<string, OpenApiSecurity>
                    {
                        {
                            nameof(TrelloAuthSettings.TrelloAPIKey),
                            new OpenApiSecurity
                            {
                                Type = OpenApiSecurityType.apiKey, Name = nameof(TrelloAuthSettings.TrelloAPIKey),
                                In = OpenApiIn.header
                            }
                        },
                        {
                            nameof(TrelloAuthSettings.TrelloUserToken),
                            new OpenApiSecurity
                            {
                                Type = OpenApiSecurityType.apiKey, Name = nameof(TrelloAuthSettings.TrelloUserToken),
                                In = OpenApiIn.header
                            }
                        },
                    };
                    options.OpenApi.GlobalSecurityDefinitions = new[]
                        {nameof(TrelloAuthSettings.TrelloAPIKey), nameof(TrelloAuthSettings.TrelloUserToken)};
                },
                configurator: configurator =>
                {
                    configurator.WithModelBinder<NewtonsoftJsonModelBinder>();
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
        }

        public void ConfigureContainer(IServiceContainer container)
        {
            container.RegisterFrom<CompositionRoot>();
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
                opt.SwaggerEndpoint("/openapi", "ReadingList");
                opt.DocumentTitle = "ReadingList";
            });
            app.UseEndpoints(builder => builder.MapCarter());
        }
    }
}