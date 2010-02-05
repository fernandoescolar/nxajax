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
	/// Label control
    /// <code>
    /// &lt;ajax:Label runat="server"&gt;&lt;/ajax:Label&gt;
    /// </code>
	/// </summary>
    [AspNetHostingPermission(SecurityAction.Demand, Level = AspNetHostingPermissionLevel.Minimal)]
    [AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    [Designer("Framework.Ajax.UI.Design.AjaxControlDesigner")]
	[ToolboxData("<{0}:Label runat=\"server\"></{0}:Label>")]
    [ToolboxItem(true)]
    [ToolboxBitmap(typeof(System.Web.UI.WebControls.Label))]
    [DefaultProperty("Text")]
	public class Label : AjaxContainerControl
	{
        /// <summary>
        /// Gets/Sets Label text
        /// </summary>
		[Category("Appearance"), DefaultValue(""), Description("The text contained.")]
		public string Text
		{
			get { return InnerText; }
            set { InnerText = value; AjaxUpdate(); }
		}
        /// <summary>
        /// Gets/Sets Parent Control
        /// </summary>
        [Category("Appearance"), DefaultValue(""), Description("The text contained.")]
        public Control ForControl
        {
            get { return Page.FindControl(this.For); }
            set { this.For = value.ID; }
        }
        /// <summary>
        /// Gets/Sets Parent control ID
        /// </summary>
        [DefaultValue("")]
        [Description("")]
        [Category("Appearance")]
        public string For
        {
            get
            {
                string s = Attributes["for"];
                return (s == null) ? String.Empty : s;
            }
            set
            {
                if (value == null)
                    Attributes.Remove("for");
                else
                    Attributes["for"] = value;
                AjaxUpdate();
            }
        }
		
        /// <summary>
        /// Creates a new Label control
        /// </summary>
		public Label(): base("div")	{}

        protected override void Render(HtmlTextWriter writer)
        {
            base.Render(writer);
        }

		public override void PutPostValue(string obj)
		{
            this.Text = obj;
		}
	}
}
