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
	/// Input Hidden Control
    /// <code>
    /// &lt;ajax:Hidden runat="server"&gt;&lt;/ajax:Hidden&gt;
    /// </code>
	/// </summary>
    [AspNetHostingPermission(SecurityAction.Demand, Level = AspNetHostingPermissionLevel.Minimal)]
    [AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    [Designer("nxAjax.UI.Design.nxControlDesigner")]
	[ToolboxData("<{0}:Hidden runat=\"server\"></{0}:Hidden>")]
    [ToolboxItem(true)]
    [ToolboxBitmap(typeof(Hidden), "images.Hidden.bmp")]
    [DefaultProperty("Value")]
	public class Hidden : InputControl
	{
        /// <summary>
        /// Creates a new Hidden control
        /// </summary>
        public Hidden() : base("hidden") { }

		public override void PutPostValue(string obj)
		{
			this.Value = obj;
		}
	}
}
