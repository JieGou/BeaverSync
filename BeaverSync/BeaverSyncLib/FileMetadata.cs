using System;

namespace BeaverSyncLib
{
    /// <summary>
    /// ���������� �����
    /// </summary>
    public struct FileMetadata
    {
        /// <summary>
        /// ���� ���������
        /// </summary>
        public DateTime LastModified { get; set; }
        /// <summary>
        /// ������ ����� � ������
        /// </summary>
        public long ByteSize { get; set; }

        public override string ToString()
        {
            return String.Format("LastModified = {0:dd.MM.yyyy HH:mm:ss}, ByteSize = {1}", LastModified, ByteSize);
        }
    }
}