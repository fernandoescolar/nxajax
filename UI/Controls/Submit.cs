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
	/// Submit button control
    /// <code>
    /// &lt;ajax:Submit runat="server"&gt;&lt;/ajax:Submit&gt;
    /// </code>
	/// </summary>
    [AspNetHostingPermission(SecurityAction.Demand, Level = AspNetHostingPermissionLevel.Minimal)]
    [AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    [Designer("Framework.Ajax.UI.Design.AjaxControlDesigner")]
    [DefaultEventAttribute("ServerClick")]
	[ToolboxData("<{0}:Submit runat=\"server\"></{0}:Submit>")]
    [ToolboxBitmap(typeof(System.Web.UI.HtmlControls.HtmlInputSubmit))]
    [ToolboxItem(true)]
    [DefaultProperty("Value")]
    public class Submit : Button, ISubmit
	{
        /// <summary>
        /// Creates a new Submit button control
        /// </summary>
        public Submit() : base("submit") { }
	}
}
