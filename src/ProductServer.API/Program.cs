using NLog;
using NLog.Web;
using ProductServer;
using ProductServer.API.Middleware;
using ProductServer.Application;
using ProductServer.Infrastructure;
using ProductServer.Infrastructure.Bootstrap;

Logger logger = NLogBuilder.ConfigureNLog("nlog.config").GetCurrentClassLogger();

try
{
    logger.Info($"STARTING Product Service VERSION {typeof(Program).Assembly.GetName().Version}");
    
    WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
    
    // NLog
    builder.Logging.ClearProviders();
    builder.Host.UseNLog();
    
    builder.Services.AddApplicationLayer();
    builder.Services.AddInfrastructureLayer(builder.Configuration, builder.Environment);
    builder.Services.AddPresentationLayer();
    
    builder.Services.AddHostedService<DatabaseBootstrap>();
    
    WebApplication app = builder.Build();
    
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }
    
    app.UseHttpsRedirection();
    
    app.UseAuthorization();
    
    app.MapControllers();

    app.UseMiddleware<ExceptionMiddleware>();

    app.Run();
}
catch (Exception ex)
{  
    logger.Error(ex, "STOPPED PROGRAM BECAUSE OF EXCEPTION" + Environment.NewLine);
        throw;
}
finally
{
    LogManager.Shutdown();
}

// Expose presentation starting point assembly
namespace ProductServer
{ public partial class Program { } }