using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
using _3D66.Download.DataFile;
using _3D66.Download.Model;
using _3D66.Download.Scripts;
using CefSharp;
using CefSharp.Wpf;
using Newtonsoft.Json;

namespace _3D66.Download
{
    /// <summary>
    /// AutoGetModelForm.xaml 的交互逻辑
    /// </summary>
    public partial class AutoGetModelForm : Window
    {
        private Thread AutoThread { get; set; }
        //private const String MainAddress = "https://so.3d66.com/res/_1_1_1_0.html";
        private const String MainAddress = "https://www.3d66.com/model/1_0_0_0_4_1_0_1.html";
        private bool IsWebLoaded { get; set; } = false;
        private static Random rd = new Random();
        public AutoGetModelForm()
        {
            InitializeComponent();
            DataControl.ReadDownloadedeIDs();
            Web.Address = MainAddress;
            Web.FrameLoadEnd += Web_FrameLoadEnd;
            AutoThread = new Thread(() =>
            {
                while (true)
                {
                    EvalScript("GetAllModel", json =>
                    {
                        dynamic js = json.Result;
                        foreach (dynamic r in js)
                        {
                            try
                            {

                                LiuLiuModel model = new LiuLiuModel()
                                {
                                    ID = Convert.ToInt32(r.ll_id),
                                    Image = r.img,
                                    KeyWord = r.key,
                                    Name = r.name,
                                };
                                if (DataControl.DownloadedIDs.Where(id => id == model.ID).Count() > 0) continue;
                                Dispatcher.Invoke(() =>
                                {
                                    MainWindow.MainDownloadWindow.WaitDownload(model);
                                });
                            }
                            catch
                            {
                                Dispatcher.Invoke(() =>
                                {
                                    MainWindow.MainDownloadWindow.ShowQuestion("下载错误" + js ?? "");
                                });
                            }
                            Thread.Sleep(10000 + rd.Next(1000, 8000));
                        }
                    });
                    EvalScript("NextPage", j => { });
                    Thread.Sleep(5000);
                }
            });
        }

        private void Web_FrameLoadEnd(object sender, FrameLoadEndEventArgs e)
        {
            IsWebLoaded = true;
            Start();
        }



        public void Start()
        {
            if (!IsWebLoaded)
            {
                throw new Exception("无法在浏览器未加载时进行操作");
            }
            AutoThread.Start();
        }
        /// <summary>
        /// 执行Script
        /// </summary>
        public void EvalScript(String ScriptName, Action<JavascriptResponse> CallBack)
        {
            var s = Web.EvaluateScriptAsync(ScriptControl.GetScript(ScriptName));
            s.Wait();
            CallBack?.Invoke(s.Result);
        }
        protected override void OnClosed(EventArgs e)
        {
            AutoThread.Abort();
            base.OnClosed(e);
        }
    }
}
