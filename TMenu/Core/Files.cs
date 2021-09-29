using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json.Linq;
using TerrariaUI.Base;
using TerrariaUI.Widgets;
using TMenu.Controls;
using TShockAPI;
using static Terraria.WorldBuilding.Searches;

namespace TMenu.Core
{
    internal class Files
    {
        public static string SavePath => Path.Combine(TShock.SavePath, "TMenu");
        public static string ConfigFilePath => Path.Combine(SavePath, "TMenuConfig.json");
        public static Dictionary<Type, string> ControlName { get; set; } = new();
        public static void Init()
        {
            Config._instance = null;
            ControlName.Add(typeof(TButton), "button");
            ControlName.Add(typeof(TContainer), "container");
            ControlName.Add(typeof(VisualSign), "sign");
        }
        public static void Deserilize(string path)
        {
            try
            {
                if (File.Exists(path))
                    DeserilizeInner(File.ReadAllText(path));
            }
            catch (Exception ex)
            {
                TShock.Log.ConsoleError($"Unable read menu file {Path.GetFileName(path)}\r\n{ex}");
            }
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
                        throw new("Unable to read menu file, please check if the format is correct");
                    }
                    else
                        throw new("");
                }
                else
                    throw new("Unable to parse json.");
            }
            catch (Exception ex)
            {
                throw ex;
            }
            bool TryParsePanel(JToken json, out TPanel panel)
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
                catch
                {
                    return false;
                }
            }
            void AddChilds(dynamic parent, JToken json)
            {
                if (json.Value<JArray>("child") is { } childs)
                {
                    childs.ForEach(c =>
                    {
                        if (c.Value<string>("type") is { } typeName && ControlName.FirstOrDefault(c => c.Value == typeName.ToLower()) is { Key: not null } type)
                        {
                            if (TryParseControl(c, type.Key, out dynamic tempButton))
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
            bool TryParseControl(JToken json, Type t, out object control)
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
}
