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
    /// Label GridColumnStyle Type
    /// <code>
    /// &lt;ajax:GridLabelColumn runat="server"&gt;&lt;/ajax:GridLabelColumn&gt;
    /// </code>
    /// </summary>
    [ParseChildren(true)]
    [ToolboxData("<{0}:GridLabelColumn runat=\"server\"></{0}:GridLabelColumn>")]
    public class GridLabelColumn : GridColumnStyle
    {

        protected string mFormat;
        protected GridLabelCellType mGridLabelCellType;

        /// <summary>
        /// Gets/Sets inner GridLabelCells Type
        /// </summary>
        public GridLabelCellType GridLabelCellType
        {
            get { return mGridLabelCellType; }
            set { mGridLabelCellType = value; }
        }
        /// <summary>
        /// Gets/Sets inner GridLabelCells string format
        /// </summary>
        public string Format
        {
            get { return mFormat; }
            set { mFormat = value; }
        }
        #region Factory
        /// <summary>
        /// Grid column style constructor
        /// </summary>
        public GridLabelColumn() : base() {}
        /// <summary>
        /// Grid column style constructor
        /// </summary>
        /// <param name="title">Column Header Title</param>
		public GridLabelColumn(string title) : base(title) { }
        /// <summary>
        /// Grid column style constructor
        /// </summary>
        /// <param name="title">Column Header Title</param>
        /// <param name="dataColumn">DataSource column name</param>
        /// <param name="visible">Visibility property</param>
        public GridLabelColumn(string title, string dataColumn, bool visible) : base(title, dataColumn, visible) { }
        /// <summary>
        /// Grid column style constructor
        /// </summary>
        /// <param name="title">Column Header Title</param>
        /// <param name="cssClass">Css class name</param>
        public GridLabelColumn(string title, string cssClass) : base(title, cssClass) { }
        /// <summary>
        /// Grid column style constructor
        /// </summary>
        /// <param name="title">Column Header Title</param>
        /// <param name="cssClass">Css class name</param>
        /// <param name="width">Column width</param>
        public GridLabelColumn(string title, string cssClass, int width) : base(title, cssClass, width) { }
        /// <summary>
        /// Grid column style constructor
        /// </summary>
        /// <param name="title">Column Header Title</param>
        /// <param name="cssClass">Css class name</param>
        /// <param name="width">Column width</param>
        /// <param name="height">Column height</param>
        public GridLabelColumn(string title, string cssClass, int width, int height) : base(title, cssClass, width, height) { }
        /// <summary>
        /// Grid column style constructor
        /// </summary>
        /// <param name="title">Column Header Title</param>
        /// <param name="cssClass">Css class name</param>
        /// <param name="width">Column width</param>
        /// <param name="dataColumn">DataSource column name</param>
        public GridLabelColumn(string title, string cssClass, int width, string dataColumn) : base(title, cssClass, width, dataColumn) { }
        /// <summary>
        /// Grid column style constructor
        /// </summary>
        /// <param name="title">Column Header Title</param>
        /// <param name="width">Column width</param>
		public GridLabelColumn(string title, int width):base(title, width) { }
        /// <summary>
        /// Grid column style constructor
        /// </summary>
        /// <param name="title">Column Header Title</param>
        /// <param name="width">Column width</param>
        /// <param name="dataColumn">DataSource column name</param>
        public GridLabelColumn(string title, int width, string dataColumn):base(title, width, dataColumn) { }
        /// <summary>
        /// Grid column style constructor
        /// </summary>
        /// <param name="title">Column Header Title</param>
        /// <param name="width">Column width</param>
        /// <param name="dataColumn">DataSource column name</param>
        /// <param name="type">Tipo de valor contenido</param>
        public GridLabelColumn(string title, int width, string dataColumn, GridLabelCellType type) : base(title, width, dataColumn) 
        {
            mGridLabelCellType = type;
        }
        /// <summary>
        /// Grid column style constructor
        /// </summary>
        /// <param name="title">Column Header Title</param>
        /// <param name="width">Column width</param>
        /// <param name="dataColumn">DataSource column name</param>
        /// <param name="type">Tipo de valor contenido</param>
        /// <param name="mFormat">Formato del objeto para mostrar</param>
        public GridLabelColumn(string title, int width, string dataColumn, GridLabelCellType type, string format) : this(title, width, dataColumn, type)
        {
            mFormat = format;
        }
        #endregion

        public override GridCell CreateGridCell()
        {
            if (this.GridLabelCellType != GridLabelCellType.Text)
            {
                if (this.Format != string.Empty)
                    return new GridLabelCell(this, this.GridLabelCellType, this.Format);
                else
                    return new GridLabelCell(this, this.GridLabelCellType);
            }
            else
                return new GridLabelCell(this);
        }
        public override GridCell CreateGridCell(object value)
        {
            GridCell g = CreateGridCell();
            g.Value = value;
            return g;
        }

        public override void LoadViewState(object savedState)
        {
            object[] pieces = savedState as object[];
            base.LoadViewState(pieces[0]);
            mFormat = (string)pieces[1];
            mGridLabelCellType = (GridLabelCellType)pieces[2];

        }
        public override object SaveViewState()
        {
            object[] res = new object[3];
            res[0] = base.SaveViewState();
            res[1] = mFormat;
            res[2] = mGridLabelCellType;
            return (res);
        }
    }
}
