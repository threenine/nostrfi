using EFCoreSecondLevelCacheInterceptor;
using Microsoft.EntityFrameworkCore;
using Nostrfi;
using Nostrfi.Extensions;
using Nostrfi.Nostrize;
using Nostrfi.Nostrize.Settings;
using Nostrfi.Relay.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(opt => opt.AddDefaultPolicy(c => c.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin()));

builder.Services.ValidateConnectionStrings().ValidateOnStart();

builder.Services.AddDbContextFactory<NostrfiDbContext>((provider, optionsBuilder) =>
{
    var conn = builder.Configuration.GetConnectionString(ConnectionStringNames.Postgre);
    optionsBuilder.UseNpgsql(builder.Configuration.GetConnectionString(ConnectionStringNames.Postgre), options =>
    {
        options.EnableRetryOnFailure(10);
        options.SetPostgresVersion(16, 0);
    });
   optionsBuilder.AddInterceptors(provider.GetRequiredService<SecondLevelCacheInterceptor>());
});

builder.Services.AddEFSecondLevelCache(options =>
    options.UseMemoryCacheProvider(CacheExpirationMode.Sliding, TimeSpan.FromMinutes(5)).DisableLogging(true).UseCacheKeyPrefix(Constants.CachePreFix));

builder.Services.AddHostedService<MigrationsHostedService>();
builder.Services.AddNostrServices();

var nostr = builder.Configuration.GetSection(nameof(Nostr)).Get<Nostr>();
builder.WebHost.UseUrls(nostr.BaseUrl);


var app = builder.Build();


app.UseWebSockets();
app.AddNostr();



app.Run();
