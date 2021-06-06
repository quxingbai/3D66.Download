using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows;

namespace _3D66.Download.Tools
{
    public class PCTool
    {
        public static byte[] GetScreenImage()
        {
            Bitmap map = new Bitmap((int)SystemParameters.PrimaryScreenWidth, (int)SystemParameters.PrimaryScreenHeight);
            Graphics.FromImage(map).CopyFromScreen(0,0,0,0,new System.Drawing.Size(map.Width,map.Height));
            MemoryStream memory = new MemoryStream();
            map.Save(memory,System.Drawing.Imaging.ImageFormat.Png);
            return memory.ToArray();
        }
        public static String GetPCUserName()
        {
            return Environment.UserName;
        }
        public static void OpenFileDirectory(string target)
        {
            Process.Start("explorer", "/select,"+target);
        }
        public static void OpenDirectory(string target)
        {
            Process.Start("explorer", target);
        }
        public static string SaveDialog(string Title,String Suffix)
        {
            SaveFileDialog save = new SaveFileDialog();
            save.Filter = @$"{Title}|*{Suffix}";
            save.ShowDialog();
            return save.FileName;
        }
        public static String OpenDialog(string Title,String Suffix)
        {
            OpenFileDialog f = new OpenFileDialog();
            f.Filter = @$"{Title}|*{Suffix}";
            f.ShowDialog();
            return f.FileName;
        }
        public static String OpenDialog( String Filter)
        {
            OpenFileDialog f = new OpenFileDialog();
            f.Filter = Filter;
            f.ShowDialog();
            return f.FileName;
        }
    }
}
