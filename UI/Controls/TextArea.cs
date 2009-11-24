/*
 * Licensing
 *  Mozilla Public License 1.1 (MPL 1.1)
 * 
 * Fernando Escolar Martínez-Berganza <fer.escolar@gmail.com>
 * 
 */
using System;
using System.Web.UI;
using System.Drawing;
using System.Globalization;
using System.ComponentModel;

namespace nxAjax.UI.Controls
{
    /// <summary>
    /// TextArea control
    /// <code>
    /// &lt;ajax:TextArea runat="server"&gt;&lt;/ajax:TextArea&gt;
    /// </code>
    /// </summary>
    [Designer("nxAjax.UI.Design.nxControlDesigner")]
    [DefaultEventAttribute("ServerChange")]
    [ToolboxData("<{0}:TextArea runat=\"server\"></{0}:TextArea>")]
    [ToolboxItem(true)]
    [ToolboxBitmap(typeof(System.Web.UI.HtmlControls.HtmlTextArea))]
    public class TextArea : nxContainerControl
    {
        #region Public Javascript Client Events
        /// <summary>
        /// Gets/Sets Javascript onchange event code
        /// <remarks>Is a client event</remarks>
        /// </summary>
        [Category("Action"), DefaultValue(""), Description("OnClick javascript event in client.")]
        public string ClientChange
        {
            get
            {
                string s = Attributes["onchange"];
                return (s == null) ? string.Empty : s;
            }
            set
            {
                if (value == null)
                    Attributes.Remove("onchange");
                else
                    Attributes["onchange"] = value;
            }
        }
        /// <summary>
        /// Gets/Sets Javascript onfocus event code
        /// <remarks>Is a client event</remarks>
        /// </summary>
        [Category("Action"), DefaultValue(""), Description("OnFocus javascript event in client.")]
        public string ClientFocus
        {
            get
            {
                string s = Attributes["onfocus"];
                return (s == null) ? string.Empty : s;
            }
            set
            {
                if (value == null)
                    Attributes.Remove("onfocus");
                else
                    Attributes["onfocus"] = value;
            }
        }
        /// <summary>
        /// Gets/Sets Javascript onblur event code
        /// <remarks>Is a client event</remarks>
        /// </summary>
        [Category("Action"), DefaultValue(""), Description("OnBlur javascript event in client.")]
        public string ClientBlur
        {
            get
            {
                string s = Attributes["onblur"];
                return (s == null) ? string.Empty : s;
            }
            set
            {
                if (value == null)
                    Attributes.Remove("onblur");
                else
                    Attributes["onblur"] = value;
            }
        }
        /// <summary>
        /// Gets/Sets Javascript onselect event code
        /// <remarks>Is a client event</remarks>
        /// </summary>
        [Category("Action"), DefaultValue(""), Description("OnSelect javascript event in client.")]
        public string ClientSelect
        {
            get
            {
                string s = Attributes["onselect"];
                return (s == null) ? string.Empty : s;
            }
            set
            {
                if (value == null)
                    Attributes.Remove("onselect");
                else
                    Attributes["onselect"] = value;
            }
        }
        #endregion

        #region Public Server Events
        /// <summary>
        /// Raises on Control Value change
        /// <remarks>Is a server event</remarks>
        /// </summary>
        public event nxEventHandler ServerChange;
        /// <summary>
        /// Raises on Control gets focus
        /// <remarks>Is a server event</remarks>
        /// </summary>
        public event nxEventHandler ServerFocus;
        /// <summary>
        /// Raises on Control lost focus
        /// <remarks>Is a server event</remarks>
        /// </summary>
        public event nxEventHandler ServerBlur;
        /// <summary>
        /// Raises on Control is selected
        /// <remarks>Is a server event</remarks>
        /// </summary>
        public event nxEventHandler ServerSelect;
        #endregion

