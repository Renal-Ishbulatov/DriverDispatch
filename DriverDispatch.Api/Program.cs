using DriverDispatch.Api.Middleware;
using DriverDispatch.Api.Options;
using DriverDispatch.Api.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<MapSettings>(builder.Configuration.GetSection("MapSettings"));
builder.Services.Configure<ServiceSettings>(builder.Configuration.GetSection("Settings"));

builder.Services.AddSingleton<DriverStore>();
builder.Services.AddScoped<IDispatchService, DispatchService>();
builder.Services.AddHttpClient();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<ParallelLimitMiddleware>();

app.UseAuthorization();
app.MapControllers();

app.Run();