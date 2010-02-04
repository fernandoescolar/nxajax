/*
 * Licensing
 *  Mozilla Public License 1.1 (MPL 1.1)
 * 
 * Fernando Escolar Martínez-Berganza <fer.escolar@gmail.com>
 * 
 */

using System;
using System.Web;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Text;

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
        private const bool DO_GZIP = true;
        private readonly static TimeSpan CACHE_DURATION = TimeSpan.FromDays(30);
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

            // Read setName, contentType and version. All are required. They are
            // used as cache key
            string setName = Request["src"] ?? string.Empty;
            string contentType = Request["t"] ?? "application/x-javascript";
            string version = Request["v"] ?? string.Empty;

            Response.ContentType = "application/x-javascript";

            // Decide if browser supports compressed response
            bool isCompressed = DO_GZIP && this.CanGZip(context.Request);

            // Response is written as UTF8 encoding. If you are using languages like
            // Arabic, you should change this to proper encoding 
            UTF8Encoding encoding = new UTF8Encoding(false);

            // If the set has already been cached, write the response directly from
            // cache. Otherwise generate the response and cache it
            if (!this.WriteFromCache(context, setName, version, isCompressed, contentType))
            {
                using (MemoryStream memoryStream = new MemoryStream(5000))
                {
                    // Decide regular stream or GZipStream based on whether the response
                    // can be cached or not
                    using (Stream writer = isCompressed ? (Stream)(new GZipStream(memoryStream, CompressionMode.Compress)) : memoryStream)
                    {
                        string[] fileNames;
                        if (setName == "AjaxScripts")
                        {
                            //Loads default nxAjax scripts
                            fileNames = new string[] {  "res.jquery-1.3.2.min.js",
                                                        "res.jquery.cookie.js",
                                                        "res.jquery.ajaxqueue.js",
                                                        "res.jquery.datepick.pack.js",
                                                        "res.jquery.treeview.pack.js",
                                                        "res.jquery.history.js",
                                                        "res.jquery.common.js",
                                                        //"res.nxAjax.js",
                                                        "res.jquery.nxApplication.js",
                                                        "res.jquery.ajaxupload.js",
                                                        "res.nxTextBox.js",
                                                        //"res.nxAnimation.js",
                                                        "res.jquery.wysiwyg.js",
                                                        //"res.nxHTMLEditor.js",
                                                        //"res.nxDatePicker.js",
                                                        //"res.nxTree.js",
                                                        "res.nxEditable.js",
                                                        "res.nxDragnDrop.js" 
                                                    };
                            foreach (string fileName in fileNames)
                            {
                                System.IO.Stream reader = System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream("nxAjax." + fileName);
                                byte[] buffer = new byte[reader.Length];
                                reader.Read(buffer, 0, buffer.Length);
                                writer.Write(buffer, 0, buffer.Length);
                                reader.Close();
                            }
                        }
                        else
                        {
                            // Load the files defined in <appSettings> and process each file
                            string setDefinition = System.Configuration.ConfigurationSettings.AppSettings[setName] ?? "";
                            fileNames = setDefinition.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                            foreach (string fileName in fileNames)
                            {
                                byte[] fileBytes = this.GetFileBytes(context, fileName.Trim(), encoding);
                                writer.Write(fileBytes, 0, fileBytes.Length);
                            }
                        }

                        writer.Close();
                    }

                    // Cache the combined response so that it can be directly written
                    // in subsequent calls 
                    byte[] responseBytes = memoryStream.ToArray();
                    context.Cache.Insert(GetCacheKey(setName, version, isCompressed),
                        responseBytes, null, System.Web.Caching.Cache.NoAbsoluteExpiration,
                        CACHE_DURATION);

                    // Generate the response
                    this.WriteBytes(responseBytes, context, isCompressed, contentType);
                }
            }
            //if (Request.QueryString["src"] != null)
            //{
            //    string script = DecodeName(Request.QueryString["src"]);
            //    if (context.Session != null)
            //        script = script.Replace(context.Session.SessionID, "");
            //    else
            //    {
            //        int position = script.LastIndexOf("res.");
            //        script = script.Substring(position, script.Length - position);
            //    }
            //    switch (script)
            //    {
            //        case "res.jquery-1.3.2.min.js":
            //        case "res.jquery.cookie.js":
            //        case "res.jquery.ajaxqueue.js":
            //        case "res.jquery.datepick.pack.js":
            //        case "res.jquery.treeview.pack.js":
            //        case "res.jquery.history.js":
            //        case "res.jquery.common.js":
            //        //case "res.nxAjax.js":
            //        case "res.jquery.nxApplication.js":
            //        case "res.jquery.ajaxupload.js":
            //        case "res.nxTextBox.js":
            //        //case "res.nxAnimation.js":
            //        case "res.jquery.wysiwyg.js":
            //        //case "res.nxHTMLEditor.js":
            //        //case "res.nxDatePicker.js":
            //        //case "res.nxTree.js":
            //        case "res.nxEditable.js":
            //        case "res.nxDragnDrop.js":
            //            System.IO.Stream reader = System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream("nxAjax." + script);
            //            //byte[] buffer = new byte[1024];
            //            //int read = 0;
            //            //do
            //            //{
            //            //    read = reader.Read(buffer, 0, buffer.Length);
            //            //    char [] cArray= System.Text.Encoding.UTF8.GetString(buffer).ToCharArray();
            //            //    if (read>0)
            //            //        writer.Write(cArray, 0, cArray.Length);
            //            //}while(read>0);
            //            byte[] buffer = new byte[reader.Length];
            //            reader.Read(buffer, 0, buffer.Length);
            //            Response.Write(System.Text.Encoding.UTF8.GetString(buffer));
            //            return;
            //        default:
            //            Response.End();
            //            return;
            //    }
            //}
        }

        ///// <summary>
        ///// Decode a resource name
        ///// </summary>
        ///// <param name="name">encoded name</param>
        ///// <returns>decoded name</returns>
        //private string DecodeName(string name)
        //{
        //    System.Text.UTF8Encoding encoder = new System.Text.UTF8Encoding();
        //    System.Text.Decoder utf8Decode = encoder.GetDecoder();

        //    byte[] todecode_byte = Convert.FromBase64String(name);
        //    int charCount = utf8Decode.GetCharCount(todecode_byte, 0, todecode_byte.Length);
        //    char[] decoded_char = new char[charCount];
        //    utf8Decode.GetChars(todecode_byte, 0, todecode_byte.Length, decoded_char, 0);
        //    string result = new String(decoded_char);
        //    return result;
        //}

        private byte[] GetFileBytes(HttpContext context, string virtualPath, Encoding encoding)
        {
            if (virtualPath.StartsWith("http://", StringComparison.InvariantCultureIgnoreCase))
            {
                using (WebClient client = new WebClient())
                {
                    return client.DownloadData(virtualPath);
                }
            }
            else
            {
                string physicalPath = context.Server.MapPath(virtualPath);
                byte[] bytes = File.ReadAllBytes(physicalPath);
                // TODO: Convert unicode files to specified encoding. For now, assuming
                // files are either ASCII or UTF8
                return bytes;
            }
        }

        private bool WriteFromCache(HttpContext context, string setName, string version, bool isCompressed, string contentType)
        {
            byte[] responseBytes = context.Cache[GetCacheKey(setName, version, isCompressed)] as byte[];

            if (null == responseBytes || 0 == responseBytes.Length) return false;

            this.WriteBytes(responseBytes, context, isCompressed, contentType);
            return true;
        }

        private void WriteBytes(byte[] bytes, HttpContext context, bool isCompressed, string contentType)
        {
            HttpResponse response = context.Response;

            response.AppendHeader("Content-Length", bytes.Length.ToString());
            response.ContentType = contentType;
            if (isCompressed)
                response.AppendHeader("Content-Encoding", "gzip");

            context.Response.Cache.SetCacheability(HttpCacheability.Public);
            context.Response.Cache.SetExpires(DateTime.Now.Add(CACHE_DURATION));
            context.Response.Cache.SetMaxAge(CACHE_DURATION);
            context.Response.Cache.AppendCacheExtension("must-revalidate, proxy-revalidate");

            response.OutputStream.Write(bytes, 0, bytes.Length);
            response.Flush();
        }

        private bool CanGZip(HttpRequest request)
        {
            string acceptEncoding = request.Headers["Accept-Encoding"];
            if (!string.IsNullOrEmpty(acceptEncoding) &&
                 (acceptEncoding.Contains("gzip") || acceptEncoding.Contains("deflate")))
                return true;
            return false;
        }

        private string GetCacheKey(string setName, string version, bool isCompressed)
        {
            return "ScriptResourceHandler." + setName + "." + version + "." + isCompressed;
        }

    }
}
