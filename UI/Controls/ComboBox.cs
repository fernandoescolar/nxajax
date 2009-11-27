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
    /// nxAjax framework comboBox control
    /// <code>
    /// &lt;ajax:ComboBox runat="server"&gt;
    ///     &lt;ajax:ComboBoxItem runat="server"/&gt;
    ///     ...
    /// &lt;/ajax:ComboBox&gt;
    /// </code>
	/// </summary>
    [AspNetHostingPermission(SecurityAction.Demand, Level = AspNetHostingPermissionLevel.Minimal)]
    [AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    [Designer("nxAjax.UI.Design.nxControlDesigner")]
    [DefaultEvent("ServerChange")]
	[ToolboxData("<{0}:ComboBox runat=\"server\"></{0}:ComboBox>")]
    [ToolboxItem(true)]
    [ParseChildren(true, "Items")]
    [PersistChildren(false)]
    [DefaultProperty("Items")]
    [ToolboxBitmap(typeof(System.Web.UI.WebControls.DropDownList))]
	public class ComboBox : nxContainerControl
    {
        #region Protected Attributes
        protected ComboBoxItemCollection mItems;
        protected int selected;
		#endregion
		
		#region Public Properties
        public override string InnerHtml
        {
            get
            {
                throw new NotSupportedException();
            }
            set
            {
                throw new NotSupportedException();
            }
        }
        public override string InnerText
        {
            get
            {
                throw new NotSupportedException();
            }
            set
            {
                throw new NotSupportedException();
            }
        }
        /// <summary>
        /// nxControl Name
        /// </summary>
        [DefaultValue("")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Description("")]
        [Category("Behavior")]
        public virtual string Name
        {
            get { return ID; }
            set { ; }
        }
        /// <summary>
        /// ComboBox Item Collection
        /// </summary>
		[Category("Data"), DefaultValue(""), Description("Items contained in control.")]
		public ComboBoxItemCollection Items	{ get { return mItems; }	}
        /// <summary>
        /// Selected Item Index
        /// </summary>
		[Category("Data"), DefaultValue(0), Description("Selected Item Index.")]
		public int SelectedIndex { get { return selected; } set {selected = value; }}
        /// <summary>
        /// Seleted Item Value
        /// </summary>
		[Category("Data"), DefaultValue(""), Description("Selected Item Value.")]
		public string SelectedValue { get { try { return mItems[selected].Value; } catch { return null; } } set { for(int i=0; i<mItems.Count; i++)if(mItems[i].Value==value)selected = i; }}
        /// <summary>
        /// Selected Item Text
        /// </summary>
        [Category("Data"), DefaultValue(""), Description("Selected Item Text.")]
        public string SelectedText { get { try { return mItems[selected].Text; } catch { return null; } } set { for (int i = 0; i < mItems.Count; i++)if (mItems[i].Text == value)selected = i; } }
        /// <summary>
        /// Gets/Sets the comboBox Size
        /// <remarks>
        /// if &lt;= 0 it renders like a comboBox
        /// else it renders like a listBox
        /// </remarks>
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int Size
        {
            get
            {
                string size = Attributes["size"];

                if (size == null)
                {
                    return (-1);
                }

                return (Int32.Parse(size, CultureInfo.InvariantCulture));
            }
            set
            {
                if (value == -1)
                {
                    Attributes.Remove("size");
                }
                else
                {
                    Attributes["size"] = value.ToString();
                }
            }
        }
		#endregion

        #region Public Javascript Client Events
        /// <summary>
        /// Gets/Sets Javascript onchange event code
        /// <remarks>Is a client event</remarks>
        /// </summary>
        [Category("Action"), DefaultValue(""), Description("OnChange javascript event in client.")]
        public string ClientChange
        {
            get
            {
                string s = Attributes["onchange"];
                return (s == null) ? string.Empty : s;
            }
            set
            {
                if (value == null)
                    Attributes.Remove("onchange");
                else
                    Attributes["onchange"] = value;
            }
        }
        /// <summary>
        /// Gets/Sets Javascript onfocus event code
        /// <remarks>Is a client event</remarks>
        /// </summary>
        [Category("Action"), DefaultValue(""), Description("OnFocus javascript event in client.")]
        public string ClientFocus
        {
            get
            {
                string s = Attributes["onfocus"];
                return (s == null) ? string.Empty : s;
            }
            set
            {
                if (value == null)
                    Attributes.Remove("onfocus");
                else
                    Attributes["onfocus"] = value;
            }
        }
        /// <summary>
        /// Gets/Sets Javascript onblur event code
        /// <remarks>Is a client event</remarks>
        /// </summary>
        [Category("Action"), DefaultValue(""), Description("OnBlur javascript event in client.")]
        public string ClientBlur
        {
            get
            {
                string s = Attributes["onblur"];
                return (s == null) ? string.Empty : s;
            }
            set
            {
                if (value == null)
                    Attributes.Remove("onblur");
                else
                    Attributes["onblur"] = value;
            }
        }
        #endregion

        #region Public Server Events
        /// <summary>
        /// Raises on Control selected item change
        /// <remarks>Is a server event</remarks>
        /// </summary>
        public event nxEventHandler ServerChange;
        /// <summary>
        /// Raises on Control get focus
        /// <remarks>Is a server event</remarks>
        /// </summary>
        public event nxEventHandler ServerFocus;
        /// <summary>
        /// Raises on Control lost focus
        /// <remarks>Is a server event</remarks>
        /// </summary>
        public event nxEventHandler ServerBlur;
        #endregion

        /// <summary>
        /// ComboBox Constructor
        /// </summary>
		public ComboBox():base("select") { selected = 0; mItems = new ComboBoxItemCollection(this); }

		#region Renders
        protected override bool hasEvent(string eventName)
        {
            switch (eventName.ToLower())
            {
                case "onchange":
                    return (ServerChange != null);
                case "onfocus":
                    return (ServerFocus != null);
                case "onblur":
                    return (ServerBlur != null);
            }
            return false;
        }
        protected override void RenderAttributes(nxAjaxTextWriter writer)
        {
            if (Attributes["name"] == null)
            {
                writer.WriteAttribute("name", Name);
            }
            RenderClientEventAttribute(writer, "onchange", "this.options[this.selectedIndex].value");
            RenderClientEventAttribute(writer, "onfocus");
            RenderClientEventAttribute(writer, "onblur", "this.options[this.selectedIndex].value");

            base.RenderAttributes(writer);
        }
		public override void RenderHTML(nxAjaxTextWriter writer)
		{
            RenderBeginTag(writer);
			
            for(int i=0; i<mItems.Count; i++)
			{
                writer.Indent++;
                writer.WriteBeginTag("option");
                if (i == selected)
                    writer.WriteAttribute("selected", "selected");
                writer.WriteAttribute("value", (mItems ^ i).Value, true);
                writer.Write(HtmlTextWriter.TagRightChar);

                writer.Write(HttpUtility.HtmlEncode((mItems ^ i).Text));
                writer.WriteEndTag("option");

                writer.Indent--;
			}

            RenderEndTag(writer);

            if (PostBackMode == PostBackMode.Async && LoadingImg != string.Empty)
                RenderLoadingImage(writer);

            hasChanged = false;
		}
        public override void RenderJS(nxAjaxTextWriter writer)
		{
            base.RenderJS(writer);
			if (this.hasChanged)
			{
                writer.Write("var combo" + ID + "_options = '';");
                if (Size>=0)
                    writer.Write("combo" + ID + "[0].size = " + Size + ";");
				for(int i=0; i<mItems.Count; i++)
				{
                    writer.Write("combo" + ID + "_options += '<option value=\"" + (mItems ^ i).Value.Replace("\"", "\\\"").Replace("'", "\\'") + "\"" + ((selected == i) ? " selected=\"selected\"" : "") + ">" + (mItems ^ i).Text.Replace("\"", "\\\"").Replace("'", "\\'") + "</option>';"); 
				}
                writer.Write("$('#" + ID + "').html(combo" + ID + "_options);");
				hasChanged = false;
			}
		}
		#endregion

		public override void RaiseEvent(string action, string value)
		{
            if (action.ToLower() != "onentereditmode") //if it is an EditableComboLabel in edit mode ignore value
                PutPostValue(value);
			switch(action.ToLower())
			{
				case "onfocus":
                    if (ServerFocus != null)
                        ServerFocus(this, value);
					break;
				case "onchange":
                    if (ServerChange != null)
                        ServerChange(this, value);
					break;
				case "onblur":
                    if (ServerBlur != null)
                        ServerBlur(this, value);
					break;
			}
		}
		protected override void LoadViewState(object savedState)
		{
			object[] state = (object[])(savedState);
			base.LoadViewState(state[0]);
			mItems = (ComboBoxItemCollection) state[1];
			selected = (int)state[2];

            mItems.Refresh(this);
		}
		protected override object SaveViewState()
		{
			object[] state = new object[3];
			state[0] = base.SaveViewState();
			state[1] = mItems;
			state[2] = selected;
			return state;
		}
		public override void PutPostValue(string obj)
		{
			this.SelectedValue = obj;
		}
	}
}
