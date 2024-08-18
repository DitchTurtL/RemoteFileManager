using RemoteFileManager.Services;
using RemoteFileManager.ViewModels;
using System.Windows.Controls;

namespace RemoteFileManager.Views;

public partial class MainWindow
{ 
    private MainWindowViewModel dataContext => (MainWindowViewModel)DataContext;
    private readonly IRemotingService _remotingService;
    public MainWindow(IRemotingService remotingService)
    {
        DataContext = new MainWindowViewModel();
        _remotingService = remotingService;

        InitializeComponent();
    }

    private void PasswordBox_PasswordChanged(object sender, System.Windows.RoutedEventArgs e)
    {
        if (this.DataContext == null)
            return;
            
        dataContext.Password = ((PasswordBox)sender).Password;
        
    }
}