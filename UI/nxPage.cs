/*
 * Licensing
 *  Mozilla Public License 1.1 (MPL 1.1)
 * 
 * Fernando Escolar Martínez-Berganza <fer.escolar@gmail.com>
 * 
 */

using System;
using System.ComponentModel;
using nxAjax.UI.Controls;

namespace nxAjax.UI
{
	/// <summary>
	/// nxAjax Web Page Base
	/// </summary>
	public class nxPage : System.Web.UI.Page
	{
		protected bool itIsPostBack = false;
		protected Templates template;
		internal protected Language lang;
		protected nxControlCollection containedControls = new nxControlCollection();
        protected nxUserControlCollection containedUserControls = new nxUserControlCollection();
		protected string path, sPage,code="";

        /// <summary>
        /// Get contained nxControlCollection
        /// </summary>
		public nxControlCollection nxControls
		{
			get
			{
				return containedControls;
			}
		}

        /// <summary>
        /// Get contained nxUserControlCollection
        /// </summary>
        public nxUserControlCollection nxUserControls
        {
            get
            {
                return containedUserControls;
            }
        }
		
		protected override void AddedControl(System.Web.UI.Control control, int index)
		{
			try
			{
                AddControl(control);
                control.Page = this;
                if (control is nxControl)
				    containedControls += (nxControl)control;
                if (control is nxUserControl)
                    containedUserControls += (nxUserControl)control;
			}
			catch{}
		}
        protected virtual void AddControl(System.Web.UI.Control control)
        {
            if (control.Controls.Count > 0 && !(control is IChildnxControlContainer))
                foreach (System.Web.UI.Control c in control.Controls)
                    AddedControl(c, 0);
        }

        /// <summary>
        /// Get the nxPage url
        /// </summary>
		public string PageUrl{
			get { 
				string[] aux = System.Web.HttpContext.Current.Request.Url.ToString().Split('?');
				return aux[0];
			}
		}

        /// <summary>
        /// Get/Set Template file name
        /// </summary>
		[Category("Content"), DefaultValue(null)]
		public string TemplateFile
		{
			get { return path + sPage; }
			set { 
				string[] s = value.Split('/');
				sPage = s[s.Length-1];
				path = value.Replace(sPage, "");
			}
		}

        /// <summary>
        /// Creates a new nxPage
        /// </summary>
		public nxPage():base()
		{
			this.lang = null;
			this.template = null;
			this.EnableViewState = true;
			this.code = "";
            this.path = this.sPage = "";
		}

        /// <summary>
        /// Replaces System.Web.UI.Page.IsPostBack property
        /// </summary>
		public new bool IsPostBack
		{
            get
            {
                return itIsPostBack;
            }
		}
		protected override object LoadPageStateFromPersistenceMedium()
		{
			//return base.LoadPageStateFromPersistenceMedium ();
			return Session["__" + this.GetType().Name + "__ViewState" + "__" + Session.SessionID];
		}
		protected override void SavePageStateToPersistenceMedium(object viewState)
		{
			//base.SavePageStateToPersistenceMedium (viewState);
			Session["__" + this.GetType().Name + "__ViewState" + "__" + Session.SessionID] = viewState;
			ClientScript.RegisterHiddenField("__VIEWSTATE", "");
		}

		protected override void OnLoad(EventArgs e)
		{
            if (IsPostBack && Session["__" + this.GetType().Name + "__ViewState" + "__" + Session.SessionID] == null)
            {
                Response.Clear();
                Response.Write("alert(\"Session expired!\");");
                Response.Write("window.location.reload();");
                Response.End();
            }
			base.OnLoad (e);
            foreach (nxUserControl ctrl in containedUserControls)
                ctrl.OnLoad(e);
		}

		protected override void Render(System.Web.UI.HtmlTextWriter writer)
		{
			base.Render(writer);
		}
		protected override void OnPreRender(System.EventArgs e)	{ }
	
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

        /// <summary>
        /// Returns an nxAjax PostBack code for a control event
        /// </summary>
        /// <param name="ctrl">nxControl container</param>
        /// <param name="_event">name of the event</param>
        /// <returns>javascript code with the postback call</returns>
        public virtual string GetPostBackAjaxEvent(nxControl ctrl, string _event)
        {
            if (ctrl.PostBackMode == PostBackMode.Sync)
                return "$.nxApplication.DoPostBack('" + ctrl.ID + "', '" + _event + "', '" + ctrl.nxPage.PageUrl + "', false, null);";
            else if (ctrl.PostBackMode == PostBackMode.Async)
                return "$.nxApplication.DoPostBack('" + ctrl.ID + "', '" + _event + "', '" + ctrl.nxPage.PageUrl + "', true, '" + ctrl.ID + "_loading');";
            
            return "";
            
        }
        /// <summary>
        /// Returns an nxAjax PostBack code for a control event with an specified value
        /// </summary>
        /// <param name="ctrl">nxControl container</param>
        /// <param name="_event">name of the event</param>
        /// <param name="value">value to set</param>
        /// <returns>javascript code with the postback call</returns>
        public virtual string GetPostBackWithValueAjaxEvent(nxControl ctrl, string _event, string value)
        {
            if (ctrl.PostBackMode == PostBackMode.Sync)
                return "$.nxApplication.DoPostBackWithValue('" + ctrl.ID + "', '" + _event + "', '" + ctrl.nxPage.PageUrl + "', " + value + ");";
            else if (ctrl.PostBackMode == PostBackMode.Async)
                return "$.nxApplication.DoAsyncPostBackWithValue('" + ctrl.ID + "', '" + _event + "', '" + ctrl.nxPage.PageUrl + "', " + value + ", '" + ((ctrl.LoadingImgID != string.Empty) ? ctrl.LoadingImgID : ctrl.ID + "_loading") + "');";

            return "";
        }
	}
}
