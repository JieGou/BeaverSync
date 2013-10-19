using System.IO;

namespace BeaverSyncLib
{
    /// <summary>
    /// �������� �������� �������
    /// �������� � ������������� ��� System.IO
    /// </summary>
    public class FileSystemManager : IFileSystemManager
    {
        #region Singleton
        private FileSystemManager()
        {
        }

        private static FileSystemManager _instance = null;

        public static FileSystemManager Instance 
        {
            get
            {
                if(_instance == null)
                    _instance = new FileSystemManager();

                return _instance;
            }
        }
        #endregion

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

        /// <summary>
        /// ��������� ���������� �� ����������, ���� �� ���������� - �������
        /// </summary>
        /// <param name="dirPath">�������� ���� � ����������</param>
        public void CreateDirIfNotExist(string dirPath)
        {
            if(!Directory.Exists(dirPath))
            {
                Directory.CreateDirectory(dirPath);
            }
        }
    }
}