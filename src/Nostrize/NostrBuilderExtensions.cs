using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Nostrfi.Nostrize.Handlers;

namespace Nostrfi.Nostrize;

public static class NostrBuilderExtensions
{

    public static IApplicationBuilder AddNostr(this IApplicationBuilder app)
    {
        
        app.UseMiddleware<WebSocketMiddleware>();
        return app;
    }

    public static IServiceCollection AddNostrServices(this IServiceCollection services)
    {
        services.AddSingleton<NostrHandler>();
        services.AddTransient<ConnectionManager>();
     
        return services;
    }
}