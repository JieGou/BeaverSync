using System;

namespace BeaverSyncLib
{
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
    }
}