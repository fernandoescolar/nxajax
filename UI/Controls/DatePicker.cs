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
using System.Globalization;
using System.Security.Permissions;

namespace Framework.Ajax.UI.Controls
{
	/// <summary>
    /// nxAjax framework date picker control
    /// <code>
    /// &lt;ajax:DatePicker runat="server"&gt;&lt;/ajax:DatePicker&gt;
    /// </code>
	/// </summary>
    [AspNetHostingPermission(SecurityAction.Demand, Level = AspNetHostingPermissionLevel.Minimal)]
    [AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    [Designer("Framework.Ajax.UI.Design.AjaxControlDesigner")]
    [ToolboxData("<{0}:DatePicker runat=\"server\"></{0}:DatePicker>")]
    [ToolboxItem(true)]
    [ToolboxBitmap(typeof(System.Web.UI.WebControls.Calendar))]
    [DefaultProperty("DateTimeValue")]
	public class DatePicker : TextBox
	{
		#region Private Attributes
		private string imageSrc;
		#endregion

		#region Public Properties
        public override string Value
        {
            get
            {
                return base.Value;
            }
            set
            {
                try
                {
                    if (value == "0:00:00")
                    {
                        base.Value = "";
                    }
                    else
                    {
                        DateTime d = DateTime.Parse(value);
                        if (d == DateTime.MinValue)
                            base.Value = "";
                        else
                        {
                            DateTimeFormatInfo dfi = CultureInfo.CurrentCulture.DateTimeFormat;
                            base.Value = d.ToString(dfi.ShortDatePattern, dfi);
                        }
                    }
                }
                catch
                {
                    base.Value = "";
                }
                
            }
        }
        /// <summary>
        /// Gets/Sets the control value in DateTime format
        /// </summary>
		[Category("Appearance"), DefaultValue(""), Description("Value of input text.")]
        public DateTime DateTimeValue 
        { 
            get 
            {
                try
                {
                    return DateTime.Parse(Value);
                }
                catch (FormatException fex)
                {
                    System.Diagnostics.Debug.WriteLine("Supported format exception in datepicker: " + fex.Message);
                    return DateTime.MinValue;
                }
            } 
            set {
                try
                {
                    DateTimeFormatInfo dfi = CultureInfo.CurrentCulture.DateTimeFormat;
                    Value = value.ToString(dfi.ShortDatePattern, dfi);
                }
                catch (FormatException fex)
                {
                    System.Diagnostics.Debug.WriteLine("Supported format exception in datepicker: " + fex.Message);
                    Value = "";
                } 
                AjaxUpdate(); 
            } 
        }
        /// <summary>
        /// Gets/Sets DatePicker Icon Image path 
        /// </summary>
		[Category("Appearance"), DefaultValue(""), Description("Url DatePicker Image.")]
		public string ImageSource { get { return imageSrc; } set { imageSrc = value; }}
		#endregion

        public DatePicker() : base() { DateTimeFormatInfo dfi = CultureInfo.CurrentCulture.DateTimeFormat; Value = DateTime.Now.ToString(dfi.ShortDatePattern, dfi); imageSrc = ""; }

		#region Renders
		public override void RenderHTML(AjaxTextWriter writer)
		{
            writer.WriteBeginTag("span");
            if (CssClass != string.Empty)
                writer.WriteAttribute("class", CssClass);
            writer.Write(AjaxTextWriter.TagRightChar);

            if (Value == null)
                Value = "";
            base.RenderHTML(writer);

            writer.WriteEndTag("span");

            if (PostBackMode == PostBackMode.Async && LoadingImg != string.Empty)
                RenderLoadingImage(writer);

			hasChanged = false;
		}
        public override void RenderJS(AjaxTextWriter writer)
		{
            base.RenderJS(writer);
			if (hasChanged)
			{
                if (Disabled)
                    writer.Write("$('#" + ID + "').datepick('disable');");
                else
                    writer.Write("$('#" + ID + "').datepick('enable');");
				hasChanged = false;
			}
            if (!this.AjaxController.IsPostBack)
			{
                DateTimeFormatInfo dfi = CultureInfo.CurrentCulture.DateTimeFormat;
                writer.Write("$('#" + ID + "').datepick({showOn: 'button', buttonImageOnly: true, buttonImage: '" + imageSrc + "', dateFormat: '" + dfi.ShortDatePattern.Replace("MM", "mm").Replace("yyyy", "yy") + "'});");
                if (Disabled)
                    writer.Write("$('#" + ID + "').datepick('disable');");
			}
           
		}
		#endregion

		
		protected override void AjaxLoadViewState(object savedState)
		{
			object[] state = (object[])(savedState);
			base.AjaxLoadViewState(state[0]);
			imageSrc = (string)state[1];
		}
		protected override object AjaxSaveViewState()
		{
			object[] state = new object[2];
			state[0] = base.AjaxSaveViewState();
			state[1] = imageSrc;
			return state;
		}
		public override void PutPostValue(string obj)
		{
			try
			{
				this.DateTimeValue = DateTime.Parse(obj);
			}
			catch
			{
                this.DateTimeValue = DateTime.MinValue;
			}
		}
	}
}
