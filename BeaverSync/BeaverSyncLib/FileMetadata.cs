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

        public override string ToString()
        {
            return String.Format("LastModified = {0:dd.MM.yyyy HH:mm:ss}, ByteSize = {1}", LastModified, ByteSize);
        }
    }
}