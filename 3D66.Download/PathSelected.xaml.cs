using Microsoft.Win32;
using System;
using System.Collections.Generic;
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
    /// PathSelected.xaml 的交互逻辑
    /// </summary>
    public partial class PathSelected : Window
    {


        public String Path
        {
            get { return (String)GetValue(PathProperty); }
            set { SetValue(PathProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Path.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PathProperty =
            DependencyProperty.Register("Path", typeof(String), typeof(PathSelected), new PropertyMetadata(null));

        public Func<String, bool> CallBack;
        public PathSelected()
        {
            InitializeComponent();
        }
        private void BT_Select_Click(object sender, RoutedEventArgs e)
        {
            String path = "";
            OpenFileDialog di = new OpenFileDialog();
            di.Multiselect = false;
            di.Title = "选择路径";
            di.ShowDialog();
            path = di.FileName;
            this.Path = path;
        }

        private void BT_Next_Click(object sender, RoutedEventArgs e)
        {
          if (CallBack?.Invoke(Path)==true)
            {
                Close();
            }
            else
            {
                MessageBox.Show("选择错误");
            }
        }
    }
}
