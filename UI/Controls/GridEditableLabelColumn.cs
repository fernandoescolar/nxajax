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
    /// EditableLabel GridColumnStyle Type
    /// <code>
    /// &lt;ajax:GridEditableLabelColumn runat="server"&gt;&lt;/ajax:GridEditableLabelColumn&gt;
    /// </code>
    /// </summary>
    [ParseChildren(true)]
    [ToolboxData("<{0}:GridEditableLabelColumn runat=server></{0}:GridEditableLabelColumn>")]
    public class GridEditableLabelColumn : GridTextboxColumn
    {
        protected event AjaxGridEventHandler mServerEnterEditMode;
        protected event AjaxGridEventHandler mServerExitEditMode;

        /// <summary>
        /// Raises on an inner cell enters in edit mode
        /// <remarks>
        /// It is a server event
        /// </remarks>
        /// </summary>
        public event AjaxGridEventHandler ServerExitEditMode
        {
            add
            {
                mServerExitEditMode += value;
                if (parentGrid != null)
                    foreach (GridRow r in parentGrid.Rows)
                        ((GridEditableLabelCell)r.Cells[this.Index]).ServerExitEditMode += item_ServerExitEditMode;
            }
            remove
            {
                mServerExitEditMode -= value;
                if (parentGrid != null)
                    foreach (GridRow r in parentGrid.Rows)
                        ((GridEditableLabelCell)r.Cells[this.Index]).ServerExitEditMode -= item_ServerExitEditMode;
            }
        }
        /// <summary>
        /// Raises on an inner cell exits from edit mode
        /// <remarks>
        /// It is a server event
        /// </remarks>
        /// </summary>
        public event AjaxGridEventHandler ServerEnterEditMode
        {
            add
            {
                mServerEnterEditMode += value;
                if (parentGrid != null)
                    foreach (GridRow r in parentGrid.Rows)
                        ((GridEditableLabelCell)r.Cells[this.Index]).ServerEnterEditMode += item_ServerEnterEditMode;
            }
            remove
            {
                mServerEnterEditMode -= value;
                if (parentGrid != null)
                    foreach (GridRow r in parentGrid.Rows)
                        ((GridEditableLabelCell)r.Cells[this.Index]).ServerEnterEditMode -= item_ServerEnterEditMode;
            }
        }

        public override GridView ParentGridView
        {
            get { return base.ParentGridView; }
            internal set
            {
                base.ParentGridView = value;

                if (mServerExitEditMode != null)
                    foreach (GridRow r in parentGrid.Rows)
                        ((GridEditableLabelCell)r.Cells[this.Index]).ServerExitEditMode += item_ServerExitEditMode;

                if (mServerEnterEditMode != null)
                    foreach (GridRow r in parentGrid.Rows)
                        ((GridEditableLabelCell)r.Cells[this.Index]).ServerEnterEditMode += item_ServerEnterEditMode;
            }
        }

        #region Factory
        /// <summary>
        /// Grid column style constructor
        /// </summary>
        public GridEditableLabelColumn() : base() {}
        /// <summary>
        /// Grid column style constructor
        /// </summary>
        /// <param name="title">Column Header Title</param>
		public GridEditableLabelColumn(string title) : base(title) { }
        /// <summary>
        /// Grid column style constructor
        /// </summary>
        /// <param name="title">Column Header Title</param>
        /// <param name="dataColumn">DataSource column name</param>
        /// <param name="visible">Visibility property</param>
        public GridEditableLabelColumn(string title, string dataColumn, bool visible) : base(title, dataColumn, visible) { }
        /// <summary>
        /// Grid column style constructor
        /// </summary>
        /// <param name="title">Column Header Title</param>
        /// <param name="cssClass">Css class name</param>
        public GridEditableLabelColumn(string title, string cssClass) : base(title, cssClass) { }
        /// <summary>
        /// Grid column style constructor
        /// </summary>
        /// <param name="title">Column Header Title</param>
        /// <param name="cssClass">Css class name</param>
        /// <param name="width">Column width</param>
        public GridEditableLabelColumn(string title, string cssClass, int width) : base(title, cssClass, width) { }
        /// <summary>
        /// Grid column style constructor
        /// </summary>
        /// <param name="title">Column Header Title</param>
        /// <param name="cssClass">Css class name</param>
        /// <param name="width">Column width</param>
        /// <param name="height">Column height</param>
        public GridEditableLabelColumn(string title, string cssClass, int width, int height) : base(title, cssClass, width, height) { }
        /// <summary>
        /// Grid column style constructor
        /// </summary>
        /// <param name="title">Column Header Title</param>
        /// <param name="cssClass">Css class name</param>
        /// <param name="width">Column width</param>
        /// <param name="dataColumn">DataSource column name</param>
        public GridEditableLabelColumn(string title, string cssClass, int width, string dataColumn) : base(title, cssClass, width, dataColumn) { }
        /// <summary>
        /// Grid column style constructor
        /// </summary>
        /// <param name="title">Column Header Title</param>
        /// <param name="width">Column width</param>
		public GridEditableLabelColumn(string title, int width):base(title, width) { }
        /// <summary>
        /// Grid column style constructor
        /// </summary>
        /// <param name="title">Column Header Title</param>
        /// <param name="width">Column width</param>
        /// <param name="dataColumn">DataSource column name</param>
        public GridEditableLabelColumn(string title, int width, string dataColumn) : base(title, width, dataColumn) { }
        #endregion

        public override GridTextboxCell NewGridCell()
        {
            GridEditableLabelCell item = new GridEditableLabelCell(this);
            
            if (mServerEnterEditMode != null)
                item.ServerEnterEditMode += new AjaxEventHandler(item_ServerEnterEditMode);
            if (mServerExitEditMode != null)
                item.ServerExitEditMode += new AjaxEventHandler(item_ServerExitEditMode);

            return item;
        }
      

        protected void item_ServerExitEditMode(AjaxControl sender, string value)
        {
            if (mServerExitEditMode != null)
                mServerExitEditMode(sender, parentGrid.SelectedColumn, parentGrid.SelectedRow, value);
        }

        protected void item_ServerEnterEditMode(AjaxControl sender, string value)
        {
            if (mServerEnterEditMode != null)
                mServerEnterEditMode(sender, parentGrid.SelectedColumn, parentGrid.SelectedRow, value);
        }

        public override void ResetColumn(GridView parentGrid)
        {
            base.ResetColumn(parentGrid);
            mServerExitEditMode = null;
            mServerEnterEditMode = null;
        }
    }
}
