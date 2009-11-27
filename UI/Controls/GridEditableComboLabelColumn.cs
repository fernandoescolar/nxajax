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

namespace nxAjax.UI.Controls
{
    /// <summary>
    /// EditableComboLabel GridColumnStyle Type
    /// <code>
    /// &lt;ajax:GridEditableComboLabelColumn runat="server"&gt;&lt;/ajax:GridEditableComboLabelColumn&gt;
    /// </code>
    /// </summary>
    [ParseChildren(true)]
    [ToolboxData("<{0}:GridEditableComboLabelColumn runat=server></{0}:GridEditableComboLabelColumn>")]
    public class GridEditableComboLabelColumn : GridComboboxColumn
    {
        protected event nxGridEventHandler mServerExitEditMode;
        protected event nxGridEventHandler mServerEnterEditMode;

        /// <summary>
        /// Raises on an inner cell enters in edit mode
        /// <remarks>
        /// It is a server event
        /// </remarks>
        /// </summary>
        public event nxGridEventHandler ServerExitEditMode
        {
            add {
                mServerExitEditMode += value;
                if (parentGrid != null)
                    foreach (GridRow r in parentGrid.Rows)
                        ((GridEditableComboLabelCell)r.Cells[this.Index]).ServerExitEditMode += item_ServerExitEditMode;
            }
            remove {
                mServerExitEditMode -= value;
                if (parentGrid != null)
                    foreach (GridRow r in parentGrid.Rows)
                        ((GridEditableComboLabelCell)r.Cells[this.Index]).ServerExitEditMode -= item_ServerExitEditMode;
            }
        }
        /// <summary>
        /// Raises on an inner cell exits from edit mode
        /// <remarks>
        /// It is a server event
        /// </remarks>
        /// </summary>
        public event nxGridEventHandler ServerEnterEditMode
        {
            add
            {
                mServerEnterEditMode += value;
                if (parentGrid != null)
                    foreach (GridRow r in parentGrid.Rows)
                        ((GridEditableComboLabelCell)r.Cells[this.Index]).ServerEnterEditMode += item_ServerEnterEditMode;
            }
            remove
            {
                mServerEnterEditMode -= value;
                if (parentGrid != null)
                    foreach (GridRow r in parentGrid.Rows)
                        ((GridEditableComboLabelCell)r.Cells[this.Index]).ServerEnterEditMode -= item_ServerEnterEditMode;
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
                        ((GridEditableComboLabelCell)r.Cells[this.Index]).ServerExitEditMode += item_ServerExitEditMode;

                if (mServerEnterEditMode != null)
                    foreach (GridRow r in parentGrid.Rows)
                        ((GridEditableComboLabelCell)r.Cells[this.Index]).ServerEnterEditMode += item_ServerEnterEditMode;
            }
        }

        #region Factory
        /// <summary>
        /// Grid column style constructor
        /// </summary>
        public GridEditableComboLabelColumn() : base() {}
        /// <summary>
        /// Grid column style constructor
        /// </summary>
        /// <param name="title">Column Header Title</param>
		public GridEditableComboLabelColumn(string title) : base(title) { }
        /// <summary>
        /// Grid column style constructor
        /// </summary>
        /// <param name="title">Column Header Title</param>
        /// <param name="dataColumn">DataSource column name</param>
        /// <param name="visible">Visibility property</param>
        public GridEditableComboLabelColumn(string title, string dataColumn, bool visible) : base(title, dataColumn, visible) { }
        /// <summary>
        /// Grid column style constructor
        /// </summary>
        /// <param name="title">Column Header Title</param>
        /// <param name="cssClass">Css class name</param>
        public GridEditableComboLabelColumn(string title, string cssClass) : base(title, cssClass) { }
        /// <summary>
        /// Grid column style constructor
        /// </summary>
        /// <param name="title">Column Header Title</param>
        /// <param name="cssClass">Css class name</param>
        /// <param name="width">Column width</param>
        public GridEditableComboLabelColumn(string title, string cssClass, int width) : base(title, cssClass, width) { }
        /// <summary>
        /// Grid column style constructor
        /// </summary>
        /// <param name="title">Column Header Title</param>
        /// <param name="cssClass">Css class name</param>
        /// <param name="width">Column width</param>
        /// <param name="height">Column height</param>
        public GridEditableComboLabelColumn(string title, string cssClass, int width, int height) : base(title, cssClass, width, height) { }
        /// <summary>
        /// Grid column style constructor
        /// </summary>
        /// <param name="title">Column Header Title</param>
        /// <param name="cssClass">Css class name</param>
        /// <param name="width">Column width</param>
        /// <param name="dataColumn">DataSource column name</param>
        public GridEditableComboLabelColumn(string title, string cssClass, int width, string dataColumn) : base(title, cssClass, width, dataColumn) { }
        /// <summary>
        /// Grid column style constructor
        /// </summary>
        /// <param name="title">Column Header Title</param>
        /// <param name="width">Column width</param>
		public GridEditableComboLabelColumn(string title, int width):base(title, width) { }
        /// <summary>
        /// Grid column style constructor
        /// </summary>
        /// <param name="title">Column Header Title</param>
        /// <param name="width">Column width</param>
        /// <param name="dataColumn">DataSource column name</param>
        public GridEditableComboLabelColumn(string title, int width, string dataColumn) : base(title, width, dataColumn) { }
        #endregion

        public override GridComboboxCell NewGridCell()
        {
            GridEditableComboLabelCell item = new GridEditableComboLabelCell(this);
            if (mServerEnterEditMode != null)
                item.ServerEnterEditMode += new nxEventHandler(item_ServerEnterEditMode);
            if (mServerExitEditMode != null)
                item.ServerExitEditMode += new nxEventHandler(item_ServerExitEditMode);
            
            return item;
        }

        protected void item_ServerExitEditMode(nxControl sender, string value)
        {
            if (mServerExitEditMode != null)
            {
                putSelectedCellInParent(sender);
                mServerExitEditMode(sender, parentGrid.SelectedColumn, parentGrid.SelectedRow, value); 
            }
        }

        protected void item_ServerEnterEditMode(nxControl sender, string value)
        {
            if (mServerEnterEditMode != null)
            {
                putSelectedCellInParent(sender);
                mServerEnterEditMode(sender, parentGrid.SelectedColumn, parentGrid.SelectedRow, value);
            }
        }

        public override void ResetColumn(GridView parentGrid)
        {
            base.ResetColumn(parentGrid);
            mServerExitEditMode = null;
            mServerEnterEditMode = null;
        }
    }
}
