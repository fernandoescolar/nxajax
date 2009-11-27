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
	/// Container control can load differents nxContainedPages in a nxMainPage.
    /// <code>
    /// &lt;ajax:Container runat="server"&gt;&lt;/ajax:Container&gt;
    /// </code>
	/// </summary>
    [AspNetHostingPermission(SecurityAction.Demand, Level = AspNetHostingPermissionLevel.Minimal)]
    [AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    [Designer("nxAjax.UI.Design.nxControlDesigner")]
    [ToolboxData("<{0}:Container runat=\"server\"></{0}:Container>")]
    [ToolboxItem(true)]
    [ToolboxBitmap(typeof(System.Web.UI.WebControls.Panel))]
    [DefaultProperty("ContainedPage")]
	public class Container : nxContainerControl
	{
		public Container() : base("div") { containedpage = ""; }

		private string containedpage;

        /// <summary>
        /// Url of the contained nxContainedPage
        /// </summary>
		[Category("Appearance"), DefaultValue(""), Description("The Url contained.")]
		public string ContainedPage
		{
			get { return containedpage; }
			set { containedpage = value; AjaxUpdate(); }
		}
		
		public override void RenderJS(nxAjaxTextWriter writer)
		{
            base.RenderJS(writer);

            if (hasChanged || !this.nxPage.IsPostBack && containedpage != string.Empty)
			{
				string param, pag;
				param = "";
				pag = containedpage;
				if (containedpage.IndexOf('?')>0)
				{
					string[] temp = containedpage.Split('?');
					pag = temp[0];
					param = temp[1];
				}
                writer.Write("$.nxApplication.LoadPane('" + ID + "', '" + pag + "', '" + param + "');");
				hasChanged = false;
			}
		}
		protected override void LoadViewState(object savedState)
		{
			object[] state = (object[])(savedState);
			base.LoadViewState(state[0]);
			containedpage = (string)state[1];
		}
		protected override object SaveViewState()
		{
			object[] state = new object[2];
			state[0] = base.SaveViewState();
			state[1] = containedpage;
			return state;
		}
		public override void PutPostValue(string obj)
		{
		
		}
	}
}
