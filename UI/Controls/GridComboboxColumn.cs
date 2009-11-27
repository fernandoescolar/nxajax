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
    /// CheckBox GridColumnStyle Type
    /// <code>
    /// &lt;ajax:GridComboboxColumn runat="server"&gt;&lt;/ajax:GridComboboxColumn&gt;
    /// </code>
    /// </summary>
    [ParseChildren(true)]
    [ToolboxData("<{0}:GridComboboxColumn runat=\"server\"></{0}:GridComboboxColumn>")]
    public class GridComboboxColumn : GridColumnStyle
    {
        protected ComboBoxItemCollection mItems = new ComboBoxItemCollection();

        /// <summary>
        /// Gets Inner Cells ComBoxItems Collection
        /// </summary>
        public ComboBoxItemCollection Items { get { return mItems; } }

        protected event nxGridEventHandler mServerChange;
        /// <summary>
        /// Raises on a Cell Selected item change
        /// <remarks>
        /// It is a server event
        /// </remarks>
        /// </summary>
        public event nxGridEventHandler ServerChange
        {
            add
            {
                mServerChange += value;
                if (parentGrid != null)
                    foreach (GridRow r in parentGrid.Rows)
                        ((GridComboboxCell)r.Cells[this.Index]).ServerChange += item_ServerChange;
            }
            remove
            {
                mServerChange -= value;
                if (parentGrid != null)
                    foreach (GridRow r in parentGrid.Rows)
                        ((GridComboboxCell)r.Cells[this.Index]).ServerChange -= item_ServerChange;
            }
        }

        public override GridView ParentGridView
        {
            get { return base.ParentGridView; }
            internal set
            {
                base.ParentGridView = value;
                if (mServerChange != null)
                    foreach (GridRow r in parentGrid.Rows)
                        ((GridTextboxCell)r.Cells[this.Index]).ServerChange += item_ServerChange;
            }
        }

        #region Factory
        /// <summary>
        /// Grid column style constructor
        /// </summary>
        public GridComboboxColumn() : base() {}
        /// <summary>
        /// Grid column style constructor
        /// </summary>
        /// <param name="title">Column Header Title</param>
		public GridComboboxColumn(string title) : base(title) { }
        /// <summary>
        /// Grid column style constructor
        /// </summary>
        /// <param name="title">Column Header Title</param>
        /// <param name="dataColumn">DataSource column name</param>
        /// <param name="visible">Visibility property</param>
        public GridComboboxColumn(string title, string dataColumn, bool visible) : base(title, dataColumn, visible) { }
        /// <summary>
        /// Grid column style constructor
        /// </summary>
        /// <param name="title">Column Header Title</param>
        /// <param name="cssClass">Css class name</param>
        public GridComboboxColumn(string title, string cssClass) : base(title, cssClass) { }
        /// <summary>
        /// Grid column style constructor
        /// </summary>
        /// <param name="title">Column Header Title</param>
        /// <param name="cssClass">Css class name</param>
        /// <param name="width">Column width</param>
        public GridComboboxColumn(string title, string cssClass, int width) : base(title, cssClass, width) { }
        /// <summary>
        /// Grid column style constructor
        /// </summary>
        /// <param name="title">Column Header Title</param>
        /// <param name="cssClass">Css class name</param>
        /// <param name="width">Column width</param>
        /// <param name="height">Column height</param>
        public GridComboboxColumn(string title, string cssClass, int width, int height) : base(title, cssClass, width, height) { }
        /// <summary>
        /// Grid column style constructor
        /// </summary>
        /// <param name="title">Column Header Title</param>
        /// <param name="cssClass">Css class name</param>
        /// <param name="width">Column width</param>
        /// <param name="dataColumn">DataSource column name</param>
        public GridComboboxColumn(string title, string cssClass, int width, string dataColumn) : base(title, cssClass, width, dataColumn) { }
        /// <summary>
        /// Grid column style constructor
        /// </summary>
        /// <param name="title">Column Header Title</param>
        /// <param name="width">Column width</param>
		public GridComboboxColumn(string title, int width):base(title, width) { }
        /// <summary>
        /// Grid column style constructor
        /// </summary>
        /// <param name="title">Column Header Title</param>
        /// <param name="width">Column width</param>
        /// <param name="dataColumn">DataSource column name</param>
        public GridComboboxColumn(string title, int width, string dataColumn) : base(title, width, dataColumn) { }
        #endregion

        /// <summary>
        /// Creates a new ComboBox GridCell
        /// </summary>
        /// <returns></returns>
        public virtual GridComboboxCell NewGridCell()
        {
            return new GridComboboxCell(this);
        }
        public override GridCell CreateGridCell()
        {
            GridComboboxCell item = NewGridCell();
            foreach (ComboBoxItem i in mItems)
                item.Items.Add(i);

            if (mServerChange != null)
                item.ServerChange += new nxEventHandler(item_ServerChange);

            return item;
        }
        public override GridCell CreateGridCell(object value)
        {
            GridComboboxCell item = (GridComboboxCell)CreateGridCell();
            item.Value = value;

            return item;
        }

        public override void ResetColumn(GridView parentGrid)
        {
            base.ResetColumn(parentGrid);
            mServerChange = null;
        }

        public override void LoadViewState(object savedState)
        {
            object[] pieces = savedState as object[];
            base.LoadViewState(pieces[0]);
            mItems = (ComboBoxItemCollection)pieces[1];
           
        }
        public override object SaveViewState()
        {
            object[] res = new object[2];
            res[0] = base.SaveViewState();
            res[1] = mItems;
            return (res);
        }

        protected void item_ServerChange(nxControl sender, string value)
        {
            if (mServerChange != null)
            {
                putSelectedCellInParent(sender);
                mServerChange(sender, parentGrid.SelectedColumn, parentGrid.SelectedRow, value);
            }
        }
    }
}
