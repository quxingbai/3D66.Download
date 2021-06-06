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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace _3D66.Download.Controls
{
    public class AddressImage : Control
    {


        public String Address
        {
            get { return (String)GetValue(AddressProperty); }
            set { SetValue(AddressProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Address.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty AddressProperty =
            DependencyProperty.Register("Address", typeof(String), typeof(AddressImage), new PropertyMetadata(null));


        public const string DefaultImagePath = "https://pic.3d66.com/thuimg/newcache/1000/7152/715262_1072580.jpg";
        public ImageSource Image
        {
            get { return (ImageSource)GetValue(ImageProperty); }
            set { SetValue(ImageProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Image.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ImageProperty =
            DependencyProperty.Register("Image", typeof(ImageSource), typeof(AddressImage), new PropertyMetadata(null));



        public Stretch Stretch
        {
            get { return (Stretch)GetValue(StretchProperty); }
            set { SetValue(StretchProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Stretch.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty StretchProperty =
            DependencyProperty.Register("Stretch", typeof(Stretch), typeof(AddressImage), new PropertyMetadata(Stretch.None));




        static AddressImage()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(AddressImage), new FrameworkPropertyMetadata(typeof(AddressImage)));
        }

        protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            if (e.Property == AddressProperty)
            {
                String address = "";
                //如果是HTTP
               if (Address.Length > 4 && Address.Substring(0, 3).ToUpper() == "HTTP")
                {
                    address = Address;
                }
                //如果是本地路径
                else
                {
                    if (!File.Exists(address))
                    {
                        address = DefaultImagePath;
                    }
                }
                Image = new BitmapImage(new Uri(address));
            }
            base.OnPropertyChanged(e);
        }

    }
}
