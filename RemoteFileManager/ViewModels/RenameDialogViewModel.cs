using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using RemoteFileManager.Services;
using System.IO;

namespace RemoteFileManager.ViewModels;

public partial class RenameDialogViewModel : ObservableObject
{
    private readonly IRemotingService _remotingService;

    [ObservableProperty]
    private string? dialogPurpose;

    [ObservableProperty]
    private string? renameMessage;

    [ObservableProperty]
    private string? newName;

    public string? FullPath { get; set; }

    public RenameDialogViewModel(IRemotingService remotingService)
    {
        _remotingService = remotingService;

    }

    public void Rename()
    {
        var originalPath = FullPath!.Substring(0, FullPath.LastIndexOf('/'));
        var originalFilename = Path.GetFileName(FullPath);

        var newPath = originalPath + "/" + newName;
        if (newPath == FullPath)
            return;

        _remotingService.RenameFile(FullPath, newPath);

    }

    internal void New()
    {
        var newPath = FullPath + "/" + newName;
        _remotingService.CreateDirectory(newPath);
    }
}
