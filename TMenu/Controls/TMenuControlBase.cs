using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Newtonsoft.Json;
using TerrariaUI.Base;
using TerrariaUI.Base.Style;
using TShockAPI;

namespace TMenu.Controls
{
    public class NameInJsonAttribute : Attribute
    {
        public NameInJsonAttribute(string name)
        {
            Name = name;
        }
        public string Name { get; set; }
    }
    /// <summary>
    /// 控件容器 虽然感觉其实不太用的上这个
    /// </summary>
    public abstract partial class TMenuControlBase<T> where T : VisualObject
    {
        /// <summary>
        /// 由于tui的控件类型不能强制转换 需要自己在派生的构造函数里实例化tui对象
        /// </summary>
        /// <param name="name"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="configuration"></param>
        /// <param name="style"></param>
        public TMenuControlBase(string name, string x, string y, string width, string height, UIConfiguration configuration = null, UIStyle style = null, Data.Click clickCommand = null)
        {
            Name = name;
            Data = new(name, x, y, width, height, "", configuration, style, clickCommand);

            //TUIObject = (T)Activator.CreateInstance(typeof(VisualObject), new object[] { x, y, width, height, configuration, style });
        }
        public TMenuControlBase(Data.FileData data)
        {
            Name = data.Name;
            Data = data;
        }
        [JsonIgnore]
        public Type Type => typeof(T);
        public TPanel RootPanel {  get; set; }
        /// <summary>
        /// 控件的名称
        /// </summary>
        public string Name { get; set; }
        public int X { get => TUIObject.X; set => TUIObject.X = value; }
        public int Y { get => TUIObject.Y; set => TUIObject.Y = value; }
        public int Width { get => TUIObject.Width; set => TUIObject.Width = value; }
        public int Height { get => TUIObject.Height; set => TUIObject.Height = value; }
        [JsonIgnore]
        public List<VisualObject> Childs => TUIObject.Child;
        public UIStyle Style
        {
            get => TUIObject.Style;
            set
            {
                if (value is not null)
                    TUIObject.Style = value;
            }
        }
        public UIConfiguration Configuration
        {
            get => TUIObject.Configuration;
            set
            {
                if (value is not null)
                    TUIObject.Configuration = value;
            }
        }
        public Data.Click Click => Data.ClickCommand;
        public Data.FileData Data { get; set; } = new();
        [JsonIgnore]
        public T TUIObject { get; internal set; }
    }
    public abstract partial class TMenuControlBase<T> where T : VisualObject
    {
        public abstract TMenuControlBase<T> Init();
        public TMenuControlBase<T> AddChild<T>(TMenuControlBase<T> child) where T : VisualObject
        {
            if (child is null)
                throw new ArgumentNullException();
            TUIObject.Add(child.TUIObject);
            return child;
        }
        public TMenuControlBase<T> AddTo(TMenuControlBase<T> target, int? layer = null)
        {
            if (target is null)
                throw new ArgumentNullException("target");
            target.TUIObject.Add(TUIObject, layer);
            return this;
        }
        private void ClickEvent(T sender, Touch t)
        {
            Click.Command?.ForEach(c => Commands.HandleCommand(t.Player(), c));
            if (!string.IsNullOrEmpty(Click.Message))
                t.Player().SendMessage(Click.Message, Color.White);
            if (!string.IsNullOrEmpty(Click.Goto))
            {
                if (TUIObject.Root.Child.FirstOrDefault(c => c.GetType() == typeof(VisualContainer) && c.Name.ToLower() == Name.ToLower()) is { } target)
                    TUIObject.Root.SetTop(target);
                else
                    throw new($"Unable to find the specified control: \"{Click.Goto}\"");
            }
        }

        public static implicit operator VisualObject(TMenuControlBase<T> t) => t.TUIObject;
        public static explicit operator TMenuControlBase<VisualObject>(TMenuControlBase<T> t)
        {
            return t as TMenuControlBase<VisualObject>;
        }
    }
    /// <summary>
    /// 各种事件
    /// </summary>
    public abstract partial class TMenuControlBase<T> where T : VisualObject
    {
        internal void OnClick(VisualObject sender, Touch t)
        {
            switch (t.State)
            {
                case TouchState.Begin:
                    OnMouseDown((T)sender, t);
                    break;
                case TouchState.Moving:
                    OnMouseMove((T)sender, t);
                    break;
                case TouchState.End:
                    OnMouseUp((T)sender, t);
                    break;
            }
        }
        public virtual void OnMouseDown(T sender, Touch t)
        {
            ClickEvent(sender, t);
        }
        public virtual void OnMouseUp(T sender, Touch t)
        {

        }
        public virtual void OnMouseMove(T sender, Touch t)
        {

        }
    }
}
