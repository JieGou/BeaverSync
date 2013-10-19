using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BeaverSyncLib
{
    /// <summary>
    /// Метаданные файла необходимые для синхронизации
    /// </summary>
    public class SyncFile
    {
        /// <summary>
        /// Объект для работы с файловой системой
        /// </summary>
        private readonly IFileSystemManager _manager;

        /// <summary>
        /// Полный путь к файлу
        /// </summary>
        public string FullPath { get; private set; }

        /// <summary>
        /// Конструктор
        /// </summary>
        public SyncFile(string fullPath)
            :this(fullPath, FileSystemManager.Instance)
        {
        }

        /// <summary>
        /// Тест-конструктор
        /// </summary>
        public SyncFile(string fullPath, IFileSystemManager manager)
        {
            // инициализируем класс менеджера файловой системы:
            _manager = manager;
            // проставляем путь к файлу
            FullPath = fullPath;
        }

        /// <summary>
        /// Пересчитывание метаданных с помощью менеджера файловой системы
        /// </summary>
        public FileMetadata RetrieveFileMetadata()
        {
            var meta = _manager.GetFileMetadata(this.FullPath);
            return meta;
        }
    }
}
