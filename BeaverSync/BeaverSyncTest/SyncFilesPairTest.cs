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
        /// Репозиторий файлов в памяти
        /// </summary>
        private readonly SortedList<string, FileMetadata> _filesRepo;
        public MockFileSystemManager(SortedList<string, FileMetadata> filesRepository)
        {
            _filesRepo = filesRepository;
        }

        /// <summary>
        /// Удаление файла
        /// </summary>
        /// <param name="filePath">полный путь к файлу</param>
        public void DeleteFile(string filePath)
        {
            // проставляем отладочно-тестовую информацию:
            IsDeleteFileMethodCalled = true;
            DeleteFileMethod_FilePath = filePath;
            IsDeleteFileMethodCalledBeforeCopyFileMethod = !IsCopyFileMethodCalled;

            // удаляем из репозиторий файл с путём filePath:
            _filesRepo.Remove(filePath);
        }

        /// <summary>
        /// Создание нового файла путем копирования из другого
        /// </summary>
        /// <param name="existCopyFromFilePath">Путь к существующему файлу, копию которого будем создавать</param>
        /// <param name="createCopyToFilePath">Путь к файлу который будем создавать (и в который будем копировать</param>
        public void CopyFile(string existCopyFromFilePath, string createCopyToFilePath)
        {
            // проставляем отладочно-тестовую информацию:
            IsCopyFileMethodCalled = true;
            CopyFileMethod_ExistCopyFromFilePath = existCopyFromFilePath;
            CopyFileMethod_CreateCopyToFilePath = createCopyToFilePath;

            // в репозиторий добавляем новый файл с путём createCopyToFilePath
            // и метаданными из файла с путём existCopyFromFilePath:
            _filesRepo.Add(createCopyToFilePath, _filesRepo[existCopyFromFilePath]);
        }

        /// <summary>
        /// Считывание метаданных файла
        /// </summary>
        /// <param name="filePath">Путь к файлу для чтения метаданных</param>
        /// <returns>метаданные файла</returns>
        public FileMetadata GetFileMetadata(string filePath)
        {
            return _filesRepo[filePath];
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

            Assert.AreEqual(_sfp.FirstFile.LastModified, _sfp.SecondFile.LastModified, "После синхронизации метаданные файлов не совпадают - поле Дата изменения");
            Assert.AreEqual(_sfp.FirstFile.ByteSize, _sfp.SecondFile.ByteSize, "После синхронизации метаданные файлов не совпадают - поле Размер файла в байтах");
        }
    }
}
