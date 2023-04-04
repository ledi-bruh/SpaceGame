using CoreWCF;
using CoreWCF.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;
using WebHttp;

internal sealed class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddServiceModelWebServices(o =>
        {
            o.Title = "Сервер для 7 л/р по ООАИП";
            o.Version = "1.0.0";
            o.Description = "Пора работать";
        });

        services.AddSingleton(new SwaggerOptions());
    }

    public void Configure(IApplicationBuilder app)
    {
        app.UseMiddleware<SwaggerMiddleware>();
        app.UseSwaggerUI();

        app.UseServiceModel(builder =>
        {
            builder.AddService<SpaceService>();
            builder.AddServiceWebEndpoint<SpaceService, ISpaceService>(
                new WebHttpBinding
                {
                    MaxReceivedMessageSize = 5242880,
                    MaxBufferSize = 65536,
                },
                "space",
                behavior =>
                {
                    behavior.HelpEnabled = true;
                    behavior.AutomaticFormatSelectionEnabled = true;
                }
            );
        });
    }
}
