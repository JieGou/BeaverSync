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
        /// <param name="fullPath"></param>
        /// <param name="lastModified"></param>
        /// <param name="byteSize"></param>
        public SyncFile(string fullPath, DateTime lastModified, int byteSize)
        {
            FullPath = fullPath;
            LastModified = lastModified;
            ByteSize = byteSize;
        }

        /// <summary>
        /// Пересчитывание метаданных с помощью менеджера файловой системы
        /// </summary>
        public void RetrieveFileMetadata()
        {
        }
    }
}
