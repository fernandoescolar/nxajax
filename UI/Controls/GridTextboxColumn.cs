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
    /// TextBox GridColumnStyle Type
    /// <code>
    /// &lt;ajax:GridTextboxColumn runat="server"&gt;&lt;/ajax:GridTextboxColumn&gt;
    /// </code>
    /// </summary>
    [ParseChildren(true)]
    [ToolboxData("<{0}:GridTextboxColumn runat=\"server\"></{0}:GridTextboxColumn>")]
    public class GridTextboxColumn : GridColumnStyle
    {
        protected TextboxTypes mTextBoxType;
        protected int mMaxLength;

        /// <summary>
        /// Gets/Sets inner TextBox Cells Type
        /// </summary>
        public TextboxTypes TextBoxType { get { return mTextBoxType; } set { mTextBoxType = value; } }
        /// <summary>
        /// Gets/Sets Maximun length (in characters) of the inner TextBox Cells
        /// </summary>
        public int MaxLength { get { return mMaxLength; } set { mMaxLength = value; } }

        protected event AjaxGridEventHandler mServerChange;
        /// <summary>
        /// Raises on inner Cell value changes
        /// <remarks>
        /// It is a server event
        /// </remarks>
        /// </summary>
        public event AjaxGridEventHandler ServerChange
        {
            add
            {
                mServerChange += value;
                if (parentGrid != null)
                    foreach (GridRow r in parentGrid.Rows)
                        ((GridTextboxCell)r.Cells[this.Index]).ServerChange += item_ServerChange;
            }
            remove
            {
                mServerChange -= value;
                if (parentGrid != null)
                    foreach (GridRow r in parentGrid.Rows)
                        ((GridTextboxCell)r.Cells[this.Index]).ServerChange -= item_ServerChange;
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
        public GridTextboxColumn() : base() {}
        /// <summary>
        /// Grid column style constructor
        /// </summary>
        /// <param name="title">Column Header Title</param>
		public GridTextboxColumn(string title) : base(title) { }
        /// <summary>
        /// Grid column style constructor
        /// </summary>
        /// <param name="title">Column Header Title</param>
        /// <param name="dataColumn">DataSource column name</param>
        /// <param name="visible">Visibility property</param>
        public GridTextboxColumn(string title, string dataColumn, bool visible) : base(title, dataColumn, visible) { }
        /// <summary>
        /// Grid column style constructor
        /// </summary>
        /// <param name="title">Column Header Title</param>
        /// <param name="cssClass">Css class name</param>
        public GridTextboxColumn(string title, string cssClass) : base(title, cssClass) { }
        /// <summary>
        /// Grid column style constructor
        /// </summary>
        /// <param name="title">Column Header Title</param>
        /// <param name="cssClass">Css class name</param>
        /// <param name="width">Column width</param>
        public GridTextboxColumn(string title, string cssClass, int width) : base(title, cssClass, width) { }
        /// <summary>
        /// Grid column style constructor
        /// </summary>
        /// <param name="title">Column Header Title</param>
        /// <param name="cssClass">Css class name</param>
        /// <param name="width">Column width</param>
        /// <param name="height">Column height</param>
        public GridTextboxColumn(string title, string cssClass, int width, int height) : base(title, cssClass, width, height) { }
        /// <summary>
        /// Grid column style constructor
        /// </summary>
        /// <param name="title">Column Header Title</param>
        /// <param name="cssClass">Css class name</param>
        /// <param name="width">Column width</param>
        /// <param name="dataColumn">DataSource column name</param>
        public GridTextboxColumn(string title, string cssClass, int width, string dataColumn) : base(title, cssClass, width, dataColumn) { }
        /// <summary>
        /// Grid column style constructor
        /// </summary>
        /// <param name="title">Column Header Title</param>
        /// <param name="width">Column width</param>
		public GridTextboxColumn(string title, int width):base(title, width) { }
        /// <summary>
        /// Grid column style constructor
        /// </summary>
        /// <param name="title">Column Header Title</param>
        /// <param name="width">Column width</param>
        /// <param name="dataColumn">DataSource column name</param>
        public GridTextboxColumn(string title, int width, string dataColumn) : base(title, width, dataColumn) { }
        #endregion
        public virtual GridTextboxCell NewGridCell()
        {
            return new GridTextboxCell(this);
        }
        public override GridCell CreateGridCell()
        {
            GridTextboxCell item = NewGridCell();
            item.TextboxType = mTextBoxType;
            item.MaxLength = mMaxLength;

            if (mServerChange != null)
                item.ServerChange += new AjaxEventHandler(item_ServerChange);
            return item;
        }
        public override GridCell CreateGridCell(object value)
        {
            GridTextboxCell item = (GridTextboxCell)CreateGridCell();
            item.Value = value;
            return item;
        }

        protected void item_ServerChange(AjaxControl sender, string value)
        {
            if (mServerChange != null)
            {
                putSelectedCellInParent(sender);
                mServerChange(sender, parentGrid.SelectedColumn, parentGrid.SelectedRow, value);
            }
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
            mTextBoxType = (TextboxTypes)pieces[1];
            mMaxLength = (int)pieces[2];

        }
        public override object SaveViewState()
        {
            object[] res = new object[3];
            res[0] = base.SaveViewState();
            res[1] = mTextBoxType;
            res[2] = mMaxLength;
            return (res);
        }
    }
}
