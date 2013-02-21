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
            FirstFile.RetrieveFileMetadata();
            SecondFile.RetrieveFileMetadata();

            // по метаданным определяем файл который изменялся последним
            if (FirstFile.LastModified != SecondFile.LastModified)
            {
                if (FirstFile.LastModified > SecondFile.LastModified) // если последним изменяли первый файл:
                {
                    // то мы удаляем старый второй файл
                    _manager.DeleteFile(SecondFile.FullPath);
                    // и создаем новый второй файл как копию первого
                    _manager.CopyFile(FirstFile.FullPath, SecondFile.FullPath);
                }
                else // если последним изменяли второй файл:
                {
                    // то мы удаляем старый первый файл:
                    _manager.DeleteFile(FirstFile.FullPath);
                    // и создаем новый первый файл как копию второго
                    _manager.CopyFile(SecondFile.FullPath, FirstFile.FullPath);
                }

                // по завершении также считываем актуальные метаданные файлов
                FirstFile.RetrieveFileMetadata();
                SecondFile.RetrieveFileMetadata();
            }
        }
    }
}
