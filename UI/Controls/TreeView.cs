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
    /// TextArea control
    /// <code>
    /// &lt;ajax:TreeView runat="server"&gt;&lt;/ajax:TreeView&gt;
    /// </code>
    /// </summary>
    [AspNetHostingPermission(SecurityAction.Demand, Level = AspNetHostingPermissionLevel.Minimal)]
    [AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    [Designer("nxAjax.UI.Design.nxControlDesigner")]
	[ToolboxData("<{0}:TreeView runat=\"server\"></{0}:TreeView>")]
    [ParseChildren(true, "Nodes")]
    [PersistChildren(false)]
    [DefaultProperty("Nodes")]
    [ToolboxItem(true)]
    [ToolboxBitmap(typeof(nxControl), "images.TreeView.bmp")]
	public class TreeView : nxControl
	{
		#region Private Attributes
		private TreeNodeCollection nodes;
		private string selected, imagesFolder;
		#endregion

		#region Public Properties
        /// <summary>
        /// Gets inner TreeNode Collection
        /// </summary>
		[Category("Appearance"), DefaultValue(""), Description("Nodes contained in control.")]
		public TreeNodeCollection Nodes	{ get { return nodes; }	}
        /// <summary>
        /// Gets/Sets Selected TreeNode Value
        /// </summary>
		[Category("Appearance"), DefaultValue(""), Description("Selected Node Value.")]
		public string SelectedValue { get { return selected; } set { if (nodes.Search(value)!=null) selected = value; } }
        /// <summary>
        /// Gets Selected TreeNode
        /// </summary>
		[Category("Appearance"), DefaultValue(""), Description("Selected Node.")]
		public TreeNode SelectedNode { get { return nodes.Search(selected); } }
        /// <summary>
        /// Gets/Sets Selected TreeNode Text
        /// </summary>
		[Category("Appearance"), DefaultValue(""), Description("Selected Node Text.")]
		public string SelectedText { get { TreeNode n = nodes.Search(selected); if (n==null) return ""; else return n.Text; } }
        /// <summary>
        /// Gets/Sets TreeNode Images Folder Path
        /// </summary>
		[Category("Appearance"), DefaultValue(""), Description("Web Folder were images are")]
		public string ImagesFolder	{ get { return imagesFolder; }	set { imagesFolder = value; if(!imagesFolder.EndsWith("/")) imagesFolder += "/";} }
		#endregion

		#region Public Server Events
        /// <summary>
        /// Raises on Selected TreeNode change
        /// </summary>
        public event nxEventHandler ServerChange;
		#endregion

        /// <summary>
        /// Creates a new TreeView control
        /// </summary>
		public TreeView():base("ul") { nodes = new TreeNodeCollection(); imagesFolder = selected = ""; }

		#region Renders
		public override void RenderHTML(nxAjaxTextWriter writer)
		{
            RenderBeginTag(writer);

            if (DesignMode)
                writer.Write(renderJSNodes(nodes, 0));

            RenderEndTag(writer);

            if (PostBackMode == PostBackMode.Async && LoadingImg != string.Empty)
                RenderLoadingImage(writer);
		}
        public override void RenderJS(nxAjaxTextWriter writer)
		{
            base.RenderJS(writer);
            if (hasChanged || !nxPage.IsPostBack)
            {
                writer.Write("$('#" + ID + "').html('" + renderJSNodes(nodes, 0).Replace("'", "\\'") + "');");
                hasChanged = false;
            }
            if (!nxPage.IsPostBack)
                writer.Write("$('#" + ID + "').treeview();");
		}
        private void renderImg(nxAjaxTextWriter writer, string src)
        {
            writer.WriteBeginTag("img");
            writer.WriteAttribute("src", imagesFolder + src);
            writer.WriteAttribute("align", "absmiddle");
            writer.WriteAttribute("border", "0");
            //XHTML
            writer.Write(nxAjaxTextWriter.SelfClosingTagEnd);
            //HTML 4.0
            //writer.Write(nxAjaxTextWriter.TagRightChar);
        }
        private void renderNodeLine(nxAjaxTextWriter writer, TreeNode node)
        {
            writer.WriteBeginTag("span");
            if (node.CssClass != string.Empty)
                writer.WriteAttribute("class", node.CssClass);
            writer.Write(nxAjaxTextWriter.TagRightChar);

            if (ServerChange != null && node.Nodes.Count <= 0)
            {
                writer.WriteBeginTag("a");
                writer.WriteAttribute("href", "#");
                writer.WriteAttribute("onclick", nxPage.GetPostBackWithValueAjaxEvent(this, "onchange", "'" + node.Value + "'"));
                writer.Write(nxAjaxTextWriter.TagRightChar);
            }
            
            if (node.Image != string.Empty)
                renderImg(writer, node.Image);

            writer.Write(HttpUtility.HtmlEncode(node.Text));

            if (ServerChange != null && node.Nodes.Count <= 0)
                writer.WriteEndTag("a");
            writer.WriteEndTag("span");
        }
        private void renderNode(nxAjaxTextWriter writer, TreeNode node)
        {
            writer.WriteBeginTag("li");
            if (node.Nodes.Count > 0)
                writer.WriteAttribute("class", node.IsOpened ? (node.Nodes.Count > 0) ? "open collapsable" : "open" : (node.Nodes.Count > 0) ? "closed expandable" : "closed");
            writer.Write(nxAjaxTextWriter.TagRightChar);

            renderNodeLine(writer, node);

            if (node.Nodes.Count > 0)
            {
                writer.WriteFullBeginTag("ul");
                foreach (TreeNode t in node.Nodes)
                    renderNode(writer, t);
                writer.WriteEndTag("ul");
            }
            
            writer.WriteEndTag("li");
        }
		private string renderJSNodes(TreeNodeCollection tnc, int tabs)
		{
            nxAjaxTextWriter writer = new nxAjaxTextWriter();
            foreach (TreeNode node in tnc)
                renderNode(writer, node);
			return writer.ToString();
		}
		#endregion

		public override void AjaxUpdate()
		{
			this.hasChanged = true;
		}
		public override void AjaxNotUpdate()
		{
			this.hasChanged = false;
		}

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
			nodes = (TreeNodeCollection)state[1];
			selected = (string)state[2];
		}
		protected override object SaveViewState()
		{
			object[] state = new object[3];
			state[0] = base.SaveViewState();
			state[1] = nodes;
			state[2] = selected;
			return state;
		}
		public override void PutPostValue(string obj)
		{
			this.selected = obj;
		}
	}
}
