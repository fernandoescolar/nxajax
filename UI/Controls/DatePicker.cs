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

namespace nxAjax.UI.Controls
{
	/// <summary>
    /// nxAjax framework date picker control
    /// <code>
    /// &lt;ajax:DatePicker runat="server"&gt;&lt;/ajax:DatePicker&gt;
    /// </code>
	/// </summary>
    [AspNetHostingPermission(SecurityAction.Demand, Level = AspNetHostingPermissionLevel.Minimal)]
    [AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    [Designer("nxAjax.UI.Design.nxControlDesigner")]
    [ToolboxData("<{0}:DatePicker runat=\"server\"></{0}:DatePicker>")]
    [ToolboxItem(true)]
    [ToolboxBitmap(typeof(System.Web.UI.WebControls.Calendar))]
    [DefaultProperty("DateTimeValue")]
	public class DatePicker : TextBox
	{
		#region Private Attributes
		private string imagesFolder, imageSrc;
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
                            base.Value = d.ToString("dd/MM/yyyy");
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
                    Value = value.ToString("dd/MM/yyyy");
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
        /// Gets/Sets Folder path with all date picker control images
        /// </summary>
		[Category("Appearance"), DefaultValue(""), Description("Folder contains DatePicker Images.")]
		public string ImagesFolder { get { return imagesFolder; } set { imagesFolder = value; }}
        /// <summary>
        /// Gets/Sets DatePicker Icon Image path 
        /// </summary>
		[Category("Appearance"), DefaultValue(""), Description("Url DatePicker Image.")]
		public string ImageSource { get { return imageSrc; } set { imageSrc = value; }}
		#endregion

		public DatePicker(): base() { Value = DateTime.Now.ToString("dd/MM/yyyy"); imagesFolder = imageSrc = "";  }

		#region Renders
		public override void RenderHTML(nxAjaxTextWriter writer)
		{
            writer.WriteBeginTag("span");
            if (CssClass != string.Empty)
                writer.WriteAttribute("class", CssClass);
            writer.Write(nxAjaxTextWriter.TagRightChar);

            if (Value == null)
                Value = "";
            base.RenderHTML(writer);

            //writer.WriteBeginTag("img");
            //writer.WriteAttribute("id", "img" + ID);
            //writer.WriteAttribute("src", imageSrc);
            //writer.WriteAttribute("border", "0");
            //writer.Write(nxAjaxTextWriter.SelfClosingTagEnd);

            writer.WriteEndTag("span");

            if (PostBackMode == PostBackMode.Async && LoadingImg != string.Empty)
                RenderLoadingImage(writer);

			hasChanged = false;
		}
        public override void RenderJS(nxAjaxTextWriter writer)
		{
            base.RenderJS(writer);
			if (hasChanged)
			{
                //writer.Write("ObjVisible('img" + ID + "', " + Visible.ToString().ToLower() + ");");
                if (Disabled)
                    writer.Write("$('#" + ID + "').datepick('disable');");
                else
                    writer.Write("$('#" + ID + "').datepick('enable');");
				hasChanged = false;
			}
            if (!this.nxPage.IsPostBack)
			{
                //writer.Write("LoaddatePicker('" + ID + "', 'img" + ID + "', '" + imagesFolder + "');");
                writer.Write("$('#" + ID + "').datepick({showOn: 'button', buttonImageOnly: true, buttonImage: '" + imageSrc + "'});");
                if (Disabled)
                    writer.Write("$('#" + ID + "').datepick('disable');");
                //    writer.Write("$('#img" + ID + "')[0].onclick = function(){};");
                //if (!Visible)
                //    writer.Write("ObjVisible('img" + ID + "', " + Visible.ToString().ToLower() + ");");
			}
           
		}
		#endregion

		
		protected override void LoadViewState(object savedState)
		{
			object[] state = (object[])(savedState);
			base.LoadViewState(state[0]);
			imageSrc = (string)state[1];
			imagesFolder = (string)state[2];
		}
		protected override object SaveViewState()
		{
			object[] state = new object[3];
			state[0] = base.SaveViewState();
			state[1] = imageSrc;
			state[2] = imagesFolder;
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
