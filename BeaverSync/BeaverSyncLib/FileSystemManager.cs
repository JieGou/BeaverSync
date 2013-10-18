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
            File.Delete(filePath);
        }

        /// <summary>
        /// Создание нового файла путем копирования из другого
        /// </summary>
        /// <param name="existCopyFromFilePath">Путь к существующему файлу, копию которого будем создавать</param>
        /// <param name="createCopyToFilePath">Путь к файлу который будем создавать (и в который будем копировать</param>
        public void CopyFile(string existCopyFromFilePath, string createCopyToFilePath)
        {
            File.Copy(existCopyFromFilePath, createCopyToFilePath);
        }

        /// <summary>
        /// Считывание метаданных файла
        /// </summary>
        /// <param name="filePath">Путь к файлу для чтения метаданных</param>
        /// <returns>метаданные файла</returns>
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