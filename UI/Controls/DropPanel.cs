/*
 * Licensing
 *  Mozilla Public License 1.1 (MPL 1.1)
 * 
 * Fernando Escolar Martínez-Berganza <fer.escolar@gmail.com>
 * 
 */
using System.ComponentModel;
using System.Drawing;
using System.Web.UI;
using System.Web.UI.Design;
using System.Web.UI.WebControls;

namespace Framework.Ajax.UI.Controls
{
    /// <summary>
    /// nxAjax framework Drop Panel control
    /// <code>
    /// &lt;ajax:DropPanel runat="server"&gt;&lt;/ajax:DropPanel&gt;
    /// </code>
    /// </summary>
    [Designer("Framework.Ajax.UI.Design.AjaxControlDesigner")]
    [ParseChildren(false)]
    [PersistChildren(true)]
    [Description("A Drop Panel control")]
    [ToolboxData("<{0}:DropPanel runat=\"server\"></{0}:DropPanel>")]
    [ToolboxItem(true)]
    public class DropPanel : Panel
    {
        public override void RenderHTML(AjaxTextWriter writer)
        {
            if (string.IsNullOrEmpty(Attributes["class"]))
                Attributes["class"] = "ajax-dragbox-column";
            else if (Attributes["class"].IndexOf("ajax-dragbox-column") < 0)
                Attributes["class"] += " ajax-dragbox-column";

            base.RenderHTML(writer);
        }
    }
}
