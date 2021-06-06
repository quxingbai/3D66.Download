using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
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
    public class DownloadItem : ListBoxItem
    {

        public object Title
        {
            get { return (object)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Title.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register("Title", typeof(object), typeof(DownloadItem), new PropertyMetadata(""));


        public Brush ProcessBackground
        {
            get { return (Brush)GetValue(ProcessBackgroundProperty); }
            set { SetValue(ProcessBackgroundProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ProcessBackground.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ProcessBackgroundProperty =
            DependencyProperty.Register("ProcessBackground", typeof(Brush), typeof(DownloadItem), new PropertyMetadata(Brushes.Lime));


        public int Process
        {
            get { return (int)GetValue(ProcessProperty); }
            set { SetValue(ProcessProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Process.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ProcessProperty =
            DependencyProperty.Register("Process", typeof(int), typeof(DownloadItem), new PropertyMetadata(0));



        public Double ProcessWidth
        {
            get { return (Double)GetValue(ProcessWidthProperty); }
            set { SetValue(ProcessWidthProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ProcessWidth.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ProcessWidthProperty =
            DependencyProperty.Register("ProcessWidth", typeof(Double), typeof(DownloadItem), new PropertyMetadata(0.0));



        public BitmapImage TitleImage
        {
            get { return (BitmapImage)GetValue(TitleImageProperty); }
            set { SetValue(TitleImageProperty, value); }
        }

        // Using a DependencyProperty as the backing store for TitleImage.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TitleImageProperty =
            DependencyProperty.Register("TitleImage", typeof(BitmapImage), typeof(DownloadItem), new PropertyMetadata(null));



        public WebClient w = new WebClient();
        public Action<int> ProcessChanged;
        public event Action<DownloadItem> Downloaded;
        private String WaitAddress { get; set; }
        private String WaitPath { get; set; }
        static DownloadItem()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(DownloadItem), new FrameworkPropertyMetadata(typeof(DownloadItem)));
        }
        public DownloadItem()
        {
            w.Headers.Add(HttpRequestHeader.Cookie, "eyeofcloudEndUserId=oeu1619514662299r0.19497803208441433; eyeofcloudSegments=%7B%221%22%3A%22gc%22%2C%222%22%3A%22false%22%2C%223%22%3A%22referral%22%7D; eyeofcloudBuckets=%7B%7D; user_behavior_id=7952F194BF84A394CF3F4D1C37B82F710; PHPSESSID=9qi5qs93htqla2pmhdpvc2urqh; gr_user_id=28938488-22c4-4415-bdd2-b0849292fe27; 95ea25105d564481_gr_session_id=71e3f272-dfc1-46ad-8380-1c1b8c382c2c; UM_distinctid=1791298b6fe8e1-0fdacc66c4881c-d7e163f-1fa400-1791298b6ff327; 95ea25105d564481_gr_session_id_71e3f272-dfc1-46ad-8380-1c1b8c382c2c=true; Hm_lvt_bh_ud=AA82369D1DF30EDF1DD265EC30236043; login_token=34db9bc3c2cb6a05865aa4b86c4bc3bd; Hm_lvt_de9a43418888d1e2c4d93d2fc3aef899=1619514669,1619514730; last_login_type=2; userCookieData=%7B%22is_star%22%3A0%2C%22user_id%22%3A175825788%2C%22user_name%22%3A%22%5Cu77bf%5Cu661f%5Cu767e%22%2C%22is_vip%22%3A0%2C%22vip_status%22%3A0%2C%22user_img%22%3A%22https%3A%5C%2F%5C%2Fthirdwx.qlogo.cn%5C%2Fmmopen%5C%2FQJumM7PlcMCzB6fcZDy3s1v3SaqkU6VRUibIN86zoiaBZKtusgb0zeWKe5dzLypz0FXqLVHkHPyHOrIVsoqg6VFRvRd8dUtgta%5C%2F132%22%2C%22star_level%22%3A0%2C%22user_lb%22%3A0%2C%22xing_dian%22%3A0%2C%22zeng_dian%22%3A1%2C%22xuan_dian%22%3A7%2C%22cash_lb%22%3A0%2C%22vip_icon%22%3A%22%5C%2Fuser%5C%2Fimages%5C%2Fvipicon%5C%2Fvip0.png%22%2C%22vip_icon_expire%22%3A%22%5C%2Fuser%5C%2Fimages%5C%2Fvipicon%5C%2Fvip0.png%22%2C%22vip_next_vip_icon%22%3A%22%5C%2Fuser%5C%2Fimages%5C%2Fvipicon%5C%2Fvip01.png%22%2C%22vip_next_vip_icon_expire%22%3A%22%5C%2Fuser%5C%2Fimages%5C%2Fvipicon%5C%2Fvip11.png%22%2C%22star_icon%22%3A%22%5C%2Fuser%5C%2Fimages%5C%2Fstaricon%5C%2Fstar0.png%22%2C%22is_xuanran_vip%22%3A0%2C%22is_need_phone%22%3A0%2C%22is_vr_vip%22%3A0%2C%22is_soft_vip%22%3A0%2C%22is_sc_vip%22%3A0%2C%22all_vip_end_day%22%3A0%2C%22sc_end_day%22%3A0%2C%22is_one_end%22%3A0%2C%22vr_end_day%22%3A0%2C%22xr_end_day%22%3A0%2C%22soft_end_day%22%3A0%2C%22vip_rank%22%3A0%2C%22reg_time%22%3A1617721809%2C%22is_home_page%22%3A0%7D; get_cookie_time=1619557941; last_user_name=%E7%9E%BF%E6%98%9F%E7%99%BE; key_name=; Hm_lpvt_de9a43418888d1e2c4d93d2fc3aef899=1619522786");
            w.Headers.Add(HttpRequestHeader.Referer, "https://user.3d66.com/");
            w.Headers.Add(HttpRequestHeader.UserAgent, "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/90.0.4430.85 Safari/537.36");

            w.DownloadProgressChanged += W_DownloadProgressChanged;
        }

        private void W_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            Process = e.ProgressPercentage;
            ProcessChanged?.Invoke(Process);
        }

        public void Download(String Address, String Path)
        {
            Directory.CreateDirectory(new FileInfo(Path).DirectoryName);
            Uri u = new Uri(Address);
            w.DownloadFileAsync(new Uri(Address), Path);
            w.DownloadFileCompleted += (sss, eee) => { Downloaded?.Invoke(this); };
        }
        /// <summary>
        /// 等待下载
        /// </summary>
        public void WaitDownload(String Address, String Path)
        {
            Directory.CreateDirectory(new FileInfo(Path).DirectoryName);
            Uri u = new Uri(Address);
            w.DownloadFileCompleted += (sss, eee) => { Downloaded?.Invoke(this); };
            this.WaitPath = Path;
            this.WaitAddress = Address;

        }
        public void StartDownload()
        {
            w.DownloadFileAsync(new Uri(WaitAddress), WaitPath);
        }
        protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            if (e.Property == ProcessProperty)
            {
                ReRenderBackground();
            }
            else if (e.Property == ActualWidthProperty)
            {
                ReRenderBackground();
            }
            if (e.Property.Name != "IsSelectionActive")
            {
                string a = "";
            }
            base.OnPropertyChanged(e);
        }
        public void ReRenderBackground()
        {
            double p = Process * 0.01;
            ProcessWidth = (ActualWidth == double.NaN ? Width : ActualWidth) * p;
        }
    }
}
