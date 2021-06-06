using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace _3D66.Download.Scripts
{
    public class ScriptControl
    {
        private const String ScriptAddress = "./Scripts/";

        public static Dictionary<String, String> Scripts = new Dictionary<string, string>();
        static ScriptControl()
        {
            foreach(String p in Directory.GetFiles(ScriptAddress))
            {
                FileInfo f = new FileInfo(p);
                string n = f.Name.Remove(f.Name.Length - 3);

                Scripts.Add(f.Name.Remove(f.Name.Length-3), File.ReadAllText(p));
            }
        }
        public static String GetScript(String SName)
        {
            var s = Scripts.Where(s => s.Key.ToUpper() == SName.ToUpper());
            if (s.Count() > 0)
            {
                return s.First().Value;
            }
            else
            {
                throw new Exception("错误的脚本名称：" + SName);
            }
        }
    }
}
