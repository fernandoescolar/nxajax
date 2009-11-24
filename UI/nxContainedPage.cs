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
	/// It can contains web forms and these can be posted by using an Submit nxControl.
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


            if (lang == null && Session["Language"] != null)
				if (Session["Language"].GetType() == typeof(Language))
					lang = (Language)Session["Language"];
            if (lang == null && Application["Language"] != null)
                if (Application["Language"].GetType() == typeof(Language))
                    lang = (Language)Application["Language"];
			
			itIsPostBack = (Request.QueryString["__id"]!=null || Request.Form.Count>0);

			if (!itIsPostBack && reloadedViewState)
				itIsPostBack = true;

            bool getOldViewSate = false;
            if (!itIsPostBack && keepViewState && Session["__" + this.GetType().Name + "__ViewState" + "__" + Session.SessionID] != null)
            {
                itIsPostBack = true;
                getOldViewSate = true;
            }

			base.OnLoad (e);
           
            if (getOldViewSate)
                itIsPostBack = false;

			if (Request.QueryString["__id"]!=null)
			{
				string id = Request.QueryString["__id"];
				itIsPostBack = true;
				foreach(nxControl ctrl in this.containedControls)
				{
                    if (ctrl.ID == id)
					{
						string action = Request.QueryString["__action"];
						ctrl.RaiseEvent(action, Request.QueryString["__value"].Replace("\\n","\n").Replace("\\r", "\r"));
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
			else if (Request.Form.Count>0)
			{
				Submit submitButton = null;
				itIsPostBack = true;
				foreach(string key in Request.Form.Keys)
				{
					foreach(nxControl ctrl in this.containedControls)
					{
						if(ctrl.ID == key)
						{
							if (ctrl is Submit && ctrl.ID == Request.Form["__id"])
								submitButton = (Submit)ctrl;

							ctrl.PutPostValue(Request.Form[key]);
						}
                        else if (ctrl is IChildnxControlContainer && key.Contains(ctrl.ID))
                        {
                            IChildnxControlContainer iccc = ctrl as IChildnxControlContainer;
                            nxControl otherControl = iccc.FindInnerControl(key);
                            if (otherControl != null)
                            {
                                if (otherControl is Submit && otherControl.ID == Request.Form["__id"])
                                    submitButton = (Submit)otherControl;

                                otherControl.PutPostValue(Request.Form[key]);
                            }
                        }
					}
				}
				if(submitButton != null)
					submitButton.RaiseEvent("onClick",submitButton.Value);
			}
			else
			{
				parent = Request.QueryString["__parent"];
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
                    string postScript = "<INPUT type=\"hidden\" id=\"" + parent + "_CONTAINED_postscript\" name=\"CONTAINED_postscript\" value=\"";

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

                    foreach (nxControl ctrl in containedControls)
                    {
                        nxAjaxTextWriter w = new nxAjaxTextWriter();
                        ctrl.RenderJS(w);
                        postScript += w.ToString().Replace("\"", "&#34;");
                    }

                    if (code != "")
                    {
                        postScript += code.Replace("\"", "&#34;");
                        code = "";
                    }
                    postScript += "\">";

                    if (template["pageTemplate"].ContainsKey("POSTSCRIPT"))
                        template["pageTemplate"].Allocate("POSTSCRIPT", postScript);

                    writer.Write("<form name=\"frm_" + this.GetType().Name + "\" id=\"frm_" + this.GetType().Name + "\" onSubmit=\"return false;\" method=\"POST\" action=\"" + this.PageUrl + "\">");
                    writer.Write(template["pageTemplate"].ToString());
                    writer.Write("</form>");
                    
                    if (!template["pageTemplate"].ContainsKey("POSTSCRIPT"))
                        writer.Write(postScript);

                    reloadedViewState = false;
                }
                else
                { 
                    System.IO.StringWriter sw = new System.IO.StringWriter();
                    System.Web.UI.HtmlTextWriter htmlw = new System.Web.UI.HtmlTextWriter((System.IO.TextWriter)sw);
                    base.Render(htmlw);
                    htmlw.Close();
                    string tempRender = sw.ToString();
                    sw.Dispose();

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
                        //tempRender = tempRender.Remove(formPos1, 7);
                        string postscript = "<INPUT type=\"hidden\" id=\"" + parent + "_CONTAINED_postscript\" name=\"CONTAINED_postscript\" value=\"";
                        foreach (nxControl ctrl in containedControls)
                        {
                            nxAjaxTextWriter w = new nxAjaxTextWriter();
                            ctrl.RenderJS(w);
                            postscript += w.ToString().Replace("\"", "&#34;");
                        }
                        if (code != "")
                        {
                            postscript += code.Replace("\"", "&#34;");
                            code = "";
                        }
                        postscript += "\">";
                        tempRender = tempRender.Insert(formPos1 + 7, postscript);
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
			}
			else
			{	
				foreach( nxControl ctrl in containedControls)
				{
                    nxAjaxTextWriter w = new nxAjaxTextWriter();
                    ctrl.RenderJS(w);
                    string jsCode = w.ToString();//.Replace("\n", "\\n").Replace("\r", "\\r");
                    if (jsCode != "")
                        writer.Write(jsCode);
				}
				if(code!="")
				{
                    writer.Write(code);
					code = "";
				}
			}
		}
		private void RenderFromXML( XmlNode root, TemplatePage t)
		{
			foreach( XmlNode e in root.ChildNodes)
			{
				if (e.Name.ToLower() == "area")
				{
					RenderFromXML(e, t[e.Attributes["id"].Value]);
					try
					{
                        if (e.Attributes["method"].Value.ToLower() == "add")
						{
                            t.Add(e.Attributes["place"].Value.ToString().ToUpper(), t[e.Attributes["id"].InnerText].ToString());
						}
						else
						{
                            t.Allocate(e.Attributes["place"].Value.ToString().ToUpper(), t[e.Attributes["id"].InnerText].ToString());
						}
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
