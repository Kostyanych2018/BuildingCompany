using System.Reflection;
using BuildingCompany.Application;
using BuildingCompany.Infrastructure;
using BuildingCompany.Infrastructure.Data;
using BuildingCompany.UI.Services;
using CommunityToolkit.Maui;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Maui.Controls.Hosting;
using Microsoft.Maui.Hosting;
using OxyPlot.Maui.Skia;
using SkiaSharp.Views.Maui.Controls.Hosting;

namespace BuildingCompany.UI;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        string settingsStream = "BuildingCompany.UI.appsettings.json";
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .UseMauiCommunityToolkit()
            .UseOxyPlotSkia()
            .UseSkiaSharp()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                fonts.AddFont("Free-Regular-400.otf","FreeRegular");
                fonts.AddFont("Free-Solid-900.otf","FreeSolid");
                fonts.AddFont("Brands-Regular-400.otf","BrandsRegular");
            });
        var assemnbly = Assembly.GetExecutingAssembly();
        using var stream = assemnbly.GetManifestResourceStream(settingsStream);
        builder.Configuration.AddJsonStream(stream);
        var mongoDbSettings = builder.Configuration.GetSection("MongoDb");
        var connectionsString = mongoDbSettings["ConnectionString"];
        var dbName = mongoDbSettings["DatabaseName"];
        var options = new DbContextOptionsBuilder<AppDbContext>().UseMongoDB(connectionsString,dbName).Options;
        builder.Services
            .AddApplication()
            .AddInfrastructure(options)
            .RegisterPages()
            .RegisterViewModels()
            .AddSingleton<ImageService>();
        // DbInitializer.Initialize(builder.Services.BuildServiceProvider()).Wait();
#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}
