/*
 * Licensing
 *  Mozilla Public License 1.1 (MPL 1.1)
 * 
 * Fernando Escolar Martínez-Berganza <fer.escolar@gmail.com>
 * 
 */

using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Web.UI;

namespace nxAjax.UI
{
	/// <summary>
    /// nxAjax base control.
	/// </summary>
    public abstract class nxControl : System.Web.UI.Control, System.Web.UI.IAttributeAccessor
	{
        /// <summary>
        /// Has changed and if it is visible
        /// </summary>
		protected bool hasChanged, isVisible;
        /// <summary>
        /// Display Type
        /// </summary>
        protected DisplayType displayType;
        /// <summary>
        /// internal postback mode
        /// </summary>
        protected PostBackMode mPostBackMode;
        /// <summary>
        /// internal strings
        /// </summary>
        protected string mTagName, mLoadingImg, mLoadingImgID, mLoadingImgCssClass, originalID;
        /// <summary>
        /// internal attribute collection
        /// </summary>
        protected System.Web.UI.AttributeCollection mAttributes;
        private string htmlRenderedCache = string.Empty;

        /// <summary>
        /// ID name
        /// </summary>
        public override string ID
        {
            get
            {
                return base.ID;
            }
            set
            {
                if (string.IsNullOrEmpty(originalID) && !string.IsNullOrEmpty(base.ID))
                    originalID = base.ID;
                base.ID = value;
                if (!string.IsNullOrEmpty(mLoadingImg))
                    mLoadingImgID = ID + "_loading";
            }
        }
        /// <summary>
        /// Original ID name
        /// </summary>
        public string BaseID
        {
            get
            {
                return (string.IsNullOrEmpty(originalID))? ID : originalID;
            }
            internal set
            {
                originalID = value;
            }
        }

        /// <summary>
        /// Gets/Sets Postback mode (Sync|Async)
        /// </summary>
        [Category("Behavior"), DefaultValue(""), Description("Fix postback mode...")]
        public PostBackMode PostBackMode
        {
            get
            {
                return mPostBackMode;
            }
            set
            {
                mPostBackMode = value;
            }
        }

        /// <summary>
        /// Gets/Sets Disabled property
        /// </summary>
		[Category("Appearance"), DefaultValue(""), Description("Enable or Disable the Control.")]
        public bool Disabled
        {
            get
            {
                string disableAttr = Attributes["disabled"] as string;
                return (disableAttr != null);
            }
            set
            {
                if (!value)
                    Attributes.Remove("disabled");
                else
                    Attributes["disabled"] = "disabled";
                AjaxUpdate();
            }
        }

        /// <summary>
        /// Gets/Sets Visible Property
        /// </summary>
        [Category("Appearance"), DefaultValue(""), Description("Set if the Control is visible.")]
        public new bool Visible { get { return isVisible; } set { isVisible = value; this.AjaxUpdate(); } }

        /// <summary>
        /// Gets/Sets Display Property (notset, none, inline, block)
        /// </summary>
        [Category("Appearance"), DefaultValue(""), Description("Display Control property.")]
        public DisplayType Display { get { return displayType; } set { displayType = value; this.AjaxUpdate(); } }

        /// <summary>
        /// Gets/Sets css class name
        /// </summary>
        [Category("Appearance"), DefaultValue(""), Description("CSS class name.")]
        public string CssClass
        {
            get
            {
                string s = Attributes["class"];
                return (s == null) ? String.Empty : s;
            }
            set
            {
                if (value == null)
                    Attributes.Remove("class");
                else
                    Attributes["class"] = value;
                AjaxUpdate();
            }
        }

