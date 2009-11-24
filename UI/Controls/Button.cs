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

namespace nxAjax.UI.Controls
{
	/// <summary>
	/// nxAjax framework button control
    /// <code>
    /// &lt;ajax:Button runat="server"&gt;&lt;/ajax:Button&gt;
    /// </code>
	/// </summary>
    [Designer("nxAjax.UI.Design.nxControlDesigner")]
    [AspNetHostingPermission(SecurityAction.Demand,
    Level = AspNetHostingPermissionLevel.Minimal),
    AspNetHostingPermission(SecurityAction.InheritanceDemand,
    Level = AspNetHostingPermissionLevel.Minimal)]
    [DefaultEventAttribute("ServerClick")]
	[ToolboxData("<{0}:Button runat=\"server\"></{0}:Button>")]
    [ToolboxItem(true)]
    [ToolboxBitmap(typeof(System.Web.UI.WebControls.Button))]
	public class Button : InputControl
	{
		#region Public Javascript Client Events
        /// <summary>
        /// Gets/Sets Javascript onclick event code
        /// <remarks>Is a client event</remarks>
        /// </summary>
        [Category("Action"), DefaultValue(""), Description("OnClick javascript event in client.")]
        public string ClientClick 
        { 
            get 
            {
                string s = Attributes["onclick"];
                return (s == null) ? string.Empty : s;
            } 
            set 
            {
                if (value == null)
                    Attributes.Remove("onclick");
                else
                    Attributes["onclick"] = value;
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
		#endregion

		#region Public Server Events
        /// <summary>
        /// Raises on Control is clicked
        /// <remarks>Is a server event</remarks>
        /// </summary>
        public event nxEventHandler ServerClick;
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
		#endregion
		
		public Button() : base("button") { }
        internal Button(string typeName) : base(typeName) { }

		#region Renders
        protected override bool hasEvent(string eventName)
        {
            switch(eventName.ToLower())
            {
                case "onclick":
                    return (ServerClick != null);
                case "onfocus":
                    return (ServerFocus != null);
                case "onblur":
                    return (ServerBlur != null);
            }
            return false;
        }
        protected override void RenderAttributes(nxAjaxTextWriter writer)
        {
            if (!DesignMode)
            {
                RenderClientEventAttribute(writer, "onclick");
                RenderClientEventAttribute(writer, "onfocus");
                RenderClientEventAttribute(writer, "onblur");
            }
		
            base.RenderAttributes(writer);
        }
		#endregion

		
		public override void RaiseEvent(string action, string value)
		{
            PutPostValue(value);
			switch(action.ToLower())
			{
				case "onclick":
                    ServerClick(this, value);
					break;
				case "onfocus":
                    ServerFocus(this, value);
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
