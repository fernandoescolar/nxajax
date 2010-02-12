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
    /// A UserControl managed in nxAjax AjaxPage
    /// </summary>
    public class AjaxUserControl : System.Web.UI.UserControl
    {
        private IAjaxController ajaxControllerCache = null;
        protected Templates template;
        protected string path, sPage, originalID;
        protected bool isLoaded = false;

        /// <summary>
        /// Parent AjaxPage 
        /// </summary>
        public virtual IAjaxController AjaxController
        {
            get
            {
                if (ajaxControllerCache == null)
                {
                    if (base.Page is IAjaxControllerContainer)
                        ajaxControllerCache = (base.Page as IAjaxControllerContainer).AjaxController;
                    else if (base.Page.Form is IAjaxController)
                        ajaxControllerCache = (base.Page.Form as IAjaxControllerContainer).AjaxController;
                    else
                    {
                        lock (base.Page.Controls)
                        {
                            foreach (System.Web.UI.Control c in base.Page.Controls)
                                if (c is IAjaxControllerContainer)
                                    ajaxControllerCache = (c as IAjaxControllerContainer).AjaxController;
                        }
                    }
                }
                return ajaxControllerCache;
            }
        }
        /// <summary>
        /// Gets the current language object
        /// </summary>
        public Language Lang
        {
            get { return AjaxController.Lang; }
        }
        /// <summary>
        /// ID name
        /// </summary>
        public override string ID
        {
            get
            {
                return base.ID;
            }
            set
            {
                if (string.IsNullOrEmpty(originalID) && !string.IsNullOrEmpty(base.ID))
                    originalID = base.ID;
                base.ID = value;
            }
        }
        /// <summary>
        /// Original ID name
        /// </summary>
        public string BaseID
        {
            get
            {
                return (string.IsNullOrEmpty(originalID)) ? ID : originalID;
            }
            internal set
            {
                originalID = value;
            }
        }

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
            set
            {
                string[] s = value.Split('/');
                sPage = s[s.Length - 1];
                path = value.Replace(sPage, "");
            }
        }
        /// <summary>
        /// Creates a new AjaxUserControl
        /// </summary>
        public AjaxUserControl()
        {
            this.template = null;
            this.EnableViewState = true;
            this.originalID = this.path = this.sPage = string.Empty;
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
            AjaxController.AddControl(control);
            RenameControl(control);
            control.Page = this.Page;
        }
       
        internal new void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            if (this.Load != null)
            {
                this.Load(this, e);
                this.Load = null;
            }
            template = new Templates(Server.MapPath(path), Lang);
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
            isLoaded = true;
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
            return result;
        }
        internal void InternalRender(System.Web.UI.HtmlTextWriter writer)
        {
            Render(writer);
        }
        protected override void Render(System.Web.UI.HtmlTextWriter writer)
        {
            if (template.IsLoaded)
                RenderTemplated(writer);
            else
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
            foreach (AjaxControl ctrl in AjaxController.AjaxControls)
            {
                AjaxTextWriter wHTML = new AjaxTextWriter();
                ctrl.RenderHTML(wHTML);
                template["pageTemplate"].Allocate(ctrl.BaseID, wHTML.ToString());
            }
            foreach (AjaxUserControl ctrl in AjaxController.AjaxUserControls)
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
            try
            {
                if (template["pageTemplate"].ContainsValueKey("INITFORM"))
                    template["pageTemplate"].Allocate("INITFORM", GetFormHtmlBegin());

                if (template["pageTemplate"].ContainsValueKey("ENDFORM"))
                    template["pageTemplate"].Allocate("ENDFORM", GetFormHtmlEnd());
            }
            catch { }
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
                AjaxController.WriteAjaxFormBegin("frm_" + this.GetType().Name, writer);
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
                AjaxController.WriteAjaxFormEnd(writer);
                return writer.ToString();
            }
        }
    }
}
