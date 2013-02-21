using System;
using System.IO;

namespace BeaverSyncLib
{
    /// <summary>
    /// Менеджер файловой системы
    /// работает с пространством имён System.IO
    /// </summary>
    public class FileSystemManager : IFileSystemManager
    {
        /// <summary>
        /// Удаление файла
        /// </summary>
        /// <param name="filePath">полный путь к файлу</param>
        public void DeleteFile(string filePath)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Создание нового файла путем копирования из другого
        /// </summary>
        /// <param name="existCopyFromFilePath">Путь к существующему файлу, копию которого будем создавать</param>
        /// <param name="createCopyToFilePath">Путь к файлу который будем создавать (и в который будем копировать</param>
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