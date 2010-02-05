/*
 * Licensing
 *  Mozilla Public License 1.1 (MPL 1.1)
 * 
 * Fernando Escolar Martínez-Berganza <fer.escolar@gmail.com>
 * 
 */
using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Security.Permissions;
using System.Drawing.Design;
using System.Web;
using System.Web.UI;
using System.Web.UI.Design;

namespace Framework.Ajax.UI.Controls
{
    /// <summary>
    /// TreeNode Object
    /// <code>
    /// &lt;ajax:TreeNode runat="server"&gt;&lt;/ajax:TreeNode&gt;
    /// </code>
    /// </summary> 
    [AspNetHostingPermission(SecurityAction.Demand, Level = AspNetHostingPermissionLevel.Minimal)]
    [AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    [ParseChildren(true, "Nodes")]
    [PersistChildren(false)]
    [DefaultProperty("Nodes")]
    [ToolboxData("<{0}:TreeNode runat=\"server\"></{0}:TreeNode>")]
	public class TreeNode
	{
		private string text, value, cssClass;
		private string image;
		private TreeNodeCollection nodes;
        private bool isOpen = false;

        /// <summary>
        /// Gets inner TreeNodes
        /// </summary>
		public TreeNodeCollection Nodes { get { return nodes; } }
        /// <summary>
        /// Gets/Sets Image file path
        /// </summary>
		public string Image { get { return image; } set { image = value; } }
        /// <summary>
        /// Gets/Sets TreeNode Text value
        /// </summary>
		public string Text { get { return text; } set { text = value; }}
        /// <summary>
        /// Gets/Sets TreeNode value
        /// </summary>
		public string Value { get { return value; } set { this.value = value; }}
        /// <summary>
        /// Gets/Sets TreeNode css class name
        /// </summary>
        public string CssClass { get { return cssClass; } set { this.cssClass = value; } }
        /// <summary>
        /// Gets/Sets if TreeNode is Opened
        /// </summary>
        public bool IsOpened { get { return isOpen; } set { isOpen = value; } }

        /// <summary>
        /// Creates a new TreeNode
        /// </summary>
        public TreeNode() { cssClass = text = value = image = ""; nodes = new TreeNodeCollection(); }
        /// <summary>
        /// Creates a new TreeNode
        /// </summary>
        /// <param name="text">TreeNode Text value</param>
        /// <param name="value">TreeNode value</param>
		public TreeNode(string text, string value):this() { this.text = text; this.value = value; }
        /// <summary>
        /// Creates a new TreeNode
        /// </summary>
        /// <param name="text">TreeNode Text value</param>
        /// <param name="value">TreeNode value</param>
        /// <param name="cssClass">Css class name</param>
        public TreeNode(string text, string value, string cssClass) : this(text, value) { this.cssClass = cssClass; }
        /// <summary>
        /// Creates a new TreeNode
        /// </summary>
        /// <param name="text">TreeNode Text value</param>
        /// <param name="value">TreeNode value</param>
        /// <param name="cssClass">Css class name</param>
        /// <param name="opened">Initial state</param>
        public TreeNode(string text, string value, string cssClass, bool opened) : this(text, value, cssClass) { this.isOpen = opened; }
        /// <summary>
        /// Creates a new TreeNode
        /// </summary>
        /// <param name="text">TreeNode Text value</param>
        /// <param name="value">TreeNode value</param>
        /// <param name="cssClass">Css class name</param>
        /// <param name="image">TreeNode image path</param>
        public TreeNode(string text, string value, string cssClass, string image) : this(text, value, cssClass) { this.image = image; }
        /// <summary>
        /// Creates a new TreeNode
        /// </summary>
        /// <param name="text">TreeNode Text value</param>
        /// <param name="value">TreeNode value</param>
        /// <param name="cssClass">Css class name</param>
        /// <param name="image">TreeNode image path</param>
        /// <param name="opened">Initial state</param>
        public TreeNode(string text, string value, string cssClass, string image, bool opened) : this(text, value, cssClass, image) { this.isOpen = opened; }
	}
}
