using System.Net;
using System.Net.WebSockets;
using System.Text;
using EFCoreSecondLevelCacheInterceptor;
using Microsoft.EntityFrameworkCore;
using Nostrfi;
using Nostrfi.Extensions;
using Nostrfi.Relay.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(opt => opt.AddDefaultPolicy(c => c.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin()));

builder.Services.ValidateConnectionStrings().ValidateOnStart();

builder.Services.AddDbContextFactory<NostrfiDbContext>((provider, optionsBuilder) =>
{
    optionsBuilder.UseNpgsql(builder.Configuration.GetConnectionString(ConnectionStringNames.Postgre), options =>
    {
        options.EnableRetryOnFailure(10);
    });
   optionsBuilder.AddInterceptors(provider.GetRequiredService<SecondLevelCacheInterceptor>());
});

builder.Services.AddEFSecondLevelCache(options =>
    options.UseMemoryCacheProvider(CacheExpirationMode.Sliding, TimeSpan.FromMinutes(5)).DisableLogging(true).UseCacheKeyPrefix(Constants.CachePreFix));

builder.Services.AddHostedService<MigrationsHostedService>();

builder.WebHost.UseUrls("http://localhost:5000");


var app = builder.Build();

using var serviceScope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope();
var context = serviceScope.ServiceProvider.GetService<NostrfiDbContext>();
context?.Database.Migrate();
app.UseWebSockets();
app.Map("/ws", async context =>
{
    if(context.WebSockets.IsWebSocketRequest)
    {
        using var ws = await context.WebSockets.AcceptWebSocketAsync();
        var message = "Hello World!";
        var bytes = Encoding.UTF8.GetBytes(message);
        var arraySegment = new ArraySegment<byte>(bytes, 0, bytes.Length);
        await ws.SendAsync(arraySegment,
            WebSocketMessageType.Text,
            true,
            CancellationToken.None);
        
    }
    else
    {
        context.Response.StatusCode = (int) HttpStatusCode.BadRequest;
    }
});


app.Run();
