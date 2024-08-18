using CommunityToolkit.Mvvm.ComponentModel;
using RemoteFileManager.Services;
using System.Collections.ObjectModel;

namespace RemoteFileManager.ViewModels;

internal partial class NodeWindowViewModel : ObservableObject
{
    private readonly IRemotingService _remotingService;

    [ObservableProperty]
    private string? fullPath;


    [ObservableProperty]
    private string directoryName;

    [ObservableProperty]
    private ObservableCollection<string> directories = new();

    [ObservableProperty]
    private ObservableCollection<string> files = new();


    public NodeWindowViewModel(IRemotingService remotingService)
    {
        _remotingService = remotingService;

        FullPath = _remotingService.GetNodeConfiguration().BasePath;

        Refresh();
    }

    public void NewDirectory(string path)
    {

    }

    public void DeleteDirectory(string path)
    {

    }

    public void DeleteFile(string path)
    {

    }

    private void GetContent(string fullPath)
    {
        var contents = _remotingService.GetDirectoryContent(fullPath!);

        Directories.Clear();
        Files.Clear();

        foreach (var item in contents)
        {
            if (item.StartsWith("d"))
            {
                var dirPath = item.Split(' ', StringSplitOptions.RemoveEmptyEntries)[8..];
                var dirName = string.Join(' ', dirPath);
                // Skip on error
                if (dirName == null)
                    continue;

                // Skip hidden directories
                if (dirName!.StartsWith(".") && dirName != "..")
                    continue;

                Directories.Add(dirName);
            }
            else
            {
                // Skip links and the totals line and hidden files
                if (item.StartsWith("l") || item.StartsWith("t") || item.StartsWith("."))
                    continue;

                var itemPath = item.Split(' ', StringSplitOptions.RemoveEmptyEntries)[8..];
                var itemName = string.Join(' ', itemPath);
                if (itemName != null)
                    Files.Add(itemName);
            }
        }
    }

    public void Navigate(string directory)
    {
        if (directory == "..")
        {
            var lastSeparator = FullPath!.LastIndexOf('/');
            FullPath = FullPath.Substring(0, lastSeparator);
        }
        else
        {
            var separator = FullPath!.EndsWith("/") ? "" : "/";
            FullPath = $"{FullPath}{separator}{directory}";
        }

        Refresh();
    }

    public void FileDropped(string sourcePath)
    {
        _remotingService.MoveFile(sourcePath, FullPath!);
        Refresh();
    }

    public void Refresh()
    {
        DirectoryName = FullPath!.Split('/').LastOrDefault() ?? "";
        GetContent(FullPath!);
    }

    public void HideFromFileList(string selectedItem)
    {
        Files.Remove(selectedItem);
    }

    public void DirectoryDropped(string sourcePath)
    {
        if (sourcePath == null)
            return;

        // Skip if the source path is the same as the destination path
        var sourceParent = sourcePath.Substring(0, sourcePath.LastIndexOf('/'));
        if (sourceParent == FullPath!)
            return;

        _remotingService.MoveFile(sourcePath, FullPath!);
        Refresh();
    }

    public void HideFromDirectoryList(string selectedItem)
    {
        Directories.Remove(selectedItem);
    }
}
