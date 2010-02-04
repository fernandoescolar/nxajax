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
    /// In the Template page requiereds the tags: "&lt;$PRESCRIPT$&gt;" and "&lt;$POSTCRIPT$&gt;" in order to works well.
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
                tempRender = "<html xmlns=\"http://www.w3.org/1999/xhtml\" xml:lang=\"en\">" + tempRender;
                htmlPos = tempRender.ToLower().IndexOf("</html>");
                if (htmlPos < 0)
                    tempRender += "</html>";
                htmlPos = tempRender.ToLower().IndexOf("<html");
            }
            htmlPos = tempRender.IndexOf(">", htmlPos);
            int headPos = tempRender.ToLower().IndexOf("<head>");
            if (headPos < 0)
                tempRender = tempRender.Insert(htmlPos + 1, "<html xmlns=\"http://www.w3.org/1999/xhtml\" xml:lang=\"en\"><head>" + getScriptHeader() + "</head>");
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
            int formPos1 = tempRender.ToLower().IndexOf("<form ");
            int formPos2 = tempRender.IndexOf(">", formPos1);
            if (formPos1 >= 0 && formPos2 >= 0)
            {
                tempRender = tempRender.Remove(formPos1, formPos2 - formPos1 + 1);
                tempRender = tempRender.Insert(formPos1, getFormHtmlBegin());
            }
            else
            {
                Response.Clear();
                Response.Write(nxAjax.Properties.Resources.FormTagError);
                Response.End();
            }
            formPos1 = tempRender.ToLower().IndexOf(getFormHtmlEnd());
            if (formPos1 >= 0)
            {
                tempRender = tempRender.Insert(formPos1 + 7, getPostScript());
                code = "";
            }
            else
            {
                Response.Clear();
                Response.Write(nxAjax.Properties.Resources.CloseFormTagError);
                Response.End();
            }
            writer.Write(tempRender);
        }
        protected override string getPostScript()
        {
            return "<script type=\"text/javascript\"> $(document).ready(function(){ __start(); " + base.getPostScript() + "}); </script>";
        }
        protected override void fillTemplateSpecialTag()
        {
            base.fillTemplateSpecialTag();
            try
            {
                template["pageTemplate"].Allocate("PRESCRIPT", getScriptHeader());

                //old tags... not is necesary...
                if (template["pageTemplate"].ContainsValueKey("ONLOAD"))
                    template["pageTemplate"].Allocate("ONLOAD", "__start()");

                if (template["pageTemplate"].ContainsValueKey("INITFORM"))
                    template["pageTemplate"].Allocate("INITFORM", getFormHtmlBegin());

                if (template["pageTemplate"].ContainsValueKey("ENDFORM"))
                    template["pageTemplate"].Allocate("ENDFORM", getFormHtmlEnd());
            }
            catch { }
        }

		private string getScriptHeader()
		{
            string code = "";
            string url = "nxScriptResource.axd";
            //code += "<script type=\"text/javascript\" src=\"" + url + "?src=" + EncodeName(Session.SessionID + "res.jquery-1.3.2.min.js") + "\"></script>" + "\n\t";
            //code += "<script type=\"text/javascript\" src=\"" + url + "?src=" + EncodeName(Session.SessionID + "res.jquery.cookie.js") + "\"></script>" + "\n\t";
            //code += "<script type=\"text/javascript\" src=\"" + url + "?src=" + EncodeName(Session.SessionID + "res.jquery.ajaxqueue.js") + "\"></script>" + "\n\t";
            //code += "<script type=\"text/javascript\" src=\"" + url + "?src=" + EncodeName(Session.SessionID + "res.jquery.datepick.pack.js") + "\"></script>" + "\n\t";
            //code += "<script type=\"text/javascript\" src=\"" + url + "?src=" + EncodeName(Session.SessionID + "res.jquery.treeview.pack.js") + "\"></script>" + "\n\t";
            //code += "<script type=\"text/javascript\" src=\"" + url + "?src=" + EncodeName(Session.SessionID + "res.jquery.history.js") + "\"></script>" + "\n\t";
            //code += "<script type=\"text/javascript\" src=\"" + url + "?src=" + EncodeName(Session.SessionID + "res.jquery.common.js") + "\"></script>" + "\n\t";
            //code += "<script type=\"text/javascript\" src=\"" + url + "?src=" + EncodeName(Session.SessionID + "res.jquery.nxApplication.js") + "\"></script>" + "\n\t";
            //code += "<script type=\"text/javascript\" src=\"" + url + "?src=" + EncodeName(Session.SessionID + "res.jquery.ajaxupload.js") + "\"></script>" + "\n\t";
            //code += "<script type=\"text/javascript\" src=\"" + url + "?src=" + EncodeName(Session.SessionID + "res.nxTextBox.js") + "\"></script>" + "\n\t";
            //code += "<script type=\"text/javascript\" src=\"" + url + "?src=" + EncodeName(Session.SessionID + "res.jquery.wysiwyg.js") + "\"></script>" + "\n\t";
            //code += "<script type=\"text/javascript\" src=\"" + url + "?src=" + EncodeName(Session.SessionID + "res.nxEditable.js") + "\"></script>" + "\n\t";
            //code += "<script type=\"text/javascript\" src=\"" + url + "?src=" + EncodeName(Session.SessionID + "res.nxDragnDrop.js") + "\"></script>" + "\n\t";
            Version v = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
            code += "<script type=\"text/javascript\" src=\"" + url + "?src=AjaxScripts&v=" + v.ToString() + "\"></script>" + "\n\t";
            code += "<script type=\"text/javascript\">" + "\n\t";
            code += "var initialized = false;" + "\n\t";
			code += "function __start()" + "\n\t";
			code += "{" + "\n\t";
            code += "	if(!initialized){ $.nxApplication.Init('loading'); $.historyInit($.nxApplication.pageload);}" + "\n\t";
			code += "}" + "\n\t";
			code += "</script>" + "\n";
			return code;
		}
        //private string EncodeName(string name)
        //{
        //    byte[] encData_byte = new byte[name.Length];
        //    encData_byte = System.Text.Encoding.UTF8.GetBytes(name);    
        //    string encodedData = Convert.ToBase64String(encData_byte);
        //    return encodedData;
        //}
	}
}
