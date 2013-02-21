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
        SyncFile GetSyncFileMetadata(string filePath);
    }
}