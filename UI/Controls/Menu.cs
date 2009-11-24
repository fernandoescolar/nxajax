/*
 * Licensing
 *  Mozilla Public License 1.1 (MPL 1.1)
 * 
 * Fernando Escolar Martínez-Berganza <fer.escolar@gmail.com>
 * 
 */
using System;
using System.Web.UI;
using System.Drawing;
using System.ComponentModel;

namespace nxAjax.UI.Controls
{
	/// <summary>
	/// Menu control. It generates &lt;ul&gt; list HTML tags.
    /// <code>
    /// &lt;ajax:Menu runat="server"&gt;&lt;/ajax:Menu&gt;
    /// </code>
	/// </summary>
    [Designer("nxAjax.UI.Design.nxControlDesigner")]
    [DefaultEventAttribute("ServerChange")]
	[ToolboxData("<{0}:Menu runat=\"server\"></{0}:Menu>")]
    [ParseChildren(true, "Items")]
    [PersistChildren(false)]
    [DefaultProperty("Items")]
    [ToolboxItem(true)]
    [ToolboxBitmap(typeof(System.Web.UI.WebControls.Panel))]
	public class Menu : nxControl
	{
		#region Private Attributes
		private string itemCssClass, sItemCssClass, selectedValue;
		private MenuItemCollection menus;
		#endregion

        /// <summary>
        /// Creates a new Menu control
        /// </summary>
		public Menu() : base("div") { menus = new MenuItemCollection(null); itemCssClass = sItemCssClass = selectedValue = ""; }

		#region Public Properties
        /// <summary>
        /// Gets/Sets MenuItems css class name
        /// </summary>
		[Category("Appearance"), DefaultValue(""), Description("CSS class name.")]
		public string ItemCssClass
		{
			get { return itemCssClass; }
			set { itemCssClass = value; AjaxUpdate(); }
		}
        /// <summary>
        /// Gets/Sets Selected MenuItem css class name
        /// </summary>
		[Category("Appearance"), DefaultValue(""), Description("CSS class name.")]
		public string SelectedItemCssClass
		{
			get { return sItemCssClass; }
			set { sItemCssClass = value; AjaxUpdate(); }
		}
        /// <summary>
        /// Gets/Sets Selected MenuItem value
        /// </summary>
		[Category("Appearance"), DefaultValue(""), Description("Selected Item Value.")]
		public string SelectedValue
		{
			get { return selectedValue; }
			set { if(selectedValue!=value) { selectedValue = value; AjaxUpdate();} }
		}
        /// <summary>
        /// Gets selected MenuItem
        /// </summary>
		public MenuItem SelectedItem
		{
				get {
					MenuItem aux = null;
					foreach(MenuItem m in menus)
					{
						aux = SearchRecursive(m);
						if (aux!=null) return aux;
					}
					return null;
				}
		}
		private MenuItem SearchRecursive(MenuItem m)
		{
			if (m.Value == selectedValue)
				return m;
			if (m.Items.Count>0)
			{
				MenuItem aux = null;
				foreach(MenuItem i in m.Items)
				{
					aux = SearchRecursive(i);
					if (aux!=null) return aux;
				}
			}
			return null;
		}
        /// <summary>
        /// Gets MenuItems collections
        /// </summary>
		public MenuItemCollection Items 
		{ 
			get { return menus; } 
		}

		#endregion

		#region Public Server Events
        /// <summary>
        /// Raises on Control selected item change
        /// <remarks>Is a server event</remarks>
        /// </summary>
        public event nxEventHandler ServerChange;
		#endregion	
		
		#region Renders
		public override void RenderHTML(nxAjaxTextWriter writer)
		{
            RenderBeginTag(writer);
            
            if (DesignMode)
                writer.Write(RenderUL());

            RenderEndTag(writer);

            if (PostBackMode == PostBackMode.Async && LoadingImg != string.Empty)
                RenderLoadingImage(writer);

			hasChanged = false;
		}
		private string RenderUL()
		{
            nxAjaxTextWriter writer = new nxAjaxTextWriter();
            writer.WriteFullBeginTag("ul");
			
			foreach(MenuItem m in menus)
				RenderMenuItem(m, writer);

            writer.WriteEndTag("ul");

			return writer.ToString();
		}
		private void RenderMenuItem(MenuItem root, nxAjaxTextWriter writer)
		{
			string func = "";
            if (ServerChange != null)
                func += nxPage.GetPostBackWithValueAjaxEvent(this, "onchange", "'{%$&#}'");


			if (func != "")
				func = "javascript:" + func;

            writer.WriteBeginTag("li");

            if (root.Value == selectedValue && sItemCssClass != "")
                writer.WriteAttribute("class", sItemCssClass);
			else if (itemCssClass != "")
                writer.WriteAttribute("class", itemCssClass);
            writer.Write(nxAjaxTextWriter.TagRightChar);

            if (root.Items.Count <= 0)
            {
                writer.WriteBeginTag("a");

                if (func != "")
                    writer.WriteAttribute("href", func.Replace("{%$&#}", root.Value));
                else
                    writer.WriteAttribute("href", "#");
                writer.Write(nxAjaxTextWriter.TagRightChar);
            }
            writer.Write(root.Text);
            if (root.Items.Count > 0)
            {
                writer.WriteFullBeginTag("ul");
                foreach(MenuItem m in root.Items)
				    RenderMenuItem(m, writer);
                writer.WriteEndTag("ul");
            }
            if (root.Items.Count <= 0)
            {
                writer.WriteEndTag("a");
            }
            writer.WriteEndTag("li");
			
		}
        public override void RenderJS(nxAjaxTextWriter writer)
		{
            base.RenderJS(writer);

			if(this.hasChanged || !nxPage.IsPostBack)
			{
                writer.Write("$('#" + ID + "').html(' " + RenderUL().Replace("'", "\\'").Replace("\n", "\\n\\r") + "');");
                hasChanged = false;
			}
		}
		#endregion

		public override void RaiseEvent(string action, string value)
		{
            PutPostValue(value);
			switch(action.ToLower())
			{
				case "onchange":
                    ServerChange(this, value);
					break;
			}
		}
		protected override void LoadViewState(object savedState)
		{
			object[] state = (object[])(savedState);
			base.LoadViewState(state[0]);
			menus = (MenuItemCollection)state[1];
			itemCssClass = (string)state[2];
			sItemCssClass = (string)state[3];
			selectedValue = (string)state[4];
		}
		protected override object SaveViewState()
		{
			object[] state = new object[5];
			state[0] = base.SaveViewState();
			state[1] = menus;
			state[2] = itemCssClass;
			state[3] = sItemCssClass;
			state[4] = selectedValue;
			return state;
		}
        public override void PutPostValue(string obj) { SelectedValue = obj; }
	}
}
