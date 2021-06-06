using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace _3D66.Download.DataFile
{
    public class DataControl
    {
        private const String Path_DownloadedIds = "./DataFile/Downloaded.txt";
        public static List<int> DownloadedIDs = new List<int>();
        public static void WriteDownloaded(int[] ID)
        {
            FileStream FS_Downloaded = new FileStream(Path_DownloadedIds, FileMode.OpenOrCreate);
            StreamWriter SW_Downloaded = new StreamWriter(FS_Downloaded);
            String ids = "";
            foreach (int id in ID)
            {
                ids += id + ",";
            }
            ids.Remove(ids.Length);
            SW_Downloaded.WriteLine(ids);
            SW_Downloaded.Dispose();
        }
        public static void WriteDownloaded(int ID)
        {
            DownloadedIDs.Add(ID);
        }
        public static void SaveDownloadedIds()
        {
            Directory.CreateDirectory(new FileInfo(Path_DownloadedIds).DirectoryName);
            if(File.Exists(Path_DownloadedIds))File.Delete(Path_DownloadedIds);
            String ids = "";
            foreach (int id in DownloadedIDs)
            {
                ids += id + ",";
            }
            FileStream file = new FileStream(Path_DownloadedIds, FileMode.OpenOrCreate);
            byte[] bs = Encoding.UTF8.GetBytes(ids.Remove(ids.Length-1));
            file.Write(bs, 0, bs.Length);
            file.Dispose();
        }
        public static List<int> ReadDownloadedeIDs()
        {
            try
            {
                string text = File.ReadAllText(Path_DownloadedIds);
                DownloadedIDs = text.Split(',').Select(v => Convert.ToInt32(v)).ToList();
                return DownloadedIDs;
            }
            catch
            {
                return new List<int>();
            }
        }
    }
}
