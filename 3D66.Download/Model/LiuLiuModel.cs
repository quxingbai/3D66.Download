using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace _3D66.Download.Model
{
    public enum ModelType
    {
        NoType=0,
        //模型
        Model=1,
        //贴图
        Image=2,
        //材质
        Meterial=3,
    }
    public class LiuLiuModel:INotifyPropertyChanged
    {
        public int ID { get; set; } 
        public int DownloadCoin { get; set; } 
        public String Name { get; set; }
        public String KeyWord { get; set; }
        public String Image { get; set; }
        public Double SizeByte { get; set; }
        public Double SizeMB { get => Math.Round(SizeByte / 1024,2); set => SizeByte += 1024 * value; }
        public String LocalPath { get; set; }
        public ModelType ModelType { get; set; } = ModelType.Model;
        public event PropertyChangedEventHandler PropertyChanged;
        public override string ToString()
        {
            return $"ID：{ID}\n名称：{Name}";
        }
    }
}
