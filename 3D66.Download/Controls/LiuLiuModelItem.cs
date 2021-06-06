using _3D66.Download.DataFile;
using _3D66.Download.Model;
using _3D66.Download.Scripts;
using _3D66.Download.Tools;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace _3D66.Download.Controls
{
    public class LiuLiuModelItem : ListBoxItem
    {





       

        public int DownloadProcess
        {
            get { return (int)GetValue(DownloadProcessProperty); }
            set { SetValue(DownloadProcessProperty, value); }
        }

        // Using a DependencyProperty as the backing store for DownloadProcess.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DownloadProcessProperty =
            DependencyProperty.Register("DownloadProcess", typeof(int), typeof(LiuLiuModelItem), new PropertyMetadata(0));



        public LiuLiuModel LiuLiuModel
        {
            get { return (LiuLiuModel)GetValue(LiuLiuModelProperty); }
            set { SetValue(LiuLiuModelProperty, value); }
        }

        // Using a DependencyProperty as the backing store for LiuLiuModel.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty LiuLiuModelProperty =
            DependencyProperty.Register("LiuLiuModel", typeof(LiuLiuModel), typeof(LiuLiuModelItem), new PropertyMetadata(null));



        public ICommand CommandDownload
        {
            get { return (ICommand)GetValue(CommandDownloadProperty); }
            set { SetValue(CommandDownloadProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CommandDownload.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CommandDownloadProperty =
            DependencyProperty.Register("CommandDownload", typeof(ICommand), typeof(LiuLiuModelItem), new PropertyMetadata(null));



        public bool IsLocalHas
        {
            get { return (bool)GetValue(IsLocalHasProperty); }
            set { SetValue(IsLocalHasProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsLocalHas.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsLocalHasProperty =
            DependencyProperty.Register("IsLocalHas", typeof(bool), typeof(LiuLiuModelItem), new PropertyMetadata(false));


        public bool DownloadViewState
        {
            get { return (bool)GetValue(DownloadViewStateProperty); }
            set { SetValue(DownloadViewStateProperty, value); }
        }

        // Using a DependencyProperty as the backing store for DownloadViewState.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DownloadViewStateProperty =
            DependencyProperty.Register("DownloadViewState", typeof(bool), typeof(LiuLiuModelItem), new PropertyMetadata(false));



        public ICommand CommandCompressToProjectPath
        {
            get { return (ICommand)GetValue(CommandCompressToProjectPathProperty); }
            set { SetValue(CommandCompressToProjectPathProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CommandCompressToProjectPath.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CommandCompressToProjectPathProperty =
            DependencyProperty.Register("CommandCompressToProjectPath", typeof(ICommand), typeof(LiuLiuModelItem), new PropertyMetadata(null));



        public ICommand CommandDelete
        {
            get { return (ICommand)GetValue(CommandDeleteProperty); }
            set { SetValue(CommandDeleteProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CommandDelete.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CommandDeleteProperty =
            DependencyProperty.Register("CommandDelete", typeof(ICommand), typeof(LiuLiuModelItem), new PropertyMetadata(null));


        private WebClient WEB = new WebClient();
        private string DownloadPathTemp = "";
        public event Action<LiuLiuModelItem> Downloaded;
        static LiuLiuModelItem()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(LiuLiuModelItem), new FrameworkPropertyMetadata(typeof(LiuLiuModelItem)));
        }
        public LiuLiuModelItem()
        {
            WEB.DownloadProgressChanged += WEB_DownloadProgressChanged;
            WEB.DownloadFileCompleted += WEB_DownloadFileCompleted;
            WEB.Headers.Add(HttpRequestHeader.Cookie, CookieControl.GetCookie());
            WEB.Headers.Add(HttpRequestHeader.Referer, "https://user.3d66.com/");
            WEB.Headers.Add(HttpRequestHeader.UserAgent, WebHeaders.GetHeader());
            CommandDownload = new EasyCommand((obj) =>
              {
                  if (IsLocalHas)
                  {
                      string dic = new FileInfo(LiuLiuModel.LocalPath).DirectoryName;
                      Process.Start(new ProcessStartInfo() { CreateNoWindow=true,FileName="Explorer",Arguments= "/Select,"+LiuLiuModel.LocalPath });
                      return;
                  }
                  int did = LiuLiuModel.ID;
                  String name = LiuLiuModel.Name;
                  DownloadViewState = true;
                  ThreadPool.QueueUserWorkItem(c =>
                  {
                      Directory.CreateDirectory(Add66DownloadItemForm.Setting.Path_Download_LiuLiu);
                      string downloadTo = Add66DownloadItemForm.Setting.Path_Download_LiuLiu + name + "-" + did + ".rar";
                      DownloadPathTemp = downloadTo;
                      ToDownloadURLAsys(did, url =>
                      {
                          WEB.DownloadFileAsync(new Uri(url), downloadTo);
                      });
                  });
              });
            CommandCompressToProjectPath = new EasyCommand(obj =>
              {
                  if (Add66DownloadItemForm.Setting.IsAutoCompression == false)
                  {
                      return;
                  }
                  //解压
                  string toPath = Add66DownloadItemForm.Setting.Path_ProjectPath + "\\Models\\" + LiuLiuModel.Name + "-" + LiuLiuModel.ID + "";
                  Directory.CreateDirectory(toPath);
                  if (Add66DownloadItemForm.Setting.IsAutoCompression && !WinRARTool.CompressRar(LiuLiuModel.LocalPath, toPath))
                  {
                      MessageBox.Show("解压到工程目录失败");
                  }
              });
            CommandDelete = new EasyCommand(obj =>
              {
                  DataBaseXML.RemoveLiuLiuModel(LiuLiuModel.ID);
                  DataBaseXML.SelectALLLiuLiuModel();
              });
        }
        public string SuffixName()
        {
            switch (LiuLiuModel.ModelType)
            {
                case ModelType.Image:return ".jpg";break;
                case ModelType.Meterial: return ".rar";break;
                case ModelType.Model:return ".rar";break;
            }
            throw new Exception("无此后缀名");
        }
        private void WEB_DownloadFileCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            Dispatcher.Invoke(() =>
            {
                IsLocalHas = true;
                DownloadViewState = false;
                LiuLiuModel.LocalPath = DownloadPathTemp;
                DataBaseXML.AddLiuLiuModel(LiuLiuModel);
                DataBaseXML.ADDTag(LiuLiuModel.ID, LiuLiuModel.ID.ToString());
                Downloaded?.Invoke(this);

                CommandCompressToProjectPath.Execute(null);
            });
        }

        private void WEB_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            Dispatcher.Invoke(() =>
            {
                this.DownloadProcess = e.ProgressPercentage;
                DownloadViewState = true;
            });
        }



        /// <summary>
        /// 下载
        /// </summary>
        public void ToDownloadURLAsys(int ID, Action<String> CallBack)
        {
            ThreadPool.QueueUserWorkItem((s) =>
            {
                WebClient w = new WebClient();
                w.Headers.Add(HttpRequestHeader.Cookie, CookieControl.GetCookie());
                w.Headers.Add(HttpRequestHeader.Referer, "https://user.3d66.com/");
                w.Headers.Add(HttpRequestHeader.UserAgent, WebHeaders.GetHeader());
                try
                {
                    String j = "https://user.3d66.com/download/www_free_form?ll_id=" + ID;
                    string token = GetToKen(w.DownloadString(j));
                    String downloadUrl = "https://user.3d66.com/download/down_Handle/index?action=user_pay_download&ll_id=" + ID + "&rartype=1&needtype=1&action_id=&token=" + token + "&sotu_action_id=&kw=&rlai=&collect=0&dl_course=0";
                    w.Headers[HttpRequestHeader.Referer] = j;
                    dynamic url = JsonConvert.DeserializeObject(w.DownloadString(downloadUrl));
                    if (Json(url))
                    {
                        CallBack?.Invoke(url.data.Value);
                    }
                }
                catch (Exception error)
                {
                    MessageBox.Show("下载错误\n" + error.Message);
                }
                w.Dispose();
            });
        }
        public static bool Json(dynamic js)
        {
            JsonErrorMenu error = JsonRouter.GetJsonError(js);
            if (error == JsonErrorMenu.OK)
            {
                return true;
            }
            else if (error == JsonErrorMenu.NoFreeCount || error == JsonErrorMenu.NoLogin)
            {
                if (CookieControl.HasCookie3D66)
                {
                    CookieControl.PointerToNext();
                    // MainDownloadWindow.ShowQuestion("切换-Cookie" + CookieControl.CookiePointer3D66 + "-" + error);
                    MessageBox.Show("账号被冻结了或者被强制退出了，正在尝试切换登录");
                }
                else
                {
                    //  MainDownloadWindow.ShowQuestion("已无可用Cookie");
                    MessageBox.Show("账号被冻结了或者被强制退出了，需要重新登录->COOKIE");
                }

            }
            else if (error == JsonErrorMenu.NoError)
            {
                return true;
            }
            return false;
        }
        /// <summary>
        /// 获取Token
        /// </summary>
        /// <param name="html"></param>
        /// <returns></returns>
        public String GetToKen(string html)
        {
            var input = Regex.Match(html, "(?<=<input.*__token__.*=\").*(?=\")");
            return input.Value;
        }
    }
}
