/*
 * Licensing
 *  Mozilla Public License 1.1 (MPL 1.1)
 * 
 * Fernando Escolar Martínez-Berganza <fer.escolar@gmail.com>
 * 
 */
using System;
using System.ComponentModel;
using System.Drawing;
using System.Web;
using System.Web.UI;
using System.Web.UI.Design;
using System.Web.UI.WebControls;
using System.Security.Permissions;

namespace Framework.Ajax.UI.Controls
{
    /// <summary>
    /// Panel control
    /// <code>
    /// &lt;ajax:Panel runat="server"&gt;&lt;/ajax:Panel&gt;
    /// </code>
    /// </summary>
    [AspNetHostingPermission(SecurityAction.Demand, Level = AspNetHostingPermissionLevel.Minimal)]
    [AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    [Designer("Framework.Ajax.UI.Design.AjaxControlDesigner")]
    [ParseChildren(false)]
    [PersistChildren(true)]
    [Description("A panel control")]
    [ToolboxData("<{0}:Panel runat=\"server\"></{0}:Panel>")]
    [ToolboxItem(true)]
    [ToolboxBitmap(typeof(System.Web.UI.WebControls.Panel))]
    public class Panel : AjaxControl
    {
        #region Private Attributes
        internal AjaxControlCollection containedControls = new AjaxControlCollection();
		#endregion

		#region Public Properties
        /// <summary>
        /// Gets inner AjaxControls Collection
        /// </summary>
        public new AjaxControlCollection Controls
        {
            get
            {
                return containedControls;
            }
        }
		#endregion

        /// <summary>
        /// Creates a new Panel control
        /// </summary>
        public Panel() : base("div") { }

		#region Renders
		public override void RenderHTML(AjaxTextWriter writer)
		{
            RenderBeginTag(writer);
            RenderChildren(new HtmlTextWriter(writer.TextWriter));
            writer.WriteEndTag(TagName);
		}
        public override void RenderJS(AjaxTextWriter writer)
		{
            base.RenderJS(writer);
            foreach (AjaxControl c in containedControls)
                c.RenderJS(writer);
		}
		#endregion

		public override void RaiseEvent(string action, string value)
		{
			
		}
		public override void PutPostValue(string obj)
		{
			
		}
        protected override void AddedControl(System.Web.UI.Control control, int index)
        {
            try
            {
                control.Page = this.Page;
                if (control is AjaxControl)
                    containedControls += (AjaxControl)control;
            }
            catch { }
        }
    }
}
