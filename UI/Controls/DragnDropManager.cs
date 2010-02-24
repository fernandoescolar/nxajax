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
using System.Collections.Generic;
using System.Security.Permissions;

namespace Framework.Ajax.UI.Controls
{
    /// <summary>
    /// nxAjax framework Drag 'n Drop Manager control. Where positions and displays of some DragnDropPanels will be saved.
    /// <code>
    /// &lt;ajax:DragnDropManager runat="server"&gt;&lt;/ajax:DragnDropManager&gt;
    /// </code>
    /// </summary>
    [AspNetHostingPermission(SecurityAction.Demand, Level = AspNetHostingPermissionLevel.Minimal)]
    [AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    [Designer("Framework.Ajax.UI.Design.AjaxControlDesigner")]
    [ToolboxData("<{0}:DragnDropManager runat=\"server\"></{0}:DragnDropManager>")]
    [ToolboxItem(true)]
    [ToolboxBitmap(typeof(System.Web.UI.WebControls.Panel))]
    public class DragnDropManager : Hidden
    {
        public DragnDropManager() : base() { }
        
        #region Renders
        public override void RenderJS(AjaxTextWriter writer)
        {
            base.RenderJS(writer);
            if (!AjaxController.IsPostBack)
                writer.Write("$.draganddrop('" + ID + "', '" + Value + "');");
            else if (hasChanged)
                writer.Write("$.draganddrop('" + ID + "', '" + Value + "');");
        }
        #endregion

        public override void RaiseEvent(string action, string value)
        {
            
        }
        public override void PutPostValue(string obj)
        {
            base.PutPostValue(obj);
            AjaxNotUpdate();
        }
    }
}
