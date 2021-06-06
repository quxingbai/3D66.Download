using _3D66.Download.Controls;
using _3D66.Download.DataFile;
using _3D66.Download.Model;
using _3D66.Download.Tools;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
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
    /// CreateProjectForm.xaml 的交互逻辑
    /// </summary>
    public partial class CreateProjectForm : Window
    {
        public Action<ProjectEntityModel,string> SelectedCallBack { get; set; } = null;
        public CreateProjectForm()
        //public CreateProjectForm(Action<ProjectEntityModel> SelectedCallBack)
        {
            InitializeComponent();
            this.SelectedCallBack = SelectedCallBack;
            int c = 0;
            //while (c++ < 10)
            //{
            //    LIST_ProjectEntitys.Items.Add(new ProjectEntityModelItem()
            //    {
            //        ModelSource = new ProjectEntityModel()
            //        {
            //            CreateDate = DateTime.Now.ToString(),
            //            ID = 0,
            //            Name = "预设一",
            //            Version = "2014",
            //            Image = "-",
            //            Text = "预设一...................asdaklsdalksdjlksadlkajsdkl"
            //        }
            //    });
            //}
            LoadItems();
        }
        public CreateProjectForm(Action<ProjectEntityModel,string> SelectedCallBack)
        {
            InitializeComponent();
            this.SelectedCallBack = SelectedCallBack;
            //DataBaseXML.AddProjectEntity(new ProjectEntityModel()
            //{
            //    CreateDate=DateTime.Now.ToString(),
            //    FilePath= @"\\192.168.0.104\PublicDesk\LiuLiuModels\ModelTemplate\Emptys\2800×3600×2600卧室.rar",
            //    FromUser="瞿",
            //    Width=2800,
            //    Height=3600,
            //    Image= @"\\192.168.0.104\PublicDesk\LiuLiuModels\ModelTemplate\Images\57891c2101c872142b01ebd3543e50e.png",
            //    Name = "2800×3600卧室.rar",
            //    Text="卧室",
            //    Version="3ds 2014",
            //    TID=1,
            //});
            LoadItems();
        }
        public void LoadItems()
        {
            List<ProjectEntityModel> ms = DataBaseXML.SelectAllProjectEntitys_Temp();
            ThreadPool.QueueUserWorkItem(q =>
            {
                foreach (ProjectEntityModel m in ms)
                {
                    Dispatcher.Invoke(() =>
                    {
                        LIST_ProjectEntitys.Items.Add(new ProjectEntityModelItem() { ModelSource = m });
                    });
                    Thread.Sleep(100);
                }
            });
            LIST_ProjectEntitys.SelectedIndex = 0;
        }
        private void LIST_ProjectEntitys_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            LIST_ProjectEntitys.ScrollIntoView(LIST_ProjectEntitys.SelectedItem);
        }

        private void BT_Checked_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (LIST_ProjectEntitys.SelectedItem == null) return;
                ProjectEntityModelItem i = LIST_ProjectEntitys.SelectedItem as ProjectEntityModelItem;
                ProjectEntityModel p = i.ModelSource;
                string open = PCTool.SaveDialog("项目地址(文件夹名称)", "");
                if (open == "") return;
                if (Directory.Exists(open))
                {
                    if (!(MessageBox.Show("项目文件夹已存在是否继续？", "确认", MessageBoxButton.YesNo) == MessageBoxResult.Yes))
                    {
                        return;
                    }
                }
                Directory.CreateDirectory(open);
                if (!WinRARTool.CompressRar_D(p.FilePath, open))
                {
                    MessageBox.Show("创建项目失败");
                    return;
                }
                PCTool.OpenDirectory(open);
                Close();
                SelectedCallBack?.Invoke(p,open);
            }
            catch (Exception err)
            {
                MessageBox.Show("出现错误\n" + err);
            }

        }
        void CreateProjectTemplate()
        {

        }
        private void MENUITEM_MainMenu_Click(object sender, RoutedEventArgs e)
        {
            MenuItem i = sender as MenuItem;
            var selected = LIST_ProjectEntitys.SelectedItem as ProjectEntityModelItem;
            switch (i.Tag.ToString())
            {
                case "CREATEPROJECT":
                    new CreateProjectTemplateSubForm(c =>
                    {
                        try
                        {
                            if (DataBaseXML.AddProjectEntity(c))
                            {
                                MessageBox.Show("添加成功");
                                Dispatcher.Invoke(() =>
                                {
                                    LIST_ProjectEntitys.Items.Add(new ProjectEntityModelItem() { ModelSource = c });
                                });
                            }
                            else
                            {
                                MessageBox.Show("添加失败");
                            }

                        }
                        catch
                        {
                            MessageBox.Show("无法添加项目模板");
                        }
                    }).ShowDialog(); break;
                case "DELETEPROJECT":
                    if (selected == null) return;
                    if (MessageBox.Show("确认删除", "确认", MessageBoxButton.YesNo) == MessageBoxResult.No) return;
                    if (DataBaseXML.RemoveProjectTemplateByID(selected.ModelSource.ID))
                    {
                        int count = 0;
                        selected.Dispose();
                        LIST_ProjectEntitys.Items.Remove(selected);
                        try
                        {
                            File.Delete(selected.ModelSource.FilePath);
                        }
                        catch
                        {
                            count++;
                        }
                        try
                        {
                            File.Delete(selected.ModelSource.Image);
                        }
                        catch
                        {
                            count++;
                        }
                        MessageBox.Show("删除成功\t错误次数" + count);
                    }
                    ; break;
            }
        }
    }
}
