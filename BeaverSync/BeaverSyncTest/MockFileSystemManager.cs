using System;
using System.Collections.Generic;
using BeaverSyncLib;

namespace BeaverSyncTest
{
    /// <summary>
    /// �������� mock ������ IFileSystemManager
    /// </summary>
    [Obsolete]
    public class MockFileSystemManager : IFileSystemManager
    {
        /// <summary>
        /// ���� ��� �� ������ ����� DeleteFile
        /// </summary>
        public bool IsDeleteFileMethodCalled = false;
        /// <summary>
        /// ���� ���������� � ����� DeleteFile
        /// </summary>
        public string DeleteFileMethod_FilePath = null;
        /// <summary>
        /// ���� ��� �� ������ ����� CopyFile
        /// </summary>
        public bool IsCopyFileMethodCalled = false;
        /// <summary>
        /// �������� existCopyFromFilePath ������ CopyFile
        /// </summary>
        public string CopyFileMethod_ExistCopyFromFilePath = null;
        /// <summary>
        /// �������� createCopyToFilePath ������ CopyFile
        /// </summary>
        public string CopyFileMethod_CreateCopyToFilePath = null;
        /// <summary>
        /// ���� ��� �� ������ ����� DeleteFile ����� ������� ������ CopyFile
        /// </summary>
        public bool IsDeleteFileMethodCalledBeforeCopyFileMethod = false;
        /// <summary>
        /// ���� ��� �� ������ ����� GetFileMetadata
        /// </summary>
        public bool IsGetFileMetadataMethodCalled = false;
        /// <summary>
        /// ���� ���������� � ����� GetFileMetadata
        /// </summary>
        public string GetFileMetadataMethod_FilePath = null;
        

        /// <summary>
        /// ����������� ������ � ������
        /// </summary>
        private readonly SortedList<string, FileMetadata> _filesRepo;

        /// <summary>
        /// �����������
        /// </summary>
        /// <param name="filesRepository">�������� ������ � ���� ����������� ������
        /// (���� - ���� � �����, �������� - ���������� �����)</param>
        public MockFileSystemManager(SortedList<string, FileMetadata> filesRepository)
        {
            _filesRepo = filesRepository;
        }

        /// <summary>
        /// �������� �����
        /// </summary>
        /// <param name="filePath">������ ���� � �����</param>
        public void DeleteFile(string filePath)
        {
            // ����������� ���������-�������� ����������:
            IsDeleteFileMethodCalled = true;
            DeleteFileMethod_FilePath = filePath;
            IsDeleteFileMethodCalledBeforeCopyFileMethod = !IsCopyFileMethodCalled;

            // ������� �� ����������� ���� � ���� filePath:
            _filesRepo.Remove(filePath);
        }

        /// <summary>
        /// �������� ������ ����� ����� ����������� �� �������
        /// </summary>
        /// <param name="existCopyFromFilePath">���� � ������������� �����, ����� �������� ����� ���������</param>
        /// <param name="createCopyToFilePath">���� � ����� ������� ����� ��������� (� � ������� ����� ����������</param>
        public void CopyFile(string existCopyFromFilePath, string createCopyToFilePath)
        {
            // ����������� ���������-�������� ����������:
            IsCopyFileMethodCalled = true;
            CopyFileMethod_ExistCopyFromFilePath = existCopyFromFilePath;
            CopyFileMethod_CreateCopyToFilePath = createCopyToFilePath;

            // � ����������� ��������� ����� ���� � ���� createCopyToFilePath
            // � ����������� �� ����� � ���� existCopyFromFilePath:
            _filesRepo.Add(createCopyToFilePath, _filesRepo[existCopyFromFilePath]);
        }

        /// <summary>
        /// ���������� ���������� �����
        /// </summary>
        /// <param name="filePath">���� � ����� ��� ������ ����������</param>
        /// <returns>���������� �����</returns>
        public FileMetadata GetFileMetadata(string filePath)
        {
            // ����������� ���������-�������� ����������:
            IsGetFileMetadataMethodCalled = true;
            GetFileMetadataMethod_FilePath = filePath;

            // ������ ���������� ����� �� ����������� � ������
            return _filesRepo[filePath];
        }
    }
}