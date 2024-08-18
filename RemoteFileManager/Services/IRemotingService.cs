using RemoteFileManager.Data;

namespace RemoteFileManager.Services;

public interface IRemotingService
{
    string[] GetDirectoryContent(string fullPath);
    void MoveFile(string sourcePath, string destinationPath);
    NodeConfiguration GetNodeConfiguration();
    void RenameFile(string fullPath, string newPath);
    void CreateDirectory(string path);
}