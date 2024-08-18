using Microsoft.Extensions.DependencyInjection;
using RemoteFileManager.Data;
using RemoteFileManager.ViewModels;
using RemoteFileManager.Views;
using Renci.SshNet;

namespace RemoteFileManager.Services;

public class RemotingService : IRemotingService
{
    private SshClient? _sshClient;

    private SshClient GetSshClient()
    {
        if (_sshClient == null || !_sshClient.IsConnected)
        {
            var nodeConfiguration = GetNodeConfiguration();
            _sshClient = new SshClient(nodeConfiguration.Host, nodeConfiguration.Port, nodeConfiguration.Username, nodeConfiguration.Password);
            _sshClient.Connect();
        }
        return _sshClient;
    }

    public string[] GetDirectoryContent(string fullPath)
    {
        var client = GetSshClient();
        using SshCommand command = client.RunCommand($"ls -lah {FixPath(fullPath!)}");
        var result = command.Result.Split('\n', StringSplitOptions.RemoveEmptyEntries);

        return result;
    }

    public NodeConfiguration GetNodeConfiguration()
    {
        var mainWindow = App.AppHost!.Services.GetRequiredService<MainWindow>();
        var mwViewModel = (MainWindowViewModel)mainWindow.DataContext;
        return mwViewModel.GetNodeConfiguration();
    }

    public void MoveFile(string sourcePath, string destinationPath)
    {
        var client = GetSshClient();
        using SshCommand command = client.RunCommand($"mv {FixPath(sourcePath)} {FixPath(destinationPath)}");
        if (command.ExitStatus != 0)
        {
            throw new Exception($"Error moving file: {command.Error}");
        }
    }

    private string FixPath(string path)
    {
        var tmpPath = path.Replace(" ", @"\ ");
        tmpPath = tmpPath.Replace("(", @"\(");
        tmpPath = tmpPath.Replace(")", @"\)");
        return tmpPath;
    }

    public void RenameFile(string fullPath, string newPath)
    {
        var client = GetSshClient();
        using SshCommand command = client.RunCommand($"mv {FixPath(fullPath)} {FixPath(newPath)}");
        if (command.ExitStatus != 0)
        {
            throw new Exception($"Error renaming file: {command.Error}");
        }
    }

    public void CreateDirectory(string path)
    {
        var client = GetSshClient();
        using SshCommand command = client.RunCommand($"mkdir {FixPath(path)}");
        if (command.ExitStatus != 0)
        {
            throw new Exception($"Error creating directory: {command.Error}");
        }
    }
}
