using NLog.Extensions.Logging;
using SigalNET.Gateway;
using SigalNET.Gateway.Authentication;
using SigalNET.Gateway.Configuration;
using SigalNET.Gateway.Secret;

var builder = WebApplication.CreateBuilder(args);
var basePath = AppContext.BaseDirectory;

builder.WebHost.UseKestrel();

//--------------------------------- Logging block -------------------------------------------------------------
{
    builder.Logging.ClearProviders();
    builder.Logging.AddNLog();
}

//--------------------------------- Authentication block -----------------------------------------------------
{
    builder.Services.AddSingleton<IJwtAuthenticationFile>(_ =>
    {
        return new ConfigurationBuilder()
                    .SetBasePath(basePath)
                    .AddJsonFile("authentication.json", false, true)
                    .AddJsonFile($"authentication.{builder.Environment.EnvironmentName}.json", true, true)
                    .Build()
                    .Get<JwtAuthenticationFile>() ?? throw new FileNotFoundException();
    });

    builder.Services.AddSingleton<ISecretConfigurationFile>(_ =>
    {
        return new ConfigurationBuilder()
                .SetBasePath(basePath)
                .AddJsonFile("secrets.json", false, true)
                .AddJsonFile($"secrets.{builder.Environment.EnvironmentName}.json", true, true)
                .AddEnvironmentVariables(prefix: "APP__SECRETS__")
                .Build()
                .Get<SecretConfigurationFile>() ?? throw new FileNotFoundException();
    });

    builder.Services.AddJwtBearerAuthenticationServices();
}

//--------------------------------- Endpoints block ----------------------------------------------------------
{
    builder.Services.AddSingleton(_ =>
    {
        return new ConfigurationBuilder()
                .SetBasePath(basePath)
                .AddJsonFile("endpoints.json", false, true)
                .AddJsonFile($"endpoints.{builder.Environment.EnvironmentName}.json", true, true)
                .Build()
                .Get<EndpointConfiguration>() ?? throw new FileNotFoundException();
    });
}

//--------------------------------- Cors block ---------------------------------------------------------------
{
    builder.Services.AddCorsConfiguration(builder.Environment, basePath);
}

//--------------------------------- Common Block --------------------------------------------------------------
{
    builder.Services.AddHttpContextAccessor();
    builder.Services.AddControllers();
    builder.Services.AddHttpClient();

    builder.Services.AddSingleton<GatewayHttpClient>();
}

var app = builder.Build();

app.UseCorsConfiguration();

if (app.Environment.IsProduction())
{
    app.UseExceptionHandler("/error");
    app.UseHsts();
    app.MapGet("/error", () => Results.Problem("An error occurred.", statusCode: 500)).ExcludeFromDescription();
}
else
{
    app.UseDeveloperExceptionPage();
}

//--------------------------------- Endpoints Map ---------------------------------------------------------------
{
    GatewayEndpoint.Map(app);
}

await app.RunAsync().ConfigureAwait(false);
public partial class Program { }