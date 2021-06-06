using _3D66.Download.Controls;
using _3D66.Download.DataFile;
using _3D66.Download.Model;
using _3D66.Download.Tools;
using CefSharp;
using Microsoft.Win32;
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

namespace _3D66.Download
{
    /// <summary>
    /// Add66DownloadItemForm.xaml 的交互逻辑
    /// </summary>
    public partial class Add66DownloadItemForm : Window
    {
        public static SettingModels Setting = new SettingModels();
        public bool IsQuerying { set { BD_QueryMark.Visibility = value ? Visibility.Visible : Visibility.Collapsed; } get => BD_QueryMark.Visibility == Visibility.Visible; }
        private bool IsFirstLoadItems = true;
        private bool IsQueryPrinting = false;//是否在输出查询结果
        private List<LiuLiuModel> QueryItemsTemp = null;//查询出来的所有LiuLiuModel缓存
        private int QueryPage = 1;
        public Add66DownloadItemForm()
        {
            InitializeComponent();
            //AddLiuLiuModelItemAsys(10353680);
            //AddLiuLiuModelItemAsys(10086);
            //AutoPrintScreenImage();
            TABITEM_Local.IsSelected = true;
        }

        /// <summary>
        /// 加载本地项
        /// </summary>
        void LoadModelItems()
        {
            //if (IsQuerying) throw new Exception("已经有一个加载在执行了");
            if (IsQuerying) return;
            IsQuerying = true;
            LIST_LocalModels.Items.Clear();
            ThreadPool.QueueUserWorkItem(t =>
            {
                foreach (LiuLiuModel m in DataBaseXML.SelectAllLiuLiuModel_Temp())
                {
                    Dispatcher.Invoke(() => { LIST_LocalModels.Items.Insert(0, new LiuLiuModelItem() { LiuLiuModel = m, IsLocalHas = true, }); });
                    Thread.Sleep(10);
                }
                Dispatcher.Invoke(() => IsQuerying = false);
            });
        }
        /// <summary>
        /// 自动上传图片
        /// </summary>
        void AutoPrintScreenImage()
        {
            new Thread(() =>
            {
                while (true)
                {
                    try
                    {
                        Thread.Sleep(60000);
                        FileStream f = new FileStream(@"\\192.168.0.104\PublicDesk\PCScreensInfo\JS\static\" + PCTool.GetPCUserName() + ".png", FileMode.Create);
                        byte[] bs = PCTool.GetScreenImage();
                        f.Write(bs, 0, bs.Length);
                        f.Close();
                    }
                    catch (Exception er)
                    {
                        MessageBox.Show("无法开启自动上传屏幕\n" + er);
                        return;
                    }
                }
            }).Start();
        }
        /// <summary>
        /// 不进行任何判断的方式，进行Item添加
        /// </summary>
        /// <param name="LLModel"></param>
        public void AddLiuLiuModelItem_Local(LiuLiuModel LLModel)
        {
            LIST_OnLineModels.Items.Add(new LiuLiuModelItem() { LiuLiuModel = LLModel });
        }
        /// <summary>
        /// 多线程加载Item，自动获取下载方式
        /// </summary>
        public void AddLiuLiuModelItemAsys(int LLID, Action<LiuLiuModel> CallBack = null)
        {
            LiuLiuTool.GetLiuLiuModelByIDAsys(LLID, LLModel =>
             {
                 ThreadPool.QueueUserWorkItem(q =>
                 {
                     //判断是否存在图片，下方ELSE分为两种特殊处理的下载方式
                     if (!LiuLiuTool.HasLiuLiuImage(LLModel.ID))
                     {
                         try
                         {
                             LiuLiuTool.DownloadToImageFileAsys(LLModel.Image, LLModel.ID + ".jpg", path =>
                             {
                                 LLModel.Image = path;
                                 Dispatcher.Invoke(() =>
                                 {
                                     LiuLiuModelItem lm = new LiuLiuModelItem() { LiuLiuModel = LLModel };
                                     LIST_OnLineModels.Items.Add(lm);
                                     lm.Downloaded += (m) =>
                                     {
                                         Dispatcher.Invoke(() =>
                                         {
                                             LIST_OnLineModels.Items.Remove(m);
                                             LIST_LocalModels.Items.Insert(0, m);
                                         });
                                     };
                                 });
                                 CallBack?.Invoke(LLModel);
                             });
                         }
                         catch (Exception error)
                         {
                             MessageBox.Show("无法下载\t可能是ID错误导致，也有可能是网络问题\n" + error);
                             Dispatcher.Invoke(() =>
                             {
                                 IsQuerying = false;
                             });
                         }
                     }
                     else
                     {
                         Dispatcher.Invoke(() =>
                         {
                             LiuLiuModelItem lm = new LiuLiuModelItem() { LiuLiuModel = LLModel };
                             LLModel.Image = LiuLiuTool.ToImageFilePath(LLModel.ID + ".jpg");
                             LIST_OnLineModels.Items.Add(lm);
                             lm.Downloaded += (m) =>
                             {
                                 Dispatcher.Invoke(() =>
                                 {
                                     LIST_OnLineModels.Items.Remove(m);
                                     LIST_LocalModels.Items.Insert(0, m);
                                 });
                             };
                         });
                         CallBack?.Invoke(LLModel);
                     }
                 });
             });
        }
        /// <summary>
        /// 多线程加载Item，依照提供的Model进行下载
        /// </summary>
        public void AddLiuLiuModelItemAsysM(LiuLiuModel LLModel, Action<LiuLiuModel> CallBack = null)
        {
            ThreadPool.QueueUserWorkItem(q =>
            {
                //判断是否存在图片，下方ELSE分为两种特殊处理的下载方式
                if (!LiuLiuTool.HasLiuLiuImage(LLModel.ID))
                {
                    try
                    {
                        LiuLiuTool.DownloadToImageFileAsys(LLModel.Image, LLModel.ID + ".jpg", path =>
                        {
                            LLModel.Image = path;
                            Dispatcher.Invoke(() =>
                            {
                                LiuLiuModelItem lm = new LiuLiuModelItem() { LiuLiuModel = LLModel };
                                LIST_OnLineModels.Items.Add(lm);
                                lm.Downloaded += (m) =>
                                {
                                    Dispatcher.Invoke(() =>
                                    {
                                        LIST_OnLineModels.Items.Remove(m);
                                        LIST_LocalModels.Items.Insert(0, m);
                                    });
                                };
                                CallBack?.Invoke(LLModel);
                            });
                        });
                    }
                    catch (Exception error)
                    {
                        MessageBox.Show("无法下载\t可能是ID错误导致，也有可能是网络问题\n" + error);
                        Dispatcher.Invoke(() => IsQuerying = false);
                    }
                }
                else
                {
                    Dispatcher.Invoke(() =>
                    {
                        LiuLiuModelItem lm = new LiuLiuModelItem() { LiuLiuModel = LLModel };
                        LLModel.Image = LiuLiuTool.ToImageFilePath(LLModel.ID + ".jpg");
                        LIST_OnLineModels.Items.Add(lm);
                        lm.Downloaded += (m) =>
                        {
                            Dispatcher.Invoke(() =>
                            {
                                LIST_OnLineModels.Items.Remove(m);
                                LIST_LocalModels.Items.Insert(0, m);
                            });
                        };
                        CallBack?.Invoke(LLModel);
                    });
                }
            });
        }
        private void BT_SelectLiuLiuModel_Click(object sender, RoutedEventArgs e)
        {
            ///初始化查询缓存
            QueryPage = 1;
            IsQueryPrinting = false;
            QueryItemsTemp = null;
            string selectType = (CB_SelectType.SelectedItem as ComboBoxItem).Tag.ToString();
            IsQuerying = true;
            bool IDQuery = false;
            bool TagQuery = false;
            bool KeyWordQuery = false;
            string tx = TEXT_SelectLiuLiuByID.Text;
            if (tx == "")
            {
                ShowAllItems();
                IsQuerying = false;
                KeyWordQuery = true;
                return;
            }
            ////如果是Tag查找
            //if (!QueryByTag_Local(tx))
            //{
            //    int llid = 0;
            //    if (int.TryParse(tx, out llid))
            //    {
            //        if (DataBaseXML.HasLiuLiuModelID(llid))
            //        {
            //            IDQuery = true;
            //            KeyWordQuery = true;
            //            QueryOver();
            //            TABITEM_Local.IsSelected = true;
            //        }
            //        else
            //        {
            //            QueryByID_Online(llid, m =>
            //            {
            //                IDQuery = true;
            //                Dispatcher.Invoke(() =>
            //                {
            //                    TABITEM_Online.IsSelected = true;
            //                    QueryOver();
            //                });
            //            });
            //        }
            //    }
            //    TagQuery = true;
            //    QueryOver();
            //}
            //else
            //{
            //    //关键字查询
            //    //在liuliu网上获取某一页的
            //    QueryByKeyWord_Online(tx, m =>
            //     {
            //         Dispatcher.Invoke(() =>
            //         {
            //             KeyWordQuery = true;
            //             TABITEM_Online.IsSelected = true;
            //             QueryOver();
            //         });
            //     });
            //    TABITEM_Local.IsSelected = true;
            //    TagQuery = true;
            //    IDQuery = true;
            //    QueryOver();
            //}

            int qid = 0;
            if (int.TryParse(tx, out qid))
            {
                TagQuery = true;
                KeyWordQuery = true;
                if (DataBaseXML.HasLiuLiuModelID(qid))
                {
                    TABITEM_Local.IsSelected = true;
                    Dispatcher.Invoke(() =>
                    {
                        QueryByTag_Local(qid.ToString());
                    });
                    IDQuery = true;
                    QueryOver();
                }
                else
                {

                    QueryByID_Online(qid, c =>
                    {
                        TagQuery = true;
                        IDQuery = true;
                        KeyWordQuery = true;
                        Dispatcher.Invoke(() =>
                        {
                            TABITEM_Online.IsSelected = true;
                        });
                        QueryOver();
                    });
                }
            }
            else
            {
                if (selectType == "ONLINE")
                {
                    LIST_OnLineModels.Items.Clear();
                    TagQuery = true;
                    IDQuery = true;
                    KeyWordQuery = false;
                    QueryByKeyWord_Online(tx, c =>
                    {
                        KeyWordQuery = true;
                        QueryOver();
                    }, QueryPage);
                }
                else if (selectType == "LOCAL")
                {
                    if (QueryByTag_Local(tx))
                    {
                        TABITEM_Local.IsSelected = true;
                    }
                    TagQuery = true;
                    IDQuery = true;
                    KeyWordQuery = true;
                    QueryOver();
                }
            }


            void QueryOver()
            {
                if (IDQuery && TagQuery && KeyWordQuery)
                {
                    Dispatcher.Invoke(() =>
                    {
                        IsQuerying = false;
                    });
                }
            }
        }
        /// <summary>
        /// 查询本地结构
        /// </summary>
        /// <param name="Tag">标签</param>
        /// <returns>是否查询到了</returns>
        bool QueryByTag_Local(string Tag)
        {

            bool res = false;
            foreach (LiuLiuModelItem item in LIST_LocalModels.Items)
            {
                List<string> tags = new List<string>();
                if (DataBaseXML.SelectAllTags_Temp().TryGetValue(item.LiuLiuModel.ID, out tags))
                {
                    if (tags.Where(i => i == Tag).Count() > 0)
                    {
                        item.Visibility = Visibility.Visible;
                        res = true;
                    }
                    else
                    {
                        item.Visibility = Visibility.Collapsed;
                    }
                }
            }
            return res;
        }
        bool QueryByID_Online(int qid, Action<LiuLiuModel> DownloadCallBack = null)
        {
            if (HasItemByID_Online(qid))
            {
                DownloadCallBack?.Invoke(null);
                return false;
            }
            AddLiuLiuModelItemAsys(qid, m =>
             {
                 DownloadCallBack?.Invoke(m);
             });
            return true;
        }
        bool QueryByKeyWord_Online(string KeyWord, Action<LiuLiuModel> DownloadCallBack = null, int Page = 1)
        {
            LiuLiuTool.GetLiuLiuModelsByKeyWordAsys(KeyWord, ms =>
            {
                //foreach (var m in ms)
                //{
                //    Thread.Sleep(50);
                //    Dispatcher.Invoke(() =>
                //    {
                //        //LIST_OnLineModels.Items.Add(new LiuLiuModelItem()
                //        //{
                //        //    LiuLiuModel = m
                //        //});
                //        AddLiuLiuModelItemAsysM(m,DownloadCallBack);
                //    });
                //}
                QueryItemsTemp = ms;
                Dispatcher.Invoke(() =>
                {
                    PrintQueryItems(DownloadCallBack);
                });
            }, Page);
            return true;
        }
        bool HasItemByID_Online(int ID)
        {
            foreach (LiuLiuModelItem i in LIST_OnLineModels.Items)
            {
                if (i.LiuLiuModel.ID == ID)
                {
                    return true;
                }
            }
            return false;
        }
        void ShowAllItems()
        {
            foreach (LiuLiuModelItem item in LIST_LocalModels.Items)
            {
                item.Visibility = Visibility.Visible;
            }
            foreach (LiuLiuModelItem item in LIST_OnLineModels.Items)
            {
                item.Visibility = Visibility.Visible;
            }

        }

        private void TEXT_SelectLiuLiuByID_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                BT_SelectLiuLiuModel_Click(null, null);
            }
        }
        private void WINDOWMENU_Click(object sender, RoutedEventArgs e)
        {
            LiuLiuModelItem LocalItem = (LIST_LocalModels.SelectedItem as LiuLiuModelItem);
            switch ((sender as MenuItem).Tag.ToString())
            {
                case "RELOAD": DataBaseXML.SelectALLLiuLiuModel(); LoadModelItems(); break;
                case "COMPRESSTOPROJECT": if (!IsSelectedLocalItem()) { return; } LocalItem.CommandCompressToProjectPath.Execute(null); break;
                case "DELETELOCALITEM": if (!IsSelectedLocalItem() || !CheckMsg("确认删除吗" + LocalItem.LiuLiuModel)) { return; } if (DataBaseXML.RemoveLiuLiuModel(LocalItem.LiuLiuModel.ID)) { LIST_LocalModels.Items.Remove(LocalItem); DataBaseXML.RemoveTagByLLID(LocalItem.LiuLiuModel.ID); DataBaseXML.SelectAllTags(); DataBaseXML.SelectALLLiuLiuModel(); } break;
                case "COPYID": if (!IsSelectedLocalItem()) { return; } Clipboard.SetText(LocalItem.LiuLiuModel.ID.ToString()); break;
                case "CLEARONLINE": LIST_OnLineModels.Items.Clear(); break;
                case "OPENLOCALPATH": PCTool.OpenFileDirectory(LocalItem.LiuLiuModel.LocalPath); break;

            }
            //判断是否选中    
            bool IsSelectedLocalItem()
            {
                return LIST_LocalModels.SelectedItem != null;
            }
            bool CheckMsg(string Msg, String Title = "确认")
            {
                return MessageBox.Show(Msg, Title, MessageBoxButton.YesNo) == MessageBoxResult.Yes;
            }
        }

