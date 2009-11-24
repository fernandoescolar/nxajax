/*
 * Licensing
 *  Mozilla Public License 1.1 (MPL 1.1)
 * 
 * Fernando Escolar Martínez-Berganza <fer.escolar@gmail.com>
 * 
 */
using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Security.Permissions;
using System.Drawing.Design;
using System.Web;
using System.Web.UI;
using System.Web.UI.Design;

namespace nxAjax.UI.Controls
{
    /// <summary>
    /// nxAjax framework comboBox control item
    /// <code>
    /// &lt;ajax:ComboBoxItem runat="server"&gt;&lt;/ajax:ComboBoxItem&gt;
    /// </code>
    /// </summary>
    [AspNetHostingPermission(SecurityAction.Demand, Level = AspNetHostingPermissionLevel.Minimal)]
    [AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    [ParseChildren(true)]
    [ToolboxData("<{0}:ComboBoxItem runat=\"server\"></{0}:ComboBoxItem>")]
    [ToolboxItem(true)]
	public class ComboBoxItem
	{
		private string text, value;

        /// <summary>
        /// Item Text. Render Text
        /// </summary>
		public string Text { get { return text; } set { text = value; }}
        /// <summary>
        /// Item Value. Internal Value
        /// </summary>
		public string Value { get { return value; } set { this.value = value; }}

        /// <summary>
        /// Creates a new empty comboBox Item
        /// </summary>
		public ComboBoxItem() { text = value = ""; }
        /// <summary>
        /// Creates a new comboBox Item
        /// </summary>
        /// <param name="text">text value</param>
        /// <param name="value">internal value</param>
		public ComboBoxItem(string text, string value) { this.text = text; this.value = value; }
	}
}