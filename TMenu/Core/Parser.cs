using System;
using System.IO;
using System.Linq;
using Newtonsoft.Json.Linq;
using TMenu.Controls;
using TShockAPI;

namespace TMenu.Core
{
    internal class Parser
    {
        //todo
        /*public static Json Instance = new();
        public List<DeserilzerBase<VisualObject>> Deserlizers = new();
        public class TMenuFileDeserilzer<T> : JsonConverter
        {
            public override bool CanConvert(Type objectType)
            {
                return objectType.IsSubclassOf(typeof(TMenuControlBase<VisualObject>));
            }

            public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
            {
                var json = JObject.Load(reader);
                if (json.TryGetValue("menu", StringComparison.CurrentCultureIgnoreCase, out var menuJson))
                {
                    if (menuJson.Children)
            }
                else
                    throw new("");
                bool TryParsePanel()
                {

                }
            }

            public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
            {
                throw new NotImplementedException();
            }
        }
        public abstract class DeserilzerBase<T> where T : VisualObject
        {
            public DeserilzerBase()
            {
                Register();
            }
            public virtual void Register()
            {
                //Json.Instance.Deserlizers.Add(this);
            }
            public abstract TMenuControlBase<T> Deserlize(JObject jobj);
        }
        */
        public static TPanel Deserilize(string path, TSPlayer plr = null)
        {
            try
            {
                if (File.Exists(path))
                    return DeserilizeInner(path, File.ReadAllText(path), plr);
            }
            catch (Exception ex)
            {
                Logs.Error($"无法加载菜单 {Path.GetFileName(path)}\r\n{ex}");
            }
            return null;
        }
        internal static TPanel DeserilizeInner(string path, string text, TSPlayer plr = null)
        {
            try
            {
                if (Utils.TryParseJson<JObject>(text, out var json))
                {
                    if (json.TryGetValue("menu", StringComparison.CurrentCultureIgnoreCase, out var menuJson))
                    {
                        if (Data.FileData.TryParseFromJson(menuJson, out var data))
                        {
                            data.Path = path;
                            data.Player = plr; //用于计算里面的各种变量 为空则只加载信息
                            var tempPanel = new TPanel(data);
                            tempPanel.RootPanel = tempPanel;
                            ParseChilds(tempPanel, tempPanel, menuJson);
                            return tempPanel;
                        }
                        //throw new("Unable to read menu file, please check if the format is correct.");
                        throw new("无法读取菜单文件, 请检查格式.");
                    }
                    else
                        //throw new("Entry point not found: \"menu\", please check the file format.");
                        throw new("未找到入口点: \"menu\", 请检查格式.");
                }
                else
                    //throw new("Invalid json file.");
                    throw new("无效的json文件.");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private static void ParseChilds(TPanel root, dynamic parent, JToken json)
        {
            if (json.Value<JArray>("child") is { } childs)
            {
                childs.ForEach(c =>
                {
                    if (c.Value<string>("type") is { } typeName && Data.ControlName.FirstOrDefault(c => c.Value == typeName.ToLower()) is { Key: not null } type)
                    {
                        if (type.Value == "panel")
                            Logs.Error($"Cannot continue to add panels inside the menu.");
                        else if (TryParseControl(c, type.Key, out dynamic tempControl))
                        {
                            ParseChilds(root, tempControl, c);
                            tempControl.RootPanel = root;
                            parent.TUIObject.Add(tempControl.TUIObject);
                        }
                        else
                            Logs.Error($"An error occurred when initlizing \"{json.Root["menu"].Value<string>("name")}\", located in \"{json.Value<string>("name")}\".");
                    }
                    else
                        Logs.Error($"Cannot find control named: \"{c.Value<string>("type")}\".");
                });
            }
        }
        private static bool TryParseControl(JToken json, Type t, out object control)
        {
            if (Data.FileData.TryParseFromJson(json, out var data))
            {
                control = Activator.CreateInstance(t, new object[] { data });
                return true;
            }
            else
            {
                control = default;
                return false;
            }
        }
    }
}
