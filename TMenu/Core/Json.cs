using System;
using System.IO;
using System.Linq;
using Newtonsoft.Json.Linq;
using TMenu.Controls;
using TShockAPI;

namespace TMenu.Core
{
    internal class Json
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
        public static TPanel Deserilize(string path)
        {
            try
            {
                if (File.Exists(path))
                    return DeserilizeInner(File.ReadAllText(path));
            }
            catch (Exception ex)
            {
                TShock.Log.ConsoleError($"Unable read menu file {Path.GetFileName(path)}\r\n{ex}");
            }
            return null;
        }
        internal static TPanel DeserilizeInner(string text)
        {
            try
            {
                if (Utils.TryParseJson<JObject>(text, out var json))
                {
                    if (json.TryGetValue("menu", StringComparison.CurrentCultureIgnoreCase, out var menuJson))
                    {
                        if (TryParsePanel(menuJson, out var panel))
                        {
                            return panel;
                        }
                        throw new("Unable to read menu file, please check if the format is correct.");
                    }
                    else
                        throw new("Entry point not found: \"menu\", please check the file format.");
                }
                else
                    throw new("Invalid json file.");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private static bool TryParsePanel(JToken json, out TPanel panel)
        {
            panel = null;
            try
            {
                if (Data.FileData.TryParseFromJson(json, out var data))
                {
                    var tempPanel = new TPanel(data);
                    AddChilds(tempPanel, json);
                    panel = tempPanel;
                    return true;
                }
                else
                    return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private static void AddChilds(dynamic parent, JToken json)
        {
            if (json.Value<JArray>("child") is { } childs)
            {
                childs.ForEach(c =>
                {
                    if (c.Value<string>("type") is { } typeName && Data.ControlName.FirstOrDefault(c => c.Value == typeName.ToLower()) is { Key: not null } type)
                    {
                        if(type.Value == "panel")
                            TShock.Log.ConsoleInfo($"[TMenu] Cannot continue to add panels inside the menu.");
                        else if (TryParseControl(c, type.Key, out dynamic tempButton))
                        {
                            AddChilds(tempButton, c);
                            parent.TUIObject.Add(tempButton.TUIObject);
                        }
                        else
                            TShock.Log.ConsoleInfo($"[TMenu] An error occurred when initlizing \"{json.Root["menu"].Value<string>("name")}\", located in \"{json.Value<string>("name")}\".");
                    }
                    else
                        TShock.Log.ConsoleInfo($"[TMenu] Cannot find control named: \"{json.Value<string>("type")}\".");
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