        /// <summary>
        /// Gets/Sets loading image src (on async postback)
        /// </summary>
        [Category("Appearance"), DefaultValue(""), Description("If is an async postback it will be showed...")]
        public string LoadingImg
        {
            get
            {
                return mLoadingImg;
            }
            set
            {
                mLoadingImg = value;
                LoadingImgID = ID + "_loading";
                AjaxUpdate();
            }
        }
        /// <summary>
        /// Gets/Sets loading image id (on async postback)
        /// </summary>
        [Category("Appearance"), DefaultValue(""), Description("If is an async postback it will be showed...")]
        public string LoadingImgID
        {
            get
            {
                return mLoadingImgID;
            }
            set
            {
                mLoadingImgID = value;
            }
        }
        /// <summary>
        /// Gets/Sets loading image css class name (on async postback)
        /// </summary>
        [Category("Appearance"), DefaultValue(""), Description("If is an async postback it will be showed...")]
        public string LoadingImgCssClass
        {
            get
            {
                return mLoadingImgCssClass;
            }
            set
            {
                mLoadingImgCssClass = value;
            }
        }

        /// <summary>
        /// Parent nxPage 
        /// </summary>
        public nxPage nxPage
		{
			get
			{
				return (nxPage)base.Page;
			}
			set
			{
				base.Page = value;
			}
		}
		
        /// <summary>
        /// Creates a new empty nxControl
        /// </summary>
		public nxControl():this("span") { }

        /// <summary>
        /// Creates a new empty nxControl with an specific tagname
        /// </summary>
        /// <param name="tagname">tag name</param>
        public nxControl(string tagname)
        {
			base.EnableViewState = true;
			isVisible = true;
            displayType = DisplayType.NotSet;
            mTagName = tagname;
            mPostBackMode = PostBackMode.Sync;
            mLoadingImg = string.Empty;
            mLoadingImgID = string.Empty;
            mLoadingImgCssClass = string.Empty;
            originalID = string.Empty;
		}

        //internal static string AttributeToString(int n)
        //{
        //    if (n != -1) return n.ToString();
        //    return null;
        //}

        //internal static string AttributeToString(string s)
        //{
        //    if (s != null && s.Length != 0) return s;
        //    return null;
        //}

        internal void PreProcessRelativeReference(System.Web.UI.HtmlTextWriter writer, string attribName)
        {
            string attr = Attributes[attribName];
            if (attr != null)
            {
                if (attr.Length != 0)
                {
                    try
                    {
                        attr = ResolveClientUrl(attr);
                    }
                    catch (Exception)
                    {
                        throw new System.Web.HttpException(attribName + " property had malformed url");
                    }
                    writer.WriteAttribute(attribName, attr, true);
                    Attributes.Remove(attribName);
                }
            }
        }

        /// <summary>
        /// Gets an attributte by name (Keep it Sync)
        /// </summary>
        /// <param name="name">name</param>
        /// <returns>attributte value</returns>
        protected virtual string GetAttribute(string name)
        {
            return Attributes[name];
        }

        /// <summary>
        /// Sets an attribute value by name (Keep it Sync)
        /// </summary>
        /// <param name="name">name</param>
        /// <param name="value">new value</param>
        protected virtual void SetAttribute(string name, string value)
        {
            Attributes[name] = value;
        }

        string System.Web.UI.IAttributeAccessor.GetAttribute(string name)
        {
            return Attributes[name];
        }

        void System.Web.UI.IAttributeAccessor.SetAttribute(string name, string value)
        {
            Attributes[name] = value;
        }

        /// <summary>
        /// Render a tag begin
        /// </summary>
        /// <param name="writer">HTML writer</param>
        internal protected virtual void RenderBeginTag(nxAjaxTextWriter writer)
        {
            writer.WriteBeginTag(TagName);
            RenderAttributes(writer);
            writer.Write(nxAjaxTextWriter.TagRightChar);

        }
        /// <summary>
        /// Renders the end of a tag
        /// </summary>
        /// <param name="writer">HTML writer</param>
        internal protected virtual void RenderEndTag(nxAjaxTextWriter writer)
        {
            writer.WriteEndTag(TagName);
        }

