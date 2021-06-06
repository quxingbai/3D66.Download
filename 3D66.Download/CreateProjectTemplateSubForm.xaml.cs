using _3D66.Download.DataFile;
using _3D66.Download.Model;
using _3D66.Download.Tools;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
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
    /// CreateProjectTemplateSubForm.xaml 的交互逻辑
    /// </summary>
    public partial class CreateProjectTemplateSubForm : Window
    {
        public Action<ProjectEntityModel> CallBack;
        public CreateProjectTemplateSubForm(Action<ProjectEntityModel> c)
        {
            InitializeComponent();
            List<KeyValuePair<int, string>> l = DataBaseXML.SelectALLProjectTemplateType_Temp();
            LIST_PType.ItemsSource = l;
            LIST_PType.DisplayMemberPath = "Value";
            LIST_PType.SelectedValuePath = "Key";
            LIST_PType.SelectedIndex = 0;
            this.CallBack = c;

        }

        private void BT_SFile_Click(object sender, RoutedEventArgs e)
        {
            string path = PCTool.OpenDialog("Max压缩包", ".rar");
            if (path == "") return;
            string filename = Add66DownloadItemForm.Setting.Path_ProjectTemplatePath +  DateTime.Now.ToString("yyyy-MM-dd-hh-mm-ss")+".rar";
            TEXT_FilePath.Text = filename;
            File.Copy(path, filename);
        }

        private void BT_SImage_Click(object sender, RoutedEventArgs e)
        {
            string path = PCTool.OpenDialog("(图片)|*.jpg;*.png");
            if (path == "") return;
            string filename = Add66DownloadItemForm.Setting.Path_ProjectTemplateImagePath +  DateTime.Now.ToString("yyyy-MM-dd-hh-mm-ss")+new FileInfo(path).Extension;
            File.Copy(path, filename);
            TEXT_ImagePath.Text = filename;
        }
        ProjectEntityModel GetModel()
        {
            return new ProjectEntityModel()
            {
                CreateDate = DateTime.Now.ToString(),
                FilePath = TEXT_FilePath.Text,
                FromUser = TEXT_FromUser.Text,
                Height = Convert.ToInt32(TEXT_Height.Text),
                Image = TEXT_ImagePath.Text,
                Name = TEXT_Name.Text,
                Text = TEXT_Text.Text,
                TID = Convert.ToInt32(LIST_PType.SelectedValue),
                Version = TEXT_Version.Text,
                Width = Convert.ToInt32(TEXT_Width.Text),
            };
        }
        private void BT_Selected_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                CallBack?.Invoke(GetModel());
                Close();
            }
            catch(Exception err)
            {
                MessageBox.Show("输入错误\n" + err.Message);
            }
        }
    }
}
