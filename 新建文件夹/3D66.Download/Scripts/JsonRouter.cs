using System;
using System.Collections.Generic;
using System.Text;

namespace _3D66.Download.Scripts
{
    public enum JsonErrorMenu
    {
        NoError=-1,
        NoLogin=0,
        OK=200,
        /// <summary>
        /// 达到最大下载次数
        /// </summary>
        NoFreeCount=10511,

    }
    public class JsonRouter
    {
        public static bool Router666(dynamic j)
        {
            switch (GetJsonError(j))
            {
                case JsonErrorMenu.NoError:return true; ;break;
                case JsonErrorMenu.NoFreeCount: return true; ; break;
            }
            return true;
        }
        public static JsonErrorMenu GetJsonError(dynamic j)
        {
            JsonErrorMenu menu = JsonErrorMenu.NoError;
            try
            {
                menu=((JsonErrorMenu)j.status);
            }
            catch
            {

            }
            return menu;
        }

    }
}