        /// <summary>
        /// Returns if the nxControl contains one event
        /// </summary>
        /// <param name="eventName">event name</param>
        /// <returns>if it has this event</returns>
        protected virtual bool hasEvent(string eventName)
        {
            return false;
        }
        /// <summary>
        /// Renders a client event
        /// </summary>
        /// <param name="writer">HTML writer</param>
        /// <param name="eventName">event name</param>
        protected void RenderClientEventAttribute(nxAjaxTextWriter writer, string eventName)
        {
            string myEvent = String.Empty;
            if (!string.IsNullOrEmpty(Attributes[eventName]))
            {
                myEvent = Attributes[eventName] + myEvent;
                Attributes.Remove(eventName);
            }

            if (Page != null && hasEvent(eventName))
            {
                myEvent += nxPage.GetPostBackAjaxEvent(this, eventName);
            }

            if (myEvent.Length > 0)
            {
                writer.WriteAttribute(eventName, myEvent, true);
                //writer.WriteAttribute("language", "javascript");
            }
        }
        /// <summary>
        /// Renders a client event
        /// </summary>
        /// <param name="writer">HTML writer</param>
        /// <param name="eventName">event name</param>
        /// <param name="value">event value</param>
        protected void RenderClientEventAttribute(nxAjaxTextWriter writer, string eventName, string value)
        {
            string myEvent = String.Empty;
            if (!string.IsNullOrEmpty(Attributes[eventName]))
            {
                myEvent = Attributes[eventName] + myEvent;
                Attributes.Remove(eventName);
            }

            if (Page != null && hasEvent(eventName))
            {
                myEvent += nxPage.GetPostBackWithValueAjaxEvent(this, eventName, value);
            }

            if (myEvent.Length > 0)
            {
                writer.WriteAttribute(eventName, myEvent, true);
                //writer.WriteAttribute("language", "javascript");
            }
        }
        /// <summary>
        /// Renders Loading image (on async postback)
        /// </summary>
        /// <param name="writer">HTML writer</param>
        protected void RenderLoadingImage(nxAjaxTextWriter writer)
        {
            LoadingImgID = ID + "_loading";

            writer.Write(" ");
            writer.WriteBeginTag("img");
            writer.WriteAttribute("id", ID + "_loading");
            writer.WriteAttribute("src", LoadingImg);
            writer.WriteAttribute("align", "absmiddle");
            writer.WriteAttribute("style", "display: none");
            writer.WriteAttribute("border", "0");
            if (mLoadingImgCssClass != string.Empty)
                writer.WriteAttribute("class", mLoadingImgCssClass);
            writer.Write(nxAjaxTextWriter.SelfClosingTagEnd);
        }
        /// <summary>
        /// Inherit Render method
        /// </summary>
        /// <param name="writer">HtmlTextWriter</param>
		protected override void Render(System.Web.UI.HtmlTextWriter writer)
		{
            if (string.IsNullOrEmpty(htmlRenderedCache))
            {
                nxAjaxTextWriter w = new nxAjaxTextWriter();
                RenderHTML(w);
                htmlRenderedCache = w.ToString();
            }
            writer.Write(htmlRenderedCache);
		}

        /// <summary>
        /// Renders Control HTML code
        /// </summary>
        /// <param name="writer">Tag Writer</param>
        public virtual void RenderHTML(nxAjaxTextWriter writer)
        {
            RenderBeginTag(writer);
            RenderEndTag(writer);
            if (PostBackMode == PostBackMode.Async && LoadingImg != string.Empty)
                RenderLoadingImage(writer);
            hasChanged = false;
            htmlRenderedCache = writer.ToString();
        }
        
