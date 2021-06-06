using System;
using System.Collections.Generic;
using System.Text;

namespace _3D66.Download.Model
{
    public class MaxModel
    {
        public int ID { get; set; } = 0;
        public int DownloadCoin { get; set; } = 0;
        public String Name { get; set; }
        public String KeyWord { get; set; }
        public String Image { get; set; }
        public Double SizeByte { get; set; } =0;
        public Double SizeMB { get => Math.Round(SizeByte / 1024,2); set => SizeByte += 1024 * value; }

    }
}
