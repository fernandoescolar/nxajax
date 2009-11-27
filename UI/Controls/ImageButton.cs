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
    /// ImageButton control (Input type="image")
    /// </summary>
    [AspNetHostingPermission(SecurityAction.Demand, Level = AspNetHostingPermissionLevel.Minimal)]
    [AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    [Designer("nxAjax.UI.Design.nxControlDesigner")]
    [DefaultEventAttribute("ServerClick")]
    [ToolboxData("<{0}:ImageButton runat=\"server\"></{0}:ImageButton>")]
    [ToolboxItem(true)]
    [ToolboxBitmap(typeof(System.Web.UI.WebControls.ImageButton))]
    public class ImageButton : InputImage
    {
        #region Public Properties
        /// <summary>
        /// Image Width
        /// </summary>
        [DefaultValue("")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Description("")]
        [Category("Appearance")]
        public int Width
        {
            get
            {
                string border = Attributes["width"];
                if (border == null)
                    return -1;
                return Int32.Parse(border, CultureInfo.InvariantCulture);
            }
            set
            {
                if (value == -1)
                {
                    Attributes.Remove("width");
                    return;
                }
                Attributes["width"] = value.ToString(CultureInfo.InvariantCulture);
            }
        }
        /// <summary>
        /// Image Height
        /// </summary>
        [DefaultValue("")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Description("")]
        [Category("Appearance")]
        public int Height
        {
            get
            {
                string border = Attributes["height"];
                if (border == null)
                    return -1;
                return Int32.Parse(border, CultureInfo.InvariantCulture);
            }
            set
            {
                if (value == -1)
                {
                    Attributes.Remove("height");
                    return;
                }
                Attributes["height"] = value.ToString(CultureInfo.InvariantCulture);
            }
        }
        #endregion

        /// <summary>
        /// Creates a new ImageButton
        /// </summary>
        public ImageButton() : base() { mTagName = "img"; Style.Add("cursor", "pointer"); }
    }
}
