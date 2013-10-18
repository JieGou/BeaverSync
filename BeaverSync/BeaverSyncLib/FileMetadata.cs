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
    }
}