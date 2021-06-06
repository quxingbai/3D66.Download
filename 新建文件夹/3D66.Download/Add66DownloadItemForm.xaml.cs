using _3D66.Download.Model;
using CefSharp;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace _3D66.Download
{
    /// <summary>
    /// Add66DownloadItemForm.xaml 的交互逻辑
    /// </summary>
    public partial class Add66DownloadItemForm : Window
    {
        private bool IsWebLoaded;
        public Add66DownloadItemForm()
        {
            InitializeComponent();
            WEB_OnLineBrowser.FrameLoadEnd += WEB_OnLineBrowser_FrameLoadEnd;
            LIST_OnLineModels.Items.Add(new MaxModel()
            {
                ID=1,
                Image= "https://pic.3d66.com/thuimg/newcache/1000/7152/715262_1072580.jpg",
                KeyWord="油漆罐",
                Name="油漆罐-1",
            });
            LIST_OnLineModels.Items.Add(new MaxModel()
            {
                ID = 1,
                Image = "https://pic.3d66.com/thuimg/newcache/1000/7152/715262_1072580.jpg",
                KeyWord = "油漆罐",
                Name = "油漆罐-1",
            });
            LIST_OnLineModels.Items.Add(new MaxModel()
            {
                ID = 1,
                Image = "https://pic.3d66.com/thuimg/newcache/1000/7152/715262_1072580.jpg",
                KeyWord = "油漆罐",
                Name = "油漆罐-1",
            });
        }

        private void WEB_OnLineBrowser_FrameLoadEnd(object sender, FrameLoadEndEventArgs e)
        {
            IsWebLoaded = true;
        }

        void DownloadStart(int ID)
        {
            IsWebLoaded = false;
            ThreadPool.QueueUserWorkItem(c=>
            {
                WEB_OnLineBrowser.Address = "https://www.3d66.com/reshtml/model/items/7M9L/7M9LgT9n.html?kw=" + ID;
                int timeout = 100000;
                int time = 0;
                while (!IsWebLoaded) { Thread.Sleep(100);if ((time += 100) >= timeout) return; } ;
                WEB_OnLineBrowser.EvaluateScriptAsync(Scripts.ScriptControl.GetScript("GetModelInfo")).Wait();

            });
        }
    }
}