        #region Public Properties
        /// <summary>
        /// Gets/Sets Cols attribute value.
        /// <remarks>Number of characters per line</remarks>
        /// </summary>
        [DefaultValue("")]
        [Description("")]
        [Category("Appearance")]
        public int Cols
        {
            get
            {
                string s = Attributes["cols"];
                return (s == null) ? -1 : Convert.ToInt32(s);
            }
            set
            {
                if (value == -1)
                    Attributes.Remove("cols");
                else
                    Attributes["cols"] = value.ToString(CultureInfo.InvariantCulture);
            }
        }

        /// <summary>
        /// Gets/Sets Name attribute value
        /// </summary>
        [DefaultValue("")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Description("")]
        [Category("Behavior")]
        public virtual string Name
        {
            get { return ID; }
            set { ; }
        }

        /// <summary>
        /// Gets/Sets Rows attribute value.
        /// <remarks>
        /// Number of lines.
        /// </remarks>
        /// </summary>
        [DefaultValue("")]
        [Description("")]
        [Category("Appearance")]
        public int Rows
        {
            get
            {
                string s = Attributes["rows"];
                return (s == null) ? -1 : Convert.ToInt32(s);
            }
            set
            {
                if (value == -1)
                    Attributes.Remove("rows");
                else
                    Attributes["rows"] = value.ToString(CultureInfo.InvariantCulture);
            }
        }

        /// <summary>
        /// Gets/Sets Contained Value
        /// </summary>
        [DefaultValue("")]
        [Description("")]
        [Category("Appearance")]
        public string Value
        {
            get { return InnerText; }
            set { InnerText = value; }
        }
        /// <summary>
        /// Gets/Sets Readonly property
        /// </summary>
        [DefaultValue(false)]
        [Category("Appearance"), Description("Is ReadOnly")]
        public bool ReadOnly
        {
            get
            {
                string s = Attributes["readonly"];
                return (s != null);
            }
            set
            {
                if (!value)
                    Attributes.Remove("readonly");
                else
                    Attributes["readonly"] = "readonly";
            }
        }
        #endregion

        /// <summary>
        /// Creates a new TextArea control
        /// </summary>
        public TextArea() : base("textarea") { }

        #region Renders
        protected override bool hasEvent(string eventName)
        {
            switch (eventName.ToLower())
            {
                case "onchange":
                    return (ServerChange != null);
                case "onfocus":
                    return (ServerFocus != null);
                case "onblur":
                    return (ServerBlur != null);
                case "onselect":
                    return (ServerSelect != null);
            }
            return false;
        }
        protected override void RenderAttributes(nxAjaxTextWriter writer)
        {
            if (Attributes["name"] == null)
                writer.WriteAttribute("name", Name);
            
            RenderClientEventAttribute(writer, "onchange", "this.value");
            RenderClientEventAttribute(writer, "onfocus");
            RenderClientEventAttribute(writer, "onblur", "this.value");
            RenderClientEventAttribute(writer, "onselect");

            base.RenderAttributes(writer);
        }
        public override void RenderJS(nxAjaxTextWriter writer)
        {
            base.RenderJS(writer);
            if (hasChanged)
            {
                writer.Write("$('#" + ID + "').val(\"" + Value.Replace("'", "\\'").Replace("\"", "\\\"").Replace("\n", "\\n").Replace("\r", "\\r") + "\");");
                writer.Write("$('#" + ID + "').readonly(" + ReadOnly.ToString().ToLower() + ");");
            }
        }
        #endregion


        public override void RaiseEvent(string action, string value)
        {
            PutPostValue(value);
            switch (action.ToLower())
            {
                case "onfocus":
                    ServerFocus(this, value);
                    break;
                case "onchange":
                    ServerChange(this, value);
                    break;
                case "onselect":
                    ServerSelect(this, value);
                    break;
                case "onblur":
                    ServerBlur(this, value);
                    break;
            }
        }
        public override void PutPostValue(string obj)
        {
            this.Value = obj;
        }
    }
}
