using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Windows;

namespace _3D66.Download.Tools
{
    public class WinRARTool
    {
         static WinRARTool()
        {
            if (!Exist())
            {
                if(MessageBox.Show("未安装WinRar，是否安装","确认", MessageBoxButton.YesNo)== MessageBoxResult.Yes)
                {
                    Setup();
                }
            }
        }
        public static String RARPath()
        {
            var key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\WinRAR");
            string ks = key.GetValue("exe64").ToString();
            key.Close();
            return ks;
        }
        public static bool CompressRar(string from, string to)
        {
            try
            {
                string cmd = string.Format(" e {0} {1}", from, to);
                string rarpath = RARPath();
                Process.Start(new ProcessStartInfo()
                {
                    FileName = rarpath,
                    Arguments = cmd,
                });
                return true;
            }
            catch (Exception error)
            {
                return false;
            }
        }
        public static bool CompressRar_D(string from, string to)
        {
            try
            {
                string cmd = string.Format(" x {0} {1}", from, to);
                string rarpath = RARPath();
                Process.Start(new ProcessStartInfo()
                {
                    FileName = rarpath,
                    Arguments = cmd,
                });
                return true;
            }
            catch (Exception error)
            {
                return false;
            }
        }
        public static void Setup()
        {
            FileInfo f = new FileInfo(Add66DownloadItemForm.Setting.Path_WinRARSetup);
            if (f.Exists)
            {
                Process.Start(Add66DownloadItemForm.Setting.Path_WinRARSetup);
            }
            else
            {
                throw new Exception("安装包不存在");
            }
        }
        public static bool Exist()
        {
            try
            {
                return new FileInfo(RARPath()).Exists;
            }
            catch
            {
                return false;
            }
        }
    }
}
