/*
 * Licensing
 *  Mozilla Public License 1.1 (MPL 1.1)
 * 
 * Fernando Escolar Martínez-Berganza <fer.escolar@gmail.com>
 * 
 */
using System;
using System.ComponentModel;
using System.Web.UI;
using System.Web.UI.HtmlControls;

namespace nxAjax.UI.Controls
{
    /// <summary>
    /// Base class for input type web controls like &lt;input type="button"... or &lt;input type="text" ...
    /// </summary>
    public abstract class InputControl : nxControl
    {
        /// <summary>
        /// Abstract InputControl constructor
        /// </summary>
        /// <param name="type">input type (button, text, password, ...)</param>
        protected InputControl(string type) : base ("input")
		{
			if (type == null)
				type = String.Empty;

			Attributes ["type"] = type;
		}


        /// <summary>
        /// Gets/Sets Name attributte
        /// </summary>
		[DefaultValue ("")]
		[DesignerSerializationVisibility (DesignerSerializationVisibility.Hidden)]
		[Description("")]
		[Category("Behavior")]
		public virtual string Name {
			get { return ID; }
			set { ; }
		}

        /// <summary>
        /// Gets Type attributte
        /// </summary>
		[DefaultValue ("")]
		[DesignerSerializationVisibility (DesignerSerializationVisibility.Hidden)]
        [Description("")]
		[Category("Behavior")]
		public string Type {
			get { return Attributes ["type"]; }
		}

        /// <summary>
        /// Gets/Sets Value attributte
        /// </summary>
		[DefaultValue ("")]
        [Description("")]
		[Category("Appearance")]
		public virtual string Value {
			get {
				string s = Attributes ["value"];
				return (s == null) ? String.Empty : s;
			}
			set {
				if (value == null)
					Attributes.Remove ("value");
				else
					Attributes ["value"] = value;
                AjaxUpdate();
			}
		}

        /// <summary>
        /// Gets/Sets Readonly property
        /// </summary>
        [DefaultValue(false)]
        [Category("Appearance"), Description("Is ReadOnly")]
        public bool ReadOnly
        {
            get
            {
                string s = Attributes["readonly"];
                return (s != null);
            }
            set
            {
                if (!value)
                    Attributes.Remove("readonly");
                else
                    Attributes["readonly"] = "readonly";

                AjaxUpdate();
            }
        }

        /// <summary>
        /// Renders Control HTML code
        /// </summary>
        /// <param name="writer">Tag Writer</param>
        public override void RenderHTML(nxAjaxTextWriter writer)
        {
            writer.WriteBeginTag(TagName);
            RenderAttributes(writer);
            writer.Write(nxAjaxTextWriter.SelfClosingTagEnd);

            if (PostBackMode == PostBackMode.Async && LoadingImg != string.Empty)
                RenderLoadingImage(writer);
            hasChanged = false;
        }
        /// <summary>
        /// Renders Control Javascript functions
        /// </summary>
        /// <param name="writer">javascript writer</param>
        public override void RenderJS(nxAjaxTextWriter writer)
        {
            base.RenderJS(writer);
            if (nxPage.IsPostBack && hasChanged)
            {
                //writer.Write("EnabledObj('" + ID + "', " + (!Disabled).ToString().ToLower() + ");");
                writer.Write("$('#" + ID + "').val(\"" + Value.Replace("'", "\\'").Replace("\"", "\\\"").Replace("\n", "\\n").Replace("\r", "\\r") + "\");");
                writer.Write("$('#" + ID + "').readonly(" + ReadOnly.ToString().ToLower() + ");");
            }
        }
        /// <summary>
        /// Renders Control Attributes HTML code part
        /// </summary>
        /// <param name="writer"></param>
        protected override void RenderAttributes(nxAjaxTextWriter writer)
		{
			if (Attributes ["name"] == null) {
				writer.WriteAttribute ("name", Name);
			}
			base.RenderAttributes (writer);
		}

    }
}
