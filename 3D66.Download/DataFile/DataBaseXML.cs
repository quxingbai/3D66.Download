using _3D66.Download.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace _3D66.Download.DataFile
{
    public static class DataBaseXML
    {
        /// <summary>
        /// 标签
        /// </summary>
        private static Dictionary<int, List<string>> TagsTemp = null;
        /// <summary>
        /// 模型数据的缓存，最后一次查询的数据
        /// </summary>
        private static List<LiuLiuModel> LiuLiuModelTemp = null;
        private static List<ProjectEntityModel> ProjectModelsTemp = null;
        private static XmlDocument xml = new XmlDocument();
        private static XmlNode NODE_LiuLiuModels;
        private static XmlNode NODE_Tags;
        private static XmlNode NODE_ProjectEntitys;
        private static XmlNode NODE_Config;
        private static String[] NoGetSetMembers = { "SizeMB" };
        private static String[] KeysTemp;
        private static List<KeyValuePair<int, string>> ProjectTemplateTypesTemp = null;
        private static XmlNode NODE_ProjectEntityType;
        static DataBaseXML()
        {
            xml.Load(Add66DownloadItemForm.Setting.Path_DataBaseXMl);
            NODE_LiuLiuModels = xml["DataBase"]["LiuLiuModels"];
            NODE_Tags = xml["DataBase"]["Tags"];
            NODE_ProjectEntitys = xml["DataBase"]["ProjectEntitys"];
            NODE_Config = xml["DataBase"]["Config"];
            NODE_ProjectEntityType = xml["DataBase"]["ProjectEntityType"];
        }
        /// <summary>
        /// 模型转换为Node
        /// </summary>
        public static XmlNode ToNode(Object m)
        {
            var type = m.GetType();
            XmlNode n = xml.CreateElement(type.Name);
            foreach (var p in type.GetProperties())
            {
                if (NoGetSetMembers.Where(w => w == p.Name).Count() == 0)
                    n.Attributes.Append(Attr(p.Name, p.GetValue(m).ToString()));
            }
            return n;

        }
        /// <summary>
        /// 将Node 转换为模型
        /// </summary>
        public static LiuLiuModel ToLiuLiuModel(XmlNode n)
        {
            LiuLiuModel ll = new LiuLiuModel();
            foreach (XmlAttribute attr in n.Attributes)
            {
                if (attr.Name == "ModelType")
                {
                    ModelType mt = Enum.Parse<ModelType>(attr.Value);
                    ll.ModelType = mt;
                }
                else
                {
                    TypeCode c = Enum.Parse<TypeCode>(ll.GetType().GetProperty(attr.Name).PropertyType.Name);
                    ll.GetType().GetProperty(attr.Name).SetValue(ll, Convert.ChangeType(attr.Value, c));
                }
            }
            return ll;
        }
        /// <summary>
        /// 查询所有模型
        /// </summary>
        public static List<LiuLiuModel> SelectALLLiuLiuModel()
        {
            List<LiuLiuModel> m = new List<LiuLiuModel>();
            foreach (XmlNode n in NODE_LiuLiuModels.ChildNodes)
            {
                LiuLiuModel lm = ToLiuLiuModel(n);
                m.Add(lm);
            }
            LiuLiuModelTemp = m;
            return m;
        }
        /// <summary>
        /// 查询缓存 LiuLiuModel，如果没有缓存将自造缓存
        /// </summary>
        public static List<LiuLiuModel> SelectAllLiuLiuModel_Temp()
        {
            if (LiuLiuModelTemp == null) SelectALLLiuLiuModel();
            return LiuLiuModelTemp;
        }
        /// <summary>
        /// 添加一个LiuLiuModel数据
        /// </summary>
        public static bool AddLiuLiuModel(LiuLiuModel m, bool IsSave = true)
        {
            if (SelectAllLiuLiuModel_Temp().Where(l => l.ID == m.ID).Count() > 0)
            {
                return false;
            }
            NODE_LiuLiuModels.AppendChild(ToNode(m));
            if (IsSave) Save();
            LiuLiuModelTemp.Add(m);
            return true;
        }
        /// <summary>
        /// 删除一个LiuLiuModel
        /// </summary>
        public static bool RemoveLiuLiuModel(int LLID, bool IsSave = true)
        {
            if (HasLiuLiuModelID(LLID))
            {
                XmlNode n = null;
                foreach (XmlNode fn in NODE_LiuLiuModels.ChildNodes)
                {
                    if (fn.Attributes["ID"].Value == LLID.ToString())
                    {
                        n = fn;
                        break;
                    }
                }
                NODE_LiuLiuModels.RemoveChild(n);
                if (IsSave)
                {
                    Save();
                }
                return true;
            }
            return false;
        }
        /// <summary>
        /// 创建一个标签属性
        /// </summary>
        public static XmlAttribute Attr(string Name, String Value)
        {
            XmlAttribute a = xml.CreateAttribute(Name);
            a.Value = Value;
            return a;
        }
        /// <summary>
        /// 创建一个Node
        /// </summary>
        public static XmlNode Node(string NodeName)
        {
            XmlNode n = xml.CreateElement(NodeName);
            return n;
        }
        /// <summary>
        /// 保存
        /// </summary>
        public static void Save()
        {
            xml.Save(Add66DownloadItemForm.Setting.Path_DataBaseXMl);
        }
        public static bool HasLiuLiuModelID(int ID)
        {
            return SelectAllLiuLiuModel_Temp().Where(l => l.ID == ID).Count() > 0;
        }
        /// <summary>
        /// 从缓存中获取一个模型
        /// </summary>
        public static LiuLiuModel SelectLiuLiuModelByID_Temp(int LLID)
        {
            if (!HasLiuLiuModelID(LLID)) return null;
            return SelectAllLiuLiuModel_Temp().Where(m => m.ID == LLID).First();
        }
        /// <summary>
        /// 根据标签查询LiuLiuModel
        /// </summary>
        public static List<LiuLiuModel> SelectLiuLiuModelByTag_Temp(string Tag)
        {
            List<LiuLiuModel> l = new List<LiuLiuModel>();
            foreach (var d in SelectAllTags_Temp())
            {
                if (d.Value.Where(v => v == Tag).Count() > 0)
                {
                    l.Add(SelectLiuLiuModelByID_Temp(d.Key));
                    continue;
                }
            }
            return l;
        }
        /// <summary>
        /// 查询所有标签
        /// </summary>
        public static Dictionary<int, List<string>> SelectAllTags()
        {
            Dictionary<int, List<string>> d = new Dictionary<int, List<string>>();
            foreach (XmlNode n in NODE_Tags.ChildNodes)
            {
                int LLID = Convert.ToInt32(n.Attributes["LLID"].Value);
                string Text = Convert.ToString(n.Attributes["Text"].Value);
                if (d.Keys.Where(k => k == LLID).Count() == 0)
                {
                    d.Add(LLID, new List<string>());
                }
                d[LLID].Add(Text);
            }
            TagsTemp = d;
            return d;
        }
        /// <summary>
        /// 查询标签缓存，如果没有就创建缓存
        /// </summary>
        /// <returns></returns>
        public static Dictionary<int, List<string>> SelectAllTags_Temp()
        {
            if (TagsTemp == null) return SelectAllTags();
            return TagsTemp;
        }
        /// <summary>
        /// 添加一个标签
        /// </summary>
        public static bool ADDTag(int LLID, string Tag, bool IsSave = true)
        {
            try
            {
                if (HasTag(LLID, Tag))
                {
                    return false;
                }
                XmlNode n = Node("Tag");
                n.Attributes.Append(Attr("LLID", LLID.ToString()));
                n.Attributes.Append(Attr("Text", Tag));
                NODE_Tags.AppendChild(n);
                if (IsSave)
                {
                    Save();
                }
                if (TagsTemp.Keys.Where(k => k == LLID).Count() == 0) TagsTemp.Add(LLID, new List<string>());
                TagsTemp[LLID].Add(Tag);
                return true;
            }
            catch
            {
                return false;
            }
        }
        public static int RemoveTagByLLID(int LLID, bool IsSave = true)
        {
            int count = 0;
            foreach (XmlNode n in NODE_Tags.ChildNodes)
            {
                if (n.Attributes["LLID"].Value == LLID.ToString())
                {
                    NODE_Tags.RemoveChild(n);
                    count += 1;
                }
            }
            if (IsSave && count > 0)
            {
                Save();
            }
            return count;
        }
        /// <summary>
        /// 此模型是否已经存在此标签
        /// </summary>
        public static bool HasTag(int LLID, string Tag)
        {
            try
            {
                List<string> ks = SelectAllTags_Temp()[LLID];
                return ks.Where(k => k == Tag).Count() > 0;
            }
            catch
            {
                return false;
            }
        }
        /// <summary>
        /// 转换为项目实例
        /// </summary>
        /// <param name="n"></param>
        /// <returns></returns>
        public static ProjectEntityModel ToProjectEntityModel(XmlNode n)
        {
            ProjectEntityModel ll = new ProjectEntityModel();
            foreach (XmlAttribute attr in n.Attributes)
            {
                TypeCode c = Enum.Parse<TypeCode>(ll.GetType().GetProperty(attr.Name).PropertyType.Name);
                ll.GetType().GetProperty(attr.Name).SetValue(ll, Convert.ChangeType(attr.Value, c));
            }
            return ll;
        }
        /// <summary>
        /// 查询所有项目实例
        /// </summary>
        public static List<ProjectEntityModel> SelectAllProjectEntitys()
        {
            List<ProjectEntityModel> ms = new List<ProjectEntityModel>();
            foreach (XmlNode n in NODE_ProjectEntitys.ChildNodes)
            {
                ms.Add(ToProjectEntityModel(n));
            }
            ProjectModelsTemp = ms;
            return ms;
        }
        /// <summary>
        /// 获取一个设置详情
        /// </summary>
        public static string GetConfig(string key)
        {
            return NODE_Config["ProjectEntityModelNextID"].Attributes["Value"].Value;
        }
        /// <summary>
        /// 设置一个设置详情
        /// </summary>
        public static bool SetConfig(string key, string value, bool IsSave = true)
        {
            try
            {
                NODE_Config["ProjectEntityModelNextID"].Attributes["Value"].Value = value;
                if (IsSave)
                {
                    Save();
                }
                return true;
            }
            catch
            {
                return false;
            }
        }
        /// <summary>
        /// 尝试在缓存中查询出所有项目实例，无则创建
        /// </summary>
        public static List<ProjectEntityModel> SelectAllProjectEntitys_Temp()
        {
            if (ProjectModelsTemp == null)
            {
                return SelectAllProjectEntitys();
            }
            else
            {
                return ProjectModelsTemp;
            }
        }
        /// <summary>
        /// 添加一个项目实例
        /// </summary>
        public static bool AddProjectEntity(ProjectEntityModel model, bool IsSave = true)
        {
            try
            {
                model.ID = GetNextProjectEntityID();
                XmlNode n = ToNode(model);
                NODE_ProjectEntitys.AppendChild(n);
                SetConfig("ProjectEntityModelNextID", (model.ID + 1).ToString());
                if (IsSave)
                {
                    Save();
                }
            }
            catch
            {
                return false;
            }
            return true;
        }
        /// <summary>
        /// 获取下一个ID
        /// </summary>
        /// <returns></returns>
        public static int GetNextProjectEntityID()
        {
            return Convert.ToInt32(GetConfig("ProjectEntityModelNextID"));
        }
        public static int GetProjectEntityTypeNextID()
        {
            return Convert.ToInt32(GetConfig("ProjectEntityTypeNextID"));
        }
        /// <summary>
        /// 获取所有项目模板类型
        /// </summary>
        /// <returns></returns>
        public static List<KeyValuePair<int, string>> SelectALLProjectTemplateType()
        {
            List<KeyValuePair<int, string>> l = new List<KeyValuePair<int, string>>();
            foreach (XmlNode n in NODE_ProjectEntityType.ChildNodes)
            {
                int id = Convert.ToInt32(n.Attributes["PETID"].Value);
                string val = n.Attributes["Value"].Value;
                l.Add(new KeyValuePair<int, string>(id, val));
            }
            ProjectTemplateTypesTemp = l;
            return l;
        }
        public static bool AddProjectTemplateTypeItem(string val,bool IsSave=true)
        {
            try
            {
                XmlNode n = Node("MenuItem");
                n.Attributes.Append(Attr("PETID", GetProjectEntityTypeNextID().ToString()));
                n.Attributes.Append(Attr("Value", val));
                NODE_ProjectEntityType.AppendChild(n);
                if (IsSave) Save();
                DataBaseXML.SelectALLProjectTemplateType();
            }
            catch
            {
                return false;
            }
            return true;
        }
        public static List<KeyValuePair<int, string>> SelectALLProjectTemplateType_Temp()
        {
            return ProjectTemplateTypesTemp ?? SelectALLProjectTemplateType();
        }
        /// <summary>
        /// 删除一个项目模型
        /// </summary>
        public static bool RemoveProjectTemplateByID(int ID,bool IsSave=true)
        {
            foreach (XmlNode n in NODE_ProjectEntitys.ChildNodes)
            {
                if (n.Attributes["ID"].Value == ID.ToString())
                {
                    NODE_ProjectEntitys.RemoveChild(n);
                    if (IsSave) Save();
                    SelectAllProjectEntitys();
                    return true;
                }
            }
            return false;
        }
    }
}
