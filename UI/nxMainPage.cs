/*
 * Licensing
 *  Mozilla Public License 1.1 (MPL 1.1)
 * 
 * Fernando Escolar Martínez-Berganza <fer.escolar@gmail.com>
 * 
 */

using System;
using nxAjax.UI.Controls;

namespace nxAjax.UI
{
	/// <summary>
	/// nxMainPage is the main container page (or master page). It provides all javascripts and the engine to nxAjax framework works.
    /// <code>
    /// In the Template page requiereds the tags: "&lt;$PRESCRIPT$&gt;", "&lt;$ONLOAD$&gt;" and "&lt;$POSTCRIPT$&gt;" in order to works well.
    /// </code>
	/// </summary>
	public class nxMainPage : nxPage
	{
        /// <summary>
        /// nxMainPage Constructor
        /// </summary>
		public nxMainPage() : base() { }

		protected override void OnLoad(EventArgs e)
		{
			foreach(nxControl ctrl in containedControls)
				ctrl.AjaxNotUpdate();


            if (lang == null && Session["Language"] != null)
				if (Session["Language"].GetType() == typeof(Language))
					lang = (Language)Session["Language"];
            if (lang == null && Application["Language"] != null)
                if (Application["Language"].GetType() == typeof(Language))
                    lang = (Language)Application["Language"];
			
			itIsPostBack = (Request.QueryString["__id"]!=null || Request.Form.Count>0);
			base.OnLoad (e);

			if (Request.QueryString["__id"]!=null)
			{
				string id = Request.QueryString["__id"];
				foreach(nxControl ctrl in this.containedControls)
				{
                    if (ctrl.ID == id)
					{
						string action = Request.QueryString["__action"];
                        string val = Request.QueryString["__value"].Replace("\\n", "\n").Replace("\\r", "\r");
						ctrl.RaiseEvent(action, val);
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
                        if (ctrl.ID == key)
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
                try
                {
                    template = new Templates(Server.MapPath(path), lang);
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
		protected override void Render(System.Web.UI.HtmlTextWriter writer)
		{
			if (!itIsPostBack)
			{
                if (template.IsLoaded)
                {
                    try
                    {
                        template["pageTemplate"].Allocate("PRESCRIPT", getScriptHeader());
                        
                        if (template["pageTemplate"].ContainsKey("ONLOAD"))
                            template["pageTemplate"].Allocate("ONLOAD", "__start()");
                        
                        template["pageTemplate"].Allocate("POSTSCRIPT", "<script language=\"javascript\">");
                        template["pageTemplate"].Allocate("INIT_FORM", "<form name=\"frm_" + this.GetType().Name + "\" id=\"frm_" + this.GetType().Name + "\" onSubmit=\"return false;\" method=\"POST\" action=\"" + this.PageUrl + "\">");
                        template["pageTemplate"].Allocate("END_FORM", "</form>");
                    }
                    catch { }
                    nxAjaxTextWriter jsHTML = new nxAjaxTextWriter();
                    foreach (nxControl ctrl in containedControls)
                    {
                        nxAjaxTextWriter wHTML = new nxAjaxTextWriter();
                        
                        ctrl.RenderHTML(wHTML);
                        ctrl.RenderJS(jsHTML);
                        template["pageTemplate"].Allocate(ctrl.ID, wHTML.ToString());
                    }
                    foreach (nxUserControl ctrl in containedUserControls)
                    {
                        System.IO.StringWriter sw = new System.IO.StringWriter();
                        System.Web.UI.HtmlTextWriter htmlw = new System.Web.UI.HtmlTextWriter((System.IO.TextWriter)sw);
                        ctrl.InternalRender(htmlw);
                        htmlw.Close();
                        template["pageTemplate"].Allocate(ctrl.ID, sw.ToString());
                    }
                    template["pageTemplate"].Add("POSTSCRIPT", "$(document).ready(function(){");
                    template["pageTemplate"].Add("POSTSCRIPT", "__start();");
                    template["pageTemplate"].Add("POSTSCRIPT", jsHTML.ToString());
                    if (code != "")
                    {
                        template["pageTemplate"].Add("POSTSCRIPT", code);
                        code = "";
                    }
                    template["pageTemplate"].Add("POSTSCRIPT", "});");
                    template["pageTemplate"].Add("POSTSCRIPT", "</script>");
                    writer.Write(template["pageTemplate"].ToString());
                }
                else
                {
                    System.IO.StringWriter sw = new System.IO.StringWriter();
                    System.Web.UI.HtmlTextWriter htmlw = new System.Web.UI.HtmlTextWriter((System.IO.TextWriter)sw);
                    base.Render(htmlw);
                    htmlw.Close();
                    string tempRender = sw.ToString();
                    sw.Dispose();

                    /*Cabecera*/
                    int htmlPos = tempRender.ToLower().IndexOf("<html");
                    if (htmlPos < 0)
                    {
                        tempRender = "<html>" + tempRender;
                        htmlPos = tempRender.ToLower().IndexOf("</html>");
                        if (htmlPos < 0)
                            tempRender += "</html>";
                        htmlPos = tempRender.ToLower().IndexOf("<html");
                    }
                    htmlPos = tempRender.IndexOf(">", htmlPos);
                    int headPos = tempRender.ToLower().IndexOf("<head>");
                    if (headPos < 0)
                        tempRender = tempRender.Insert(htmlPos + 1, "<html><head>" + getScriptHeader() + "</head>");
                    else
                        tempRender = tempRender.Insert(headPos + 6, getScriptHeader());
                    /*Cuerpo*/
                    int bodyPos = tempRender.ToLower().IndexOf("<body");
                    if (bodyPos >= 0)
                    {
                        bodyPos = tempRender.ToLower().IndexOf(">", bodyPos);
                        int onloadPos = tempRender.ToLower().IndexOf("onload=");
                        if (onloadPos < 0)
                            tempRender = tempRender.Insert(bodyPos, " onload=\"__start();\"");
                        else
                            tempRender = tempRender.Insert(onloadPos + 8, "__start();");
                    }
                    else 
                    {
                        headPos = tempRender.ToLower().IndexOf("</head>");
                        tempRender = tempRender.Insert(headPos + 7, "<body onload=\"__start();\">");
                        bodyPos = tempRender.ToLower().IndexOf("</body>");
                        if (bodyPos < 0)
                        {
                            htmlPos = tempRender.ToLower().IndexOf("</html>");
                            tempRender.Insert(htmlPos, "</body>");
                        }
                    }
                    /*Formulario y postscript*/
                    tempRender = tempRender.Replace("<div>\r\n<input type=\"hidden\" name=\"__VIEWSTATE\" id=\"__VIEWSTATE\" value=\"\" />\r\n<input type=\"hidden\" name=\"__VIEWSTATE\" id=\"\r\n__VIEWSTATE\" value=\"\" />\r\n</div>\r\n", "");
                    int formPos1 = tempRender.ToLower().IndexOf("<form ");
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
                    formPos1 = tempRender.ToLower().IndexOf("</form>");
                    if (formPos1 >= 0)
                    {
                        //tempRender = tempRender.Remove(formPos1, 7);
                        nxAjaxTextWriter jsWriter = new nxAjaxTextWriter();
                        jsWriter.WriteLine("<script language=\"javascript\">");
                        jsWriter.WriteLine("$(document).ready(function(){");
                        jsWriter.WriteLine("__start();");
                        foreach (nxControl ctrl in containedControls)
                            ctrl.RenderJS(jsWriter);
                        if (code != "")
                        {
                            jsWriter.WriteLine(code);
                            code = "";
                        }
                        jsWriter.WriteLine("});");
                        jsWriter.WriteLine("</script>");
                        tempRender = tempRender.Insert(formPos1 + 7, jsWriter.ToString());
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
                nxAjaxTextWriter jsWriter = new nxAjaxTextWriter();
				foreach( nxControl ctrl in containedControls)
				{
                    ctrl.RenderJS(jsWriter);
				}
                writer.Write(jsWriter.ToString().Replace("\n", "\\n").Replace("\r", "\\r"));
				if(code!="")
				{
					writer.Write(code);
					code = "";
				}
			}
		}

		private string getScriptHeader()
		{
            string code = "";
            string url = "nxScriptResource.axd";
            code += "<script language=\"javascript\" src=\"" + url + "?src=" + EncodeName(Session.SessionID + "res.jquery-1.3.2.min.js") + "\"></script>" + "\n\t";
            code += "<script language=\"javascript\" src=\"" + url + "?src=" + EncodeName(Session.SessionID + "res.jquery.cookie.js") + "\"></script>" + "\n\t";
            code += "<script language=\"javascript\" src=\"" + url + "?src=" + EncodeName(Session.SessionID + "res.jquery.ajaxqueue.js") + "\"></script>" + "\n\t";
            code += "<script language=\"javascript\" src=\"" + url + "?src=" + EncodeName(Session.SessionID + "res.jquery.datepick.pack.js") + "\"></script>" + "\n\t";
            code += "<script language=\"javascript\" src=\"" + url + "?src=" + EncodeName(Session.SessionID + "res.jquery.treeview.pack.js") + "\"></script>" + "\n\t";
            code += "<script language=\"javascript\" src=\"" + url + "?src=" + EncodeName(Session.SessionID + "res.jquery.history.js") + "\"></script>" + "\n\t";
            code += "<script language=\"javascript\" src=\"" + url + "?src=" + EncodeName(Session.SessionID + "res.jquery.common.js") + "\"></script>" + "\n\t";
            //code += "<script language=\"javascript\" src=\"" + Request.Url.AbsolutePath + "?src=" + EncodeName(Session.SessionID + "res.nxAjax.js") + "\"></script>" + "\n\t";
            code += "<script language=\"javascript\" src=\"" + url + "?src=" + EncodeName(Session.SessionID + "res.jquery.nxApplication.js") + "\"></script>" + "\n\t";
            code += "<script language=\"javascript\" src=\"" + url + "?src=" + EncodeName(Session.SessionID + "res.jquery.ajaxupload.js") + "\"></script>" + "\n\t";
            code += "<script language=\"javascript\" src=\"" + url + "?src=" + EncodeName(Session.SessionID + "res.nxTextBox.js") + "\"></script>" + "\n\t";
            //code += "<script language=\"javascript\" src=\"" + Request.Url.AbsolutePath + "?src=" + EncodeName(Session.SessionID + "res.nxAnimation.js") + "\"></script>" + "\n\t";
            //code += "<script language=\"javascript\" src=\"" + Request.Url.AbsolutePath + "?src=" + EncodeName(Session.SessionID + "res.nxHTMLEditor.js") + "\"></script>" + "\n\t";
            code += "<script language=\"javascript\" src=\"" + url + "?src=" + EncodeName(Session.SessionID + "res.jquery.wysiwyg.js") + "\"></script>" + "\n\t";
            //code += "<script language=\"javascript\" src=\"" + Request.Url.AbsolutePath + "?src=" + EncodeName(Session.SessionID + "res.nxDatePicker.js") + "\"></script>" + "\n\t";
            //code += "<script language=\"javascript\" src=\"" + Request.Url.AbsolutePath + "?src=" + EncodeName(Session.SessionID + "res.nxTree.js") + "\"></script>" + "\n\t";
            code += "<script language=\"javascript\" src=\"" + url + "?src=" + EncodeName(Session.SessionID + "res.nxEditable.js") + "\"></script>" + "\n\t";
            code += "<script language=\"javascript\" src=\"" + url + "?src=" + EncodeName(Session.SessionID + "res.nxDragnDrop.js") + "\"></script>" + "\n\t";
			code += "<script language=\"javascript\">" + "\n\t";
            //code += "var __ajax;" + "\n\t";
			//code += "var __application;" + "\n\t";
            code += "var initialized = false;" + "\n\t";
			code += "function __start()" + "\n\t";
			code += "{" + "\n\t";
			//code += "	if(!__ajax) __ajax = new nxAjax('loading', 'nada');" + "\n\t";
            code += "	if(!initialized){ $.nxApplication.Init('loading'); $.historyInit($.nxApplication.pageload);}" + "\n\t";
			//code += "	setTimeout(\"__application.DoEvents();\",100);" + "\n\t";
			code += "}" + "\n\t";
			code += "</script>" + "\n";
			return code;
		}
		private string EncodeName(string name)
		{
			byte[] encData_byte = new byte[name.Length];
			encData_byte = System.Text.Encoding.UTF8.GetBytes(name);    
			string encodedData = Convert.ToBase64String(encData_byte);
			return encodedData;
		}
	}
}
