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
            File.Delete(filePath);
        }

        /// <summary>
        /// �������� ������ ����� ����� ����������� �� �������
        /// </summary>
        /// <param name="existCopyFromFilePath">���� � ������������� �����, ����� �������� ����� ���������</param>
        /// <param name="createCopyToFilePath">���� � ����� ������� ����� ��������� (� � ������� ����� ����������</param>
        public void CopyFile(string existCopyFromFilePath, string createCopyToFilePath)
        {
            File.Copy(existCopyFromFilePath, createCopyToFilePath);
        }

        /// <summary>
        /// ���������� ���������� �����
        /// </summary>
        /// <param name="filePath">���� � ����� ��� ������ ����������</param>
        /// <returns>���������� �����</returns>
        public FileMetadata GetFileMetadata(string filePath)
        {
            var fileInfo = new FileInfo(filePath);
            var meta = new FileMetadata
            {
                ByteSize = fileInfo.Length, 
                LastModified = fileInfo.LastWriteTimeUtc
            };

            return meta;
        }
    }
}