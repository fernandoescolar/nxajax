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
using System.Security.Permissions;

namespace Framework.Ajax.UI.Controls
{
    /// <summary>
    /// Base control type. It contains other AjaxControls
    /// </summary>
    [AspNetHostingPermission(SecurityAction.Demand, Level = AspNetHostingPermissionLevel.Minimal)]
    [AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    public abstract class AjaxContainerControl : AjaxControl
    {
        protected AjaxContainerControl() : this("span") { }
        internal protected AjaxContainerControl(string tag) : base(tag) { }

        /// <summary>
        /// Gets/Sets Inner Html code
        /// </summary>
        [BrowsableAttribute(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual string InnerHtml
        {
            get
            {
                if (Controls.Count == 0)
                    return String.Empty;

                if (Controls.Count >= 1)
                {
                    Control ctrl = Controls[0];
                    LiteralControl lc = ctrl as LiteralControl;
                    if (lc != null)
                        return lc.Text;

                    DataBoundLiteralControl dblc = ctrl as DataBoundLiteralControl;
                    if (dblc != null)
                        return dblc.Text;
                }

                throw new HttpException("There is no literal content!");
            }

            set
            {
                Controls.Clear();
                Controls.Add(new LiteralControl(value));
                AjaxUpdate();
            }
        }
        /// <summary>
        /// Gets/Sets Inner Text (makes Html encode)
        /// </summary>
        [BrowsableAttribute(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual string InnerText
        {
            get
            {
                return HttpUtility.HtmlDecode(InnerHtml);
            }

            set
            {
                InnerHtml = HttpUtility.HtmlEncode(value);
            }
        }

        public override void RenderHTML(AjaxTextWriter writer)
        {
            RenderBeginTag(writer);
            RenderChildren(new HtmlTextWriter(writer.TextWriter));
            RenderEndTag(writer);

            if (PostBackMode == PostBackMode.Async && LoadingImg != string.Empty)
                RenderLoadingImage(writer);
        }
        public override void RenderJS(AjaxTextWriter writer)
        {
            base.RenderJS(writer);
            if (AjaxPage.IsPostBack && hasChanged)
            {
                try
                {
                    writer.Write("$('" + ID + "').html(' " + InnerHtml.Replace("'", "\\'").Replace("\"", "\\\"").Replace("\n", "\\n").Replace("\r", "\\r") + "');");
                }
                catch (NotSupportedException ex) { System.Console.WriteLine("Supported exception:" + ex.Message); }
            }
        }
        protected override void RenderAttributes(AjaxTextWriter writer)
        {
            ViewState.Remove("innerhtml");
            base.RenderAttributes(writer);
        }

        protected override void LoadViewState(object savedState)
        {
            object[] state = (object[])(savedState);
            base.LoadViewState(state[0]);
            try
            {
                InnerHtml = (string)state[1];
            }
            catch (NotSupportedException ex) { System.Console.WriteLine("Supported exception:" + ex.Message); }
        }
        protected override object SaveViewState()
        {
            object[] state = new object[2];
            state[0] = base.SaveViewState();
            try
            {
                state[1] = InnerHtml;
            }
            catch (NotSupportedException ex) { System.Console.WriteLine("Supported exception:" + ex.Message); }
            return state;
        }

    }
}
