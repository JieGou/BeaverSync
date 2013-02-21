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
    /// Тестовый mock объект IFileSystemManager
    /// </summary>
    public class MockFileSystemManager : IFileSystemManager
    {
        /// <summary>
        /// Флаг был ли вызван метод DeleteFile
        /// </summary>
        public bool IsDeleteFileMethodCalled = false;
        /// <summary>
        /// Путь переданный в метод DeleteFile
        /// </summary>
        public string DeleteFileMethod_FilePath = null;
        /// <summary>
        /// Флаг был ли вызван метод CopyFile
        /// </summary>
        public bool IsCopyFileMethodCalled = false;
        /// <summary>
        /// Параметр existCopyFromFilePath метода CopyFile
        /// </summary>
        public string CopyFileMethod_ExistCopyFromFilePath = null;
        /// <summary>
        /// Параметр createCopyToFilePath метода CopyFile
        /// </summary>
        public string CopyFileMethod_CreateCopyToFilePath = null;
        /// <summary>
        /// Флаг был ли вызван метод DeleteFile перед вызовом метода CopyFile
        /// </summary>
        public bool IsDeleteFileMethodCalledBeforeCopyFileMethod = false;

        /// <summary>
        /// Удаление файла
        /// </summary>
        /// <param name="filePath">полный путь к файлу</param>
        public void DeleteFile(string filePath)
        {
            IsDeleteFileMethodCalled = true;
            DeleteFileMethod_FilePath = filePath;
            IsDeleteFileMethodCalledBeforeCopyFileMethod = !IsCopyFileMethodCalled;
        }

        /// <summary>
        /// Создание нового файла путем копирования из другого
        /// </summary>
        /// <param name="existCopyFromFilePath">Путь к существующему файлу, копию которого будем создавать</param>
        /// <param name="createCopyToFilePath">Путь к файлу который будем создавать (и в который будем копировать</param>
        public void CopyFile(string existCopyFromFilePath, string createCopyToFilePath)
        {
            IsCopyFileMethodCalled = true;
            CopyFileMethod_ExistCopyFromFilePath = existCopyFromFilePath;
            CopyFileMethod_CreateCopyToFilePath = createCopyToFilePath;
        }

        public SyncFile GetSyncFileMetadata(string filePath)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// Класс тестирующий функциональность SyncFilesPair
    /// </summary>
    [TestClass]
    public class SyncFilesPairTest
    {
        private SyncFilesPair _sfp;
        private MockFileSystemManager _injectedManager;
        private readonly SyncFile file1 = new SyncFile(@"C:\some_folder\some_file1.csv", new DateTime(2013, 1, 5), 10);
        private readonly SyncFile file2 = new SyncFile(@"D:\some_folder2\stuff\some_file1.csv", new DateTime(2013, 1, 7), 15);
        private readonly SyncFile file2WithDifferName = new SyncFile(@"D:\some_folder2\stuff\some_file1_with_differ_name.csv", new DateTime(2013, 1, 7), 17);
        private readonly SyncFile file2WithDifferExtension = new SyncFile(@"D:\some_folder2\stuff\some_file1.with_differ_extension", new DateTime(2013, 1, 7), 18);

        /// <summary>
        /// Set up mocks
        /// </summary>
        [TestInitialize]
        public void InitializeTest()
        {
            // делаем иньекцию тестового менеджера файловой системы
            _injectedManager = new MockFileSystemManager();
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
            _sfp.SetFirstFile(file1);
            Assert.AreEqual(_sfp.FirstFile, file1);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void SetSecondFile_Null_GotException()
        {
            _sfp.SetFirstFile(file1);
            _sfp.SetSecondFile(null);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void SetSecondFile_NotSetFirstFileBefore_GotException()
        {
            _sfp.SetSecondFile(file2);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void SetSecondFile_SecondFileNameDifferFirstFileName_GotException()
        {
            _sfp.SetFirstFile(file1);
            _sfp.SetSecondFile(file2WithDifferName);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void SetSecondFile_SecondFileExtensionDifferFirstFileExtension_GotException()
        {
            _sfp.SetFirstFile(file1);
            _sfp.SetSecondFile(file2WithDifferExtension);
        }

        [TestMethod]
        public void SetSecondFile_CorrectSecondFile_NoException()
        {
            _sfp.SetFirstFile(file1);
            _sfp.SetSecondFile(file2);
            Assert.AreEqual(_sfp.SecondFile, file2);
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
            _sfp.SetFirstFile(file1);
            _sfp.Sync();
        }

        [TestMethod]
        public void Sync_AllFilesSet_NoException()
        {
            _sfp.SetFirstFile(file1);
            _sfp.SetSecondFile(file2);
            _sfp.Sync();

            Assert.IsTrue(_injectedManager.IsDeleteFileMethodCalled, "Не удалили неактуальный файл в синхропаре");
            Assert.IsTrue(_injectedManager.IsCopyFileMethodCalled, "Не скопировали актуальный файл на место неактуального в синхропаре");
            Assert.IsTrue(_injectedManager.IsDeleteFileMethodCalledBeforeCopyFileMethod, "Не удалили неактуальный файл перед копированием актуального файла на его место");

            Assert.AreEqual(_sfp.FirstFile.LastModified, _sfp.SecondFile.LastModified, "После синхронизации метаданные файлов не совпадают - поле Дата изменения");
            Assert.AreEqual(_sfp.FirstFile.ByteSize, _sfp.SecondFile.ByteSize, "После синхронизации метаданные файлов не совпадают - поле Размер файла в байтах");
        }
    }
}
