using System;

namespace BeaverSyncLib
{
    /// <summary>
    /// ������� ������ DateTime.Now 
    /// ����� ���� �������������� ��� ������
    /// <remarks>�������, ��� ������������ ��������, �� �� ���������� �� DI ��� ����� �������..)</remarks>
    /// </summary>
    public static class SystemTime
    {
        /// <summary>
        /// ����� ������ ����������������� �������� 
        /// ������:
        /// SystemTime.Now = () => new DateTime(2013, 01, 07, 15, 50, 10);
        /// </summary>
        public static Func<DateTime> Now = () => DateTime.Now;
    }
}