using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using BeaverSyncLib;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BeaverSyncTest
{
    /// <summary>
    /// Класс тестирующий функциональность SyncFile
    /// </summary>
    [TestClass]
    public class SyncFileTest
    {
        private MockFileSystemManager _injectedManager;
        private SyncFile _file1;
        private readonly int _file1ByteSize = 10;
        private readonly DateTime _file1LastModified = new DateTime(2013, 1, 5);

        /// <summary>
        /// Set up mocks
        /// </summary>
        [TestInitialize]
        public void InitializeTest()
        {
            // создаём тестовый набор данных:
            var file1Path = @"C:\some_folder\some_file1.csv";
            var file1Meta = new FileMetadata { ByteSize = _file1ByteSize, LastModified = _file1LastModified };
            
            var filesRepo = new SortedList<string, FileMetadata>
            {
                {file1Path, file1Meta}
            };

            // делаем иньекцию тестового менеджера файловой системы
            _injectedManager = new MockFileSystemManager(filesRepo);
            _file1 = new SyncFile(file1Path, _injectedManager);
        }

        [TestMethod]
        public void RetrieveFileMetadata_NoException()
        {
            var meta = _file1.RetrieveFileMetadata();

            Assert.IsTrue(_injectedManager.IsGetFileMetadataMethodCalled, "Не использовали менеджер файловой системы IFileSystemManager для считывания метаданных файла");
            Assert.AreEqual(_injectedManager.GetFileMetadataMethod_FilePath, _file1.FullPath, "Неверный путь к файлу при попытке считать метаданные файла через IFileSystemManager");

            Assert.AreEqual(meta.ByteSize, _file1ByteSize, "Получены неверные метаданные от MockFileSystemManager! Проверить заполнение тестовых данных.");
            Assert.AreEqual(meta.LastModified, _file1LastModified, "Получены неверные метаданные от MockFileSystemManager! Проверить заполнение тестовых данных.");
        }
    }
}
