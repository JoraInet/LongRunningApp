using Asp.Versioning;
using LongRunningApp.Api.Hubs;
using LongRunningApp.Api.Middleware;
using LongRunningApp.Api.Services;
using LongRunningApp.Application;
using LongRunningApp.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.ConfigureApplicationLayer();
builder.ConfigureInfrastructureLayer();

builder.Services.AddSingleton<ITextProcessorService, TextProcessorService>();

builder.Services.Configure<RouteOptions>(options =>
{
    options.LowercaseUrls = true;
});

builder.Services.AddApiVersioning(options =>
{
    options.DefaultApiVersion = new ApiVersion(1, 0);
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.ReportApiVersions = true;
    options.ApiVersionReader = ApiVersionReader.Combine(new UrlSegmentApiVersionReader(),
                                                        new HeaderApiVersionReader("x-api-version"),
                                                        new MediaTypeApiVersionReader("x-api-version"));
}).AddApiExplorer(options => {
    options.GroupNameFormat = "'v'VVV";
    options.SubstituteApiVersionInUrl = true;
});


builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSignalR(options =>
{
    options.DisableImplicitFromServicesParameters = true;
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigins",
        builder =>
        {
            builder.WithOrigins("http://localhost:4200")
                   .AllowAnyHeader()
                   .AllowAnyMethod()
                   .AllowCredentials();
        });
});


var app = builder.Build();
app.UseCors("AllowSpecificOrigins");
app.UseMiddleware<ExceptionMiddleware>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapControllers();

app.MapApplicationHubs();

app.Run();

// Make the Program class partial to support WebApplicationFactory
public partial class Program { }