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
	/// nxAjax Web Page Base
	/// </summary>
    public class AjaxPage : System.Web.UI.Page, IAjaxControllerContainer
    {
        #region Not public attributes
        internal AjaxGenericController ajaxController;
        protected Templates template;
		protected string path, sPage;
        #endregion

        #region Public Properties
        /// <summary>
        /// Overrides and shadows base Load Event...
        /// </summary>
        public new event EventHandler Load;
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
        /// Gets the current language object
        /// </summary>
        public Language Lang
        {
            get { return ajaxController.Lang; }
        }
        /// <summary>
        /// Gets/Sets ViewState Storage Mode
        /// </summary>
        public ViewStateMode ViewStateMode
        {
            get { return ajaxController.ViewStateMode; }
            set { ajaxController.ViewStateMode = value; }
        }
        public IAjaxController AjaxController
        {
            get { return ajaxController; }
        }
        #endregion
        #region Factory
        /// <summary>
        /// Creates a new AjaxPage
        /// </summary>
		public AjaxPage():base()
		{
            this.ajaxController = new AjaxGenericController(this);
			this.template = null;
			this.EnableViewState = false;
            this.EnableEventValidation = false;
            this.ViewStateEncryptionMode = System.Web.UI.ViewStateEncryptionMode.Never;
            this.EnableViewStateMac = false;
            this.path = this.sPage = "";
        }
        #endregion
        #region Load Methods
        protected override void AddedControl(System.Web.UI.Control control, int index)
        {
            ajaxController.AddControl(control);
            control.Page = this;
        }

		protected override void OnLoad(EventArgs e)
		{
            //if (IsPostBack && Session["__" + this.GetType().Name + "__ViewState" + "__" + Session.SessionID] == null)
            //{
            //    Response.Clear();
            //    Response.Write("alert(\"Session expired!\");");
            //    Response.Write("window.location.reload();");
            //    Response.End();
            //}
            ajaxController.Init();
            base.OnLoad (e);
            if (this.Load != null)
            {
                this.Load(this, e);
                this.Load = null;
            }
            ajaxController.Load();
		}
        protected virtual void processPostback()
        {
            if (!ajaxController.IsPostBack)
            {
                template = new Templates(Server.MapPath(path), Lang);
                try
                {
                    if (sPage != "")
                        template.Load("pageTemplate", sPage);
                }
                catch (Exception ex)
                {
                    Response.Clear();
                    Response.Write(Properties.Resources.CannotLoadTemplate + ": ");
                    Response.Write(ex.Message);
                    Response.End();
                }
            }
            else
                ajaxController.AjaxRequestProcess();
        }

        protected override object LoadPageStateFromPersistenceMedium()
        {
            ajaxController.CheckPostback();
            object result = base.LoadPageStateFromPersistenceMedium();
            if (ViewStateMode != ViewStateMode.InputHidden && ajaxController.IsPostBack)
                foreach (AjaxControl ac in ajaxController.AjaxControls)
                    ac.ProtectedLoadViewState(null);
           
            return result;
        }
        protected override void SavePageStateToPersistenceMedium(object viewState)
        {
            if (ViewStateMode != ViewStateMode.InputHidden)
                foreach (AjaxControl ac in ajaxController.AjaxControls)
                    ac.ProtectedSaveViewState();
            base.SavePageStateToPersistenceMedium (viewState);
        }
        #endregion
        #region Render methods
		protected override void Render(System.Web.UI.HtmlTextWriter writer)
		{
            if (!ajaxController.IsPostBack)
            {
                if (template.IsLoaded)
                    RenderTemplated(writer);
                else
                    RenderOverAsp(writer);
            }
            else
                ajaxController.AjaxRender();
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
            bool checkXml = true;
            if (doc != null)
            {
                foreach (XmlElement e in doc["page"].ChildNodes)
                    if (e.Name.ToLower() != "form" && e.Name.ToLower() != "area")
                        if (e.Attributes.Count > 0)
                        {
                            checkXml = false;
                            break;
                        }
            }
            else
                checkXml = false;

            if (checkXml)
                fillTemplateFromXml(doc["page"], template["pageTemplate"]);
            else
                fillTemplateFromControlId();

            fillTemplateSpecialTag();
            writer.Write(template["pageTemplate"].ToString());
        }
        /// <summary>
        /// Fills template from each AjaxControl ID
        /// </summary>
        protected virtual void fillTemplateFromControlId()
        {
            foreach (AjaxControl ctrl in ajaxController.AjaxControls)
            {
                AjaxTextWriter wHTML = new AjaxTextWriter();
                ctrl.RenderHTML(wHTML);
                template["pageTemplate"].Allocate(ctrl.BaseID, wHTML.ToString());
            }
            foreach (AjaxUserControl ctrl in ajaxController.AjaxUserControls)
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
                template["pageTemplate"].Allocate("POSTSCRIPT", GetPostScript());
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
                Response.Write(Framework.Ajax.Properties.Resources.W3CValidationError + ex.Message);
                Response.End();
                return null;
            }
            
        }
        /// <summary>
        /// Gets nxAjax form tag begin
        /// </summary>
        /// <param name="writer">Ajax Writer</param>
        internal string GetFormHtmlBegin()
        {
            using (AjaxTextWriter writer = new AjaxTextWriter())
            {
                ajaxController.WriteAjaxFormBegin("frm_" + this.GetType().Name, writer);
                return writer.ToString();
            }
        }
        /// <summary>
        /// Gets form end tag
        /// </summary>
        /// <param name="writer">Ajax Writer</param>
        internal string GetFormHtmlEnd()
        {
            using (AjaxTextWriter writer = new AjaxTextWriter())
            {
                ajaxController.WriteAjaxFormEnd(writer);
                return writer.ToString();
            }
        }
        protected virtual string GetPostScript()
        {
            return ajaxController.GetPostbackJavascript();
        }
        #endregion
        #region Javascript commands
        /// <summary>
        /// Execute an specified javascript code in the next callback response
        /// </summary>
        /// <param name="lines">Javascript code</param>
        public void ExecuteJavascript(string lines)
        {
            ajaxController.ExecuteJavascript(lines);
        }
        /// <summary>
        /// Shows an alert dialog in the next callback response
        /// </summary>
        /// <param name="msg"></param>
        public void DocumentAlert(string msg)
        {
            ajaxController.DocumentAlert(msg);
        }
        /// <summary>
        /// Redirects to an specified url in the next callback
        /// </summary>
        /// <param name="url">url to redirect</param>
        public void DocumentRedirect(string url)
        {
            ajaxController.DocumentRedirect(url);
        }
        #endregion
    }
}
