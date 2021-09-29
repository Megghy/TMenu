using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using TerrariaUI.Base;
using TMenu.Controls;

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
    }
}
