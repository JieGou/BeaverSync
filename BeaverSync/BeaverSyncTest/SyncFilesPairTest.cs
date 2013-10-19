using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using BeaverSyncLib;
using FakeItEasy;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BeaverSyncTest
{
    /// <summary>
    /// Класс тестирующий функциональность SyncFilesPair
    /// </summary>
    [TestClass]
    public class SyncFilesPairTest
    {
        /// <summary>
        /// Объект синхропара для тестирования
        /// </summary>
        private SyncFilesPair _sfp;
        /// <summary>
        /// Тестовый менеджер файловой системы
        /// </summary>
        private IFileSystemManager _injectedManager;
        /// <summary>
        /// Файл номер 1, неактуальный (более старый)
        /// </summary>
        private SyncFile _nonActualFile;
        /// <summary>
        /// Файл номер 2, актуальный (более новый)
        /// </summary>
        private SyncFile _actualFile;
        private SyncFile _actualFileWithDifferName;
        private SyncFile _actualFileWithDifferExtension;

        /// <summary>
        /// Значение заменитель для вызовов DateTime.Now в рамках тестов
        /// </summary>
        private readonly DateTime _testDateTimeNow = new DateTime(2013, 01, 07, 15, 50, 10);

        /// <summary>
        /// Set up mocks
        /// </summary>
        [TestInitialize]
        public void InitializeTest()
        {
            // создаём тестовый набор данных:

            // иньекция системного времени:
            SystemTime.Now = () => _testDateTimeNow;

            // файл номер 1
            var file1Path = @"C:\some_folder\some_file1.csv"; // путь
            var file1Meta = new FileMetadata {ByteSize = 10, LastModified = new DateTime(2013, 1, 5)}; // свойства

            // файл номер 2, три файла(три пути) с одинаковыми свойствами
            var file2Path = @"D:\some_folder2\stuff\some_file1.csv";
            var file2WithDifferNamePath = @"D:\some_folder2\stuff\some_file1_with_differ_name.csv";
            var file2WithDifferExtensionPath = @"D:\some_folder2\stuff\some_file1.with_differ_extension";
            var file2Meta = new FileMetadata { ByteSize = 15, LastModified = new DateTime(2013, 1, 7) };
            
            // создаем тестовый менеджер файловой системы
            _injectedManager = A.Fake<IFileSystemManager>();
            // прописываем какие свойства (матаданные) он должен возвращать по каждому файлу
            A.CallTo(() => _injectedManager.GetFileMetadata(file1Path)).Returns(file1Meta);
            A.CallTo(() => _injectedManager.GetFileMetadata(file2Path)).Returns(file2Meta);
            A.CallTo(() => _injectedManager.GetFileMetadata(file2WithDifferNamePath)).Returns(file2Meta);
            A.CallTo(() => _injectedManager.GetFileMetadata(file2WithDifferExtensionPath)).Returns(file2Meta);

            #region делаем иньекцию тестового менеджера файловой системы:

            // создаем объекты SyncFile для файлов:
            // объект SyncFile для файла номер 1, неактуальный (более старый)
            _nonActualFile = new SyncFile(file1Path, _injectedManager);
            // объект SyncFile для файла номер 2, актуальный (более новый)
            _actualFile = new SyncFile(file2Path, _injectedManager);

            //  объект SyncFile для файла номер 2, актуальный (более новый) с другим именем
            _actualFileWithDifferName = new SyncFile(file2WithDifferNamePath, _injectedManager);
            //  объект SyncFile для файла номер 2, актуальный (более новый) с другим расширением
            _actualFileWithDifferExtension = new SyncFile(file2WithDifferExtensionPath, _injectedManager);

            _sfp = new SyncFilesPair(_injectedManager);
            #endregion
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void SetFirstFile_Null_GotException()
        {
            // Act
            _sfp.SetFirstFile(null);
        }

        [TestMethod]
        public void SetFirstFile_CorrectSyncFile_NoException()
        {
            // Act
            _sfp.SetFirstFile(_nonActualFile);

            // Assert
            Assert.AreEqual(_sfp.FirstFile, _nonActualFile);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void SetSecondFile_Null_GotException()
        {
            // Arange
            _sfp.SetFirstFile(_nonActualFile);

            // Act
            _sfp.SetSecondFile(null);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void SetSecondFile_NotSetFirstFileBefore_GotException()
        {
            // Act
            _sfp.SetSecondFile(_actualFile);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void SetSecondFile_SecondFileNameDifferFirstFileName_GotException()
        {
            // Arange
            _sfp.SetFirstFile(_nonActualFile);

            // Act
            _sfp.SetSecondFile(_actualFileWithDifferName);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void SetSecondFile_SecondFileExtensionDifferFirstFileExtension_GotException()
        {
            // Arange
            _sfp.SetFirstFile(_nonActualFile);

            // Act
            _sfp.SetSecondFile(_actualFileWithDifferExtension);
        }

        [TestMethod]
        public void SetSecondFile_CorrectSecondFile_NoException()
        {
            // Arange
            _sfp.SetFirstFile(_nonActualFile);

            // Act
            _sfp.SetSecondFile(_actualFile);

            // Assert
            Assert.AreEqual(_sfp.SecondFile, _actualFile);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Sync_NotSetBothFiles_GotException()
        {
            // Act
            _sfp.Sync();
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Sync_NotSetSecondFile_GotException()
        {
            // Arrange
            _sfp.SetFirstFile(_nonActualFile);

            // Act
            _sfp.Sync();
        }

        [TestMethod]
        public void Sync_AllFilesSetAndActualIsSecond_NoException()
        {
            // Arrange
            _sfp.SetFirstFile(_nonActualFile);
            _sfp.SetSecondFile(_actualFile); // актуальный второй

            // Act
            _sfp.Sync();

            // Assert
            SyncMethodInnerBehaviourAssert();
        }

        [TestMethod]
        public void Sync_AllFilesSetAndActualIsFirst_NoException()
        {
            // Arrange
            _sfp.SetFirstFile(_actualFile); // актуальный первый
            _sfp.SetSecondFile(_nonActualFile);

            // Act
            _sfp.Sync();

            // Assert
            SyncMethodInnerBehaviourAssert();
        }

        /// <summary>
        ///  (!) ниже сложный поведенческий ассерт для метода Sync,
        /// т.к. результат выполнения метода это состояние файловой системы
        /// мы проверяем внутреннее ожидаемое поведение метода, а именно
        /// порядок вызова методов менеджера файловой системы:
        /// </summary>
        private void SyncMethodInnerBehaviourAssert()
        {
            int i = 0; // переменная счетчик определяющего порядковый номер метода

            // 1. Проверяем, что бекап неактуального файла делается и делается до его перезаписи:
            A.CallTo(
                () =>
                _injectedManager.CopyFile(_nonActualFile.FullPath,
                String.Format("backup\\{0}[{1:yyyy-MM-dd hh-mm-ss}]{2}",
                Path.GetFileNameWithoutExtension(_nonActualFile.FullPath),
                _testDateTimeNow, Path.GetExtension(_nonActualFile.FullPath)))
                ).Invokes(() =>
                {
                    //  - 1.1 проверяем что создание бекапа в самом начале
                    Assert.IsFalse(i == 0, "Неверный порядок вызовов. Бекап неактуального файла нужно делать в самом начале");
                    i++;
                }).MustHaveHappened(Repeated.Exactly.Once); // - 1.2 и что создание бекапа было только один раз

            // 2. Проверяем что удаляем неактуальный файл после того как его забекапим
            A.CallTo(
                () => _injectedManager.DeleteFile(_nonActualFile.FullPath)
                ).Invokes(() =>
                {
                    // - 2.1 удаляем после бекапа
                    Assert.IsFalse(i == 1, "Неверный порядок вызовов. Удалять неактуальный файл нужно после того как сделали его бекап, и перед тем как копировать на его место актуальный");
                    i++;
                }).MustHaveHappened(Repeated.Exactly.Once); // - 2.2 и что удалени было только один раз

            // 3. Проверяем что копируем актуальный после того как удалили неактуальный (и забекапили его)
            A.CallTo(
                () => _injectedManager.CopyFile(_actualFile.FullPath, _nonActualFile.FullPath)
                ).Invokes(() =>
                {
                    // - 3.1 копируем после удаления
                    Assert.IsFalse(i == 3, "Неверный порядок вызовов. Копировать актуальный файл нужно в самом конце");
                    i++;
                }).MustHaveHappened(Repeated.Exactly.Once); // - 3.2 и что копирование было только один раз
        }
        
    }
}
