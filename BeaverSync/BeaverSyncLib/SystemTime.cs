using System;

namespace BeaverSyncLib
{
    /// <summary>
    /// Обертка вокруг DateTime.Now 
    /// может быть переопределена для тестов
    /// <remarks>понятно, что инкапсуляция нарушена, но не выдумывать же DI для такой малости..)</remarks>
    /// </summary>
    public static class SystemTime
    {
        /// <summary>
        /// Можно задать предустановленное значение 
        /// Пример:
        /// SystemTime.Now = () => new DateTime(2013, 01, 07, 15, 50, 10);
        /// </summary>
        public static Func<DateTime> Now = () => DateTime.Now;
    }
}