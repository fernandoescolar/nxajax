/*
 * Licensing
 *  Mozilla Public License 1.1 (MPL 1.1)
 * 
 * Fernando Escolar Martínez-Berganza <fer.escolar@gmail.com>
 * 
 */
using System.ComponentModel;
using System.Drawing;
using System.Web.UI;
using System.Web.UI.Design;
using System.Web.UI.WebControls;

namespace nxAjax.UI.Controls
{
    /// <summary>
    /// nxAjax framework Drag 'n Drop Panel control
    /// <code>
    /// &lt;ajax:DragnDropPanel runat="server"&gt;&lt;/ajax:DragnDropPanel&gt;
    /// </code>
    /// </summary>
    [Designer("nxAjax.UI.Design.nxControlDesigner")]
    [ParseChildren(false)]
    [PersistChildren(true)]
    [Description("A Dragable 'n Dropable Panel control")]
    [ToolboxData("<{0}:DragnDropPanel runat=\"server\"></{0}:DragnDropPanel>")]
    [ToolboxItem(true)]
    public class DragnDropPanel : nxControl
    {
        #region Private Attributes
        private Label divTitle = new Label();
        private Panel divContent = new Panel();
        internal nxControlCollection containedControls = new nxControlCollection();
		#endregion

		#region Public Properties
        /// <summary>
        /// Panel Title
        /// </summary>
        [Category("Appearance"), DefaultValue(""), Description("Title")]
        public string Title
        {
            get { return divTitle.Text; }
            set { divTitle.Text = value; }
        }
        /// <summary>
        /// Panel Title Css class name
        /// </summary>
        [Category("Appearance"), DefaultValue(""), Description("Title CSS class name.")]
        public string TitleCssClass
        {
            get { return divTitle.CssClass; }
            set { divTitle.CssClass = value; }
        }
        /// <summary>
        /// Panel content css class name
        /// </summary>
        [Category("Appearance"), DefaultValue(""), Description("Content CSS class name.")]
        public string ContentCssClass
        {
            get { return divContent.CssClass; }
            set { divContent.CssClass = value; }
        }

        public new nxControlCollection Controls
        {
            get
            {
                return containedControls;
            }
        }
		#endregion

        public DragnDropPanel() : base("div") 
        { 
            divTitle.ID = ID + "_Title";
            divContent.ID = ID + "_Content";
        }

		#region Renders
		public override void RenderHTML(nxAjaxTextWriter writer)
		{
            RenderBeginTag(writer);
            divTitle.RenderHTML(writer);
            divContent.RenderBeginTag(writer);
            RenderChildren(new HtmlTextWriter(writer.TextWriter));
            divContent.RenderEndTag(writer);
            RenderEndTag(writer);
		}
        public override void RenderJS(nxAjaxTextWriter writer)
		{
            base.RenderJS(writer);
            divTitle.RenderJS(writer);
            divContent.RenderJS(writer);
            foreach (nxControl c in containedControls)
                c.RenderJS(writer);
		}
		#endregion

		protected override void LoadViewState(object savedState)
		{
            object[] state = (object[])(savedState);
			base.LoadViewState(state[0]);
            divTitle.ProtectedLoadViewState(state[1]);
            divContent.ProtectedLoadViewState(state[2]);
		}
		protected override object SaveViewState()
		{
			object[] state = new object[3];
			state[0] = base.SaveViewState();
            state[1] = divTitle.ProtectedSaveViewState();
            state[2] = divContent.ProtectedSaveViewState();
			return state;
		}
		public override void RaiseEvent(string action, string value)
		{
			
		}
		public override void PutPostValue(string obj)
		{
			
		}
        protected override void AddedControl(System.Web.UI.Control control, int index)
        {
            try
            {
                control.Page = this.Page;
                if (control is nxControl)
                    containedControls += (nxControl)control;
            }
            catch { }
        }
    }
}
