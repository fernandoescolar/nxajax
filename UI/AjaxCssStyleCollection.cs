using System;
using System.Web.UI;
using System.IO;
using System.Collections;
using System.Security.Permissions;
using System.Text;


namespace Framework.Ajax.UI
{
    public sealed class AjaxCssStyleCollection
    {
        StateBag bag;
        StateBag style;
        string last_string;

        internal AjaxCssStyleCollection(StateBag bag)
        {
            this.bag = bag;
            if (bag != null)
            {
                last_string = (string)bag["style"];
                style = new StateBag();
                if (last_string != null)
                    FillStyle(last_string);
            }
        }

        void FillStyle(string s)
        {
            int mark = s.IndexOf(':');
            if (mark == -1)
                return;
            string key = s.Substring(0, mark).Trim();
            if (mark + 1 > s.Length)
                return;

            string fullValue = s.Substring(mark + 1);
            if (fullValue == "")
                return;

            mark = fullValue.IndexOf(';');
            string value;
            if (mark == -1)
                value = fullValue.Trim();
            else
                value = fullValue.Substring(0, mark).Trim();

            style.Add(key, value);
            if (mark + 1 > fullValue.Length)
                return;
            FillStyle(fullValue.Substring(mark + 1));
        }

        string BagToString()
        {
            if (last_string != null)
                return last_string;

            StringBuilder sb = new StringBuilder();
            foreach (string key in Keys)
                sb.AppendFormat("{0}: {1};", key, style[key]);

            last_string = sb.ToString();
            return last_string;
        }

        public int Count
        {
            get
            {
                if (bag == null)
                    throw new NullReferenceException();
                return style.Count;
            }
        }

        public string this[string key]
        {
            get
            {
                return style[key] as string;
            }

            set
            {
                Add(key, value);
            }
        }

        public ICollection Keys
        {
            get { return style.Keys; }
        }

        public void Add(string key, string value)
        {
            if (style == null)
                style = new StateBag();
            style[key] = value;
            last_string = null;
            bag["style"] = BagToString();
        }

		public void Add(HtmlTextWriterStyle key, string value)
        {
            Add(AjaxTextWriter.StaticGetStyleName(key), value);
        }

        public void Clear()
        {
            if (style != null)
                style.Clear();
            last_string = null;
            bag.Remove("style");
        }

        public void Remove(string key)
        {
            if (style != null)
                style.Remove(key);
            last_string = null;
            bag["style"] = BagToString();
        }
		public string this [HtmlTextWriterStyle key] {
			get {
				return style [AjaxTextWriter.StaticGetStyleName (key)] as string;
			}
			set {
                Add(AjaxTextWriter.StaticGetStyleName(key), value);
			}
		}

		public void Remove (HtmlTextWriterStyle key)
		{
            Remove(AjaxTextWriter.StaticGetStyleName(key));
		}

		public string Value
        {
            get { return BagToString(); }
            set
            {
                if (style != null)
                    style = new StateBag();
                style.Clear();
                last_string = value;
                bag["style"] = value;
                if (value != null)
                    FillStyle(value);
            }
        }
    }

}
