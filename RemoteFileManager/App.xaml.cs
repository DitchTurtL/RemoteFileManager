using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RemoteFileManager.Services;
using RemoteFileManager.Views;
using System.IO;
using System.Windows;

namespace RemoteFileManager;

public partial class App : Application
{
    public static IHost? AppHost { get; private set; }

    public App()
    {
        AppHost = Host.CreateDefaultBuilder()
            .ConfigureServices((context, services) =>
            {
                services
                    .AddSingleton<MainWindow>()
                    .AddTransient<NodeWindow>()
                    .AddTransient<RenameDialog>()
                    .AddSingleton<IRemotingService, RemotingService>();
            })
            .Build();
    }

    protected override async void OnStartup(StartupEventArgs e)
    {
        await AppHost!.StartAsync();

        var mainForm = AppHost.Services.GetRequiredService<MainWindow>();
        mainForm.Show();

        base.OnStartup(e);
    }

    protected override async void OnExit(ExitEventArgs e)
    {
        await AppHost!.StopAsync();
        base.OnExit(e);
    }

}
