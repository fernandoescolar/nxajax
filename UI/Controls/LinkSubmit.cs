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
    [ToolboxData("<{0}:LinkSubmit runat=\"server\"></{0}:LinkSubmit>")]
    [ToolboxBitmap(typeof(System.Web.UI.HtmlControls.HtmlInputSubmit))]
    [ToolboxItem(true)]
    [DefaultProperty("Text")]
    public class LinkSubmit : LinkButton, ISubmit
    {
        public LinkSubmit() : base()
        {
            Attributes.Add("type", "submit");
        }
    }
}
