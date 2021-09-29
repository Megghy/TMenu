using System;
using System.IO;
using Newtonsoft.Json.Linq;
using TerrariaUI.Base;
using TMenu.Controls;
using TShockAPI;
using static Terraria.WorldBuilding.Searches;

namespace TMenu.Core
{
    internal class FileManager
    {
        public static string SavePath => Path.Combine(TShockAPI.TShock.SavePath, "TMenu");
        public static string ConfigFilePath => Path.Combine(SavePath, "TMenuConfig.json");

        public static void Init()
        {
            Config._instance = null;

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
            void AddChilds<T>(TMenuControlBase<T> parent, JToken json) where T : VisualObject
            {
                if (json.Value<JArray>("child") is { } childs)
                {
                    childs.ForEach(c =>
                    {
                        if (c.Value<string>("type") is { } type)
                        {
                            switch (type)
                            {
                                case "button":
                                    if (TryParseControl<TButton>(c, out var tempButton))
                                    {
                                        AddChilds(tempButton, c);
                                        parent.AddChild(tempButton);
                                    }
                                    else
                                        LogError(c);
                                    break;
                                case "container":
                                    if (TryParseControl<TContainer>(c, out var tempContainer))
                                    {
                                        AddChilds(tempContainer, c);
                                        parent.AddChild(tempContainer);
                                    }
                                    else
                                        LogError(c);
                                    break;
                            }
                        }
                    });
                }
            }
            void LogError(JToken json)
            {
                TShock.Log.ConsoleInfo($"[TMenu] An error occurred when initlizing \"{json.Root["menu"].Value<string>("name")}\", located in \"{json.Value<string>("name")}\".");
            }
            bool TryParseControl<T>(JToken json, out T control)
            {
                if (Data.FileData.TryParseFromJson(json, out var data))
{
                    control = (T)Activator.CreateInstance(typeof(T), new object[] { data });
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
