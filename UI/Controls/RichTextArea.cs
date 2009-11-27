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
    /// WYSIWYG Html code editor
    /// <code>
    /// &lt;ajax:RichTextArea runat="server"&gt;&lt;/ajax:RichTextArea&gt;
    /// </code>
	/// </summary>
    [AspNetHostingPermission(SecurityAction.Demand, Level = AspNetHostingPermissionLevel.Minimal)]
    [AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    [Designer("nxAjax.UI.Design.nxControlDesigner")]
    [ToolboxData("<{0}:RichTextArea runat=\"server\"></{0}:RichTextArea>")]
    [ToolboxItem(true)]
    [ToolboxBitmap(typeof(nxControl), "images.RichTextArea.bmp")]
	public class RichTextArea : TextArea
	{
        /// <summary>
        /// Creates a new RichTextArea control
        /// </summary>
        public RichTextArea() : base() { }

		public override void RenderJS(nxAjaxTextWriter writer)
		{
            base.RenderJS(writer);

            if (!this.nxPage.IsPostBack)
			{
                writer.Write("$('#" + ID + "').wysiwyg();");
			}
		}
	}
}
