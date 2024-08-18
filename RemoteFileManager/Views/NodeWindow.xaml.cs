using Microsoft.Extensions.DependencyInjection;
using RemoteFileManager.Services;
using RemoteFileManager.ViewModels;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;

namespace RemoteFileManager.Views;

public partial class NodeWindow
{
    private NodeWindowViewModel dataContext => (NodeWindowViewModel)DataContext;
 
    public NodeWindow(IRemotingService remotingService)
    {
        DataContext = new NodeWindowViewModel(remotingService);

        InitializeComponent();
    }

    private void Directories_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
    {
        // Call Navigate() on the dataContext
        var selectedItem = ((ListBox)sender).SelectedItem as string;

        if (selectedItem != null)
            dataContext.Navigate(selectedItem);
    }

    private void Files_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
    {
        // Get the item under the mouse to be dragged
        var selectedItem = ((ListBox)sender).SelectedItem as string;

        // Drag the item
        if (selectedItem != null && e.LeftButton == System.Windows.Input.MouseButtonState.Pressed)
        {
            var fullPath = dataContext.FullPath + "/" + selectedItem;
            DragDrop.DoDragDrop((ListBox)sender, fullPath, DragDropEffects.Move);

            dataContext.HideFromFileList(selectedItem);
        }
    }

    private void Files_Dropped(object sender, DragEventArgs e)
    {
        var sourcePath = (string)e.Data.GetData(DataFormats.Text);
        if (sourcePath == null || sourcePath.EndsWith(".."))
            return;

        dataContext.FileDropped(sourcePath);
    }

    private void Files_DragOver(object sender, DragEventArgs e)
    {
        e.Effects = DragDropEffects.None;

        if (e.Data.GetDataPresent(DataFormats.StringFormat))
        {
            string dataString = (string)e.Data.GetData(DataFormats.StringFormat);

            if (dataString != null) 
                e.Effects = DragDropEffects.Move;
        }
    }

    private void Directories_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
    {
        // Get the item under the mouse to be dragged
        var selectedItem = ((ListBox)sender).SelectedItem as string;

        // Drag the item
        if (selectedItem != null && e.LeftButton == System.Windows.Input.MouseButtonState.Pressed)
        {
            var fullPath = dataContext.FullPath + "/" + selectedItem;
            DragDrop.DoDragDrop((ListBox)sender, fullPath, DragDropEffects.Move);

            dataContext.HideFromDirectoryList(selectedItem);
        }
    }

    private void Directories_DragOver(object sender, DragEventArgs e)
    {
        e.Effects = DragDropEffects.None;

        if (e.Data.GetDataPresent(DataFormats.StringFormat))
        {
            string dataString = (string)e.Data.GetData(DataFormats.StringFormat);

            if (dataString != null)
                e.Effects = DragDropEffects.Move;
        }
    }

    private void Directories_Dropped(object sender, DragEventArgs e)
    {
        var sourcePath = (string)e.Data.GetData(DataFormats.Text);
        if (sourcePath == null || sourcePath.EndsWith(".."))
            return;

        dataContext.DirectoryDropped(sourcePath);
    }

    private void NewDirectory_Click(object sender, RoutedEventArgs e)
    {
        var newRenameDialog = App.AppHost!.Services.GetRequiredService<RenameDialog>();
        var message = "New Directory:";
        newRenameDialog.Create(dataContext.FullPath + "/New Directory", message, "new");

    }

    private void DeleteDirectory_Click(object sender, RoutedEventArgs e)
    {
        var selectedItem = ((ListBox)sender).SelectedItem as string;

    }

    private void DeleteFile_Click(object sender, RoutedEventArgs e)
    {
        var selectedItem = ((ListBox)sender).SelectedItem as string;

    }

    private void RenameFile_Click(object sender, RoutedEventArgs e)
    {
        var menuItem = sender as MenuItem;
        var contextMenu = menuItem!.Parent as ContextMenu;
        var listBox = contextMenu!.PlacementTarget as ListBox;
        var selectedItem = listBox!.SelectedItem;

        var newRenameDialog = App.AppHost!.Services.GetRequiredService<RenameDialog>();
        var fullPath = dataContext.FullPath + "/" + selectedItem;
        var message = $"Renaming: \n {fullPath}";
        newRenameDialog.Create(fullPath, message, "rename");
    }

    private void RenameDirectory_Click(object sender, RoutedEventArgs e)
    {
        var menuItem = sender as MenuItem;
        var contextMenu = menuItem!.Parent as ContextMenu;
        var listBox = contextMenu!.PlacementTarget as ListBox;
        var selectedItem = listBox!.SelectedItem;

        var newRenameDialog = App.AppHost!.Services.GetRequiredService<RenameDialog>();
        var fullPath = dataContext.FullPath + "/" + selectedItem;
        var message = $"Renaming: \n {fullPath}";
        newRenameDialog.Create(fullPath, message, "rename");
    }
}
