using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using RemoteFileManager.Data;
using RemoteFileManager.Views;
using System.ComponentModel;
using System.Windows.Input;

namespace RemoteFileManager.ViewModels;

internal partial class MainWindowViewModel : ObservableObject
{
    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(NewNodeCommand))]
    private string? hostName;

    [ObservableProperty]
    private int port = 22;

    [ObservableProperty]
    private string? username;

    public string? Password { get; set; }

    [ObservableProperty]
    private string? basePath;

    public MainWindowViewModel()
    {
        HostName = Properties.Settings.Default.Host;
        Port = Properties.Settings.Default.Port;
        Username = Properties.Settings.Default.Username;
        BasePath = Properties.Settings.Default.BasePath;
    }

    [RelayCommand]
    private void NewNode()
    {
        var newWindow = App.AppHost!.Services.GetRequiredService<NodeWindow>();
        newWindow.Show();
    }

    public NodeConfiguration GetNodeConfiguration()
    {
        return new NodeConfiguration
        {
            Host = HostName!,
            Port = Port,
            Username = Username!,
            Password = Password!,
            BasePath = BasePath!
        };
    }

    partial void OnHostNameChanged(string? oldValue, string? newValue)
    {
        Properties.Settings.Default.Host = newValue;
        Properties.Settings.Default.Save();
    }

    partial void OnPortChanged(int oldValue, int newValue)
    {
        Properties.Settings.Default.Port = newValue;
        Properties.Settings.Default.Save();
    }

    partial void OnUsernameChanged(string? oldValue, string? newValue)
    {
        Properties.Settings.Default.Username = newValue;
        Properties.Settings.Default.Save();
    }

    partial void OnBasePathChanged(string? oldValue, string? newValue)
    {
        Properties.Settings.Default.BasePath = newValue;
        Properties.Settings.Default.Save();
    }
}
