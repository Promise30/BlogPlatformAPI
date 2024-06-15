

using BloggingAPI.Constants;
using BloggingAPI.Data;
using BloggingAPI.Domain.Entities;
using BloggingAPI.Domain.Repository.Implementation;
using BloggingAPI.Domain.Repository.Interface;
using BloggingAPI.Extensions;
using BloggingAPI.Infrastructure.Services.Implementation;
using BloggingAPI.Infrastructure.Services.Interface;
using BloggingAPI.Network.Implementation;
using BloggingAPI.Network.Interface;
using Hangfire;
using Hangfire.SqlServer;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using System.Reflection;
using System.Text;
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateLogger();
try
{
    Log.Information("starting server.");
    var builder = WebApplication.CreateBuilder(args);
    builder.Host.UseSerilog((context, loggerConfiguration) =>
    {
        loggerConfiguration.WriteTo.Console();
        loggerConfiguration.ReadFrom.Configuration(context.Configuration);
    });

    // Add services to the container.
    var emailConfig = builder.Configuration.GetSection("EmailConfiguration").Get<EmailConfiguration>();
    builder.Services.AddSingleton(emailConfig);
    builder.Services.ConfigureSqlContext(builder.Configuration);
    builder.Services.AddScoped<IRepositoryManager, RepositoryManager>();
    builder.Services.ConfigureIdentity();
    builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
    builder.Services.AddScoped<IBloggingService, BloggingService>();
    builder.Services.AddHttpClient();
    builder.Services.AddScoped<IApiClient, ApiClient>();
    builder.Services.AddScoped<IEmailService, EmailService>();
    builder.Services.AddScoped<IEmailNotificationService, EmailNotificationService>();
    builder.Services.AddScoped<ICloudinaryService, CloudinaryService>();
    builder.Services.ConfigureHangFire(builder.Configuration);
    builder.Services.AddHangfireServer();
    builder.Services.ConfigureAuthentication(builder.Configuration);
    builder.Services.ConfigureCors();
    builder.Services.AddControllers();
    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.ConfigureSwaggerGen();
    
    var app = builder.Build();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI(config =>
        {
            config.SwaggerEndpoint("/swagger/v1/swagger.json", "Blog Platform API");
        });
    }
    app.UseHttpsRedirection();
    app.UseAuthentication();
    app.UseAuthorization();
    app.UseHangfireDashboard();
    app.UseCors();
    app.MapControllers();
    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "server terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}

NewtonsoftJsonPatchInputFormatter GetJsonPatchInputFormatter()=> new ServiceCollection().AddLogging().AddMvc().AddNewtonsoftJson()
.Services.BuildServiceProvider().GetRequiredService<IOptions<MvcOptions>>().Value.InputFormatters
.OfType<NewtonsoftJsonPatchInputFormatter>().First();