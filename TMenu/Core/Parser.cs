using System;
using System.IO;
using Newtonsoft.Json.Linq;
using TMenu.Controls;
using TShockAPI;

namespace TMenu.Core
{
    public static class Parser
    {
        public static TPanel DeserilizeFromFile(string path, TSPlayer plr) => Deserilize(ReadOriginDataFromFile(path), plr);
        public static TPanel Deserilize(this Data.MenuOriginData data, TSPlayer plr)
        {
            try
            {
                data.Player = plr;
                var menu = new TPanel(data);
                data.Childs.ForEach(c => DeserilizeChild(menu, menu, c, plr));
                return menu;
            }
            catch (Exception ex)
            {
                Logs.Error($"无法加载菜单 {data.Name}\r\n{ex}");
            }
            return null;
            void DeserilizeChild(TPanel root, dynamic parent, Data.MenuOriginData data, TSPlayer plr)
            {
                if (TryParseControl(data, plr, out var tempControl))
                {
                    tempControl.RootPanel = root;
                    tempControl.Data = data;
                    data.Parent = tempControl;
                    parent.TUIObject.Add(tempControl.TUIObject);
                    data.Childs.ForEach(c => DeserilizeChild(root, tempControl, c, plr));
                }
                else
                    Logs.Error($"An error occurred when initlizing \"{root.Name}\", located in \"{data.Name}\".");
                
            }
        }
        public static Data.MenuOriginData ReadOriginDataFromFile(string path)
        {
            try
            {
                if (File.Exists(path))
                    return ReadOriginDataInner(path, File.ReadAllText(path));
            }
            catch (Exception ex)
            {
                Logs.Error($"无法加载菜单 {Path.GetFileName(path)}\r\n{ex}");
            }
            return null;
        }
        public static Data.MenuOriginData ReadOriginDataInner(string path, string text)
        {
            try
            {
                if (Utils.TryParseJson<JObject>(text, out var json))
                {
                    if (json.TryGetValue("menu", StringComparison.CurrentCultureIgnoreCase, out var menuJson))
                    {
                        if (Data.MenuOriginData.TryParseFromJson(menuJson, out var data))
                        {
                            data.Path = path;
                            ReadChild(data, menuJson);
                            return data;
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
            void ReadChild(Data.MenuOriginData parent, JToken json)
            {
                if (json.Value<JArray>("child") is { } childs)
                {
                    childs.ForEach(c =>
                    {
                        if (c.Value<string>("type") is { } typeName && Data.ControlName.TryGetValue(typeName, out var t))
                        {
                            if (t == typeof(TPanel))
                                Logs.Error($"Cannot continue to add panels inside the menu.");
                            else
                            {
                                try
                                {
                                    if (Data.MenuOriginData.ParseFromJson(c) is { } tempData)
                                    {
                                        ReadChild(tempData, c);
                                        parent.Childs.Add(tempData);
                                    }
                                    else
                                        Logs.Error("Unknown error.");
                                }
                                catch(Exception ex)
                                {
                                    Logs.Error($"An error occurred when initlizing \"{json.Root["menu"].Value<string>("name")}\", located in \"{c.Value<string>("name")}\".\r\n{ex}");
                                }
                            }   
                        }
                        else
                            Logs.Error($"Cannot find control named: \"{c.Value<string>("type")}\".");
                    });
                }
            }
        }
        private static bool TryParseControl(Data.MenuOriginData data, TSPlayer plr, out dynamic control)
        {
            if (Data.ControlName.TryGetValue(data.Type, out var t))
            {
                data.Player = plr;
                control = Activator.CreateInstance(t, new object[] { data });
                return true;
            }
            control = false;
            return false;
        }
    }
}
