using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using SideSeams.Data;
using SideSeams.Data.Services;
using MudBlazor.Services; // ✅ Import MudBlazor
using Data.Repositories;
using SideSeams.Testing;



namespace SideSeams
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                });

            builder.Services.AddMauiBlazorWebView();
            builder.Services.AddMudServices(); // ✅ Add MudBlazor Services

            string dbPath = Path.Combine(FileSystem.AppDataDirectory, "SideSeams.db");

            builder.Services.AddDbContext<ClientContext>(options =>
            options.UseSqlite($"Data Source={dbPath}"));

            builder.Services.AddScoped<ClientServices>();
            builder.Services.AddScoped<IClientRepository, ClientRepository>(); // ✅ Register repository


#if DEBUG
            builder.Services.AddBlazorWebViewDeveloperTools();
    		builder.Logging.AddDebug();
#endif
            var app = builder.Build();


            using (var scope = app.Services.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<ClientContext>();
                db.Database.EnsureCreated(); // ✅ This ensures tables exist
                db.Database.Migrate();
            }
            //RunTestAsync(app.Services).Wait();

            return app;
        }
        //private static async Task RunTestAsync(IServiceProvider services)
        //{
        //    await TestClientRepository.RunTests(services);
        //}
    }
}
