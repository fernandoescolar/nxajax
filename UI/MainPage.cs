/*
 * Licensing
 *  Mozilla Public License 1.1 (MPL 1.1)
 * 
 * Fernando Escolar Martínez-Berganza <fer.escolar@gmail.com>
 * 
 */

using System;
using Framework.Ajax.UI.Controls;

namespace Framework.Ajax.UI
{
	/// <summary>
	/// MainPage is the main container page (or master page). It provides all javascripts and the engine to nxAjax framework works.
    /// <code>
    /// In the Template page requiereds the tags: "&lt;$PRESCRIPT$&gt;" and "&lt;$POSTCRIPT$&gt;" in order to works well.
    /// </code>
	/// </summary>
	public class MainPage : AjaxPage
	{
        /// <summary>
        /// MainPage Constructor
        /// </summary>
		public MainPage() : base() { }

		protected override void OnLoad(EventArgs e)
		{
			foreach(AjaxControl ctrl in AjaxController.AjaxControls)
				ctrl.AjaxNotUpdate();
		
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
                tempRender = tempRender.Insert(htmlPos + 1, "<html xmlns=\"http://www.w3.org/1999/xhtml\" xml:lang=\"en\"><head>" + GetScriptHeader() + "</head>");
            else
                tempRender = tempRender.Insert(headPos + 6, GetScriptHeader());
            /*Form and postscript*/
            int formPos1 = tempRender.ToLower().IndexOf("<form ");
            int formPos2 = tempRender.IndexOf(">", formPos1);
            if (formPos1 >= 0 && formPos2 >= 0)
            {
                tempRender = tempRender.Remove(formPos1, formPos2 - formPos1 + 1);
                tempRender = tempRender.Insert(formPos1, GetFormHtmlBegin());
            }
            else
            {
                Response.Clear();
                Response.Write(Framework.Ajax.Properties.Resources.FormTagError);
                Response.End();
            }
            formPos1 = tempRender.ToLower().IndexOf(GetFormHtmlEnd());
            if (formPos1 >= 0)
            {
                tempRender = tempRender.Insert(formPos1 + 7, GetPostScript());
            }
            else
            {
                Response.Clear();
                Response.Write(Framework.Ajax.Properties.Resources.CloseFormTagError);
                Response.End();
            }
            writer.Write(tempRender);
        }
        protected override string GetPostScript()
        {
            return "<script type=\"text/javascript\"> $(document).ready(function(){ __start(); " + base.GetPostScript() + "}); </script>";
        }
        protected override void fillTemplateSpecialTag()
        {
            base.fillTemplateSpecialTag();
            try
            {
                template["pageTemplate"].Allocate("PRESCRIPT", GetScriptHeader());

                //old tags... not is necesary...
                if (template["pageTemplate"].ContainsValueKey("ONLOAD"))
                    template["pageTemplate"].Allocate("ONLOAD", "__start()");

                if (template["pageTemplate"].ContainsValueKey("INITFORM"))
                    template["pageTemplate"].Allocate("INITFORM", GetFormHtmlBegin());

                if (template["pageTemplate"].ContainsValueKey("ENDFORM"))
                    template["pageTemplate"].Allocate("ENDFORM", GetFormHtmlEnd());
            }
            catch { }
        }

		private string GetScriptHeader()
		{
            using (AjaxTextWriter writer = new AjaxTextWriter())
            {
                ajaxController.WriteScriptHeader(writer);
                return writer.ToString();
            }
		}
	}
}
