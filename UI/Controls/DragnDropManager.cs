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
using System.Collections.Generic;
using System.Security.Permissions;

namespace Framework.Ajax.UI.Controls
{
    /// <summary>
    /// nxAjax framework Drag 'n Drop Manager control. Where positions and displays of some DragnDropPanels will be saved.
    /// <code>
    /// &lt;ajax:DragnDropManager runat="server"&gt;&lt;/ajax:DragnDropManager&gt;
    /// </code>
    /// </summary>
    [AspNetHostingPermission(SecurityAction.Demand, Level = AspNetHostingPermissionLevel.Minimal)]
    [AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    [Designer("Framework.Ajax.UI.Design.AjaxControlDesigner")]
    [ToolboxData("<{0}:DragnDropManager runat=\"server\"></{0}:DragnDropManager>")]
    [ToolboxItem(true)]
    [ToolboxBitmap(typeof(System.Web.UI.WebControls.Panel))]
    public class DragnDropManager : Hidden
    {
        private List<string> panels = new List<string>();
        private List<string> dndpanels = new List<string>();
        private List<string> disposition = new List<string>();
        private List<Size> sizes = new List<Size>();

        /// <summary>
        /// Gets/Sets Selected panel CSS class name
        /// </summary>
        [Category("Appearance"), DefaultValue(""), Description("Selected panel CSS class name.")]
        public string SelectedCssClass
        {
            get
            {
                string s = Attributes["selectedclass"];
                return (s == null) ? String.Empty : s;
            }
            set
            {
                if (value == null)
                    Attributes.Remove("selectedclass");
                else
                    Attributes["selectedclass"] = value;
                AjaxUpdate();
            }
        }

        public DragnDropManager():base() { }
        
        #region Renders
        public override void RenderHTML(AjaxTextWriter writer)
        {
            this.Value = GenerateValue();
            base.RenderHTML(writer);
        }
        public override void RenderJS(AjaxTextWriter writer)
        {
            base.RenderJS(writer);
            if ((hasChanged && AjaxController.IsPostBack) || (!AjaxController.IsPostBack))
            {
                writer.Write("__dragndrop.Init('" + ID + "', '" + CssClass + "', '" + SelectedCssClass + "');");
                foreach (string s in dndpanels)
                    writer.Write("__dragndrop.Load('" + s + "');");
            }
        }
        #endregion

        /// <summary>
        /// Adds a new container panel to manage
        /// </summary>
        /// <param name="panel">New panel object</param>
        /// <param name="size">Size of new panel</param>
        public void AddPanel(Panel panel, Size size)
        {
            panels.Add(panel.ID);
            sizes.Add(size);
            disposition.Add("");
        }
        /// <summary>
        /// Adds a new container panel to manage
        /// </summary>
        /// <param name="panel">New panel object</param>
        /// <param name="w">New panel Width</param>
        /// <param name="h">New panel Height</param>
        public void AddPanel(Panel panel, int w, int h)
        {
            AddPanel(panel, new Size(w, h));
        }
        /// <summary>
        /// Adds a new Drag 'n Drop panel to manage
        /// </summary>
        /// <param name="panel">New Drag 'n Drop panel object</param>
        /// <param name="container">New Drag 'n Drop panel object container panel</param>
        public void AddDragnDropPanel(DragnDropPanel panel, Panel container)
        {
            dndpanels.Add(panel.ID);
            for (int i = 0; i < panels.Count; i++)
                if (container.ID == panels[i])
                {
                    disposition[i] = panel.ID;
                    break;
                }
        }


        protected string GenerateSizes()
        { 
            string aux = "";
            foreach (Size s in sizes)
                aux += ((aux == "") ? "" : ";") + s.Width + "," + s.Height;
            return aux;
        }
        protected string GenerateItems()
        {
            string aux = "";
            foreach (string s in panels)
                aux += ((aux == "") ? "" : ";") + s;
            return aux;
        }
        protected string GenerateDisposition()
        {
            string aux = "";
            foreach (string s in disposition)
                aux += ((aux == "") ? "" : ";") + s;
            return aux;
        }
        protected string GenerateValue()
        {
            return GenerateSizes() + "|" + GenerateItems() + "|" + GenerateDisposition();
        }

        protected override void AjaxLoadViewState(object savedState)
        {
            object[] state = (object[])(savedState);
            base.AjaxLoadViewState(state[0]);
            panels = (List<string>)state[1];
            dndpanels = (List<string>)state[2];
            disposition = (List<string>)state[3];
            sizes = (List<Size>)state[4];
        }
        protected override object AjaxSaveViewState()
        {
            object[] state = new object[5];
            state[0] = base.AjaxSaveViewState();
            state[1] = panels;
            state[2] = dndpanels;
            state[3] = disposition;
            state[4] = sizes;
            return state;
        }
        public override void RaiseEvent(string action, string value)
        {
            
        }
        public override void PutPostValue(string obj)
        {
            string[] configuration = obj.Split(new char[] {'|'});
            string[] sizes = configuration[0].Split(new char[] { ';' });
            string[] panels = configuration[1].Split(new char[] { ';' });
            string[] disposition = configuration[2].Split(new char[] { ';' });

            this.sizes.Clear();
            this.panels.Clear();
            this.disposition.Clear();

            foreach (string s in sizes)
            {
                string[] aux = s.Split(new char[] { ',' });
                this.sizes.Add(new Size(int.Parse(aux[0]), int.Parse(aux[1])));
            }
            foreach (string s in panels)
                this.panels.Add(s);
            foreach (string s in disposition)
                this.disposition.Add(s);
        }
    }
}
