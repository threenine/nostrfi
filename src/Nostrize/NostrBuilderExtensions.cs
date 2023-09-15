using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Nostrfi.Nostrize.Handlers;
using Nostrfi.Nostrize.MessageHandlers;

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
        services.AddSingleton<ConnectionManager>();

        services.AddSingleton<IMessageHandler, EventMessageHandler>();
        services.AddSingleton<IMessageHandler, RequestMessageHandler>();
        services.AddSingleton<IMessageHandler, CloseMessageHandler>();
        return services;
    }
}