using Carter;
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
            services.AddCarter();
            
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