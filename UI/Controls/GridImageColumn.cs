/*
 * Licensing
 *  Mozilla Public License 1.1 (MPL 1.1)
 * 
 * Fernando Escolar Martínez-Berganza <fer.escolar@gmail.com>
 * 
 */
using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI;

namespace Framework.Ajax.UI.Controls
{
    /// <summary>
    /// Image GridColumnStyle Type
    /// <code>
    /// &lt;ajax:GridImageColumn runat="server"&gt;&lt;/ajax:GridImageColumn&gt;
    /// </code>
    /// </summary>
    [ParseChildren(true)]
    [ToolboxData("<{0}:GridImageColumn runat=\"server\"></{0}:GridImageColumn>")]
    public class GridImageColumn : GridColumnStyle
    {
        protected event AjaxGridEventHandler mServerClick;
        /// <summary>
        /// Raises on a inner image cell is clicked
        /// <remarks>
        /// It is a server event
        /// </remarks>
        /// </summary>
        public event AjaxGridEventHandler ServerClick
        {
            add
            {
                mServerClick += value;
                if (parentGrid != null)
                    foreach (GridRow r in parentGrid.Rows)
                        ((GridImageCell)r.Cells[this.Index]).ServerClick += item_ServerClick;
            }
            remove
            {
                mServerClick -= value;
                if (parentGrid != null)
                    foreach (GridRow r in parentGrid.Rows)
                        ((GridImageCell)r.Cells[this.Index]).ServerClick -= item_ServerClick;
            }

        }

        public override GridView ParentGridView
        {
            get { return base.ParentGridView; }
            internal set
            {
                base.ParentGridView = value;
                if (mServerClick != null)
                    foreach (GridRow r in parentGrid.Rows)
                        ((GridImageCell)r.Cells[this.Index]).ServerClick += item_ServerClick;
            }
        }

        #region Factory
        /// <summary>
        /// Grid column style constructor
        /// </summary>
        public GridImageColumn() : base() {}
        /// <summary>
        /// Grid column style constructor
        /// </summary>
        /// <param name="title">Column Header Title</param>
		public GridImageColumn(string title) : base(title) { }
        /// <summary>
        /// Grid column style constructor
        /// </summary>
        /// <param name="title">Column Header Title</param>
        /// <param name="dataColumn">DataSource column name</param>
        /// <param name="visible">Visibility property</param>
        public GridImageColumn(string title, string dataColumn, bool visible) : base(title, dataColumn, visible) { }
        /// <summary>
        /// Grid column style constructor
        /// </summary>
        /// <param name="title">Column Header Title</param>
        /// <param name="cssClass">Css class name</param>
        public GridImageColumn(string title, string cssClass) : base(title, cssClass) { }
        /// <summary>
        /// Grid column style constructor
        /// </summary>
        /// <param name="title">Column Header Title</param>
        /// <param name="cssClass">Css class name</param>
        /// <param name="width">Column width</param>
        public GridImageColumn(string title, string cssClass, int width) : base(title, cssClass, width) { }
        /// <summary>
        /// Grid column style constructor
        /// </summary>
        /// <param name="title">Column Header Title</param>
        /// <param name="cssClass">Css class name</param>
        /// <param name="width">Column width</param>
        /// <param name="height">Column height</param>
        public GridImageColumn(string title, string cssClass, int width, int height) : base(title, cssClass, width, height) { }
        /// <summary>
        /// Grid column style constructor
        /// </summary>
        /// <param name="title">Column Header Title</param>
        /// <param name="cssClass">Css class name</param>
        /// <param name="width">Column width</param>
        /// <param name="dataColumn">DataSource column name</param>
        public GridImageColumn(string title, string cssClass, int width, string dataColumn) : base(title, cssClass, width, dataColumn) { }
        /// <summary>
        /// Grid column style constructor
        /// </summary>
        /// <param name="title">Column Header Title</param>
        /// <param name="width">Column width</param>
		public GridImageColumn(string title, int width):base(title, width) { }
        /// <summary>
        /// Grid column style constructor
        /// </summary>
        /// <param name="title">Column Header Title</param>
        /// <param name="width">Column width</param>
        /// <param name="dataColumn">DataSource column name</param>
        public GridImageColumn(string title, int width, string dataColumn) : base(title, width, dataColumn) { }
        #endregion

        public override GridCell CreateGridCell()
        {
            GridImageCell item = new GridImageCell(this);
            if (mServerClick != null)
                item.ServerClick += new AjaxEventHandler(item_ServerClick);
            return item;
        }
        public override GridCell CreateGridCell(object value)
        {
            GridImageCell item = (GridImageCell)CreateGridCell();
            item.Value = value;
            return item;
        }

        protected void item_ServerClick(AjaxControl sender, string value)
        {
            if (mServerClick != null)
                mServerClick(sender, parentGrid.SelectedColumn, parentGrid.SelectedRow, value);
        }

        public override void ResetColumn(GridView parentGrid)
        {
            base.ResetColumn(parentGrid);
            mServerClick = null;
        }
    }
}
