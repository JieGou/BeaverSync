using System;
using System.Collections.Generic;
using BeaverSyncLib;

namespace BeaverSyncTest
{
    /// <summary>
    /// Тестовый mock объект IFileSystemManager
    /// </summary>
    [Obsolete]
    public class MockFileSystemManager : IFileSystemManager
    {
        /// <summary>
        /// Флаг был ли вызван метод DeleteFile
        /// </summary>
        public bool IsDeleteFileMethodCalled = false;
        /// <summary>
        /// Путь переданный в метод DeleteFile
        /// </summary>
        public string DeleteFileMethod_FilePath = null;
        /// <summary>
        /// Флаг был ли вызван метод CopyFile
        /// </summary>
        public bool IsCopyFileMethodCalled = false;
        /// <summary>
        /// Параметр existCopyFromFilePath метода CopyFile
        /// </summary>
        public string CopyFileMethod_ExistCopyFromFilePath = null;
        /// <summary>
        /// Параметр createCopyToFilePath метода CopyFile
        /// </summary>
        public string CopyFileMethod_CreateCopyToFilePath = null;
        /// <summary>
        /// Флаг был ли вызван метод DeleteFile перед вызовом метода CopyFile
        /// </summary>
        public bool IsDeleteFileMethodCalledBeforeCopyFileMethod = false;
        /// <summary>
        /// Флаг был ли вызван метод GetFileMetadata
        /// </summary>
        public bool IsGetFileMetadataMethodCalled = false;
        /// <summary>
        /// Путь переданный в метод GetFileMetadata
        /// </summary>
        public string GetFileMetadataMethod_FilePath = null;
        

        /// <summary>
        /// Репозиторий файлов в памяти
        /// </summary>
        private readonly SortedList<string, FileMetadata> _filesRepo;

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="filesRepository">тестовые данные в виде репозитория файлов
        /// (ключ - путь к файлу, значение - метаданные файла)</param>
        public MockFileSystemManager(SortedList<string, FileMetadata> filesRepository)
        {
            _filesRepo = filesRepository;
        }

        /// <summary>
        /// Удаление файла
        /// </summary>
        /// <param name="filePath">полный путь к файлу</param>
        public void DeleteFile(string filePath)
        {
            // проставляем отладочно-тестовую информацию:
            IsDeleteFileMethodCalled = true;
            DeleteFileMethod_FilePath = filePath;
            IsDeleteFileMethodCalledBeforeCopyFileMethod = !IsCopyFileMethodCalled;

            // удаляем из репозиторий файл с путём filePath:
            _filesRepo.Remove(filePath);
        }

        /// <summary>
        /// Создание нового файла путем копирования из другого
        /// </summary>
        /// <param name="existCopyFromFilePath">Путь к существующему файлу, копию которого будем создавать</param>
        /// <param name="createCopyToFilePath">Путь к файлу который будем создавать (и в который будем копировать</param>
        public void CopyFile(string existCopyFromFilePath, string createCopyToFilePath)
        {
            // проставляем отладочно-тестовую информацию:
            IsCopyFileMethodCalled = true;
            CopyFileMethod_ExistCopyFromFilePath = existCopyFromFilePath;
            CopyFileMethod_CreateCopyToFilePath = createCopyToFilePath;

            // в репозиторий добавляем новый файл с путём createCopyToFilePath
            // и метаданными из файла с путём existCopyFromFilePath:
            _filesRepo.Add(createCopyToFilePath, _filesRepo[existCopyFromFilePath]);
        }

        /// <summary>
        /// Считывание метаданных файла
        /// </summary>
        /// <param name="filePath">Путь к файлу для чтения метаданных</param>
        /// <returns>метаданные файла</returns>
        public FileMetadata GetFileMetadata(string filePath)
        {
            // проставляем отладочно-тестовую информацию:
            IsGetFileMetadataMethodCalled = true;
            GetFileMetadataMethod_FilePath = filePath;

            // достаём метаданные файла из репозитория в памяти
            return _filesRepo[filePath];
        }
    }
}