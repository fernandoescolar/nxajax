using System;
using System.Web;
using System.Web.SessionState;
using System.IO;

namespace nxAjax.HttpHandlers
{
    public class FileUploadHandler : IHttpHandler, IRequiresSessionState
    {
        /// <summary>
        /// Gets if the Handler is reusable
        /// </summary>
        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
        /// <summary>
        /// Process an internal upload request
        /// </summary>
        /// <param name="context">Current http context</param>
        public void ProcessRequest(HttpContext context) 
        {
            //System.Threading.Thread.Sleep(5000);
            string controlID = context.Request.Form["id"].ToString();
            context.Session[controlID] = context.Request.Files[0];
            context.Response.ContentType = "text/plain"; 
            //context.Response.Write("success"); 
        } 
    }
}
