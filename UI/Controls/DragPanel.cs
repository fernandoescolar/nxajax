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

namespace Framework.Ajax.UI.Controls
{
    /// <summary>
    /// nxAjax framework Drag Panel control
    /// <code>
    /// &lt;ajax:DragPanel runat="server"&gt;&lt;/ajax:DragPanel&gt;
    /// </code>
    /// </summary>
    [Designer("Framework.Ajax.UI.Design.AjaxControlDesigner")]
    [ParseChildren(false)]
    [PersistChildren(true)]
    [Description("A Dragable Panel control")]
    [ToolboxData("<{0}:DragPanel runat=\"server\"></{0}:DragPanel>")]
    [ToolboxItem(true)]
    public class DragPanel : AjaxControl
    {
        #region Private Attributes
        private Label divTitle = new Label();
        private Panel divContent = new Panel();
        internal AjaxControlCollection containedControls = new AjaxControlCollection();
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

        public new AjaxControlCollection Controls
        {
            get
            {
                return containedControls;
            }
        }
		#endregion

        public DragPanel() : base("div") 
        { 
            
        }

		#region Renders
        protected bool controlsPrepared = false;
        protected void prepareControls()
        {
            if (controlsPrepared)
                return;

            divTitle.ID = ID + "_Title";
            divContent.ID = ID + "_Content";

            divTitle.Page = this.Page;
            divContent.Page = this.Page;

            if (string.IsNullOrEmpty(divTitle.Attributes["class"]))
                divTitle.Attributes["class"] = "ajax-dragbox-title";
            else if (divTitle.Attributes["class"].IndexOf("ajax-dragbox-title") < 0)
                divTitle.Attributes["class"] += " ajax-dragbox-title";

            if (string.IsNullOrEmpty(divContent.Attributes["class"]))
                divContent.Attributes["class"] = "ajax-dragbox-content";
            else if (divTitle.Attributes["class"].IndexOf("ajax-dragbox-content") < 0)
                divContent.Attributes["class"] += " ajax-dragbox-content";
        }
		public override void RenderHTML(AjaxTextWriter writer)
		{
            prepareControls();

            if (string.IsNullOrEmpty(Attributes["class"]))
                Attributes["class"] = "ajax-dragbox";
            else if (Attributes["class"].IndexOf("ajax-dragbox") < 0)
                Attributes["class"] += " ajax-dragbox";

            RenderBeginTag(writer);
            divTitle.RenderHTML(writer);
            divContent.RenderBeginTag(writer);
            RenderChildren(new HtmlTextWriter(writer.TextWriter));
            divContent.RenderEndTag(writer);
            RenderEndTag(writer);
		}
        public override void RenderJS(AjaxTextWriter writer)
		{
            prepareControls();
            base.RenderJS(writer);
            divTitle.RenderJS(writer);
            divContent.RenderJS(writer);
            foreach (AjaxControl c in containedControls)
                c.RenderJS(writer);
		}
		#endregion

		protected override void AjaxLoadViewState(object savedState)
		{
            object[] state = (object[])(savedState);
			base.AjaxLoadViewState(state[0]);
            divTitle.ProtectedLoadViewState(state[1]);
            divContent.ProtectedLoadViewState(state[2]);
		}
		protected override object AjaxSaveViewState()
		{
			object[] state = new object[3];
			state[0] = base.AjaxSaveViewState();
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
                if (control is AjaxControl)
                    containedControls += (AjaxControl)control;
            }
            catch { }
        }
    }
}
