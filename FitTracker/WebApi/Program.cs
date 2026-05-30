using Application;
using Application.Abstractions.Auth;
using Hangfire;
using Infrastructure.Data;
using Infrastructure.ExternalSources;
using Infrastructure.FileService;
using Infrastructure.Identity;
using Infrastucture.EmailService;
using Infrastucture.Jobs.Hangfire;
using Microsoft.AspNetCore.Http.Features;
using Presentation;
using Presentation.Extensions;
using Microsoft.Extensions.FileProviders;
using Presentation.Middlewares;

var builder = WebApplication.CreateBuilder(args);

builder.Host.AddLogging(builder.Configuration);

builder.WebHost.ConfigureKestrel(options =>
{
    options.Limits.MaxRequestBodySize = null;
});

builder.Services.AddControllers();
builder.Services.Configure<FormOptions>(o =>
{
    o.MultipartBodyLengthLimit = long.MaxValue;
    o.ValueLengthLimit = int.MaxValue;
    o.MultipartHeadersLengthLimit = int.MaxValue;
});
builder.Services.AddHttpContextAccessor();

builder.Services.AddCors(options =>
{
    options.AddPolicy("Frontend", policy =>
    {
        policy.WithOrigins(
                "http://localhost:5223",
                "http://127.0.0.1:5223",
                "https://localhost:7299",
                "http://localhost:7299",
                "http://localhost:5500",
                "http://127.0.0.1:5500",
                "http://localhost:3000")
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
    });
});

builder.Services.AddSwagger();

builder.Services.AddIdentityServices(builder.Configuration);
builder.Services.AddDataBase(builder.Configuration);
builder.Services.AddFileStorage(builder.Configuration);
builder.Services.AddEmailService(builder.Configuration);
builder.Services.AddBackgroundJobs(builder.Configuration);
builder.Services.AddApiIntegration(builder.Configuration);

builder.Services.AddApplication();

builder.Services.AddScoped<ICurrentUserAccessor, CurrentUserAccessor>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseHangfireDashboard();
}

app.UseCors("Frontend");

var frontendPath = Path.GetFullPath(Path.Combine(app.Environment.ContentRootPath, "..", "..", "frontend"));
if (Directory.Exists(frontendPath))
{
    var frontendFiles = new PhysicalFileProvider(frontendPath);
    app.UseDefaultFiles(new DefaultFilesOptions { FileProvider = frontendFiles });
    app.UseStaticFiles(new StaticFileOptions { FileProvider = frontendFiles });
}

app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseAuthentication();
app.UseMiddleware<TokenBlacklistMiddleware>();
app.UseAuthorization();

app.MapControllers();

app.Run();