using System;
using System.Web;
using System.Web.UI;
using System.Drawing;
using System.ComponentModel;
using System.Security.Permissions;

namespace Framework.Ajax.UI.Controls
{
    public static class FormExtension
    {
        public static Form GetForm(this System.Web.UI.Page page)
        { 
            if (page.Form is Form)
                return (page.Form as Form);
            else
                return null;
        }
    }

    /// <summary>
    /// Try to isolate nxAjax system in only one "<ajax:Form ..." control.
    /// Don't use this WIP
    /// </summary>
    [Designer("Framework.Ajax.UI.Design.AjaxControlDesigner")]
    [AspNetHostingPermission(SecurityAction.Demand, Level = AspNetHostingPermissionLevel.Minimal),
    AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    [DefaultEventAttribute("ServerClick")]
    [ToolboxData("<{0}:Form runat=\"server\"></{0}:Form>")]
    [ToolboxItem(true)]
    [ToolboxBitmap(typeof(System.Web.UI.WebControls.Button))]
    public class Form : System.Web.UI.HtmlControls.HtmlForm
    {
        protected bool itIsPostBack = false;
        protected AjaxControlCollection containedControls = new AjaxControlCollection();
        protected AjaxUserControlCollection containedUserControls = new AjaxUserControlCollection();
        protected string code = "";
        /// <summary>
        /// Overrides and shadows base Load Event...
        /// </summary>
        public new event EventHandler Load;
        /// <summary>
        /// Get contained nxControlCollection
        /// </summary>
        public AjaxControlCollection AjaxControls
        {
            get
            {
                return containedControls;
            }
        }
        /// <summary>
        /// Get contained AjaxUserControlCollection
        /// </summary>
        public AjaxUserControlCollection AjaxUserControls
        {
            get
            {
                return containedUserControls;
            }
        }
        /// <summary>
        /// Get the AjaxPage url
        /// </summary>
        public string PageUrl
        {
            get
            {
                string[] aux = System.Web.HttpContext.Current.Request.Url.ToString().Split('?');
                return aux[0];
            }
        }
        protected override void LoadControlState(object savedState)
        {
            base.LoadControlState(savedState);
        }
        protected override void AddedControl(System.Web.UI.Control control, int index)
        {
            try
            {
                AddControl(control);
                control.Page = this.Page;
                if (control is AjaxControl)
                    containedControls += (AjaxControl)control;
                if (control is AjaxUserControl)
                    containedUserControls += (AjaxUserControl)control;
            }
            catch { }
        }
        protected virtual void AddControl(System.Web.UI.Control control)
        {
            if (control.Controls.Count > 0 && !(control is IChildAjaxControlContainer))
                foreach (System.Web.UI.Control c in control.Controls)
                    AddedControl(c, 0);
        }
        protected virtual void getIfItIsPostback()
        {
            itIsPostBack = (System.Web.HttpContext.Current.Request.QueryString["__id"] != null || System.Web.HttpContext.Current.Request.Form.Count > 0);
        }
        protected override void OnLoad(EventArgs e)
        {
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
            
            foreach (AjaxUserControl ctrl in containedUserControls)
                ctrl.OnLoad(e);
        }
        protected override void Render(System.Web.UI.HtmlTextWriter writer)
        {
            if (!itIsPostBack)
            {
                writer.Write(getFormHtmlBegin());
                RenderChildren(writer);
                writer.Write(getFormHtmlEnd());
                writer.Write(getPostScript());
            }
            else
                RenderJSOnly(writer);
        }
        /// <summary>
        /// Only renders js script code
        /// </summary>
        /// <param name="writer">writer</param>
        protected virtual void RenderJSOnly(System.Web.UI.HtmlTextWriter writer)
        {
            System.Web.HttpContext.Current.Response.Clear();
            AjaxTextWriter jsWriter = new AjaxTextWriter();
            foreach (AjaxControl ctrl in containedControls)
            {
                ctrl.RenderJS(jsWriter);
            }
            writer.Write(jsWriter.ToString()/*.Replace("\n", "\\n").Replace("\r", "\\r")*/);
            if (code != "")
            {
                writer.Write(code);
                code = "";
            }
            writer.Flush();
            System.Web.HttpContext.Current.Response.End();
        }
        /// <summary>
        /// Replaces System.Web.UI.Page.IsPostBack property
        /// </summary>
        public bool IsPostBack
        {
            get
            {
                return itIsPostBack;
            }
        }
        /// <summary>
        /// Gets the post javascript code
        /// </summary>
        /// <returns>javascript code</returns>
        protected virtual string getPostScript()
        {
            string postScript = string.Empty;
            foreach (AjaxControl ctrl in containedControls)
            {
                AjaxTextWriter w = new AjaxTextWriter();
                ctrl.RenderJS(w);
                postScript += w.ToString();
            }
            if (code != "")
                postScript += code;
            return postScript;
        }
        /// <summary>
        /// Returns nxAjax form tag begin
        /// </summary>
        /// <returns></returns>
        internal string getFormHtmlBegin()
        {
            using (AjaxTextWriter writer = new AjaxTextWriter())
            {
                writer.WriteBeginTag("form");
                //Not used in XHMTL
                //writer.WriteAttribute("name", "frm_" + this.GetType().Name);
                writer.WriteAttribute("id", "frm_" + this.GetType().Name);
                writer.WriteAttribute("onsubmit", "return false;");
                writer.WriteAttribute("method", "post");
                writer.WriteAttribute("action", this.PageUrl);
                writer.Write(AjaxTextWriter.TagRightChar);

                return writer.ToString();
            }
        }
        /// <summary>
        /// Returns form end tag
        /// </summary>
        /// <returns></returns>
        internal string getFormHtmlEnd()
        {
            using (AjaxTextWriter writer = new AjaxTextWriter())
            {
                writer.WriteEndTag("form");
                return writer.ToString();
            }
        }
        #region Special javascript Methods
        /// <summary>
        /// Execute an specified javascript code in the next callback response
        /// </summary>
        /// <param name="lines">Javascript code</param>
        public void DocumentExecuteJavascript(string lines)
        {
            code += lines.Replace("\n", "\\n").Replace("\r", "\\r");
        }
        /// <summary>
        /// Shows an alert dialog in the next callback response
        /// </summary>
        /// <param name="msg"></param>
        public void DocumentAlert(string msg)
        {
            DocumentExecuteJavascript("alert(\"" + msg.Replace("\n", "\\n").Replace("\r", "\\r").Replace("\"", "\\\"") + "\"); ");
        }
        /// <summary>
        /// Redirects to an specified url in the next callback
        /// </summary>
        /// <param name="url">url to redirect</param>
        public void DocumentRedirect(string url)
        {
            DocumentExecuteJavascript("window.location.href = \"" + url + "\"; ");
        }
        #endregion
        #region Event Javascript Code Generator Methods
        /// <summary>
        /// Returns an nxAjax PostBack code for a control event
        /// </summary>
        /// <param name="ctrl">AjaxControl container</param>
        /// <param name="_event">name of the event</param>
        /// <returns>javascript code with the postback call</returns>
        public virtual string GetPostBackAjaxEvent(AjaxControl ctrl, string _event)
        {
            if (ctrl.PostBackMode == PostBackMode.Sync)
                return "$.nxApplication.DoPostBack('" + ctrl.ID + "', '" + _event + "', '" + ctrl.AjaxPage.PageUrl + "', false, null);";
            else if (ctrl.PostBackMode == PostBackMode.Async)
                return "$.nxApplication.DoPostBack('" + ctrl.ID + "', '" + _event + "', '" + ctrl.AjaxPage.PageUrl + "', true, '" + ctrl.ID + "_loading');";

            return "";

        }
        /// <summary>
        /// Returns an nxAjax PostBack code for a control event with an specified value
        /// </summary>
        /// <param name="ctrl">AjaxControl container</param>
        /// <param name="_event">name of the event</param>
        /// <param name="value">value to set</param>
        /// <returns>javascript code with the postback call</returns>
        public virtual string GetPostBackWithValueAjaxEvent(AjaxControl ctrl, string _event, string value)
        {
            if (ctrl.PostBackMode == PostBackMode.Sync)
                return "$.nxApplication.DoPostBackWithValue('" + ctrl.ID + "', '" + _event + "', '" + ctrl.AjaxPage.PageUrl + "', " + value + ");";
            else if (ctrl.PostBackMode == PostBackMode.Async)
                return "$.nxApplication.DoAsyncPostBackWithValue('" + ctrl.ID + "', '" + _event + "', '" + ctrl.AjaxPage.PageUrl + "', " + value + ", '" + ((ctrl.LoadingImgID != string.Empty) ? ctrl.LoadingImgID : ctrl.ID + "_loading") + "');";

            return "";
        }
        #endregion
    }
}
