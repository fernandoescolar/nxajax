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
using System.ComponentModel;

namespace nxAjax.UI.Controls
{
	/// <summary>
	/// Submit button control
    /// <code>
    /// &lt;ajax:Submit runat="server"&gt;&lt;/ajax:Submit&gt;
    /// </code>
	/// </summary>
    [Designer("nxAjax.UI.Design.nxControlDesigner")]
    [DefaultEventAttribute("ServerClick")]
	[ToolboxData("<{0}:Submit runat=\"server\"></{0}:Submit>")]
    [ToolboxBitmap(typeof(System.Web.UI.HtmlControls.HtmlInputSubmit))]
    [ToolboxItem(true)]
	public class Submit : Button
	{
        /// <summary>
        /// Creates a new Submit button control
        /// </summary>
        public Submit() : base("submit") { }
	}
}
