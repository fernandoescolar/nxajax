/*
 * Licensing
 *  Mozilla Public License 1.1 (MPL 1.1)
 * 
 * Fernando Escolar Martínez-Berganza <fer.escolar@gmail.com>
 * 
 */

using System;
using System.Web;

/// <summary>
/// nxAjax framework Handlers
/// </summary>
namespace nxAjax.HttpHandlers
{
    /// <summary>
    /// Script Resource Handler: Gets internal javascripts from nxAjax assembly
    /// </summary>
    public class ScriptResourceHandler : System.Web.IHttpHandler
    {
        /// <summary>
        /// Gets if the Handler is reusable
        /// </summary>
        public bool IsReusable
        {
            get
            {
                return true;
            }
        }
        /// <summary>
        /// Process an internal javascript resource request
        /// </summary>
        /// <param name="context">Current http context</param>
        public void ProcessRequest(HttpContext context)
        {
            HttpResponse Response = context.Response;
            HttpRequest Request = context.Request;

            Response.ContentType = "application/x-javascript";

            if (Request.QueryString["src"] != null)
            {
                string script = DecodeName(Request.QueryString["src"]);
                if (context.Session != null)
                    script = script.Replace(context.Session.SessionID, "");
                else
                {
                    int position = script.LastIndexOf("res.");
                    script = script.Substring(position, script.Length - position);
                }
                switch (script)
                {
                    case "res.jquery-1.3.2.min.js":
                    case "res.jquery.cookie.js":
                    case "res.jquery.ajaxqueue.js":
                    case "res.jquery.datepick.pack.js":
                    case "res.jquery.treeview.pack.js":
                    case "res.jquery.history.js":
                    case "res.jquery.common.js":
                    //case "res.nxAjax.js":
                    case "res.jquery.nxApplication.js":
                    case "res.jquery.ajaxupload.js":
                    case "res.nxTextBox.js":
                    //case "res.nxAnimation.js":
                    case "res.jquery.wysiwyg.js":
                    //case "res.nxHTMLEditor.js":
                    //case "res.nxDatePicker.js":
                    //case "res.nxTree.js":
                    case "res.nxEditable.js":
                    case "res.nxDragnDrop.js":
                        System.IO.Stream reader = System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream("nxAjax." + script);
                        //byte[] buffer = new byte[1024];
                        //int read = 0;
                        //do
                        //{
                        //    read = reader.Read(buffer, 0, buffer.Length);
                        //    char [] cArray= System.Text.Encoding.UTF8.GetString(buffer).ToCharArray();
                        //    if (read>0)
                        //        writer.Write(cArray, 0, cArray.Length);
                        //}while(read>0);
                        byte[] buffer = new byte[reader.Length];
                        reader.Read(buffer, 0, buffer.Length);
                        Response.Write(System.Text.Encoding.UTF8.GetString(buffer));
                        return;
                    default:
                        Response.End();
                        return;
                }
            }
        }

        /// <summary>
        /// Decode a resource name
        /// </summary>
        /// <param name="name">encoded name</param>
        /// <returns>decoded name</returns>
        private string DecodeName(string name)
        {
            System.Text.UTF8Encoding encoder = new System.Text.UTF8Encoding();
            System.Text.Decoder utf8Decode = encoder.GetDecoder();

            byte[] todecode_byte = Convert.FromBase64String(name);
            int charCount = utf8Decode.GetCharCount(todecode_byte, 0, todecode_byte.Length);
            char[] decoded_char = new char[charCount];
            utf8Decode.GetChars(todecode_byte, 0, todecode_byte.Length, decoded_char, 0);
            string result = new String(decoded_char);
            return result;
        }


    }
}
