namespace BeaverSyncLib
{
    /// <summary>
    /// ��������� ��������� �������� �������
    /// </summary>
    public interface IFileSystemManager
    {
        /// <summary>
        /// �������� �����
        /// </summary>
        /// <param name="filePath">������ ���� � �����</param>
        void DeleteFile(string filePath);
        /// <summary>
        /// �������� ������ ����� ����� ����������� �� �������
        /// </summary>
        /// <param name="existCopyFromFilePath">���� � ������������� �����, ����� �������� ����� ���������</param>
        /// <param name="createCopyToFilePath">���� � ����� ������� ����� ��������� (� � ������� ����� ����������</param>
        void CopyFile(string existCopyFromFilePath, string createCopyToFilePath);
        SyncFile GetSyncFileMetadata(string filePath);
    }
}