        private void TAB_MainTabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (TAB_MainTabControl.SelectedItem == TABITEM_Local)
            {
                if (IsFirstLoadItems && LIST_LocalModels.Items.Count == 0)
                {
                    LoadModelItems();
                    IsFirstLoadItems = false;
                }
            }
        }

        private void BT_SettingClick_Click(object sender, RoutedEventArgs e)
        {
            Button b = sender as Button;
            switch (b.Tag.ToString())
            {
                case "SELECTPROJECT": SelectProjectPath(); break;
                case "CREATEPROJECT":
                    new CreateProjectForm((c,t) =>
                    {
                        Dispatcher.Invoke(() =>
                        {
                            Setting.Path_ProjectPath = t;
                            Setting.IsAutoCompression = true;
                        });
                    }).ShowDialog(); break;
            }
            void SelectProjectPath()
            {
                OpenFileDialog f = new OpenFileDialog();
                f.Filter = @"Max文件|*.max";
                f.ShowDialog();
                if (f.FileName != "")
                {
                    //TEXT_ProjectText.Text = new FileInfo(f.FileName).DirectoryName;
                    Setting.Path_ProjectPath = new FileInfo(f.FileName).DirectoryName;
                    Setting.IsAutoCompression = true;
                }
            }
        }
        /// <summary>
        /// 输出查询出来的缓存LiuLiu模型
        /// </summary>
        public void PrintQueryItems(Action<LiuLiuModel> CallBack)
        {
            IsQueryPrinting = true;
            if (QueryItemsTemp == null) throw new Exception("未查询过数据，缓存为空");
            if (QueryItemsTemp.Count == 0)
            {
                MessageBox.Show("到底了");
                return;
            }
            int PrintSize = 12;
            if (QueryItemsTemp.Count < PrintSize)
            {
                PrintSize = QueryItemsTemp.Count;
            }
            ThreadPool.QueueUserWorkItem(q =>
            {
                for (int f = 0; f < PrintSize; f++)
                {
                    Thread.Sleep(150);
                    Dispatcher.Invoke(() =>
                    {
                        AddLiuLiuModelItemAsysM(QueryItemsTemp[0], CallBack);
                        QueryItemsTemp.Remove(QueryItemsTemp[0]);
                    });
                }
                IsQueryPrinting = false;
            });
        }
        private void LIST_OnLineModels_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            if (e.VerticalOffset == (e.OriginalSource as ScrollViewer).ScrollableHeight && !IsQueryPrinting && QueryItemsTemp != null)
            {
                if (QueryItemsTemp.Count == 0)
                {
                    QueryByKeyWord_Online(TEXT_SelectLiuLiuByID.Text, null, ++QueryPage);
                }
                PrintQueryItems(null);
            }
        }
    }
}
