using System;
using System.IO;

namespace BeaverSyncLib
{
    /// <summary>
    /// �������� �������� �������
    /// �������� � ������������� ��� System.IO
    /// </summary>
    public class FileSystemManager : IFileSystemManager
    {
        /// <summary>
        /// �������� �����
        /// </summary>
        /// <param name="filePath">������ ���� � �����</param>
        public void DeleteFile(string filePath)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// �������� ������ ����� ����� ����������� �� �������
        /// </summary>
        /// <param name="existCopyFromFilePath">���� � ������������� �����, ����� �������� ����� ���������</param>
        /// <param name="createCopyToFilePath">���� � ����� ������� ����� ��������� (� � ������� ����� ����������</param>
        public void CopyFile(string existCopyFromFilePath, string createCopyToFilePath)
        {
            throw new NotImplementedException();
        }

        public SyncFile GetSyncFileMetadata(string filePath)
        {
            throw new NotImplementedException();
        }
    }
}