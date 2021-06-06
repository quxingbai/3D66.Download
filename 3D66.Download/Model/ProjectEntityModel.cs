using PropertyChanged;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace _3D66.Download.Model
{
    public class ProjectEntityModel : INotifyPropertyChanged
    {
        public int ID { get; set; }
        public int TID { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public String FilePath { get; set; }
        public String Version { get; set; }
        public String Name { get; set; }
        public String FromUser { get; set; }
        public String Text { get; set; }
        public String Image { get; set; }
        public String CreateDate { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
