using System;

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

        /// <summary>
        /// ���������� ���������� �����
        /// </summary>
        /// <param name="filePath">���� � ����� ��� ������ ����������</param>
        /// <returns>���������� �����</returns>
        FileMetadata GetFileMetadata(string filePath);
    }

    /// <summary>
    /// ���������� �����
    /// </summary>
    public struct FileMetadata
    {
        /// <summary>
        /// ���� ���������
        /// </summary>
        public DateTime LastModified { get; set; }
        /// <summary>
        /// ������ ����� � ������
        /// </summary>
        public long ByteSize { get; set; }

        public override string ToString()
        {
            return String.Format("LastModified = {0:dd.MM.yyyy HH:mm:ss}, ByteSize = {1}", LastModified, ByteSize);
        }
    }
}