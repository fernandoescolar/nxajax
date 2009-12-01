/*
 * Licensing
 *  Mozilla Public License 1.1 (MPL 1.1)
 * 
 * Fernando Escolar Martínez-Berganza <fer.escolar@gmail.com>
 * 
 */

using System;
using System.Xml;
using System.ComponentModel;
using nxAjax.UI.Controls;

namespace nxAjax.UI
{
	/// <summary>
	/// nxContainedPage is a WebPage type that it will loaded into a Container Control object.
	/// It can contains web forms and these can be posted by using an ISubmit nxControl.
    /// <code>
    /// In the template page it requieres the tag "&lt;$POSTSCRIPT$&gt;".
    /// </code>
	/// </summary>
	public class nxContainedPage : nxPage
	{
		private string parent = "";
		private bool reloadedViewState = false;
		private bool keepViewState = false;

        /// <summary>
        /// Init event
        /// </summary>
		public new event System.EventHandler Init;

        /// <summary>
        /// Gets/Sets if it can keep the view state along session life.
        /// </summary>
		[Category("Content"), DefaultValue(false), Description("Keep ViewState during the Session.")]
		public bool KeepViewState
		{
			get { return keepViewState; }
			set { keepViewState = value; }
		}

		protected override void OnLoad(EventArgs e)
		{
            
			foreach(nxControl ctrl in containedControls)
			{
				ctrl.AjaxNotUpdate();
				ctrl.ID = this.GetType().Name + "_" + ctrl.ID;
			}
            foreach (nxUserControl ctrl in containedUserControls)
            {
                ctrl.ID = this.GetType().Name + "_" + ctrl.ID;
            }

            getLoadedLanguage();
            getIfItIsPostback();

            bool getOldViewSate = false;
            if (!itIsPostBack && keepViewState && Session["__" + this.GetType().Name + "__ViewState" + "__" + Session.SessionID] != null)
            {
                itIsPostBack = true;
                getOldViewSate = true;
            }
			base.OnLoad (e);          
            if (getOldViewSate)
                itIsPostBack = false;

            if (Request.QueryString["__parent"] != null)
                parent = Request.QueryString["__parent"];

            processPostback();
		}

        protected override void getIfItIsPostback()
        {
            base.getIfItIsPostback();

            if (!itIsPostBack && reloadedViewState)
                itIsPostBack = true;
        }
		protected override void LoadViewState(object savedState)
		{
			object[] state = (object[])(savedState);
			base.LoadViewState(state[0]);
			parent = (string)state[1];
		}
		protected override object SaveViewState()
		{
			object[] state = new object[3];
			state[0] = base.SaveViewState();
			state[1] = parent;
			return state;
		}

		protected override void Render(System.Web.UI.HtmlTextWriter writer)
		{
			if (!itIsPostBack || reloadedViewState)
			{
				if (reloadedViewState)
					itIsPostBack = false;
                if (template.IsLoaded)
                {
                    RenderTemplated(writer);
                    reloadedViewState = false;
                    if (!template["pageTemplate"].ContainsValueKey("POSTSCRIPT"))
                    {
                        writer.Write(getPostScript());
                        code = "";
                    }
                }
                else
                    RenderOverAsp(writer);
			}
			else
                RenderJSOnly(writer);
		}
        protected override void RenderOverAsp(System.Web.UI.HtmlTextWriter writer)
        {
            string tempRender = baseRenderResult();

            tempRender = tempRender.Replace("<div>\r\n<input type=\"hidden\" name=\"__VIEWSTATE\" id=\"__VIEWSTATE\" value=\"\" />\r\n<input type=\"hidden\" name=\"__VIEWSTATE\" id=\"\r\n__VIEWSTATE\" value=\"\" />\r\n</div>\r\n", "");

            int formPos1 = tempRender.IndexOf("<form ");
            int formPos2 = tempRender.IndexOf(">", formPos1);
            if (formPos1 >= 0 && formPos2 >= 0)
            {
                tempRender = tempRender.Remove(formPos1, formPos2 - formPos1 + 1);
                tempRender = tempRender.Insert(formPos1, "<form name=\"frm_" + this.GetType().Name + "\" id=\"frm_" + this.GetType().Name + "\" onSubmit=\"return false;\" method=\"POST\" action=\"" + this.PageUrl + "\">");
            }
            else
            {
                Response.Clear();
                Response.Write("No se pudo cargar: ");
                Response.Write("Falta la etiqueta '<form ... runat=\"server\" ...>' que se ejecute en el servidor....");
                Response.End();
            }
            formPos1 = tempRender.IndexOf("</form>");
            if (formPos1 >= 0)
            {
                tempRender = tempRender.Insert(formPos1 + 7, getPostScript());
                code = "";
            }
            else
            {
                Response.Clear();
                Response.Write("No se pudo cargar: ");
                Response.Write("Falta el cierre de la etiqueta '<form ... runat=\"server\" ...>': </form>");
                Response.End();
            }
            writer.Write(tempRender);
        }
        protected override string getPostScript()
        {
            return "<INPUT type=\"hidden\" id=\"" + parent + "_CONTAINED_postscript\" name=\"CONTAINED_postscript\" value=\"" + base.getPostScript().Replace("\"", "&#34;") + "\" />";
        }

		protected override System.Collections.Specialized.NameValueCollection DeterminePostBackMode()
		{
			System.Collections.Specialized.NameValueCollection aux = base.DeterminePostBackMode();
			if (Init!=null)
				Init(this, new System.EventArgs());
            if (aux == null && LoadPageStateFromPersistenceMedium() != null && keepViewState)
			{
                reloadedViewState = true;
				aux = new System.Collections.Specialized.NameValueCollection();
				aux.Add("__VIEWSTATE","");
			}
			return aux;
		}

        /// <summary>
        /// Loads other nxContainedPage into the parent Container in the next callback response
        /// </summary>
        /// <param name="url">Url of the new nxContainedPage</param>
        /// <param name="param">QueryString params</param>
		public void ParentLoadPage(string url, string param)
		{
			lock(code)
			{
                code += "$.nxApplication.LoadPane('" + parent + "', '" + url + "', '" + param + "');";
			}
		}
        /// <summary>
        /// Loads other nxContainedPage into the parent Container in the next callback response
        /// </summary>
        /// <param name="url">Url of the new nxContainedPage</param>
        public void ParentLoadPage(string url)
        {
            ParentLoadPage(url, string.Empty);
        }

        /// <summary>
        /// nxContainedPage Constructor
        /// </summary>
		public nxContainedPage() : base()
		{
			this.lang = null;
			this.template = null;
			this.EnableViewState = true;
		}
	}
}
