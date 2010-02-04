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
using System.Globalization;
using System.ComponentModel;
using System.Security.Permissions;

namespace nxAjax.UI.Controls
{
    /// <summary>
    /// Input Image Control
    /// <code>
    /// &lt;ajax:InputImage runat="server"&gt;&lt;/ajax:InputImage&gt;
    /// </code>
    /// </summary>
    [AspNetHostingPermission(SecurityAction.Demand, Level = AspNetHostingPermissionLevel.Minimal)]
    [AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    [Designer("nxAjax.UI.Design.nxControlDesigner")]
    [DefaultEventAttribute("ServerClick")]
    [ToolboxData("<{0}:InputImage runat=\"server\"></{0}:InputImage>")]
    [ToolboxItem(true)]
    [ToolboxBitmap(typeof(System.Web.UI.HtmlControls.HtmlInputImage))]
    [DefaultProperty("Src")]
    public class InputImage : Button
    {
        /// <summary>
        /// Gets/Sets "Align" attribute value
        /// </summary>
        [DefaultValue("")]
        [Description("")]
        [Category("Appearance")]
        public string Align
        {
            get { return GetAtt("align"); }
            set { SetAtt("align", value); }
        }
        /// <summary>
        /// Gets/Sets "Alt" attribute value
        /// </summary>
        [DefaultValue("")]
		[Localizable (true)]
        [Description("")]
        [Category("Appearance")]
        public string Alt
        {
            get { return GetAtt("alt"); }
            set { SetAtt("alt", value); }
        }
        /// <summary>
        /// Gets/Sets contained image file path
        /// </summary>
        [DefaultValue("")]
        [Description("")]
        [Category("Appearance")]
		[UrlProperty]
        public string Src
        {
            get { return GetAtt("src"); }
            set { SetAtt("src", value); }
        }
 	    /// <summary>
 	    /// Gets/Sets "border" attribute value
 	    /// </summary>
		[DefaultValue("")]
		[Description("")]
		[Category("Appearance")]
		public int Border {
			get {
				string border = Attributes ["border"];
				if (border == null)
					return -1;
				return Int32.Parse (border, CultureInfo.InvariantCulture);
			}
			set {
				if (value == -1) {
					Attributes.Remove ("border");
					return;
				}
				Attributes ["border"] = value.ToString (CultureInfo.InvariantCulture);
			}
		}

        protected void SetAtt(string name, string value)
        {
            if ((value == null) || (value.Length == 0))
                Attributes.Remove(name);
            else
                Attributes[name] = value;

            AjaxUpdate();
        }

        protected string GetAtt(string name)
        {
            string res = Attributes[name];
            if (res == null)
                return String.Empty;
            return res;
        }

        public override void RenderJS(nxAjaxTextWriter writer)
        {
            if (hasChanged && nxPage.IsPostBack)
            {
                writer.Write("$('#" + ID + "').attr('src', '" + Src + "');");
            }
            base.RenderJS(writer);
        }

        /// <summary>
        /// Renders Control Attributes HTML code part
        /// </summary>
        /// <param name="writer"></param>
        protected override void RenderAttributes(nxAjaxTextWriter writer)
        {
            if (Attributes ["alt"] == null) {
                writer.WriteAttribute ("alt", ID);
            }
            base.RenderAttributes(writer);
        }
    }
}
