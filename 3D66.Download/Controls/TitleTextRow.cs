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
    public class TitleTextRow : TextBox
    {




        public Object TitleContent
        {
            get { return (Object)GetValue(TitleContentProperty); }
            set { SetValue(TitleContentProperty, value); }
        }

        // Using a DependencyProperty as the backing store for TitleContent.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TitleContentProperty =
            DependencyProperty.Register("TitleContent", typeof(Object), typeof(TitleTextRow), new PropertyMetadata(null));


        static TitleTextRow()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(TitleTextRow), new FrameworkPropertyMetadata(typeof(TitleTextRow)));
        }
    }
}
