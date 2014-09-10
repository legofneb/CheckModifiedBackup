using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheckModifiedBackup
{
    class Program
    {
        static void Main(string[] args)
        {
            const string DirectoryBackup = @"C:\test";
            const string DirectoryCurrent = @"C:\test2";
            const string SaveFileLocation = @"C:\file.csv";

            DirectoryInfo dir1 = new DirectoryInfo(DirectoryBackup);
            DirectoryInfo dir2 = new DirectoryInfo(DirectoryCurrent);

            IEnumerable<FileInfo> list1 = dir1.GetFiles("*", System.IO.SearchOption.AllDirectories);
            IEnumerable<FileInfo> list2 = dir2.GetFiles("*", System.IO.SearchOption.AllDirectories);

            if (File.Exists(SaveFileLocation))
            {
                File.Delete(SaveFileLocation); // Erasing previous file.
            }

            using (StreamWriter sw = new StreamWriter(SaveFileLocation))
            {

                foreach (FileInfo file1 in list1)
                {
                    if (!list2.Any(n => n.Name == file1.Name))
                    {
                        WriteToCSV(sw, file1, null, NewFile.NotFound);
                        continue;
                    }

                    FileInfo file2 = list2.Where(n => n.Name == file1.Name).FirstOrDefault();

                    if (file2.LastWriteTime < file1.LastWriteTime)
                    {
                        WriteToCSV(sw, file1, file2, NewFile.IsOlder);
                    }
                    else
                    {
                        WriteToCSV(sw, file1, file2, NewFile.Matches);
                    }
                }
            }
        }

        static void WriteToCSV(StreamWriter sw, FileInfo file1, FileInfo file2, NewFile FileStatus)
        {
            try
            {
                if (FileStatus == NewFile.NotFound)
                {
                    sw.WriteLine(String.Format("{0},,{1},N", file1.FullName, file1.LastWriteTime));
                }
                else if (FileStatus == NewFile.Matches)
                {
                    sw.WriteLine(String.Format("{0},{1},{2},{3},Y", file1.FullName, file2.FullName, file1.LastWriteTime, file2.LastWriteTime));
                }
                else if (FileStatus == NewFile.IsOlder)
                {
                    sw.WriteLine(String.Format("{0},{1},{2},{3},N", file1.FullName, file2.FullName, file1.LastWriteTime, file2.LastWriteTime));
                }
            }
            catch
            {
                // File too long exceptions, I haven't implemented a fix yet. 
            }
        }

        public enum NewFile { Matches, IsOlder, NotFound }
    }
}
