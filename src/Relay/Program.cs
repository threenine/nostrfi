using EFCoreSecondLevelCacheInterceptor;
using Microsoft.EntityFrameworkCore;
using Nostrfi;
using Nostrfi.Extensions;
using Nostrfi.Nostrize;
using Nostrfi.Relay;
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
builder.Services.AddNostrServices();


builder.WebHost.UseUrls("http://localhost:5000");


var app = builder.Build();


app.UseWebSockets();
app.AddNostr();



app.Run();
