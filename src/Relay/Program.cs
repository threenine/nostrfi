using System.Net;
using System.Net.WebSockets;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Nostrfi;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(opt => opt.AddDefaultPolicy(c => c.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin()));
var connectionString = builder.Configuration.GetConnectionString("Nostrfi_DB");
builder.Services.AddDbContext<NostrfiDbContext>(x => x.UseNpgsql(connectionString));


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
app.MapGet("/", () => "Hello World!");

app.Run();
