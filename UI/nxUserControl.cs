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
    /// A UserControl managed in nxAjax nxPage
    /// </summary>
    public class nxUserControl : System.Web.UI.UserControl
    {
        protected Templates template;
        internal protected Language lang;
        protected nxControlCollection containedControls = new nxControlCollection();
        protected string path, sPage, originalID;
        protected bool isLoaded = false;

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
                return originalID;
            }
            internal set
            {
                originalID = value;
            }
        }

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
        /// Get/Set Parent nxPage
        /// </summary>
        public nxPage nxPage
        {
            get
            {
                return (nxPage)base.Page;
            }
            set
            {
                base.Page = value;
            }
        }
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
        /// Creates a new nxUserControl
        /// </summary>
        public nxUserControl()
        {
            this.lang = null;
            this.template = null;
            this.EnableViewState = true;
            this.originalID = this.path = this.sPage = string.Empty;
        }

        protected override void AddedControl(System.Web.UI.Control control, int index)
        {
            try
            {
                AddControl(control);
                control.Page = this.Page;
                if (control is nxControl)
                    containedControls += (nxControl)control;
            }
            catch { }
        }
        protected virtual void AddControl(System.Web.UI.Control control)
        {
            if (control.Controls.Count > 0 && !(control is IChildnxControlContainer))
                foreach (System.Web.UI.Control c in control.Controls)
                    AddedControl(c, 0);
        }

        internal new void OnLoad(EventArgs e)
        {
            if (isLoaded)
                return;

            if (lang == null && Session["Language"] != null)
                if (Session["Language"].GetType() == typeof(Language))
                    lang = (Language)Session["Language"];
            if (lang == null && Application["Language"] != null)
                if (Application["Language"].GetType() == typeof(Language))
                    lang = (Language)Application["Language"];

            base.OnLoad(e);

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
            isLoaded = true;
        }

        internal void InternalRender(System.Web.UI.HtmlTextWriter writer)
        {
            Render(writer);
        }
        protected override void Render(System.Web.UI.HtmlTextWriter writer)
        {
            if (template.IsLoaded)
            {
                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                System.IO.StringWriter swriter = new System.IO.StringWriter(sb);
                System.Web.UI.HtmlTextWriter hwriter = new System.Web.UI.HtmlTextWriter(swriter);
                hwriter.WriteLine("<?xml version=\"1.0\" encoding=\"utf-8\"?>");
                hwriter.WriteLine("<page>");
                base.Render(hwriter);
                hwriter.WriteLine("</page>");
                hwriter.Close();
                XmlDocument doc = new XmlDocument();
                try
                {
                    XmlTextReader reader = new XmlTextReader(new System.IO.StringReader(sb.ToString()));
                    reader.Normalization = false;
                    doc.Load((XmlReader)reader);
                }
                catch (Exception ex)
                {
                    //string msg = ex.Message;
                    Response.Clear();
                    Response.Write("Error W3C. Se ha encontrado un error en la conversión a estandar XML, es posible que alguna etiqueta no esté cerrada: " + ex.Message);
                    Response.End();
                    return;
                }
                RenderFromXML(doc["page"], template["pageTemplate"]);
                writer.Write(template["pageTemplate"].ToString());
            }
            else
                base.Render(writer);
            
        }

        private void RenderFromXML(XmlNode root, TemplatePage t)
        {
            foreach (XmlNode e in root.ChildNodes)
            {
                if (e.Name.ToLower() == "area")
                {
                    RenderFromXML(e, t[e.Attributes["id"].Value]);
                    try
                    {
                        if (e.Attributes["method"].Value.ToLower() == "add")
                            t.Add(e.Attributes["place"].Value.ToString().ToUpper(), t[e.Attributes["id"].InnerText].ToString());
                        else
                            t.Allocate(e.Attributes["place"].Value.ToString().ToUpper(), t[e.Attributes["id"].InnerText].ToString());
                    }
                    catch
                    {
                        t.Allocate(e.Attributes["place"].Value.ToString().ToUpper(), t[e.Attributes["id"].InnerText].ToString());
                    }

                }
                else
                    t.Allocate(e.Name.ToUpper(), e.InnerXml);
            }
        }
    }
}
