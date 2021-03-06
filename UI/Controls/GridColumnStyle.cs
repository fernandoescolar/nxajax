/*
 * Licensing
 *  Mozilla Public License 1.1 (MPL 1.1)
 * 
 * Fernando Escolar Martínez-Berganza <fer.escolar@gmail.com>
 * 
 */
using System;
using System.Web.UI;
using System.Security.Permissions;
using System.Web;
using System.ComponentModel;


namespace Framework.Ajax.UI.Controls
{
	/// <summary>
	/// Column Style base object
	/// </summary>
    [AspNetHostingPermission(SecurityAction.Demand, Level = AspNetHostingPermissionLevel.Minimal)]
    [AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    [TypeConverter(typeof(System.ComponentModel.ExpandableObjectConverter))]
    //[ParseChildren(true)]
    [ToolboxData("<{0}:GridColumnStyle runat=\"server\"></{0}:GridColumnStyle>")]
	public abstract class GridColumnStyle : IStateManager
	{
        protected GridView parentGrid;
        bool tracking_viewstate;

        protected string jsonSelect, mDataColumn, mHeaderTitle, mCssClass, mID;
        protected int mWidth, mHeight, mIndex;
        protected bool mVisible = true;

        /// <summary>
        /// Gets/Sets Column Style ID
        /// </summary>
        public string ID { get { return mID; } set { mID = value; } }
        /// <summary>
        /// Gets/Sets Css class name
        /// </summary>
        public string CssClass { get { return mCssClass; } set { mCssClass = value; } }
        /// <summary>
        /// Gets/Sets Column Index
        /// </summary>
        public int Index { get { return mIndex; } set { mIndex = value; } }
        /// <summary>
        /// Gets/Sets Column Header Title text
        /// </summary>
        public string HeaderTitle { get { return mHeaderTitle; } set { mHeaderTitle = value; } }
        /// <summary>
        /// Gets/Sets Column Width
        /// </summary>
        public int Width { get { return mWidth; } set { mWidth = value; } }
        /// <summary>
        /// Gets/Sets Column Height
        /// </summary>
        public int Height { get { return mHeight; } set { mHeight = value; } }
        /// <summary>
        /// Gets/Sets data column name
        /// </summary>
		public string DataColumn { get { return mDataColumn; } set { mDataColumn=value; }}
        /// <summary>
        /// Gets Parent GridView control
        /// </summary>
        public virtual GridView ParentGridView { get { return parentGrid; } internal set { parentGrid = value; } }
        /// <summary>
        /// Gets/Sets Visibility property
        /// </summary>
        public bool Visible { get { return mVisible; } set { mVisible = value; } }

        /// <summary>
        /// Gets/Sets on select javascript event
        /// <remarks>
        /// It is a client event
        /// </remarks>
        /// </summary>
        public string ClientSelect { get { return jsonSelect; } set { jsonSelect = value; } }
        /// <summary>
        /// Raises on GridCell in the column is seleted
        /// <remarks>
        /// It is a server event
        /// </remarks>
        /// </summary>
        public event AjaxGridEventHandler ServerSelect;

        #region Factory
        /// <summary>
        /// Grid column style constructor
        /// </summary>
        public GridColumnStyle()
		{
			mHeaderTitle = jsonSelect = mDataColumn = "";
			mWidth = mHeight = -1;
		}
        /// <summary>
        /// Grid column style constructor
        /// </summary>
        /// <param name="title">Column Header Title</param>
		public GridColumnStyle(string title):this()
		{
			this.mHeaderTitle = title;
		}
        /// <summary>
        /// Grid column style constructor
        /// </summary>
        /// <param name="title">Column Header Title</param>
        /// <param name="dataColumn">DataSource column name</param>
        /// <param name="visible">visibility property</param>
        public GridColumnStyle(string title, string dataColumn, bool visible):this(title, 0, dataColumn)
        {
            this.Visible = visible;
        }
        /// <summary>
        /// Grid column style constructor
        /// </summary>
        /// <param name="title">Column Header Title</param>
        /// <param name="cssClass">Css class name</param>
		public GridColumnStyle(string title, string cssClass):this(title) {
			this.CssClass = cssClass;
		}
        /// <summary>
        /// Grid column style constructor
        /// </summary>
        /// <param name="title">Column Header Title</param>
        /// <param name="cssClass">Css class name</param>
        /// <param name="width">Column width</param>
		public GridColumnStyle(string title, string cssClass, int width):this(title, cssClass)
		{
			this.mWidth = width;
		}
        /// <summary>
        /// Grid column style constructor
        /// </summary>
        /// <param name="title">Column Header Title</param>
        /// <param name="cssClass">Css class name</param>
        /// <param name="width">Column width</param>
        /// <param name="height">Column height</param>
        public GridColumnStyle(string title, string cssClass, int width, int height):this(title, cssClass, width)
        {
            this.mHeight = height;
        }
        /// <summary>
        /// Grid column style constructor
        /// </summary>
        /// <param name="title">Column Header Title</param>
        /// <param name="cssClass">Css class name</param>
        /// <param name="width">Column width</param>
        /// <param name="dataColumn">DataSource column name</param>
        public GridColumnStyle(string title, string cssClass, int width, string dataColumn):this(title, cssClass, width)
        {
            this.mDataColumn = dataColumn;
        }
        /// <summary>
        /// Grid column style constructor
        /// </summary>
        /// <param name="title">Column Header Title</param>
        /// <param name="width">Column width</param>
		public GridColumnStyle(string title, int width):this(title)
		{
			this.mWidth = width;
		}
        /// <summary>
        /// Grid column style constructor
        /// </summary>
        /// <param name="title">Column Header Title</param>
        /// <param name="width">Column width</param>
        /// <param name="dataColumn">DataSource column name</param>
        public GridColumnStyle(string title, int width, string dataColumn):this(title, width)
		{
            this.mDataColumn = dataColumn;
		}
        #endregion

        internal bool DesignMode
        {
            get { return parentGrid.isInDesignMode; }
        }
        /// <summary>
        /// Returns javascipt events
        /// </summary>
        /// <param name="cell">Source GridCell</param>
        /// <returns>javascript code</returns>
        public string getJavaScript(GridCell cell)
		{
			string js = "";
			if(hasJavacript())
			{
                if (jsonSelect != "")
                    js += jsonSelect;
                if (ServerSelect != null)
                    js += parentGrid.AjaxController.GetPostBackWithValueAjaxEvent(parentGrid, "onselect", "'" + cell.RowIndex + ";" + mIndex + ";'");
			}
			return js;
		}
        /// <summary>
        /// Returns if the column has javascript code
        /// </summary>
        /// <returns></returns>
		public bool hasJavacript()
		{
			return (ServerSelect!=null || jsonSelect != "");
		}
        /// <summary>
        /// Renders Column Header
        /// </summary>
        /// <param name="writer">Tag writer</param>
        public void RenderHeaderCell(AjaxTextWriter writer)
        {
            writer.WriteBeginTag("td");
            if (mWidth > 0)
                writer.WriteAttribute("style", "width:" + mWidth);
            writer.Write(AjaxTextWriter.TagRightChar);
            if (!parentGrid.isInDesignMode)
                if (parentGrid.Sortable)
                {
                    writer.WriteBeginTag("a");
                    writer.WriteAttribute("href", "javascript:" + parentGrid.AjaxController.GetPostBackWithValueAjaxEvent(parentGrid, "onsort", "'" + mIndex.ToString() + "'"));
                    writer.Write(AjaxTextWriter.TagRightChar);
                }

            writer.WriteEncodedText(mHeaderTitle);

            if (!parentGrid.isInDesignMode)
                if (parentGrid.Sortable)
                    writer.WriteEndTag("a");

            if (parentGrid.Sortable && parentGrid.SortedIndex == mIndex)
            {
                writer.Write(" " + ((parentGrid.SortedAsc) ? "↑" : "↓"));
            }

            writer.WriteEndTag("td");
        }
        /// <summary>
        /// Raises an event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="value">object value</param>
		public void RaiseEvent(AjaxControl sender, string value)
		{
			if (ServerSelect!=null)
                ServerSelect(sender, parentGrid.SelectedColumn, parentGrid.SelectedRow, value);
		}

        /// <summary>
        /// Creates a new GridCell for this Column Style
        /// </summary>
        /// <returns>New GridCell</returns>
        public abstract GridCell CreateGridCell();
        /// <summary>
        /// Creates a new GridCell for this Column Style
        /// </summary>
        /// <param name="value">GridCell initial value</param>
        /// <returns>New GridCell</returns>
        public abstract GridCell CreateGridCell(object value);

        /// <summary>
        /// Reset column values
        /// </summary>
        /// <param name="parentGrid">Parent GridView Control</param>
        public virtual void ResetColumn(GridView parentGrid)
        {
            this.parentGrid = parentGrid;
            ServerSelect = null;
        }

        protected virtual void putSelectedCellInParent(AjaxControl sender)
        {
            parentGrid.SelectedColumn = Index;
            foreach (GridRow r in parentGrid.Rows)
                if (r.Cells[this.Index].InnerControl == sender)
                    parentGrid.SelectedRow = r.Index;
        }
        /// <summary>
        /// Load view-state
        /// </summary>
        /// <param name="savedState">Saved view-state object</param>
        public virtual void LoadViewState(object savedState)
        {
            object[] pieces = savedState as object[];

            if (pieces == null)
            {
                return;
            }

            mID = (string)pieces[0];
            mCssClass = (string)pieces[1];
            mHeaderTitle = (string)pieces[2];
            mDataColumn = (string)pieces[3];
            jsonSelect = (string)pieces[4];
            mIndex = (int)pieces[5];
            mHeight = (int)pieces[6];
            mWidth = (int)pieces[7];
            mVisible = (bool)pieces[8];
        }
        /// <summary>
        /// Saves view-state
        /// </summary>
        /// <returns>Saved view-state object</returns>
        public virtual object SaveViewState()
        {
            object[] res = new object[9];
            res[0] = mID;
            res[1] = mCssClass;
            res[2] = mHeaderTitle;
            res[3] = mDataColumn;
            res[4] = jsonSelect;
            res[5] = mIndex;
            res[6] = mHeight;
            res[7] = mWidth;
            res[8] = mVisible;
            return (res);
        }
        /// <summary>
        /// Causes tracking of view-state changes to the GridColumnStyle
        /// </summary>
        public virtual void TrackViewState()
        {
            tracking_viewstate = true;
        }
        /// <summary>
        /// Gets tracking of view-state state
        /// </summary>
        public bool IsTrackingViewState
        {
            get
            {
                return (tracking_viewstate);
            }
        }

	}
}
