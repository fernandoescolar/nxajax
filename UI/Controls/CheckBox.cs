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
    /// nxAjax framework checkBox control
    /// <code>
    /// &lt;ajax:CheckBox runat="server"&gt;&lt;/ajax:CheckBox&gt;
    /// </code>
	/// </summary>
    [AspNetHostingPermission(SecurityAction.Demand, Level = AspNetHostingPermissionLevel.Minimal)]
    [AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    [Designer("Framework.Ajax.UI.Design.AjaxControlDesigner")]
    [DefaultEventAttribute("ServerClick")]
    [ToolboxData("<{0}:CheckBox runat=\"server\"></{0}:CheckBox>")]
    [ToolboxItem(true)]
    [ToolboxBitmap(typeof(System.Web.UI.WebControls.CheckBox))]
    [DefaultProperty("Checked")]
	public class CheckBox : Button
	{
        /// <summary>
        /// Gets/Sets if the control is checked
        /// </summary>
        [Category("Appearance"), DefaultValue(""), Description("Enable or Disable the Control.")]
        public bool Checked
        {
            get
            {
                string checkedAttr = Attributes["checked"] as string;
                return (checkedAttr != null);
            }
            set
            {
                if (!value)
                    Attributes.Remove("checked");
                else
                    Attributes["checked"] = "checked";
                AjaxUpdate();
            }
        }
        
        public CheckBox() : base("checkbox") { }
        internal CheckBox(string typeName) : base(typeName) { }

        protected override void Render(HtmlTextWriter writer)
        {
            this.Value = ID;
            base.Render(writer);
        }
        public override void RenderJS(AjaxTextWriter writer)
        {
            this.Value = ID;
            base.RenderJS(writer);
            if (hasChanged)
                writer.Write("$('#" + ID + "').attr( 'checked', " + Checked.ToString().ToLower() + ");");
        }

		public override void PutPostValue(string obj)
		{
            //if (obj == "on")
            //    this.Checked = true;
            //else 
            if (obj == "checked" || obj == "true")
                this.Checked = true;
            //else if (obj == "true")
            //    this.Checked = true;
            //else if (obj == "")
            //{ /*ignore*/ }
            else
                this.Checked = false;
		}
	}
}
