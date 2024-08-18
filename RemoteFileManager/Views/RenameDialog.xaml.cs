using RemoteFileManager.Services;
using RemoteFileManager.ViewModels;
using System.IO;
using System.Windows;

namespace RemoteFileManager.Views;

public partial class RenameDialog
{
    private readonly IRemotingService _remotingService;
    private string Mode { get; set; }

    private RenameDialogViewModel dataContext => (RenameDialogViewModel)DataContext;

    public RenameDialog(IRemotingService remotingService)
    {
        _remotingService = remotingService;
        DataContext = new RenameDialogViewModel(remotingService);
        InitializeComponent();
    }

    public void Create(string fullPath, string message, string mode)
    {
        Mode = mode;
        dataContext.FullPath = fullPath;
        dataContext.RenameMessage = message;
        dataContext.NewName = Path.GetFileName(fullPath);
        ShowDialog();
    }

    private void Ok_Clicked(object sender, RoutedEventArgs e)
    {
        if (Mode == "rename")
            dataContext.Rename();
        else if (Mode == "new")
            dataContext.New();
        Close();
    }

    private void Cancel_Clicked(object sender, RoutedEventArgs e)
    {
        Close();
    }
}
