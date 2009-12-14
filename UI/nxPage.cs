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
	/// nxAjax Web Page Base
	/// </summary>
	public class nxPage : System.Web.UI.Page
    {
        #region Not public attributes
        protected bool itIsPostBack = false;
		protected Templates template;
		internal protected Language lang;
		protected nxControlCollection containedControls = new nxControlCollection();
        protected nxUserControlCollection containedUserControls = new nxUserControlCollection();
		protected string path, sPage,code="";
        #endregion
        #region Public Properties

        /// <summary>
        /// Overrides and shadows base Load Event...
        /// </summary>
        public event EventHandler Load;

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
        #endregion
        #region Factory
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
        #endregion
        #region Load Methods
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
            catch { }
        }
        protected virtual void AddControl(System.Web.UI.Control control)
        {
            if (control.Controls.Count > 0 && !(control is IChildnxControlContainer))
                foreach (System.Web.UI.Control c in control.Controls)
                    AddedControl(c, 0);
        }

        protected virtual void getLoadedLanguage()
        {
            if (lang == null && Session["Language"] != null)
                if (Session["Language"].GetType() == typeof(Language))
                    lang = (Language)Session["Language"];
            if (lang == null && Application["Language"] != null)
                if (Application["Language"].GetType() == typeof(Language))
                    lang = (Language)Application["Language"];

            if(lang != null)
                foreach (nxUserControl ctrl in containedUserControls)
                    ctrl.lang = lang;
        }
        protected virtual void getIfItIsPostback()
        {
            itIsPostBack = (Request.QueryString["__id"] != null || Request.Form.Count > 0);
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
            this.Load(this, e);
            this.Load = null;

            foreach (nxUserControl ctrl in containedUserControls)
                ctrl.OnLoad(e);
		}
        protected virtual void processPostback()
        {
            if (Request.QueryString["__id"] != null)
            {
                string id = Request.QueryString["__id"];
                itIsPostBack = true;
                foreach (nxControl ctrl in this.containedControls)
                {
                    if (ctrl.ID == id)
                    {
                        string action = Request.QueryString["__action"];
                        ctrl.RaiseEvent(action, Request.QueryString["__value"].Replace("\\n", "\n").Replace("\\r", "\r"));
                    }
                    else if (ctrl is IChildnxControlContainer && id.Contains(ctrl.ID))
                    {
                        IChildnxControlContainer iccc = ctrl as IChildnxControlContainer;
                        nxControl otherControl = iccc.FindInnerControl(id);
                        if (otherControl != null)
                        {
                            string action = Request.QueryString["__action"];
                            otherControl.RaiseEvent(action, Request.QueryString["__value"].Replace("\\n", "\n").Replace("\\r", "\r"));
                        }
                    }
                }
            }
            else if (Request.Form.Count > 0)
            {
                ISubmit submitButton = null;
                itIsPostBack = true;
                foreach (string key in Request.Form.Keys)
                {
                    foreach (nxControl ctrl in this.containedControls)
                    {
                        if (ctrl.ID == key)
                        {
                            if (ctrl is ISubmit && ctrl.ID == Request.Form["__id"])
                                submitButton = (ISubmit)ctrl;

                            ctrl.PutPostValue(Request.Form[key]);
                        }
                        else if (ctrl is IChildnxControlContainer && key.Contains(ctrl.ID))
                        {
                            IChildnxControlContainer iccc = ctrl as IChildnxControlContainer;
                            nxControl otherControl = iccc.FindInnerControl(key);
                            if (otherControl != null)
                            {
                                if (otherControl is ISubmit && otherControl.ID == Request.Form["__id"])
                                    submitButton = (ISubmit)otherControl;

                                otherControl.PutPostValue(Request.Form[key]);
                            }
                        }
                    }
                }
                if (submitButton != null)
                    if (submitButton is nxControl)
                    {
                        nxControl submitControl = (submitButton as nxControl);
                        submitControl.RaiseEvent("onClick", submitButton.Value);
                    }
            }
            else
            {
                template = new Templates(Server.MapPath(path), lang);
                try
                {
                    if (sPage != "")
                        template.Load("pageTemplate", sPage);
                }
                catch (Exception ex)
                {
                    Response.Clear();
                    Response.Write("No se pudo cargar la Plantilla: ");
                    Response.Write(ex.Message);
                    Response.End();
                }
            }
        }
        #endregion
        #region Render methods
        protected override void OnPreRender(System.EventArgs e) { /*Do Nothing*/ }
		protected override void Render(System.Web.UI.HtmlTextWriter writer)
		{
            if (!itIsPostBack)
            {
                if (template.IsLoaded)
                    RenderTemplated(writer);
                else
                    RenderOverAsp(writer);
            }
            else
                RenderJSOnly(writer);
		}
        /// <summary>
        /// Renders in a string the System.Web.UI.Page Render result
        /// </summary>
        /// <returns></returns>
        protected string baseRenderResult()
        {
            string result = string.Empty;
            System.IO.StringWriter swriter = new System.IO.StringWriter();
            System.Web.UI.HtmlTextWriter hwriter = new System.Web.UI.HtmlTextWriter(swriter);
            base.Render(hwriter);
            hwriter.Close();
            result = swriter.ToString();
            swriter.Close();
            hwriter.Dispose();
            swriter.Dispose();
            result = result.Replace("<div>\r\n<input type=\"hidden\" name=\"__VIEWSTATE\" id=\"__VIEWSTATE\" value=\"\" />\r\n<input type=\"hidden\" name=\"__VIEWSTATE\" id=\"\r\n__VIEWSTATE\" value=\"\" />\r\n</div>\r\n", "");
            return result;
        }
        /// <summary>
        /// Only renders js script code
        /// </summary>
        /// <param name="writer">writer</param>
        protected virtual void RenderJSOnly(System.Web.UI.HtmlTextWriter writer)
        {
            nxAjaxTextWriter jsWriter = new nxAjaxTextWriter();
            foreach (nxControl ctrl in containedControls)
            {
                ctrl.RenderJS(jsWriter);
            }
            writer.Write(jsWriter.ToString()/*.Replace("\n", "\\n").Replace("\r", "\\r")*/);
            if (code != "")
            {
                writer.Write(code);
                code = "";
            }
        }
        /// <summary>
        /// Renders over System.Web.UI.Page Render result
        /// </summary>
        /// <param name="writer">writer</param>
        protected virtual void RenderOverAsp(System.Web.UI.HtmlTextWriter writer)
        {
            base.Render(writer);
        }
        /// <summary>
        /// Renders Web with Template based system
        /// </summary>
        /// <param name="writer">writer</param>
        protected virtual void RenderTemplated(System.Web.UI.HtmlTextWriter writer)
        {
            XmlDocument doc = prepareTemplatedXml();
            if (doc == null)
                return;

            bool checkXml = true;
            foreach(XmlElement e in doc["page"].ChildNodes)
                if (e.Name.ToLower() != "form" && e.Name.ToLower() != "area")
                    if (e.Attributes.Count > 0)
                    {
                        checkXml = false;
                        break;
                    }

            if (checkXml)
                fillTemplateFromXml(doc["page"], template["pageTemplate"]);
            else
                fillTemplateFromControlId();

            fillTemplateSpecialTag();
            writer.Write(template["pageTemplate"].ToString());
        }
        /// <summary>
        /// Fills template from each nxControl ID
        /// </summary>
        protected virtual void fillTemplateFromControlId()
        {
            foreach (nxControl ctrl in containedControls)
            {
                nxAjaxTextWriter wHTML = new nxAjaxTextWriter();
                ctrl.RenderHTML(wHTML);
                template["pageTemplate"].Allocate(ctrl.BaseID, wHTML.ToString());
            }
            foreach (nxUserControl ctrl in containedUserControls)
            {
                System.IO.StringWriter sw = new System.IO.StringWriter();
                System.Web.UI.HtmlTextWriter htmlw = new System.Web.UI.HtmlTextWriter((System.IO.TextWriter)sw);
                ctrl.InternalRender(htmlw);
                htmlw.Close();
                template["pageTemplate"].Allocate(ctrl.BaseID, sw.ToString());
            }
        }
        /// <summary>
        /// Fills template from Xml Node (recursive)
        /// </summary>
        /// <param name="root">Xml Node source</param>
        /// <param name="t">Template page to fill</param>
        protected virtual void fillTemplateFromXml(XmlNode root, TemplatePage t)
        {
            foreach (XmlNode e in root.ChildNodes)
            {
                if (e.Name.ToLower() == "area")
                {
                    fillTemplateFromXml(e, t[e.Attributes["id"].Value]);
                    try
                    {
                        bool allocate = false;
                        if (e.Attributes["method"] != null)
                        {
                            if (e.Attributes["method"].Value.ToLower() == "allocate")
                                allocate = true;
                            else if (e.Attributes["method"].Value.ToLower() != "add")
                                throw new FormatException("Area method name not valid (\"Add\" or \"Allocate\").");
                        }
                        if (allocate)
                            t.Allocate(e.Attributes["place"].Value.ToString().ToUpper(), t[e.Attributes["id"].InnerText].ToString());
                        else
                            t.Add(e.Attributes["place"].Value.ToString().ToUpper(), t[e.Attributes["id"].InnerText].ToString());
                    }
                    catch
                    {
                        t.Allocate(e.Attributes["place"].Value.ToString().ToUpper(), t[e.Attributes["id"].InnerText].ToString());
                    }

                }
                else
                {
                    t.Allocate(e.Name.ToUpper(), e.InnerXml);
                }
            }
        }
        /// <summary>
        /// Fills special template tags (like POSTBACK)
        /// </summary>
        protected virtual void fillTemplateSpecialTag()
        {
            if (template["pageTemplate"].ContainsValueKey("POSTSCRIPT"))
            {
                template["pageTemplate"].Allocate("POSTSCRIPT", getPostScript());
                code = "";
            }
        }
        /// <summary>
        /// Prepares and Creates a new XmlDocument for the template system
        /// </summary>
        /// <returns>generated Xml Document</returns>
        protected virtual XmlDocument prepareTemplatedXml()
        {
            string xml = baseRenderResult();

            if (xml.IndexOf("<page>") < 0)
                xml = "<page>" + xml + "</page>";
            if (xml.IndexOf("<?xml ") < 0)
                xml = "<?xml version=\"1.0\" encoding=\"utf-8\"?>" + xml;
            else //xml tag must be the first line of an xml document
                xml = xml.Remove(0, xml.IndexOf("<?xml "));


            try 
            {
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(xml);
                return doc;
            }
            catch (Exception ex)
            {
                //string msg = ex.Message;
                Response.Clear();
                Response.Write("Error W3C. Se ha encontrado un error en la conversión a estandar XML, es posible que alguna etiqueta no esté cerrada: " + ex.Message);
                Response.End();
                return null;
            }
            
        }
        /// <summary>
        /// Gets the post javascript code
        /// </summary>
        /// <returns>javascript code</returns>
        protected virtual string getPostScript()
        {
            string postScript = string.Empty;
            foreach (nxControl ctrl in containedControls)
            {
                nxAjaxTextWriter w = new nxAjaxTextWriter();
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
            using (nxAjaxTextWriter writer = new nxAjaxTextWriter())
            {
                writer.WriteBeginTag("form");
                writer.WriteAttribute("name", "frm_" + this.GetType().Name);
                writer.WriteAttribute("id", "frm_" + this.GetType().Name);
                writer.WriteAttribute("onSubmit", "return false;");
                writer.WriteAttribute("method", "POST");
                writer.WriteAttribute("action", this.PageUrl);
                writer.Write(nxAjaxTextWriter.TagRightChar);

                return writer.ToString();
            }
        }
        /// <summary>
        /// Returns form end tag
        /// </summary>
        /// <returns></returns>
        internal string getFormHtmlEnd()
        {
            using (nxAjaxTextWriter writer = new nxAjaxTextWriter())
            {
                writer.WriteEndTag("form");
                return writer.ToString();
            }
        }
        #endregion
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
        #endregion
    }
}
