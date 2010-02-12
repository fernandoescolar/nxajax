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
using Framework.Ajax.UI.Controls;

namespace Framework.Ajax.UI
{
	/// <summary>
	/// ContainedPage is a WebPage type that it will loaded into a Container Control object.
	/// It can contains web forms and these can be posted by using an ISubmit AjaxControl.
    /// <code>
    /// In the template page is optional the special tag "&lt;$POSTSCRIPT$&gt;".
    /// </code>
	/// </summary>
	public class ContainedPage : AjaxPage
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
            
			foreach(AjaxControl ctrl in ajaxController.AjaxControls)
				ctrl.AjaxNotUpdate();

            
			base.OnLoad (e);          
            

            if (Request.QueryString["__parent"] != null)
                parent = Request.QueryString["__parent"];

            processPostback();
		}

        protected void RenameControl(System.Web.UI.Control control)
        {
            if (control is AjaxControl || control is AjaxUserControl)
                control.ID = this.GetType().Name + "_" + control.ID;
            foreach (System.Web.UI.Control c in control.Controls)
                RenameControl(c);
        }
        protected override void AddedControl(System.Web.UI.Control control, int index)
        {
            ajaxController.AddControl(control);
            RenameControl(control);
            control.Page = this;
        }
        protected object GetViewState(object savedState)
        {
            switch (AjaxController.ViewStateMode)
            {
                case ViewStateMode.InputHidden:
                    return savedState;
                case ViewStateMode.Session:
                    System.Collections.Hashtable viewState = (Page.Session["__viewState__" + Page.Session.SessionID] as System.Collections.Hashtable);
                    System.Collections.Hashtable pageViewState = (viewState[Page.GetType().Name] as System.Collections.Hashtable);
                    return pageViewState[this.ID];
                case ViewStateMode.Cache:
                    return null;
                default:
                    return null;
            }
        }

        protected object SetViewState(object savedState)
        {
            switch (AjaxController.ViewStateMode)
            {
                case ViewStateMode.InputHidden:
                    return savedState;
                case ViewStateMode.Session:
                    System.Collections.Hashtable viewState = (Page.Session["__viewState__" + Page.Session.SessionID] as System.Collections.Hashtable);
                    System.Collections.Hashtable pageViewState = (viewState[Page.GetType().Name] as System.Collections.Hashtable);
                    if (pageViewState.ContainsKey(this.ID))
                        pageViewState[this.ID] = savedState;
                    else
                        pageViewState.Add(this.ID, savedState);
                    return null;
                case ViewStateMode.Cache:
                    return null;
                default:
                    return null;
            }
        }
		protected override void LoadViewState(object savedState)
		{
            object[] state = (object[])(GetViewState(savedState));
			base.LoadViewState(state[0]);
			parent = (string)state[1];
		}
		protected override object SaveViewState()
		{
			object[] state = new object[2];
			state[0] = base.SaveViewState();
			state[1] = parent;
            return SetViewState(state);
		}

		protected override void Render(System.Web.UI.HtmlTextWriter writer)
		{
            if (!ajaxController.IsPostBack || reloadedViewState)
            {
                if (template.IsLoaded)
                {
                    writer.Write(GetFormHtmlBegin());
                    RenderTemplated(writer);
                    writer.Write(GetFormHtmlEnd());
                    reloadedViewState = false;
                    if (!template["pageTemplate"].ContainsValueKey("POSTSCRIPT"))
                    {
                        writer.Write(GetPostScript());
                    }
                }
                else
                    RenderOverAsp(writer);
            }
            else
                ajaxController.AjaxRender();
		}
        protected override void RenderOverAsp(System.Web.UI.HtmlTextWriter writer)
        {
            string tempRender = baseRenderResult();

            int formPos1 = tempRender.IndexOf("<form ");

            int formPos2 = (formPos1<0)? 0 : tempRender.IndexOf(">", formPos1);
            if (formPos1 >= 0 && formPos2 >= 0)
            {
                tempRender = tempRender.Remove(formPos1, formPos2 - formPos1 + 1);
                tempRender = tempRender.Insert(formPos1, GetFormHtmlBegin());
            }
            else
            {
                Response.Clear();
                Response.Write(Properties.Resources.FormTagError);
                Response.End();
            }
            formPos1 = tempRender.IndexOf(GetFormHtmlEnd());
            if (formPos1 >= 0)
            {
                tempRender = tempRender.Insert(formPos1 + 7, GetPostScript());
            }
            else
            {
                Response.Clear();
                Response.Write(Properties.Resources.CloseFormTagError);
                Response.End();
            }
            tempRender = tempRender.Replace("\r\n<div>\r\n<input type=\"hidden\" name=\"__VIEWSTATE\" id=\"\r\n__VIEWSTATE\" value=\"\" />\r\n</div>", "");
            writer.Write(tempRender);
        }

        protected override string GetPostScript()
        {
            using (AjaxTextWriter writer = new AjaxTextWriter())
            {
                writer.WriteBeginTag("input");
                writer.WriteAttribute("type", "hidden");
                writer.WriteAttribute("id", parent + "_CONTAINED_postscript");
                writer.WriteAttribute("name", "CONTAINED_postscript");
                writer.WriteAttribute("value", base.GetPostScript().Replace("&quot;", "\\&quot;").Replace("&#34;", "\\&#34;").Replace("\"", "&#34;"));
                //XHTML
                writer.Write(AjaxTextWriter.SelfClosingTagEnd);
                //HTML 4.0
                //writer.Write(AjaxTextWriter.TagRightChar);

                return writer.ToString();
            }
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
        protected override object LoadPageStateFromPersistenceMedium()
        {
            object result = base.LoadPageStateFromPersistenceMedium();
            if (ViewStateMode != ViewStateMode.InputHidden && ajaxController.IsPostBack)
                LoadViewState(null);
            return result;
        }
        protected override void SavePageStateToPersistenceMedium(object viewState)
        {
            if (ViewStateMode != ViewStateMode.InputHidden)
                base.SavePageStateToPersistenceMedium(null);
            else
                base.SavePageStateToPersistenceMedium(viewState);
        }

        /// <summary>
        /// Loads other ContainedPage into the parent Container in the next callback response
        /// </summary>
        /// <param name="url">Url of the new ContainedPage</param>
        /// <param name="param">QueryString params</param>
		public void ParentLoadPage(string url, string param)
		{
            ajaxController.ExecuteJavascript("$.nxApplication.LoadPane('" + parent + "', '" + url + "', '" + param + "');");
		}
        /// <summary>
        /// Loads other ContainedPage into the parent Container in the next callback response
        /// </summary>
        /// <param name="url">Url of the new ContainedPage</param>
        public void ParentLoadPage(string url)
        {
            ParentLoadPage(url, string.Empty);
        }

        /// <summary>
        /// ContainedPage Constructor
        /// </summary>
		public ContainedPage() : base()
		{
			this.template = null;
			this.EnableViewState = true;
		}
	}
}
