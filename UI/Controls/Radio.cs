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
	/// Radio control
    /// <code>
    /// &lt;ajax:Radio runat="server"&gt;&lt;/ajax:Radio&gt;
    /// </code>
	/// </summary>
    [AspNetHostingPermission(SecurityAction.Demand, Level = AspNetHostingPermissionLevel.Minimal)]
    [AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    [Designer("Framework.Ajax.UI.Design.AjaxControlDesigner")]
    [DefaultEventAttribute("ServerClick")]
    [ToolboxData("<{0}:Radio runat=\"server\"></{0}:Radio>")]
    [ToolboxItem(true)]
    [ToolboxBitmap(typeof(System.Web.UI.WebControls.RadioButton))]
	public class Radio : CheckBox
	{
        /// <summary>
        /// Group Radios Key
        /// </summary>
        [DefaultValue("")]
        [Description("")]
        [Category("Appearance")]
        public virtual string Group
        {
            get
            {
                string s = Attributes["name"];
                return (s == null) ? String.Empty : s;
            }
            set
            {
                if (value == null)
                    Attributes.Remove("name");
                else
                    Attributes["name"] = value;
            }
        }
	    /// <summary>
	    /// Creates a new Radio control
	    /// </summary>
		public Radio() : base("radio") { }
	}
}
