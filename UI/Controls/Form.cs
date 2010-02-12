using System;
using System.Web;
using System.Web.UI;
using System.Drawing;
using System.ComponentModel;
using System.Security.Permissions;

namespace Framework.Ajax.UI.Controls
{
    /// <summary>
    /// Try to isolate nxAjax system in only one "<ajax:Form ..." control.
    /// Don't use this WIP
    /// </summary>
    [Designer("Framework.Ajax.UI.Design.AjaxControlDesigner")]
    [AspNetHostingPermission(SecurityAction.Demand, Level = AspNetHostingPermissionLevel.Minimal),
    AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    [ToolboxData("<{0}:Form runat=\"server\"></{0}:Form>")]
    [ToolboxItem(true)]
    [ToolboxBitmap(typeof(System.Web.UI.WebControls.Button))]
    public class Form : AjaxControl, IAjaxControllerContainer
    {
        private AjaxGenericController ajaxController;

        /// <summary>
        /// Overrides and shadows base Load Event...
        /// </summary>
        public new event EventHandler Load;
        
        /// <summary>
        /// Gets/Sets ViewState Storage Mode
        /// </summary>
        public ViewStateMode ViewStateMode
        {
            get { return ajaxController.ViewStateMode; }
            set { ajaxController.ViewStateMode = value; }
        }
        public override IAjaxController AjaxController
        {
            get { return ajaxController; }
        }

        protected override void AddedControl(System.Web.UI.Control control, int index)
        {
            ajaxController.AddControl(control);
        }
        protected override ControlCollection CreateControlCollection()
        {
            ajaxController = new AjaxGenericController();
            return base.CreateControlCollection();
        }
        protected override void OnLoad(EventArgs e)
        {
            ajaxController.Page = this.Page;
            ajaxController.Init();
            
            
            //if (IsPostBack && System.Web.HttpContext.Current.Session["__" + this.GetType().Name + "__ViewState" + "__" + System.Web.HttpContext.Current.Session.SessionID] == null)
            //{
            //    System.Web.HttpContext.Current.Response.Clear();
            //    System.Web.HttpContext.Current.Response.Write("alert(\"Session expired!\");");
            //    System.Web.HttpContext.Current.Response.Write("window.location.reload();");
            //    System.Web.HttpContext.Current.Response.End();
            //}
            base.OnLoad(e);
            if (this.Load != null)
            {
                this.Load(this, e);
                this.Load = null;
            }
            ajaxController.Load();
            ajaxController.AjaxRequestProcess();
        }

        protected override void Render(System.Web.UI.HtmlTextWriter writer)
        {
            if (!ajaxController.IsPostBack)
            {
                AjaxTextWriter ajaxWriter = new AjaxTextWriter(writer);
                ajaxController.WriteScriptHeader(ajaxWriter);
                ajaxController.WriteAjaxFormBegin(this.ID, ajaxWriter);
                RenderChildren(writer);
                ajaxController.WriteAjaxFormEnd(ajaxWriter);
                WritePostScript(ajaxWriter);
            }
            else
                ajaxController.AjaxRender();
        }
        
        /// <summary>
        /// Gets the post javascript code
        /// </summary>
        /// <returns>javascript code</returns>
        protected virtual void WritePostScript(AjaxTextWriter writer)
        {
             writer.Write("<script type=\"text/javascript\"> $(document).ready(function(){ __start(); " + ajaxController.GetPostbackJavascript() + "}); </script>");
        }

        public override void PutPostValue(string obj)
        {
            //there is no postback value            
        }      
    }
}
