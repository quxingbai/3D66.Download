using _3D66.Download.Controls;
using _3D66.Download.DataFile;
using _3D66.Download.Model;
using _3D66.Download.Scripts;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace _3D66.Download
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //private Queue<MaxModel> DownloadTasks = new Queue<MaxModel>();
        private const int MaxDownloadCount = 10;
        private const String LocalSavePath = "./Download/3DModel/";
        private const String LocalImageSavePath = "./Download/Image/";
        private Thread TaskAutoPush = null;
        public static MainWindow MainDownloadWindow { get; set; }
        public MainWindow()
        {
            InitializeComponent();
            //MainDownloadWindow = this;
            //(TaskAutoPush = new Thread(() =>
            //{
            //    while (true)
            //    {
            //        for (int f = LIST_WaitDownload.Items.Count; f > 0; f++)
            //        {
            //            if (LIST_Downloading.Items.Count < MaxDownloadCount && LIST_WaitDownload.Items.Count > 0)
            //            {
            //                Dispatcher.Invoke(() =>
            //                {
            //                    NextDownload();
            //                });
            //            }
            //        }
            //        Thread.Sleep(5000);
            //    }
            //})).Start();
            //new AutoGetModelForm().Show();
            //WaitDownload(new MaxModel()
            //{
            //    ID = 715262,
            //    KeyWord = "A",
            //    Name = "B",
            //    Image= "https://pic.3d66.com/thuimg/newcache/1000/7152/715262_1072580.jpg",
            //});
            new Add66DownloadItemForm().Show();
        }
        public void WaitDownload(MaxModel model)
        {
            DownloadItem download = new DownloadItem();
            download.Title = model.KeyWord + "-" + model.Name;
            ShowMessage($"尝试下载[{download.Title}]");

            ToDownloadURLAsys(model.ID, j =>
            {
                Dispatcher.Invoke(() =>
                {
                    try
                    {
                        download.WaitDownload(j, ToLocalPath(model.KeyWord + "/" + model.ID + "-" + Name));
                        download.TitleImage = new BitmapImage(new Uri(model.Image));
                    }
                    catch (Exception error)
                    {
                        ShowQuestion($"下载错误：{error}");
                        return;
                    }
                    LIST_WaitDownload.Items.Add(download);
                });
            });
            download.Downloaded += (d) =>
            {
                if (LIST_Downloaded.Items.Count > 20)
                {
                    LIST_Downloaded.Items.Clear();
                    ShowMessage($"下载完成：{model.ID}{model.Name}");
                }
                LIST_Downloading.Items.Remove(download);
                LIST_Downloaded.Items.Add(download);
                DataControl.WriteDownloaded(model.ID);
            };
        }
        public void WaitDownload66(MaxModel model)
        {
            DownloadItem download = new DownloadItem();
            download.Title = model.KeyWord + "-" + model.Name;
            String DownloadToPath = "";
            ShowMessage($"尝试下载[{download.Title}]");

            ToDownloadURLAsys(model.ID, j =>
            {
                Dispatcher.Invoke(() =>
                {
                    try
                    {
                        if (RB_URLPath.IsChecked ?? true)
                        {
                            DownloadToPath = @"\\DESKTOP-O7KRSB3\Puc\vip模型\-" + model.ID + ".rar";
                        }
                        else
                        {
                            Directory.CreateDirectory("./Download");
                            DownloadToPath = @".\Download\-" + model.ID + ".rar";
                        }
                        download.WaitDownload(j, DownloadToPath);
                    }
                    catch (Exception error)
                    {
                        ShowQuestion($"下载错误：{error}");
                        return;
                    }
                    LIST_WaitDownload.Items.Add(download);
                });
            });
            download.Downloaded += (d) =>
            {
                if (LIST_Downloaded.Items.Count > 20)
                {
                    LIST_Downloaded.Items.Clear();
                    ShowMessage($"下载完成：{model.ID}{model.Name}");
                }
                try
                {
                    Dispatcher.Invoke(() =>
                    {
                        Process.Start("explorer.exe",new FileInfo(DownloadToPath).Directory.ToString());
                    });

                }
                catch
                {
                    ShowMessage("打开文件夹失败");
                }
                LIST_Downloading.Items.Remove(download);
                LIST_Downloaded.Items.Add(download);
                DataControl.WriteDownloaded(model.ID);
            };
        }
        public void DownloadFileAsys(String Address, String SavePath, Action<string> CallBack)
        {
            try
            {
                ThreadPool.QueueUserWorkItem(c =>
                {
                    try
                    {
                        Directory.CreateDirectory(new FileInfo(SavePath).DirectoryName);
                        WebClient web = new WebClient();
                        web.DownloadFile(Address, SavePath);
                        web.Dispose();
                        Dispatcher.Invoke(() =>
                        {
                            CallBack?.Invoke(SavePath);
                        });
                    }
                    catch
                    {

                        Dispatcher.Invoke(() =>
                        {
                            CallBack?.Invoke("");
                        });
                    }
                });
            }
            catch
            {
            }
        }


        public String ToDownloadURL(int ID)
        {
            WebClient w = new WebClient();
            w.Headers.Add(HttpRequestHeader.Cookie, "eyeofcloudEndUserId=oeu1619514662299r0.19497803208441433; eyeofcloudSegments=%7B%221%22%3A%22gc%22%2C%222%22%3A%22false%22%2C%223%22%3A%22referral%22%7D; eyeofcloudBuckets=%7B%7D; user_behavior_id=7952F194BF84A394CF3F4D1C37B82F710; PHPSESSID=9qi5qs93htqla2pmhdpvc2urqh; gr_user_id=28938488-22c4-4415-bdd2-b0849292fe27; 95ea25105d564481_gr_session_id=71e3f272-dfc1-46ad-8380-1c1b8c382c2c; UM_distinctid=1791298b6fe8e1-0fdacc66c4881c-d7e163f-1fa400-1791298b6ff327; 95ea25105d564481_gr_session_id_71e3f272-dfc1-46ad-8380-1c1b8c382c2c=true; Hm_lvt_bh_ud=AA82369D1DF30EDF1DD265EC30236043; login_token=34db9bc3c2cb6a05865aa4b86c4bc3bd; Hm_lvt_de9a43418888d1e2c4d93d2fc3aef899=1619514669,1619514730; last_login_type=2; userCookieData=%7B%22is_star%22%3A0%2C%22user_id%22%3A175825788%2C%22user_name%22%3A%22%5Cu77bf%5Cu661f%5Cu767e%22%2C%22is_vip%22%3A0%2C%22vip_status%22%3A0%2C%22user_img%22%3A%22https%3A%5C%2F%5C%2Fthirdwx.qlogo.cn%5C%2Fmmopen%5C%2FQJumM7PlcMCzB6fcZDy3s1v3SaqkU6VRUibIN86zoiaBZKtusgb0zeWKe5dzLypz0FXqLVHkHPyHOrIVsoqg6VFRvRd8dUtgta%5C%2F132%22%2C%22star_level%22%3A0%2C%22user_lb%22%3A0%2C%22xing_dian%22%3A0%2C%22zeng_dian%22%3A1%2C%22xuan_dian%22%3A7%2C%22cash_lb%22%3A0%2C%22vip_icon%22%3A%22%5C%2Fuser%5C%2Fimages%5C%2Fvipicon%5C%2Fvip0.png%22%2C%22vip_icon_expire%22%3A%22%5C%2Fuser%5C%2Fimages%5C%2Fvipicon%5C%2Fvip0.png%22%2C%22vip_next_vip_icon%22%3A%22%5C%2Fuser%5C%2Fimages%5C%2Fvipicon%5C%2Fvip01.png%22%2C%22vip_next_vip_icon_expire%22%3A%22%5C%2Fuser%5C%2Fimages%5C%2Fvipicon%5C%2Fvip11.png%22%2C%22star_icon%22%3A%22%5C%2Fuser%5C%2Fimages%5C%2Fstaricon%5C%2Fstar0.png%22%2C%22is_xuanran_vip%22%3A0%2C%22is_need_phone%22%3A0%2C%22is_vr_vip%22%3A0%2C%22is_soft_vip%22%3A0%2C%22is_sc_vip%22%3A0%2C%22all_vip_end_day%22%3A0%2C%22sc_end_day%22%3A0%2C%22is_one_end%22%3A0%2C%22vr_end_day%22%3A0%2C%22xr_end_day%22%3A0%2C%22soft_end_day%22%3A0%2C%22vip_rank%22%3A0%2C%22reg_time%22%3A1617721809%2C%22is_home_page%22%3A0%7D; get_cookie_time=1619557941; last_user_name=%E7%9E%BF%E6%98%9F%E7%99%BE; key_name=; Hm_lpvt_de9a43418888d1e2c4d93d2fc3aef899=1619522786");
            w.Headers.Add(HttpRequestHeader.Referer, "https://user.3d66.com/");
            w.Headers.Add(HttpRequestHeader.UserAgent, "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/90.0.4430.85 Safari/537.36");
            string j = w.DownloadString("https://user.3d66.com/download/down_Handle/index?action=user_pay_download&ll_id=" + ID + "&rartype=1&needtype=1&action_id=0&token=a306901c34c5d0eae7544f3eab070c1b&sotu_action_id=&kw=%E6%9F%9C%E9%97%A8&rlai=1&collect=0&dl_course=0");
            dynamic url = JsonConvert.DeserializeObject(j);
            return url.data.Value;
        }
        /// <summary>
        /// 变为下载路径
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
                    String j = w.DownloadString(new Uri("https://user.3d66.com/download/down_Handle/index?action=user_pay_download&ll_id=" + ID + "&rartype=1&needtype=1&action_id=0&token=a306901c34c5d0eae7544f3eab070c1b&sotu_action_id=&kw=%E6%9F%9C%E9%97%A8&rlai=1&collect=0&dl_course=0"));
                    dynamic url = JsonConvert.DeserializeObject(j);
                    if (Json(url))
                    {
                        CallBack?.Invoke(url.data.Value);
                    }
                }
                catch
                {
                    MainDownloadWindow.ShowQuestion("下载路径转换错误-" + ID);
                }
                w.Dispose();
            });
        }
        public String ToLocalPath(String FileName, String Suffix = ".rar")
        {
            return LocalSavePath + FileName + Suffix;
        }
        public bool NextDownload()
        {
            if (LIST_WaitDownload.Items.Count == 0 && LIST_Downloading.Items.Count < MaxDownloadCount)
            {
                return false;
            }
            else
            {
                DownloadItem item = LIST_WaitDownload.Items[0] as DownloadItem;
                LIST_WaitDownload.Items.Remove(item);
                LIST_Downloading.Items.Insert(0, item);
                item.StartDownload();
            }
            return true;
        }
        /// <summary>
        /// 显示短消息
        /// </summary>
        public void ShowMessage(Object Msg)
        {
            Dispatcher.Invoke(() =>
            {
                LB_Msg.Content = Msg;
            });
        }
        /// <summary>
        /// 显示一个问题
        /// </summary>
        public void ShowQuestion(Object Msg)
        {
            Dispatcher.Invoke(() =>
            {
                LB_Msg_Question.Content = Msg;
            });
        }
        /// <summary>
        /// 清除消息
        /// </summary>
        public void ClearMessage()
        {
            Dispatcher.Invoke(() =>
            {
                LB_Msg_Question.Content = "";
                LB_Msg.Content = "";
            });
        }
        protected override void OnClosed(EventArgs e)
        {
            DataControl.SaveDownloadedIds();
            base.OnClosed(e);
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
                    MainDownloadWindow.ShowQuestion("切换-Cookie" + CookieControl.CookiePointer3D66 + "-" + error);
                }
                else
                {
                    MainDownloadWindow.Dispatcher.Invoke(() =>
                    {
                        if (MainDownloadWindow.LIST_WaitDownload.Items.Count > 0 || MainDownloadWindow.LIST_Downloading.Items.Count > 0)
                        {
                            MainDownloadWindow.ShowQuestion("已无可用Cookie");
                        }
                        else
                        {
                            MainDownloadWindow.Close();
                            Application.Current.Shutdown();
                        }
                    });
                }
            }
            else if (error == JsonErrorMenu.NoError)
            {
                return true;
            }
            return false;
        }


        private void TEXT_66ID_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {

                if (MessageBox.Show("确定下载[" + TEXT_66ID.Text + "]吗", "", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    WaitDownload66(new MaxModel()
                    {
                        ID = Convert.ToInt32(TEXT_66ID.Text),
                        Image = null,
                        KeyWord = null,
                        Name = "-" + TEXT_66ID.Text,
                    });
                    TEXT_66ID.Text = "";
                }
            }
        }
    }
}
