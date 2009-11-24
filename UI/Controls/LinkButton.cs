﻿/*
 * Licensing
 *  Mozilla Public License 1.1 (MPL 1.1)
 * 
 * Fernando Escolar Martínez-Berganza <fer.escolar@gmail.com>
 * 
 */
using System;
using System.Web.UI;
using System.Drawing;
using System.ComponentModel;

namespace nxAjax.UI.Controls
{
    /// <summary>
    /// LinkButton control
    /// <code>
    /// &lt;ajax:LinkButton runat="server"&gt;&lt;/ajax:LinkButton&gt;
    /// </code>
    /// </summary>
    [Designer("nxAjax.UI.Design.nxControlDesigner")]
    [DefaultEventAttribute("ServerClick")]
    [ToolboxData("<{0}:LinkButton runat=\"server\"></{0}:LinkButton>")]
    [ToolboxItem(true)]
    [ToolboxBitmap(typeof(System.Web.UI.WebControls.LinkButton))]
    public class LinkButton : nxContainerControl
	{
        /// <summary>
        /// Gets/Sets LinkButton text shown
        /// </summary>
		[Category("Appearance"), DefaultValue(""), Description("The text contained.")]
		public string Text
		{
			get { return InnerText; }
            set { InnerText = value; }
		}
        /// <summary>
        /// Gets/Sets value/text shown
        /// </summary>
        [Category("Appearance"), DefaultValue(""), Description("The value contained.")]
        public string Value
        {
            get { return Text; }
            set { Text = value; }
        }
		/// <summary>
		/// Creates a new LinkButton control
		/// </summary>
		public LinkButton(): base("a")	{}

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
                string s = Attributes["href"];
                return (s == null) ? string.Empty : s;
            }
            set
            {
                if (value == null)
                    Attributes.Remove("href");
                else
                    Attributes["href"] = value;
            }
        }
        /// <summary>
        /// Gets/Sets Javascript onMouseOver event code
        /// <remarks>Is a client event</remarks>
        /// </summary>
        [Category("Action"), DefaultValue(""), Description("OnFocus javascript event in client.")]
        public string ClientMouseOver
        {
            get
            {
                string s = Attributes["onmouseover"];
                return (s == null) ? string.Empty : s;
            }
            set
            {
                if (value == null)
                    Attributes.Remove("onmouseover");
                else
                    Attributes["onmouseover"] = value;
            }
        }
        /// <summary>
        /// Gets/Sets Javascript onMouseOut event code
        /// <remarks>Is a client event</remarks>
        /// </summary>
        [Category("Action"), DefaultValue(""), Description("OnBlur javascript event in client.")]
        public string ClientMouseOut
        {
            get
            {
                string s = Attributes["onmouseout"];
                return (s == null) ? string.Empty : s;
            }
            set
            {
                if (value == null)
                    Attributes.Remove("onmouseout");
                else
                    Attributes["onmouseout"] = value;
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
        /// Raises on mouse puts over the control
        /// <remarks>Is a server event</remarks>
        /// </summary>
        public event nxEventHandler ServerMouseOver;
        /// <summary>
        /// Raises on mouse exits over the control
        /// <remarks>Is a server event</remarks>
        /// </summary>
        public event nxEventHandler ServerMouseOut;
        #endregion

        #region Renders
        protected void RenderClientHrefAttribute(nxAjaxTextWriter writer)
        {
            string myEvent = String.Empty;
            if (Attributes["href"] != null)
            {
                myEvent = Attributes["href"] + myEvent;
                Attributes.Remove("href");
            }

            if (Page != null && hasEvent("href"))
            {
                myEvent += nxPage.GetPostBackAjaxEvent(this, "onclick");
            }

            if (myEvent.Length > 0)
            {
                myEvent = "javascript:" + myEvent;
                writer.WriteAttribute("href", myEvent, true);
            }
        }
        protected override bool hasEvent(string eventName)
        {
            switch (eventName.ToLower())
            {
                case "href":
                    return (ServerClick != null);
                case "onmouseover":
                    return (ServerMouseOver != null);
                case "onmouseout":
                    return (ServerMouseOut != null);
            }
            return false;
        }
        protected override void RenderAttributes(nxAjaxTextWriter writer)
        {
            RenderClientHrefAttribute(writer);
            RenderClientEventAttribute(writer, "onmouseover");
            RenderClientEventAttribute(writer, "onmouseout");

            base.RenderAttributes(writer);
        }
        #endregion

        
        public override void RaiseEvent(string action, string value)
        {
            //this.Text = value;
            switch (action.ToLower())
            {
                case "onclick":
                    ServerClick(this, value);
                    break;
                case "onmouseover":
                    ServerMouseOver(this, value);
                    break;
                case "onmouseout":
                    ServerMouseOut(this, value);
                    break;
            }
        }
        public override void PutPostValue(string obj)
        {
            //this.Text = obj;
        }
    }
}
