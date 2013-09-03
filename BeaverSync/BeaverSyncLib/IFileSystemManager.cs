using System;

namespace BeaverSyncLib
{
    /// <summary>
    /// Интерфейс менеджера файловой системы
    /// </summary>
    public interface IFileSystemManager
    {
        /// <summary>
        /// Удаление файла
        /// </summary>
        /// <param name="filePath">полный путь к файлу</param>
        void DeleteFile(string filePath);

        /// <summary>
        /// Создание нового файла путем копирования из другого
        /// </summary>
        /// <param name="existCopyFromFilePath">Путь к существующему файлу, копию которого будем создавать</param>
        /// <param name="createCopyToFilePath">Путь к файлу который будем создавать (и в который будем копировать</param>
        void CopyFile(string existCopyFromFilePath, string createCopyToFilePath);

        /// <summary>
        /// Считывание метаданных файла
        /// </summary>
        /// <param name="filePath">Путь к файлу для чтения метаданных</param>
        /// <returns>метаданные файла</returns>
        FileMetadata GetFileMetadata(string filePath);
    }

    /// <summary>
    /// Метаданные файла
    /// </summary>
    public struct FileMetadata
    {
        /// <summary>
        /// Дата изменения
        /// </summary>
        public DateTime LastModified { get; set; }
        /// <summary>
        /// Размер файла в байтах
        /// </summary>
        public long ByteSize { get; set; }

        public override string ToString()
        {
            return String.Format("LastModified = {0:dd.MM.yyyy HH:mm:ss}, ByteSize = {1}", LastModified, ByteSize);
        }
    }
}