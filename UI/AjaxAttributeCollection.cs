using System;
using System.Web.UI;
using System.Collections;
using System.Globalization;
using System.Security.Permissions;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace Framework.Ajax.UI
{
    [Serializable()]
    public sealed class AjaxAttributeCollection : ISerializable
    {
        [NonSerialized()]
        private StateBag bag;
        [NonSerialized()]
        private AjaxCssStyleCollection styleCollection;

        public AjaxAttributeCollection(StateBag bag)
        {
            this.bag = bag;
        }
        public AjaxAttributeCollection(SerializationInfo info, StreamingContext context)
        {
            string[] keys = info.GetString("BagKeys").Split(',');
            bag = new StateBag();
            foreach (string key in keys)
                bag.Add(key, info.GetString(key));

        }
        public int Count
        {
            get { return bag.Count; }
        }
        public AjaxCssStyleCollection CssStyle
        {
            get
            {
                if (styleCollection == null)
                    styleCollection = new AjaxCssStyleCollection(bag);
                return styleCollection;
            }
        }
        public string this[string key]
        {
            get { return bag[key] as string; }

            set
            {
                Add(key, value);
            }
        }
        public ICollection Keys
        {
            get { return bag.Keys; }
        }

        public void Add(string key, string value)
        {
            if (0 == string.Compare(key, "style", true, CultureInfo.InvariantCulture))
            {
                CssStyle.Value = value;
                return;
            }
            bag.Add(key, value);
        }

        public void AddAttributes(HtmlTextWriter writer)
        {
            foreach (string key in bag.Keys)
            {
                string value = bag[key] as string;
                writer.AddAttribute(key, value);
            }
        }

        public void Clear()
        {
            bag.Clear();
        }

        public void Remove(string key)
        {
            bag.Remove(key);
        }

        public void Render(HtmlTextWriter writer)
        {
            foreach (string key in bag.Keys)
            {
                string value = bag[key] as string;
                if (value != null)
                    writer.WriteAttribute(key, value, true);
            }
        }
        private string Join(string[] arr, string separator)
        {
            string result = string.Empty;
            foreach (string s in arr)
                result += (result == string.Empty ? "" : separator) + s;
            return result;
        }
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            string[] keys = new string[bag.Keys.Count];
            bag.Keys.CopyTo(keys, 0);
            info.AddValue("BagKeys", Join(keys, ","));
            foreach (string key in bag.Keys)
            {
                info.AddValue(key, bag[key]);
            }
        }
    }
}
