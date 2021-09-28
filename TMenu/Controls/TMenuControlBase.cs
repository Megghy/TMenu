using System;
using Newtonsoft.Json;
using TerrariaUI.Base;
using TerrariaUI.Base.Style;
using static Terraria.WorldBuilding.Searches;

namespace TMenu.Controls
{
/// <summary>
/// 储存的数据
/// </summary>
    public abstract partial class TMenuControlBase<T> where T : VisualObject
    {
        public TMenuControlBase(string name, int x, int y, int width, int height, UIConfiguration configuration = null, UIStyle style = null)
        {
            Name = name;
            TUIObject = (T)new VisualObject(x, y, width, height, configuration, style, OnClick);
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
        public T TUIObject { get; internal set; }
    }
    public abstract partial class TMenuControlBase<T> where T : VisualObject
    {
        public virtual void Init()
        {

        }
        /// <summary>
        /// 将本控件设为其他控件的子控件
        /// </summary>
        /// <param name="target">目标控件</param>
        /// <exception cref="ArgumentNullException"></exception>
        public void AddTo(TMenuControlBase<T> target, int? layer = null)
        {
            if (target is null)
                throw new ArgumentNullException("target");
            target.TUIObject.Add(TUIObject, layer);
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
                    OnMouseDown(sender, t);
                    break;
                case TouchState.Moving:
                    OnMouseMove(sender, t);
                    break;
                case TouchState.End:
                    OnMouseUp(sender, t);
                    break;
            }
        }
        public virtual void OnMouseDown(VisualObject sender, Touch t)
        {

        }
        public virtual void OnMouseUp(VisualObject sender, Touch t)
        {

        }
        public virtual void OnMouseMove(VisualObject sender, Touch t)
        {

        }
    }
}
