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
    /// <summary>
    /// 储存的数据
    /// </summary>
    public abstract partial class TMenuControlBase<T> where T : VisualObject
    {
        public struct InitInfo
        {
            public int X { get; set; }
            public int Y { get; set; }
            public int Width { get; set; }
            public int Height { get; set; }
            public UIConfiguration Configuration { get; set; }
            public UIStyle Style { get; set; }
        }
        public InitInfo TempInitInfo;
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
        public TMenuControlBase(string name, int x, int y, int width, int height, UIConfiguration configuration = null, UIStyle style = null)
        {
            Name = name;
            TempInitInfo = new InitInfo()
            {
                X = x,
                Y = y,
                Width = width,
                Height = height,
                Configuration = configuration,
                Style = style
            };

            //TUIObject = (T)Activator.CreateInstance(typeof(VisualObject), new object[] { x, y, width, height, configuration, style });
        }
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
        public Core.Events.Click Click { get; set; } = new();
        [JsonIgnore]
        public T TUIObject { get; internal set; }
    }
    public abstract partial class TMenuControlBase<T> where T : VisualObject
    {
        public TMenuControlBase<T> AddChild<T>(TMenuControlBase<T> child) where T : VisualObject
        {
            TUIObject.Add(child.TUIObject);
            return child;
        }
        /// <summary>
        /// 将本控件设为其他控件的子控件
        /// </summary>
        /// <param name="target">目标控件</param>
        /// <exception cref="ArgumentNullException"></exception>
        public void AddToTMenu(TMenuControlBase<T> target, int? layer = null)
        {
            if (target is null)
                throw new ArgumentNullException("target");
            Data.Controls.Add((TMenuControlBase<VisualObject>)this);
        }
        public TMenuControlBase<T> AddToTUI(TMenuControlBase<T> target, int? layer = null)
        {
            if (target is null)
                throw new ArgumentNullException("target");
            target.TUIObject.Add(TUIObject, layer);
            return this;
        }
        private void ClickEvent(T sender, Touch t)
        {
            Click.Command?.ForEach(c => Commands.HandleCommand(t.Player(), c));
            TShock.Utils.Broadcast(Click.Message ?? string.Empty, Color.White);
            if (!string.IsNullOrEmpty(Click.Goto) && Data.Controls.FirstOrDefault(c => c.TUIObject.GetType() == typeof(VisualContainer) && c.Name.ToLower() == Name.ToLower()) is { } target)
                target.TUIObject.SetTop(target.TUIObject);
            else
                throw new();
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
