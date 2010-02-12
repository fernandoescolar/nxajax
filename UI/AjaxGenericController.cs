using System;
using System.Web;
using System.Collections.Generic;

using Framework.Ajax.UI.Controls;

namespace Framework.Ajax.UI
{
    /// <summary>
    /// Ajax Server Logic Functions Provider
    /// </summary>
    internal class AjaxGenericController: IAjaxController
    {
        #region Private Attributes
        private AjaxControlCollection containedControls;
        private AjaxUserControlCollection containedUserControls;
        private Language ilang;
        private ViewStateMode vwStateMode;
        private System.Web.UI.Page parentPage;
        private string code;
        private bool isAjaxPostback;
        #endregion
        #region Public Properties
        /// <summary>
        /// Get contained nxControlCollection
        /// </summary>
        public AjaxControlCollection AjaxControls
        {
            get
            {
                return containedControls;
            }
        }
        /// <summary>
        /// Get contained AjaxUserControlCollection
        /// </summary>
        public AjaxUserControlCollection AjaxUserControls
        {
            get
            {
                return containedUserControls;
            }
        }
        /// <summary>
        /// Get the AjaxPage url
        /// </summary>
        public string PageUrl
        {
            get
            {
                string[] aux = System.Web.HttpContext.Current.Request.Url.ToString().Split('?');
                return aux[0];
            }
        }
        /// <summary>
        /// Gets the current language object
        /// </summary>
        public Language Lang
        {
            get { return ilang; }
        }
        /// <summary>
        /// Gets/Sets ViewState Storage Mode
        /// </summary>
        public ViewStateMode ViewStateMode
        {
            get { return vwStateMode; }
            set { 
                if (value != ViewStateMode.Session)
                    throw new NotImplementedException("Only \"ViewStateMode.Session\". Wait for next releases :D");
                vwStateMode = value; 
            }
        }
        /// <summary>
        /// Gets the parent page
        /// </summary>
        public System.Web.UI.Page Page
        {
            get { return parentPage; }
            internal set { 
                parentPage = value;
                //foreach (AjaxControl c in containedControls)
                //    c.Page = parentPage;
            }
        }
        /// <summary>
        /// Gets if it is an Ajax Postback
        /// </summary>
        public bool IsPostBack
        {
            get
            {
                return isAjaxPostback;
            }
        }
        #endregion
        #region Factory
        public AjaxGenericController(System.Web.UI.Page parentPage) : this()
        {
            this.parentPage = parentPage;
        }
        internal AjaxGenericController()
        {
            this.containedControls = new AjaxControlCollection();
            this.containedUserControls = new AjaxUserControlCollection();
            this.vwStateMode = ViewStateMode.Session;
            this.code = string.Empty;
        }
        #endregion
        #region Methods
        #region Protected
        /// <summary>
        /// Loads loaded Language object
        /// </summary>
        protected virtual void LoadLanguage()
        {

            if (ilang == null && Page.Session["Language"] != null)
                if (Page.Session["Language"].GetType() == typeof(Language))
                    ilang = (Language)Page.Session["Language"];
            if (ilang == null && Page.Application["Language"] != null)
                if (Page.Application["Language"].GetType() == typeof(Language))
                    ilang = (Language)Page.Application["Language"];
        }
        /// <summary>
        /// Check ViewStateMode and enables it
        /// </summary>
        protected void CheckViewState()
        {
            if (vwStateMode == ViewStateMode.Session)
            {
                if (Page.Session["__viewState__" + Page.Session.SessionID] == null)
                    Page.Session["__viewState__" + Page.Session.SessionID] = new System.Collections.Hashtable();

                System.Collections.Hashtable viewState = (Page.Session["__viewState__" + Page.Session.SessionID] as System.Collections.Hashtable);
                if (!viewState.ContainsKey(Page.GetType().Name))
                    viewState.Add(Page.GetType().Name, new System.Collections.Hashtable());
            }
            else if (vwStateMode == ViewStateMode.Cache)
            {
                System.Collections.Hashtable viewState = (Page.Cache["__viewState__" + Page.Session.SessionID] as System.Collections.Hashtable);
                if (viewState == null)
                    viewState = new System.Collections.Hashtable();

                Page.Cache.Add("__viewState__" + Page.Session.SessionID,
                                    new System.Collections.Hashtable(),
                                    null,
                                    DateTime.Now.AddMinutes(Page.Session.Timeout),
                                    System.Web.Caching.Cache.NoSlidingExpiration,
                                    System.Web.Caching.CacheItemPriority.Default,
                                    null);
                if (!viewState.ContainsKey(Page.GetType().Name))
                    viewState.Add(Page.GetType().Name, new System.Collections.Hashtable());
            }
        }
        #endregion
        #region Management
        /// <summary>
        /// Search a Control
        /// </summary>
        /// <param name="id">control ID</param>
        /// <returns></returns>
        public System.Web.UI.Control FindControl(string id)
        {
            foreach (System.Web.UI.Control c in Page.Controls)
            {
                if (c.ID == id)
                    return c;
                System.Web.UI.Control aux = FindControl(id, c);
                if (aux != null)
                    return aux;
                if (c is IChildAjaxControlContainer)
                    aux = (c as IChildAjaxControlContainer).FindInnerControl(id);
                if (aux != null)
                    return aux;
            }
            return null;
        }
        /// <summary>
        /// Search a Control
        /// </summary>
        /// <param name="id">Control ID</param>
        /// <param name="root">Container Control</param>
        /// <returns></returns>
        public System.Web.UI.Control FindControl(string id, System.Web.UI.Control root)
        {
            foreach (System.Web.UI.Control c in root.Controls)
            {
                if (c.ID == id)
                    return c;
                System.Web.UI.Control aux = FindControl(id, c);
                if (aux != null)
                    return aux;
                if (c is IChildAjaxControlContainer)
                    aux = (c as IChildAjaxControlContainer).FindInnerControl(id);
                if (aux != null)
                    return aux;
            }
            return null;
        }
        /// <summary>
        /// Add a new control
        /// </summary>
        /// <param name="control"></param>
        public void AddControl(System.Web.UI.Control control)
        {
            if (control.Controls.Count > 0 && !(control is IChildAjaxControlContainer))
                foreach (System.Web.UI.Control c in control.Controls)
                    AddControl(c);

            control.Page = this.Page;
            if (control is AjaxControl)
                containedControls += (AjaxControl)control;
            if (control is AjaxUserControl)
                containedUserControls += (AjaxUserControl)control;
        }
        /// <summary>
        /// Initialize Ajax Provider
        /// </summary>
        public void Init()
        {
            CheckPostback();
            LoadLanguage();
            CheckViewState();
        }
        /// <summary>
        /// Load Ajax provider
        /// </summary>
        public void Load()
        {
            foreach (AjaxUserControl ctrl in containedUserControls)
                ctrl.OnLoad(new EventArgs());
        }
        public void CheckPostback()
        {
            isAjaxPostback = (Page.Request.Form["__id"] != null);
        }
        public void AjaxRequestProcess()
        {
            HttpRequest request = Page.Request;
            
            if (Page != null)
            {
                string id = request.Form["__id"];

                if (id != null)
                {
                    System.Web.UI.Control c = FindControl(id);

                    foreach (AjaxControl ac in AjaxControls)
                        ac.AjaxNotUpdate();

                    if (c is ISubmit)
                    {
                        ISubmit s = (c as ISubmit);
                        foreach (string key in request.Form.Keys)
                        {
                            if (string.IsNullOrEmpty(key))
                                continue;
                            if (key == "__id")
                                continue;
                            foreach (AjaxControl ctrl in AjaxControls)
                            {
                                if (ctrl.ID == key)
                                {
                                    ctrl.PutPostValue(request.Form[key]);
                                    break;
                                }
                                else if (ctrl is IChildAjaxControlContainer && key.Contains(ctrl.ID))
                                {
                                    IChildAjaxControlContainer iccc = ctrl as IChildAjaxControlContainer;
                                    AjaxControl otherControl = iccc.FindInnerControl(key);
                                    if (otherControl != null)
                                    {
                                        otherControl.PutPostValue(request.Form[key]);
                                        break;
                                    }
                                }
                            }
                        }
                        if (s is AjaxControl)
                        {
                            AjaxControl submitControl = (s as AjaxControl);
                            submitControl.RaiseEvent("onClick", s.Value);
                        }
                    }
                    else if (c is AjaxControl)
                    {
                        AjaxControl ac = (c as AjaxControl);
                        ac.RaiseEvent(request.Form["__action"], request.Form["__value"]);
                    }
                }
            }
        }
        #endregion
        #region Render methods
        public void AjaxRender()
        {
            HttpResponse response = Page.Response;
            response.Clear();
            response.ContentType = "text/javascript";
            response.Write(GetPostbackJavascript());
            try
            {
                response.End();
            }
            catch (System.Threading.ThreadAbortException ex)
            {
                //Do nothing
            }
        }
        /// <summary>
        /// Writes nxAjax form tag begin
        /// </summary>
        /// <param name="id">Form Id</param>
        /// <param name="writer">Ajax Writer</param>
        public void WriteAjaxFormBegin(string id, AjaxTextWriter writer)
        {
            writer.WriteBeginTag("form");
            //Not used in XHMTL
            //writer.WriteAttribute("name", "frm_" + this.GetType().Name);
            writer.WriteAttribute("id", id);
            writer.WriteAttribute("onsubmit", "return false;");
            writer.WriteAttribute("method", "post");
            writer.WriteAttribute("action", this.PageUrl);
            writer.Write(AjaxTextWriter.TagRightChar);
        }
        /// <summary>
        /// Writes nxAjax form end tag
        /// </summary>
        /// <param name="writer">Ajax Writer</param>
        public void WriteAjaxFormEnd(AjaxTextWriter writer)
        {
            writer.WriteEndTag("form");
        }
        /// <summary>
        /// Writes Header Scripts
        /// </summary>
        /// <param name="writer">Ajax Writer</param>
        public void WriteScriptHeader(AjaxTextWriter writer)
        {
            string url = "AjaxScriptResource.axd";
            Version v = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;

            writer.WriteLine("\t<script type=\"text/javascript\" src=\"" + url + "?src=AjaxScripts&v=" + v.ToString() + "\"></script>");
            writer.WriteLine("\t<script type=\"text/javascript\">");
            writer.WriteLine("\tvar initialized = false;");
            writer.WriteLine("\tfunction __start() {");
            writer.WriteLine("\t\tif(!initialized){ $.nxApplication.Init('loading'); $.historyInit($.nxApplication.pageload);}");
            writer.WriteLine("\t}");
            writer.WriteLine("\t</script>");
        }
        /// <summary>
        /// Gets Postback javascript code
        /// </summary>
        /// <returns></returns>
        public string GetPostbackJavascript()
        {
            AjaxTextWriter jsWriter = new AjaxTextWriter();
            foreach (AjaxControl ctrl in containedControls)
            {
                ctrl.RenderJS(jsWriter);
            }

            if (!string.IsNullOrEmpty(code))
            {
                jsWriter.Write(code);
                code = string.Empty;
            }
            return jsWriter.ToString();
        }
        #endregion
        #region Javascript commands
        /// <summary>
        /// Execute an specified javascript code in the next callback response
        /// </summary>
        /// <param name="lines">Javascript code</param>
        public void ExecuteJavascript(string lines)
        {
            code += lines.Replace("\n", "\\n").Replace("\r", "\\r");
        }
        /// <summary>
        /// Shows an alert dialog in the next callback response
        /// </summary>
        /// <param name="msg"></param>
        public void DocumentAlert(string msg)
        {
            ExecuteJavascript("alert(\"" + msg.Replace("\n", "\\n").Replace("\r", "\\r").Replace("\"", "\\\"") + "\"); ");
        }
        /// <summary>
        /// Redirects to an specified url in the next callback
        /// </summary>
        /// <param name="url">url to redirect</param>
        public void DocumentRedirect(string url)
        {
            ExecuteJavascript("window.location.href = \"" + url + "\"; ");
        }
        #endregion
        #region Event Javascript Code Generator Methods
        /// <summary>
        /// Returns an nxAjax PostBack code for a control event
        /// </summary>
        /// <param name="ctrl">AjaxControl container</param>
        /// <param name="_event">name of the event</param>
        /// <returns>javascript code with the postback call</returns>
        public virtual string GetPostBackAjaxEvent(AjaxControl ctrl, string _event)
        {
            if (ctrl.PostBackMode == PostBackMode.Sync)
                return "$.nxApplication.DoPostBack('" + ctrl.ID + "', '" + _event + "', '" + ctrl.AjaxController.PageUrl + "', false, null);";
            else if (ctrl.PostBackMode == PostBackMode.Async)
                return "$.nxApplication.DoPostBack('" + ctrl.ID + "', '" + _event + "', '" + ctrl.AjaxController.PageUrl + "', true, '" + ctrl.ID + "_loading');";

            return "";
        }
        /// <summary>
        /// Returns an nxAjax PostBack code for a control event with an specified value
        /// </summary>
        /// <param name="ctrl">AjaxControl container</param>
        /// <param name="_event">name of the event</param>
        /// <param name="value">value to set</param>
        /// <returns>javascript code with the postback call</returns>
        public virtual string GetPostBackWithValueAjaxEvent(AjaxControl ctrl, string _event, string value)
        {
            if (ctrl.PostBackMode == PostBackMode.Sync)
                return "$.nxApplication.DoPostBackWithValue('" + ctrl.ID + "', '" + _event + "', '" + ctrl.AjaxController.PageUrl + "', " + value + ");";
            else if (ctrl.PostBackMode == PostBackMode.Async)
                return "$.nxApplication.DoAsyncPostBackWithValue('" + ctrl.ID + "', '" + _event + "', '" + ctrl.AjaxController.PageUrl + "', " + value + ", '" + ((ctrl.LoadingImgID != string.Empty) ? ctrl.LoadingImgID : ctrl.ID + "_loading") + "');";

            return "";
        }
        #endregion
        #endregion
    }
}
