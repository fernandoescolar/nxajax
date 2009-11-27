/*
 * Licensing
 *  Mozilla Public License 1.1 (MPL 1.1)
 * 
 * Fernando Escolar Martínez-Berganza <fer.escolar@gmail.com>
 * 
 */
using System;
using System.Security.Permissions;
using System.Web;
using System.Web.UI;
using System.Web.UI.Design;

namespace nxAjax.UI.Controls
{
	/// <summary>
	/// MenuItem 
	/// </summary>
    [AspNetHostingPermission(SecurityAction.Demand, Level = AspNetHostingPermissionLevel.Minimal)]
    [AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    [ToolboxData("<{0}:MenuItem runat=\"server\"></{0}:Menu>")]
	public class MenuItem
	{
		private string text, value, key;
		private object tag;
		private MenuItem parent = null;
		private MenuItemCollection items ;

        /// <summary>
        /// Gets/Sets MenuItem text value
        /// </summary>
		public string Text { get { return text; } set { text = value; }}
        /// <summary>
        /// Gets/Sets MenuItem Value
        /// </summary>
		public string Value { get { return value; } set { this.value = value; }}
        /// <summary>
        /// Gets/Sets MenuItem Key value
        /// </summary>
		public string Key { get { return key; } set { this.key = value; }}
        /// <summary>
        /// Gets/Sets MenuItem tag object
        /// </summary>
		public object Tag { get { return tag; } set { this.tag = value; }}
        /// <summary>
        /// Gets/Sets Parent Menu
        /// </summary>
		public MenuItem Parent { get { return parent; } set { this.parent = value; }}
        /// <summary>
        /// Gets Sub MenuItems Collection
        /// </summary>
		public MenuItemCollection Items { get { return items; } }

        /// <summary>
        /// Creates a new MenuItem
        /// </summary>
		public MenuItem() { text = value = ""; items = new MenuItemCollection(this); }
        /// <summary>
        /// Creates a new MenuItem
        /// </summary>
        /// <param name="text">Text value</param>
        /// <param name="value">MenuItem Value</param>
		public MenuItem(string text, string value):this() { this.text = text; this.value = value; }
        /// <summary>
        /// Creates a new MenuItem
        /// </summary>
        /// <param name="text">Text value</param>
        /// <param name="value">MenuItem Value</param>
        /// <param name="key">Key value</param>
		public MenuItem(string text, string value, string key) :this(text, value) { this.key = key; }
        /// <summary>
        /// Creates a new MenuItem
        /// </summary>
        /// <param name="text">Text value</param>
        /// <param name="value">MenuItem Value</param>
        /// <param name="key">Key value</param>
        /// <param name="tag">Tag object</param>
		public MenuItem(string text, string value, string key, object tag) :this(text, value, key) { this.tag = tag; }
	}
}