        /// <summary>
        /// Renders Control Javascript functions
        /// </summary>
        /// <param name="writer">javascript writer</param>
        public virtual void RenderJS(nxAjaxTextWriter writer)
        {
            if (hasChanged || (!nxPage.IsPostBack && !isVisible))
                writer.Write("$('#" + ID + "').visible(" + isVisible.ToString().ToLower() + ");");

            if ((!nxPage.IsPostBack || hasChanged) && displayType != DisplayType.NotSet)
            {
                switch (displayType)
                {
                    case DisplayType.Block:
                        writer.Write("$('#" + ID + "').display('block');");
                        break;
                    case DisplayType.Inline:
                        writer.Write("$('#" + ID + "').display('inline');");
                        break;
                    case DisplayType.None:
                        writer.Write("$('#" + ID + "').display('none');");
                        break;
                }
            }

            if (nxPage.IsPostBack && hasChanged)
            {
                writer.Write("$('#" + ID + "').attr('class', \"" + CssClass + "\");");
                writer.Write("$('#" + ID + "').enabled(" + (!Disabled).ToString().ToLower() + ");");
            }
        }

        /// <summary>
        /// Renders Control Attributes HTML code part
        /// </summary>
        /// <param name="writer"></param>
        protected virtual void RenderAttributes(nxAjaxTextWriter writer)
        {
            if (ID != null)
            {
                writer.WriteAttribute("id", ID);
            }
            Attributes.Render(new HtmlTextWriter(writer.TextWriter));
        }
        
        /// <summary>
        /// Gets nxControl Attributtes
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public System.Web.UI.AttributeCollection Attributes
        {
            get
            {
                if (mAttributes == null)
                    mAttributes = new System.Web.UI.AttributeCollection(ViewState);
                return mAttributes;
            }
        }

        /// <summary>
        /// Gets nxControl Css Style collection
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public System.Web.UI.CssStyleCollection Style
        {
            get { return Attributes.CssStyle; }
        }

        /// <summary>
        /// Gets nxControl Tag Name
        /// </summary>
        [DefaultValue("")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual string TagName
        {
            get { return mTagName; }
        }

        /// <summary>
        /// ViewStateIgnoresCase
        /// </summary>
        protected override bool ViewStateIgnoresCase
        {
            get
            {
                return true;
            }
        }

		/// <summary>
		/// Forces update nxControl in the next postback
		/// </summary>
		public virtual void AjaxUpdate()
		{
			hasChanged = true;
		}
        /// <summary>
        /// Forces NOT update nxControl in the next postback
        /// </summary>
		public virtual void AjaxNotUpdate()
		{
			hasChanged = false;
		}

        /// <summary>
        /// Raises an event
        /// </summary>
        /// <param name="action">action name</param>
        /// <param name="value">value of the event</param>
		public virtual void RaiseEvent(string action, string value) { }
        /// <summary>
        /// Loads nxControl values from callback values
        /// </summary>
        /// <param name="obj"></param>
		public abstract void PutPostValue(string obj);

        /// <summary>
        /// LoadViewState
        /// </summary>
        /// <param name="savedState">savedState</param>
        protected override void LoadViewState(object savedState)
        {
            object[] state = (object[])(savedState);
            base.LoadViewState(state[0]);
            isVisible = (bool)state[1];
            displayType = (DisplayType)state[2];
            mTagName = (string)state[3];
            mLoadingImg = (string)state[4];
            mLoadingImgID = (string)state[5];
            mLoadingImgCssClass = (string)state[6];
            mAttributes = (System.Web.UI.AttributeCollection)state[7];
        }
        /// <summary>
        /// SaveViewState
        /// </summary>
        /// <returns>savedState</returns>
        protected override object SaveViewState()
        {
            object[] state = new object[8];
            state[0] = base.SaveViewState();
            state[1] = isVisible;
            state[2] = displayType;
            state[3] = mTagName;
            state[4] = mLoadingImg;
            state[5] = mLoadingImgID;
            state[6] = mLoadingImgCssClass;
            state[7] = mAttributes;
            return state;
        }

		internal void ProtectedLoadViewState(object savedState)
		{
            LoadViewState(savedState);
            hasChanged = false;
		}
        internal object ProtectedSaveViewState()
		{
            return SaveViewState();
		}
	}
}
