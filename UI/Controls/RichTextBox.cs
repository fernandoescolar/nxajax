using System;
using System.Web.UI;
using System.Drawing;
using System.ComponentModel;

namespace nxAjax.UI.Controls
{
	/// <summary>
	/// Descripción breve de RichTextBox.
	/// </summary>
	[ToolboxBitmapAttribute(typeof(nxControl), "images.RichTextBox.bmp"), 
	ToolboxData("<{0}:RichTextBox runat=server></{0}:RichTextBox>")]
	public class RichTextBox : nxControl
	{
		#region Private Attributes
		private string value;
		#endregion

		#region Public Properties
		[Category("Appearance"), DefaultValue(""), Description("Value of input text.")]
		public string Value		{ get { return value; }		set { this.value = value; AjaxUpdate(); } }
		#endregion

		public RichTextBox() : base("div") { this.value = ""; }

		public override void RenderJS(nxAjaxTextWriter writer)
		{
            base.RenderJS(writer);

            if (hasChanged && this.nxPage.IsPostBack)
			{
                writer.Write("var mTexto" + ID + " = (document.getElementById) ? document.getElementById('" + ID + "') : document.all['" + ID + "'];");
                writer.Write("mTexto" + ID + ".value = '" + value.Replace("\"", "\\\"") + "';");
				hasChanged = false;
			}
			if (!this.nxPage.IsPostBack)
			{
                writer.Write("RenderHTMLEditor('div" + ID + "', '" + ID + "', '" + value.Replace("\"", "\\\"") + "');");
			}
		}

		protected override void LoadViewState(object savedState)
		{
			object[] state = (object[])(savedState);
			base.LoadViewState(state[0]);
			value = (string) state[1];
		}

		protected override object SaveViewState()
		{
			object[] state = new object[5];
			state[0] = base.SaveViewState();
			state[1] = value;
			return state;
		}

		public override void PutPostValue(string obj)
		{
			this.value = obj;
		}
	}
}
