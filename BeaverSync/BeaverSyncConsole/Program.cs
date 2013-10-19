using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using BeaverSyncLib;

namespace BeaverSyncConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            var syncPairs = new List<SyncFilesPair>();
            var backupDir = System.Environment.CurrentDirectory + "\\backup";
            FileSystemManager.Instance.CreateDirIfNotExist(backupDir);

            try
            {
                var xDoc = XDocument.Load(System.Environment.CurrentDirectory + "\\beaverSync.xml");
                var xPairs = xDoc.Root.Elements("SyncFilesPair");

                foreach (var xPair in xPairs)
                {
                    var pair = new SyncFilesPair {BackupDirPath = backupDir, NeedBackup = true};
                    var file1 = new SyncFile(xPair.Element("FirstFilePath").Value);
                    var file2 = new SyncFile(xPair.Element("SecondFilePath").Value);
                    pair.SetFirstFile(file1);
                    pair.SetSecondFile(file2);

                    syncPairs.Add(pair);
                }

            }
            catch (Exception exc)
            {
                Console.WriteLine("Неверный файл конфигурации. Невозможно считать синхропары.", exc);
            }

            int i = 1;
            foreach (var pair in syncPairs)
            {
                Console.WriteLine("|SyncPair #{0}", i++);
                Console.WriteLine("|");
                Console.WriteLine("|   1. {0}", pair.FirstFile.FullPath);
                Console.WriteLine("|\t {0}", pair.FirstFile.RetrieveFileMetadata());
                Console.WriteLine("|");
                Console.WriteLine("|   2. {0}", pair.SecondFile.FullPath);
                Console.WriteLine("|\t {0}", pair.SecondFile.RetrieveFileMetadata());
                Console.WriteLine();
            }

            //Console.WriteLine("Press <enter> for continue a synchronization process...");
            //Console.ReadKey();

            i = 1;
            foreach (var pair in syncPairs)
            {
                try
                {
                    Console.WriteLine("Try to synchronize a SyncPair #{0}...", i);
                    pair.Sync();
                    Console.WriteLine("SyncPair #{0} synchronized success.", i);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("SyncPair #{0} synchronization failed :(", i);
                    Console.WriteLine(ex);
                }

                Console.WriteLine();
                i++;
            }

            i = 1;
            foreach (var pair in syncPairs)
            {
                Console.WriteLine("|SyncPair #{0}", i++);
                Console.WriteLine("|");
                Console.WriteLine("|   1. {0}", pair.FirstFile.FullPath);
                Console.WriteLine("|\t {0}", pair.FirstFile.RetrieveFileMetadata());
                Console.WriteLine("|");
                Console.WriteLine("|   2. {0}", pair.SecondFile.FullPath);
                Console.WriteLine("|\t {0}", pair.SecondFile.RetrieveFileMetadata());
                Console.WriteLine();
            }

            Console.WriteLine("Press <enter> for exit...");
            Console.ReadKey();
        }


    }

}
