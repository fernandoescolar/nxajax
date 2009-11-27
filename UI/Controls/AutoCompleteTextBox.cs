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

/// <summary>
/// Custom ajax framework controls Namespace
/// </summary>
namespace nxAjax.UI.Controls
{
    /// <summary>
    /// [Not implemented] AutoComplete TextBox Control
    /// <code>
    /// &lt;ajax:AutoCompleteTextBox runat="server"&gt;&lt;/ajax:AutoCompleteTextBox&gt;
    /// </code>
    /// </summary>
    [AspNetHostingPermission(SecurityAction.Demand, Level = AspNetHostingPermissionLevel.Minimal)]
    [AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    [Designer("nxAjax.UI.Design.nxControlDesigner")]
    [ToolboxData("<{0}:AutoCompleteTextBox runat=\"server\"></{0}:AutoCompleteTextBox>")]
    [ToolboxItem(true)]
    [ToolboxBitmap(typeof(System.Web.UI.WebControls.TextBox))]
    public class AutoCompleteTextBox : TextBox
    {
        public AutoCompleteTextBox() : base() { }
    }
}
