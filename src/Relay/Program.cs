var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(opt => opt.AddDefaultPolicy(c => c.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin()));


var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.Run();
