using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace BeaverSyncLib
{
    /// <summary>
    /// Синхропара из двух файлов
    /// </summary>
    public class SyncFilesPair
    {
        /// <summary>
        /// Первый файл в синхропаре
        /// </summary>
        private SyncFile _file1;
        /// <summary>
        /// Второй файл в синхропаре
        /// </summary>
        private SyncFile _file2;
        /// <summary>
        /// Объект для работы с файловой системой
        /// </summary>
        private IFileSystemManager _manager;

        /// <summary>
        /// Флаг установлен ли первый файл синхропары
        /// </summary>
        private bool IsFirstFileSet
        {
            get { return _file1 != null; }
        }
        /// <summary>
        /// Флаг установлен ли второй файл синхропары
        /// </summary>
        private bool IsSecondFileSet
        {
            get { return _file2 != null; }
        }

        /// <summary>
        /// Первый файл в синхропаре
        /// </summary>
        public SyncFile FirstFile
        {
            get { return _file1; }
        }
        /// <summary>
        /// Второй файл в синхропаре
        /// </summary>
        public SyncFile SecondFile
        {
            get { return _file2; }
        }

        /// <summary>
        /// Конструктор
        /// </summary>
        public SyncFilesPair()
        {
            // инициализируем класс менеджера файловой системы:
            _manager = new FileSystemManager();
        }

        /// <summary>
        /// Тест-конструктор с иньекцией тестового mock объекта IFileSystemManager
        /// </summary>
        /// <param name="manager">IFileSystemManager объект</param>
        public SyncFilesPair(IFileSystemManager manager)
        {
            _manager = manager;
        }

        /// <summary>
        /// Установка первого файла синхропары
        /// </summary>
        /// <param name="file">метаописание SyncFile</param>
        public void SetFirstFile(SyncFile file)
        {
            if(file == null)
                throw new ArgumentNullException();

            _file1 = file;
        }

        /// <summary>
        /// Установка второго файла синхропары
        /// </summary>
        /// <param name="file">метаописание SyncFile</param>
        public void SetSecondFile(SyncFile file)
        {
            if (!IsFirstFileSet)
            {
                throw new InvalidOperationException("Не установлен первый файл в синхропаре.");
            }

            if (file == null)
            {
                throw new ArgumentNullException("file", "Некорретный файл для синхропары");
            }

            if (Path.GetFileName(file.FullPath) != Path.GetFileName(_file1.FullPath))
            {
                throw new ArgumentException("Некорретный файл для синхропары. Имя или тип второго файла отличны от имени или типа первого файла.", "file");
            }

            _file2 = file;
        }

        /// <summary>
        /// Синхронизация файлов в паре
        /// </summary>
        public void Sync()
        {
            if (!IsFirstFileSet)
            {
                throw new InvalidOperationException("Не установлен первый файл в синхропаре.");
            }
            if (!IsSecondFileSet)
            {
                throw new InvalidOperationException("Не установлен второй файл в синхропаре.");
            }

            // считываем актуальные метаданные файлов
            var meta1 = FirstFile.RetrieveFileMetadata();
            var meta2 = SecondFile.RetrieveFileMetadata();

            // по метаданным определяем файл который изменялся последним
            if (meta1.LastModified != meta2.LastModified)
            {
                if (meta1.LastModified > meta2.LastModified) // если последним изменяли первый файл:
                {
                    SyncTransaction(_manager, SecondFile, FirstFile);
                }
                else // если последним изменяли второй файл:
                {
                    SyncTransaction(_manager, FirstFile, SecondFile);
                }
            }
        }

        private static void SyncTransaction(IFileSystemManager manager, SyncFile nonActualFile, SyncFile actualFile)
        {
            // в самом начале делаем бекап неактуального файла в директории с исполняемым файлом
            manager.CopyFile(nonActualFile.FullPath,
                String.Format("backup/{0}[{1:yyyy-MM-dd hh:mm:ss}]{2}", 
                Path.GetFileNameWithoutExtension(nonActualFile.FullPath),
                SystemTime.Now(), Path.GetExtension(nonActualFile.FullPath)));

            // потом удаляем забекапленный неактуальный файл
            manager.DeleteFile(nonActualFile.FullPath);

            // и создаем новый файл как копию актуального на месте удаленного неактуального
            manager.CopyFile(actualFile.FullPath, nonActualFile.FullPath);
        }
    }
}
