﻿using System;
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
    public class LiuLiuModelItem : ListBoxItem
    {
        static LiuLiuModelItem()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(LiuLiuModelItem), new FrameworkPropertyMetadata(typeof(LiuLiuModelItem)));
        }
    }
}
