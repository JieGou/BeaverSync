using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using BeaverSyncLib;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BeaverSyncTest
{
    /// <summary>
    /// Класс тестирующий функциональность SyncFilesPair
    /// </summary>
    [TestClass]
    public class SyncFilesPairTest
    {
        private SyncFilesPair _sfp;
        private MockFileSystemManager _injectedManager;
        private SyncFile _file1;
        private SyncFile _file2;
        private SyncFile _file2WithDifferName;
        private SyncFile _file2WithDifferExtension;

        /// <summary>
        /// Set up mocks
        /// </summary>
        [TestInitialize]
        public void InitializeTest()
        {
            // создаём тестовый набор данных:
            var file1Path = @"C:\some_folder\some_file1.csv";
            var file1Meta = new FileMetadata {ByteSize = 10, LastModified = new DateTime(2013, 1, 5)};

            var file2Path = @"D:\some_folder2\stuff\some_file1.csv";
            var file2WithDifferNamePath = @"D:\some_folder2\stuff\some_file1_with_differ_name.csv";
            var file2WithDifferExtensionPath = @"D:\some_folder2\stuff\some_file1.with_differ_extension";
            var file2Meta = new FileMetadata { ByteSize = 15, LastModified = new DateTime(2013, 1, 7) };
            
            var filesRepo = new SortedList<string, FileMetadata>
            {
                {file1Path, file1Meta},  
                {file2Path, file2Meta},
                {file2WithDifferNamePath, file2Meta}, 
                {file2WithDifferExtensionPath, file2Meta}
            };

            // делаем иньекцию тестового менеджера файловой системы
            _injectedManager = new MockFileSystemManager(filesRepo);

            _file1 = new SyncFile(file1Path, _injectedManager);
            _file2 = new SyncFile(file2Path, _injectedManager);
            _file2WithDifferName = new SyncFile(file2WithDifferNamePath, _injectedManager);
            _file2WithDifferExtension = new SyncFile(file2WithDifferExtensionPath, _injectedManager);

            _sfp = new SyncFilesPair(_injectedManager);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void SetFirstFile_Null_GotException()
        {
            _sfp.SetFirstFile(null);
        }

        [TestMethod]
        public void SetFirstFile_CorrectSyncFile_NoException()
        {
            _sfp.SetFirstFile(_file1);
            Assert.AreEqual(_sfp.FirstFile, _file1);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void SetSecondFile_Null_GotException()
        {
            _sfp.SetFirstFile(_file1);
            _sfp.SetSecondFile(null);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void SetSecondFile_NotSetFirstFileBefore_GotException()
        {
            _sfp.SetSecondFile(_file2);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void SetSecondFile_SecondFileNameDifferFirstFileName_GotException()
        {
            _sfp.SetFirstFile(_file1);
            _sfp.SetSecondFile(_file2WithDifferName);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void SetSecondFile_SecondFileExtensionDifferFirstFileExtension_GotException()
        {
            _sfp.SetFirstFile(_file1);
            _sfp.SetSecondFile(_file2WithDifferExtension);
        }

        [TestMethod]
        public void SetSecondFile_CorrectSecondFile_NoException()
        {
            _sfp.SetFirstFile(_file1);
            _sfp.SetSecondFile(_file2);
            Assert.AreEqual(_sfp.SecondFile, _file2);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Sync_NotSetBothFiles_GotException()
        {
            _sfp.Sync();
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Sync_NotSetSecondFile_GotException()
        {
            _sfp.SetFirstFile(_file1);
            _sfp.Sync();
        }

        [TestMethod]
        public void Sync_AllFilesSet_NoException()
        {
            _sfp.SetFirstFile(_file1);
            _sfp.SetSecondFile(_file2);
            _sfp.Sync();

            Assert.IsTrue(_injectedManager.IsDeleteFileMethodCalled, "Не удалили неактуальный файл в синхропаре");
            Assert.IsTrue(_injectedManager.IsCopyFileMethodCalled, "Не скопировали актуальный файл на место неактуального в синхропаре");
            Assert.IsTrue(_injectedManager.IsDeleteFileMethodCalledBeforeCopyFileMethod, "Не удалили неактуальный файл перед копированием актуального файла на его место");

            var meta1 = _sfp.FirstFile.RetrieveFileMetadata();
            var meta2 = _sfp.FirstFile.RetrieveFileMetadata();
            Assert.AreEqual(meta1.LastModified, meta2.LastModified, "После синхронизации метаданные файлов не совпадают - поле Дата изменения");
            Assert.AreEqual(meta1.ByteSize, meta2.ByteSize, "После синхронизации метаданные файлов не совпадают - поле Размер файла в байтах");
        }
    }
}
