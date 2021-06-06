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
    public class ImageByte : Image
    {
        static ImageByte()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ImageByte), new FrameworkPropertyMetadata(typeof(ImageByte)));
        }
    }
}
