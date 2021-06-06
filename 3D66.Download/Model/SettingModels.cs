using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace _3D66.Download.Model
{
    public class SettingModels:INotifyPropertyChanged
    {
        public String Path_Download_LiuLiu { get;  set; }= @".\Model\";//下载模型地址
        public String Path_Download_LiuLiuImage { get;  set; }= @".\Images\";//下载图片地址
        public String Local_LiuLiuConfig { get;  set; }= @".\LocalConfig.xml\";//本地下载记录
        public String Path_DataBaseXMl { get;  set; }= @".\DataFile\DataBase.xml";
        public string Path_WinRARSetup { get;  set; }= @".\APPPack\winrar-x64-601scp.exe";
        public string Path_ProjectPath { get;  set; }= @".\\";
        public String Path_ProjectTemplatePath { get; set; } = @".\ModelTemplate\Emptys\";//模型模板
        public String Path_ProjectTemplateImagePath { get; set; } = @".\ModelTemplate\Images\";//模型图片
        public bool IsAutoCompression { get;  set; }= false;
        public bool IsAutoPrintScreenImage { get;  set; }

        public event PropertyChangedEventHandler PropertyChanged;

        public Object Set()
        {
            return null;
        }

    }
}
