using _3D66.Download.Model;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace _3D66.Download.Controls
{
    public class ProjectEntityModelItem : ListBoxItem
    {


        public ProjectEntityModel ModelSource
        {
            get { return (ProjectEntityModel)GetValue(ModelSourceProperty); }
            set { SetValue(ModelSourceProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ModelSource.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ModelSourceProperty =
            DependencyProperty.Register("ModelSource", typeof(ProjectEntityModel), typeof(ProjectEntityModelItem), new PropertyMetadata(null));


        public Brush TitleBrush
        {
            get { return (Brush)GetValue(TitleBrushProperty); }
            set { SetValue(TitleBrushProperty, value); }
        }

        // Using a DependencyProperty as the backing store for TitleBrush.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TitleBrushProperty =
            DependencyProperty.Register("TitleBrush", typeof(Brush), typeof(ProjectEntityModelItem), new PropertyMetadata(Brushes.Gray));





        static ProjectEntityModelItem()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ProjectEntityModelItem), new FrameworkPropertyMetadata(typeof(ProjectEntityModelItem)));
        }
       public  void Dispose()
        {
            ModelSource.Image = "";
            ModelSource = null;
        }
    }
}
