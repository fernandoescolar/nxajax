/*
 * Licensing
 *  Mozilla Public License 1.1 (MPL 1.1)
 * 
 * Fernando Escolar Mart�nez-Berganza <fer.escolar@gmail.com>
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

            getLoadedLanguage();
            getIfItIsPostback();		
			base.OnLoad (e);
            processPostback();
		}
		
        protected override void RenderOverAsp(System.Web.UI.HtmlTextWriter writer)
        {
            string tempRender = baseRenderResult();

            /*Header*/
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
            /*Body*/
            //int bodyPos = tempRender.ToLower().IndexOf("<body");
            //if (bodyPos >= 0)
            //{
            //    bodyPos = tempRender.ToLower().IndexOf(">", bodyPos);
            //    int onloadPos = tempRender.ToLower().IndexOf("onload=");
            //    if (onloadPos < 0)
            //        tempRender = tempRender.Insert(bodyPos, " onload=\"__start();\"");
            //    else
            //        tempRender = tempRender.Insert(onloadPos + 8, "__start();");
            //}
            //else
            //{
            //    headPos = tempRender.ToLower().IndexOf("</head>");
            //    tempRender = tempRender.Insert(headPos + 7, "<body onload=\"__start();\">");
            //    bodyPos = tempRender.ToLower().IndexOf("</body>");
            //    if (bodyPos < 0)
            //    {
            //        htmlPos = tempRender.ToLower().IndexOf("</html>");
            //        tempRender.Insert(htmlPos, "</body>");
            //    }
            //}
            /*Form and postscript*/
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
            return "<script language=\"javascript\"> $(document).ready(function(){ __start(); " + base.getPostScript() + "}); </script>";
        }
        protected override void fillTemplateSpecialTag()
        {
            base.fillTemplateSpecialTag();
            try
            {
                template["pageTemplate"].Allocate("PRESCRIPT", getScriptHeader());
                template["pageTemplate"].Allocate("POSTSCRIPT", "<script language=\"javascript\">");

                //old tags... not is necesary...
                if (template["pageTemplate"].ContainsKey("ONLOAD"))
                    template["pageTemplate"].Allocate("ONLOAD", "__start()");
                
                if (template["pageTemplate"].ContainsKey("INIT_FORM"))
                    template["pageTemplate"].Allocate("INIT_FORM", "<form name=\"frm_" + this.GetType().Name + "\" id=\"frm_" + this.GetType().Name + "\" onSubmit=\"return false;\" method=\"POST\" action=\"" + this.PageUrl + "\">");

                if (template["pageTemplate"].ContainsKey("END_FORM"))
                    template["pageTemplate"].Allocate("END_FORM", "</form>");
            }
            catch { }
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