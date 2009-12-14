/*
 * Licensing
 *  Mozilla Public License 1.1 (MPL 1.1)
 * 
 * Fernando Escolar Martínez-Berganza <fer.escolar@gmail.com>
 * 
 */
using System;
using System.Web;
using System.Web.UI;
using System.Drawing;
using System.ComponentModel;
using System.Collections.Generic;
using System.Security.Permissions;

namespace nxAjax.UI.Controls
{
    /// <summary>
    /// FileUpload TextBox Control
    /// <code>
    /// &lt;ajax:FileUpload runat="server"&gt;&lt;/ajax:FileUpload&gt;
    /// </code>
    /// </summary>
    [AspNetHostingPermission(SecurityAction.Demand, Level = AspNetHostingPermissionLevel.Minimal)]
    [AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    [Designer("nxAjax.UI.Design.nxControlDesigner")]
    [ToolboxData("<{0}:FileUpload runat=\"server\"></{0}:FileUpload>")]
    [ToolboxItem(true)]
    [ToolboxBitmap(typeof(System.Web.UI.WebControls.FileUpload))]
    public class FileUpload : nxContainerControl
    {
        #region Private attributes
        protected string mUploadingMessage;
        protected string badExtensionMessage;
        protected List<string> extensions;
        protected string jsonChange, jsonComplete, jsonSubmit;
        #endregion
        #region Public Javascript Client Events
        /// <summary>
        /// Gets/Sets Javascript onchange event code
        /// <remarks>Is a client event</remarks>
        /// </summary>
        [Category("Action"), DefaultValue(""), Description("OnClick javascript event in client.")]
        public string ClientChange
        {
            get
            {
                return jsonChange;
            }
            set
            {
                jsonChange = value;
            }
        }
        /// <summary>
        /// Gets/Sets Javascript on Control finish upload file
        /// <remarks>Is a client event</remarks>
        /// </summary>
        [Category("Action"), DefaultValue(""), Description("OnFocus javascript event in client.")]
        public string ClientComplete
        {
            get
            {
                return jsonComplete;
            }
            set
            {
                jsonComplete = value;
            }
        }
        /// <summary>
        /// Gets/Sets Javascript on submit file
        /// <remarks>Is a client event</remarks>
        /// </summary>
        [Category("Action"), DefaultValue(""), Description("OnBlur javascript event in client.")]
        public string ClientSubmit
        {
            get
            {
                return jsonSubmit;
            }
            set
            {
                jsonSubmit = value;
            }
        }
        #endregion
        #region Public Server Events
        /// <summary>
        /// Raises on Control Value change
        /// <remarks>Is a server event</remarks>
        /// </summary>
        public event nxEventHandler ServerChange;
        /// <summary>
        /// Raises on Control finish upload file
        /// <remarks>Is a server event</remarks>
        /// </summary>
        public event nxEventHandler ServerComplete;
        /// <summary>
        /// Raises on Control submit file
        /// <remarks>Is a server event</remarks>
        /// </summary>
        public event nxEventHandler ServerSubmit;
        #endregion
        #region Public properties
        /// <summary>
        /// Gets Posted file
        /// </summary>
        public System.Web.HttpPostedFile File
        {
            get 
            {
                if (System.Web.HttpContext.Current.Session[ID] is System.Web.HttpPostedFile)
                    return (System.Web.HttpPostedFile)System.Web.HttpContext.Current.Session[ID];
                return null;
            }
        }

        /// <summary>
        /// Gets Allowed file extensions
        /// </summary>
        public List<string> AllowedExtensions
        {
            get { return extensions; }
        }

        /// <summary>
        /// Gets/Sets the extension not allowed alert message
        /// </summary>
        public string ExtensionNotAllowedMessage
        {
            get { return badExtensionMessage; }
            set { badExtensionMessage = value; }
        }

        /// <summary>
        /// Gets/Sets the uploading message
        /// </summary>
        public string UploadingMessage
        {
            get { return mUploadingMessage; }
            set { mUploadingMessage = value; }
        }

        /// <summary>
        /// Gets/Sets FileUpload text
        /// </summary>
        [Category("Appearance"), DefaultValue(""), Description("The text contained.")]
        public string Text
        {
            get { return InnerText; }
            set { InnerText = value; AjaxUpdate(); }
        }
        #endregion
        #region Factory
        /// <summary>
        /// Creates a new FileUpload control
        /// </summary>
        public FileUpload() : base("div") { this.PostBackMode = PostBackMode.Async;  extensions = new List<string>(); badExtensionMessage = mUploadingMessage = jsonChange = jsonComplete = jsonSubmit =""; }
        #endregion
        #region Render
        protected override void Render(HtmlTextWriter writer)
        {
            base.Render(writer);
        }
        
        /// <summary>
        /// Renders Control Javascript functions
        /// </summary>
        /// <param name="writer">javascript writer</param>
        public override void RenderJS(nxAjaxTextWriter writer)
        {
            base.RenderJS(writer);
            if (!nxPage.IsPostBack)
            {
                writer.Write("new AjaxUpload('#" + ID + "', {");
                writer.Write("action: 'nxUploadFile.axd', ");
                writer.Write("name: '" + ID + "',");
                writer.Write("data: { id: '" + ID + "' },");
                //writer.Write("autoSubmit: true, ");
                //writer.Write("responseType: false, ");

                string exts = string.Empty;
                if (extensions.Count > 0)
                    foreach (string aux in extensions)
                        exts += ((exts == string.Empty) ? "" : "|") + aux;

                //On change
                writer.Write("onChange: function(file, extension){ ");
                if (!string.IsNullOrEmpty(jsonChange))
                    writer.Write(jsonChange + ";");
                if (extensions.Count > 0)
                {
                    writer.Write("if (! (extension && /^(" + exts + ")$/.test(extension))) {");
                    writer.Write(" alert(\"" + badExtensionMessage.Replace("'", "\\'").Replace("\"", "\\\"").Replace("\n", "\\n").Replace("\r", "\\r") + "\"); ");
                    writer.Write(" return false; ");
                    writer.Write("} ");
                }
                if (ServerChange != null)
                    writer.Write(nxPage.GetPostBackWithValueAjaxEvent(this, "onchange", "file + '.' + extension"));
                
                writer.Write("}, ");

                //On complete
                writer.Write("onComplete: function(file, extension){ this.enable(); $('#" + ID + "').html(file + '.' + extension);");
                if (!string.IsNullOrEmpty(jsonComplete))
                    writer.Write(jsonComplete + ";");

                if (ServerComplete != null)
                    writer.Write(nxPage.GetPostBackWithValueAjaxEvent(this, "oncomplete", "file + '.' + extension"));

                if (! string.IsNullOrEmpty(LoadingImgID))
                    writer.Write(" if ($('#" + LoadingImgID + "').exists()) $('#" + LoadingImgID + "').fadeOut('slow'); ");
               
                writer.Write(" }, ");

                //On submit
                writer.Write("onSubmit: function(file, extension){ this.disable(); ");
                
                if (!string.IsNullOrEmpty(jsonSubmit))
                    writer.Write(jsonSubmit + ";");

                if (!string.IsNullOrEmpty(mUploadingMessage))
                    writer.Write("$('#" + ID + "').html(' " + mUploadingMessage.Replace("'", "\\'").Replace("\"", "\\\"").Replace("\n", "\\n").Replace("\r", "\\r") + "');");
                
                if (!string.IsNullOrEmpty(LoadingImgID))
                    writer.Write("if ($('#" + LoadingImgID + "').exists()) $('#" + LoadingImgID + "').fadeIn('slow'); ");

                if (ServerSubmit != null)
                    writer.Write(nxPage.GetPostBackWithValueAjaxEvent(this, "onsubmit", "file + '.' + extension"));

                
                writer.Write("} });");
            }
            //if (nxPage.IsPostBack && hasChanged)
            //{
            //    if (Disabled)
            //        writer.Write("$('#" + ID + "').disable();");
            //    else
            //        writer.Write("$('#" + ID + "').enable();");
            //}
        }
        #endregion
        #region Overrides Base Methos
        public override void RaiseEvent(string action, string value)
        {
            PutPostValue(value);
            switch (action.ToLower())
            {
                case "onchange":
                    ServerChange(this, value);
                    break;
                case "oncomplete":
                    ServerComplete(this, value);
                    break;
                case "onsubmit":
                    ServerSubmit(this, value);
                    break;
            }
        }

        protected override void LoadViewState(object savedState)
        {
            object[] state = (object[])(savedState);
            base.LoadViewState(state[0]);
            extensions = (List<string>)state[1];
            badExtensionMessage = (string)state[2];
            mUploadingMessage = (string)state[3];
            jsonComplete = (string)state[4];
            jsonChange = (string)state[5];
            jsonSubmit = (string)state[6];
        }

        protected override object SaveViewState()
        {
            object[] state = new object[7];
            state[0] = base.SaveViewState();
            state[1] = extensions;
            state[2] = badExtensionMessage;
            state[3] = mUploadingMessage;
            state[4] = jsonComplete;
            state[5] = jsonChange;
            state[6] = jsonSubmit;
            return state;
        }

		public override void PutPostValue(string obj)
		{
            this.Text = obj;
        }
        #endregion
    }
}
