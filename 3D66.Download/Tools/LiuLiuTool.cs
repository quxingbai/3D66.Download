using _3D66.Download.Model;
using _3D66.Download.Scripts;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace _3D66.Download.Tools
{
    public class LiuLiuTool
    {
        /// <summary>
        /// 将路径变为图片储存位置
        /// </summary>
        public static string ToImageFilePath(string FileName)
        {
            string path = Add66DownloadItemForm.Setting.Path_Download_LiuLiuImage + FileName;
            return path;
        }
        /// <summary>
        /// 将路径变为本地储存位置
        /// </summary>
        public static String ToLocalFilePath(string FileName)
        {
            return Add66DownloadItemForm.Setting.Path_Download_LiuLiu + FileName;
        }
        /// <summary>
        /// 下载至图片文件夹
        /// </summary>
        public static void DownloadToImageFileAsys(string Address, string filename, Action<string> DownloadedCallabck)
        {
            if (Address == null) { throw new Exception("无法下载（路径空）"); };
            WebClient web = GetCookieWeb();
            string path = Add66DownloadItemForm.Setting.Path_Download_LiuLiuImage + filename;
            web.DownloadFileAsync(new Uri(Address), path);
            web.DownloadFileCompleted += (s, e) =>
            {
                web.Dispose();
                DownloadedCallabck?.Invoke(path);
            };
        }
        /// <summary>
        /// 判断文件夹中是否存在某个 溜溜图片
        /// </summary>
        public static bool HasLiuLiuImage(int LiuLiuID)
        {
            FileInfo f = new FileInfo(Add66DownloadItemForm.Setting.Path_Download_LiuLiuImage + LiuLiuID + ".jpg");
            return f.Exists;
        }

        /// <summary>
        /// 获取包含Cookie、Header等必要属性的WebClient
        /// </summary>
        public static WebClient GetCookieWeb()
        {
            WebClient web = new WebClient();
            web.Headers.Add(HttpRequestHeader.Cookie, CookieControl.GetCookie());
            web.Headers.Add(HttpRequestHeader.Referer, "https://user.3d66.com/");
            web.Headers.Add(HttpRequestHeader.UserAgent, WebHeaders.GetHeader());
            return web;
        }
        /// <summary>
        /// 获取LiuLiu信息
        /// </summary>
        public static LiuLiuModel GetLiuLiuModelByID(int LiuLiuID)
        {

            WebClient web = GetCookieWeb();
            string html = web.DownloadString($"https://so.3d66.com/res/{LiuLiuID}_1_1.html");
            LiuLiuModel m = new LiuLiuModel()
            {
                Image = GetLiuLiuHtmlImage(html),
                DownloadCoin = 0,
                ID = LiuLiuID,
                KeyWord = "",
                Name = GetLiuLiuName(html),
                SizeByte = GetLiuLiuSizeByte(html),
            };

            if (m.Name.IndexOf("模型") != -1)
            {
                m.ModelType = ModelType.Model;
            }
            else if (m.Name.IndexOf("材质") != -1)
            {
                m.ModelType = ModelType.Meterial;
            }
            else if (m.Name.IndexOf("贴图") != -1)
            {
                m.ModelType = ModelType.Image;
            }
            else
            {
                m.ModelType = ModelType.NoType;
            }
            web.Dispose();
            return m;
        }
        /// <summary>
        /// 多线程获取LiuLiu信息
        /// </summary>
        public static void GetLiuLiuModelByIDAsys(int LiuLiuID, Action<LiuLiuModel> CallBack)
        {
            ThreadPool.QueueUserWorkItem(t =>
            {
                CallBack?.Invoke(GetLiuLiuModelByID(LiuLiuID));
            });
        }
        /// <summary>
        /// 从html中提取 图片
        /// </summary>
        /// <param name="html"></param>
        /// <returns></returns>
        public static string GetLiuLiuHtmlImage(string html)
        {
            string img = Regex.Match(html, @"<img class=""llimgs"" src=""(\S+)""").Groups[1].Value;

            return img;
        }
        /// <summary>
        /// 从html中提取 下载币
        /// </summary>
        public static int GetLiuLiuCoin(string html)
        {
            string coinstr = Regex.Match(html, "(?<=id=\"res_price\">).*(?=<)").Value;
            try
            {
                return Convert.ToInt32(coinstr.Replace("下载币", ""));
            }
            catch
            {
                return 0;
            }
        }
        /// <summary>   
        /// 从html中提取 名称
        /// </summary>
        public static String GetLiuLiuName(string html)
        {
            string name = Regex.Match(html, @"<img class=""llimgs"" src="".*alt=""(\S+)""").Groups[1].Value;
            return name;
        }
        /// <summary>
        /// 从html中提取 大小（Byte）
        /// </summary>
        public static double GetLiuLiuSizeByte(string html)
        {
            html = html.Replace("\n", "").Trim();
            string size = "";
            try
            {
                size = Regex.Match(html, "(?<=素材大小：.*<span>).*(?=KB)").Value;
                return Convert.ToInt32(size);
            }
            catch
            {
                try
                {

                    size = Regex.Match(html, "(?<=素材大小：.*<span>).*(?=MB)").Value;
                    return Convert.ToInt32(size) * 1024;
                }
                catch
                {
                    return 0;
                }
            }
        }
        public static string GetLiuLiuUiInfo(string html, string keyword)
        {
            html = html.Replace("\n", "");
            string info = Regex.Match(html, "(?<=<span>素材大小：<span>.*<span>).*(?=<)").Value;

            return info.Replace(keyword, "");
        }
        /// <summary>
        /// 获取关于某关键字的某页数据
        /// </summary>
        public static void GetLiuLiuModelsByKeyWordAsys(String KeyWord, Action<List<LiuLiuModel>> CallBack, int Page = 1)
        {
            string FromAddress = "https://so.3d66.com/res/" + KeyWord + "_1_1.html";
            dynamic json = new { now = FromAddress, page = Page };
            ThreadPool.QueueUserWorkItem(q =>
            {
                List<LiuLiuModel> Ms = new List<LiuLiuModel>();
                WebClient w = GetCookieWeb();
                w.Headers.Add("Content-Type", "application/json");
                dynamic jsonResult = JsonConvert.DeserializeObject<dynamic>(w.UploadString("https://so.3d66.com/res/so_list/pageLoading", "Post", JsonConvert.SerializeObject(json)));
                string html = jsonResult.data;
                w.Dispose();
                var lis = Regex.Matches(html, @"<li>.*</li>");
                lis = Regex.Matches(html, @"<li data-ll_id=""(.*?)""[\s\S]*?>[\s\S]+?data-src=""([\s\S]+?)""[\s\S]+?</span>([\s\S]+?)</a>[\s\S]+?model-goods-size"">([\s\S]+?)</p>[\s\S]+?</li>");
                //lis = Regex.Matches(html, @"<li data-ll_id=""(.*?)""[\s\S]*?>[\s\S]+?href=""([\s\S]+?)""[\s\S]+?</span>([\s\S]+?)</a>[\s\S]+?model-goods-size"">([\s\S]+?)</p>[\s\S]+?</li>");
                IEnumerable<LiuLiuModel> ls = lis.Select(m =>
                {
                    var mg = m.Groups;
                    return new LiuLiuModel()
                    {
                        ID = Convert.ToInt32(mg[1].Value),
                        Image = mg[2].Value.Trim(),
                        Name = mg[3].Value.Trim(),
                        DownloadCoin = Convert.ToInt32(mg[4].Value.Trim().Replace("下载币", "").Replace("VIP免费", "0").Replace("免费", "0").Trim()),
                    };
                });
                CallBack?.Invoke(ls.ToList());
            });
        }
        public static LiuLiuModel GetLiuLiuName_LIST(string html)
        {
            return null;
        }
    }
}
