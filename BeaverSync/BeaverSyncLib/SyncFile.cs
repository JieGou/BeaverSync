using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BeaverSyncLib
{
    /// <summary>
    /// Метаданные файла необходимые для синхронизации
    /// </summary>
    public class SyncFile //: ISyncFile
    {
        /// <summary>
        /// Объект для работы с файловой системой
        /// </summary>
        private IFileSystemManager _manager;

        /// <summary>
        /// Полный путь к файлу
        /// </summary>
        public string FullPath { get; private set; }
        /// <summary>
        /// Дата изменения
        /// </summary>
        public DateTime LastModified { get; private set; }
        /// <summary>
        /// Размер файла в байтах
        /// </summary>
        public int ByteSize { get; private set; }

        /// <summary>
        /// Конструктор
        /// </summary>
        public SyncFile(string fullPath)
        {
            // инициализируем класс менеджера файловой системы:
            _manager = new FileSystemManager();
            // проставляем путь к файлу
            FullPath = fullPath;
            // считываем метаданные
            RetrieveFileMetadata();
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
            // считываем метаданные
            RetrieveFileMetadata();
        }

        /// <summary>
        /// Пересчитывание метаданных с помощью менеджера файловой системы
        /// </summary>
        public void RetrieveFileMetadata()
        {
            var meta = _manager.GetFileMetadata(this.FullPath);

            this.LastModified = meta.LastModified;
            this.ByteSize = meta.ByteSize;
        }
    }
}
