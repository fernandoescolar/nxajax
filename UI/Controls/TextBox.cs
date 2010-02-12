/*
 * Licensing
 *  Mozilla Public License 1.1 (MPL 1.1)
 * 
 * Fernando Escolar Martínez-Berganza <fer.escolar@gmail.com>
 * 
 */
using System;
using System.Web;
using System.Web.UI;
using System.Drawing;
using System.ComponentModel;
using System.Security.Permissions;

namespace Framework.Ajax.UI.Controls
{
    /// <summary>
    /// TextBox Input Modes
    /// </summary>
    public enum TextboxTypes
    {
        /// <summary>
        /// Plain Text
        /// </summary>
        TEXT,
        /// <summary>
        /// Shadows Plain Text
        /// </summary>
        PASSWORD,
        /// <summary>
        /// Unsigned Integer mode
        /// </summary>
        UNSIGNED_INTEGER,
        /// <summary>
        /// Signed integer mode
        /// </summary>
        SIGNED_INTEGER,
        /// <summary>
        /// unsigned currency mode (format: #,##0.00)
        /// </summary>
        UNSIGNED_MONEY,
        /// <summary>
        /// signed currency mode (format: (+|-)#,##0.00)
        /// </summary>
        SIGNED_MONEY
    }
    /// <summary>
    /// TextBox control
    /// <code>
    /// &lt;ajax:TextBox runat="server"&gt;&lt;/ajax:TextBox&gt;
    /// </code>
    /// </summary>
    [AspNetHostingPermission(SecurityAction.Demand, Level = AspNetHostingPermissionLevel.Minimal)]
    [AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    [Designer("Framework.Ajax.UI.Design.AjaxControlDesigner")]
    [DefaultEventAttribute("ServerChange")]
	[ToolboxData("<{0}:TextBox runat=\"server\"></{0}:TextBox>")]
    [ToolboxItem(true)]
    [ToolboxBitmap(typeof(System.Web.UI.WebControls.TextBox))]
    [DefaultProperty("Value")]
	public class TextBox : InputControl
	{
		#region Private Attributes
		private int maxLength;
        private TextboxTypes mTextboxType = TextboxTypes.TEXT;
		#endregion

		#region Public Properties
        /// <summary>
        /// Gets/Sets TextBox Input Mode
        /// </summary>
		[Category("Appearance"), DefaultValue(""), Description("Type of TextBox. Validating and formatting.")]
        public TextboxTypes TextboxType { get { return mTextboxType; } set { mTextboxType = value; if (value == TextboxTypes.PASSWORD) Attributes["type"] = "password"; else Attributes["type"] = "text"; AjaxUpdate(); } }
        /// <summary>
        /// Gets/Sets Maximun characters lentgh
        /// </summary>
		[Category("Appearance"), DefaultValue(""), Description("Max. Length in number of characters.")]
		public int MaxLength	{ get { return maxLength; }	set { maxLength = value; AjaxUpdate(); }}

        public override string Value
        {
            get
            {
                return base.Value;
            }
            set
            {
                base.Value = value;
            }
        }
		#endregion

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
        public event AjaxEventHandler ServerChange;
        /// <summary>
        /// Raises on Control gets focus
        /// <remarks>Is a server event</remarks>
        /// </summary>
        public event AjaxEventHandler ServerFocus;
        /// <summary>
        /// Raises on Control lost focus
        /// <remarks>Is a server event</remarks>
        /// </summary>
        public event AjaxEventHandler ServerBlur;
        /// <summary>
        /// Raises on Control is selected
        /// <remarks>Is a server event</remarks>
        /// </summary>
        public event AjaxEventHandler ServerSelect;
        #endregion

        /// <summary>
        /// Creates a new TextBox control
        /// </summary>
        public TextBox() : base("text") { maxLength = 0; }


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
        protected override void RenderAttributes(AjaxTextWriter writer)
        {
            RenderClientEventAttribute(writer, "onchange", "this.value");
            RenderClientEventAttribute(writer, "onfocus");
            RenderClientEventAttribute(writer, "onblur", "this.value");
            RenderClientEventAttribute(writer, "onselect");

            base.RenderAttributes(writer);
        }
        protected void renderInitJavaScript(AjaxTextWriter writer)
        {
            switch (TextboxType)
            {
                case TextboxTypes.TEXT:
                    if (maxLength > 0)
                        writer.Write("__textbox.LoadMaxLength('" + ID + "', " + maxLength + ");");
                    break;
                case TextboxTypes.SIGNED_INTEGER:
                    if (maxLength > 0)
                        writer.Write("__textbox.LoadSignedFormatAndMaxLength('" + ID + "', 0, " + maxLength + ");");
                    else
                        writer.Write("__textbox.LoadSignedFormat('" + ID + "', 0);");
                    break;
                case TextboxTypes.SIGNED_MONEY:
                    if (maxLength > 0)
                        writer.Write("__textbox.LoadSignedFormatAndMaxLength('" + ID + "', 2, " + maxLength + ");");
                    else
                        writer.Write("__textbox.LoadSignedFormat('" + ID + "', 2);");
                    break;
                case TextboxTypes.UNSIGNED_INTEGER:
                    if (maxLength > 0)
                        writer.Write("__textbox.LoadUnsignedFormatAndMaxLength('" + ID + "', 0, " + maxLength + ");");
                    else
                        writer.Write("__textbox.LoadUnsignedFormat('" + ID + "', 0);");
                    break;
                case TextboxTypes.UNSIGNED_MONEY:
                    if (maxLength > 0)
                        writer.Write("__textbox.LoadUnsignedFormatAndMaxLength('" + ID + "', 2, " + maxLength + ");");
                    else
                        writer.Write("__textbox.LoadUnsignedFormat('" + ID + "', 2);");
                    break;
            }
        }
		public override void RenderJS(AjaxTextWriter writer)
		{
            base.RenderJS(writer);
			if (!this.AjaxController.IsPostBack)
			{
                renderInitJavaScript(writer);
			}
		}
		#endregion
		
		public override void RaiseEvent(string action, string value)
		{
            PutPostValue(value);
			switch(action.ToLower())
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

		protected override void AjaxLoadViewState(object savedState)
		{
			object[] state = (object[])(savedState);
			base.AjaxLoadViewState(state[0]);
			maxLength = (int)state[1];
			mTextboxType = (TextboxTypes)state[2];
		}
		protected override object AjaxSaveViewState()
		{
			object[] state = new object[3];
			state[0] = base.AjaxSaveViewState();
			state[1] = maxLength;
            state[2] = mTextboxType;
			return state;
		}
		public override void PutPostValue(string obj)
		{
			this.Value = obj;
		}
	}
}
