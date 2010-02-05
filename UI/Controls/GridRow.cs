/*
 * Licensing
 *  Mozilla Public License 1.1 (MPL 1.1)
 * 
 * Fernando Escolar Martínez-Berganza <fer.escolar@gmail.com>
 * 
 */
using System;
using System.Collections;

namespace Framework.Ajax.UI.Controls
{
	/// <summary>
	/// Grid Row object
	/// </summary>
	public class GridRow
	{
        private GridView parentGrid;
		private GridCellCollection cells;
		private string mCssClass;
        protected int mIndex;
        private bool mVisible;

        /// <summary>
        /// Gets if Row is in edit mode state
        /// </summary>
        public bool IsInEditMode 
        { 
            get 
            {
                foreach (GridCell c in cells)
                    if (c is IGridEditableCell)
                        if ((c as IGridEditableCell).IsInEditMode)
                            return true;
                return false;
            } 
        }
        /// <summary>
        /// Gets Row Index
        /// </summary>
        public int Index {
            get { return mIndex; }
            internal set {
                mIndex = value;
                foreach (GridCell c in cells)
                    c.RowIndex = value;
            }
        }
        /// <summary>
        /// Gets contained GridCells in Row
        /// </summary>
		public GridCellCollection Cells { get { return cells; } }
        /// <summary>
        /// Gets/Sets Css class name
        /// </summary>
		public string CssClass { get { return mCssClass; } set { mCssClass = value; } }
        /// <summary>
        /// Gets/Sets visibility property
        /// </summary>
        public bool Visible { get { return mVisible; } set { mVisible = value; } }
        /// <summary>
        /// Gets Parent GridView control
        /// </summary>
        public GridView ParentGridView { get { return parentGrid; } internal set { parentGrid = value; } }

        /// <summary>
        /// Creates a new GridRow
        /// </summary>
        /// <param name="parentGrid">Parent GridView</param>
        /// <param name="index">Row Index</param>
        internal GridRow(GridView parentGrid, int index)
		{
            this.parentGrid = parentGrid;
            mIndex = index;
			cells = new GridCellCollection(this);
			mCssClass = "";
            mVisible = true;
		}
        /// <summary>
        /// Forces row enter in edit mode
        /// </summary>
        public void EnterOnEditMode()
        {
            foreach (GridCell c in cells)
                if (c is IGridEditableCell)
                    (c as IGridEditableCell).EnterOnEditMode();
            parentGrid.AjaxUpdate();
        }
        /// <summary>
        /// Forces row exit from edit mode
        /// </summary>
        public void ExitFromEditMode()
        {
            foreach (GridCell c in cells)
                if (c is IGridEditableCell)
                    (c as IGridEditableCell).ExitFromEditMode();
            parentGrid.AjaxUpdate();
        }
        /// <summary>
        /// Renders row
        /// </summary>
        /// <param name="writer">Tag writer</param>
		public void Render(AjaxTextWriter writer)
		{
            if (!mVisible)
                return;

            writer.WriteBeginTag("tr");
			string auxcss= (mCssClass != string.Empty) ? mCssClass : (parentGrid.SelectedRow == mIndex) ? parentGrid.SelectedRowCssClass : parentGrid.RowCssClass;
            if (auxcss != string.Empty)
                writer.WriteAttribute("class", auxcss);

            writer.Write(AjaxTextWriter.TagRightChar);

			foreach(GridCell gc in cells)
				gc.RenderCell(writer);

            writer.WriteEndTag("tr");
		}
        /// <summary>
        /// Renders row with template based system
        /// </summary>
        /// <param name="writer">Tag writer</param>
        /// <param name="template">Template page loaded</param>
        public void Render(AjaxTextWriter writer, TemplatePage template)
        {
            if (!mVisible)
                return;


            for (int i = 0; i < cells.Count; i++)
            {
                AjaxTextWriter w = new AjaxTextWriter();
                cells[i].RenderSingle(w);
                template.Allocate("COLUMN{" + i + "}", w.ToString());
            }
            writer.Write(template.ToString());
        }
        /// <summary>
        /// Renders javascript code
        /// </summary>
        /// <param name="writer">Javascript writer</param>
        public void RenderJS(AjaxTextWriter writer)
        {
            foreach (GridCell gc in cells)
                gc.RenderCellJS(writer);

        }

        internal object SaveViewState()
        {
            object[] cells = new object[parentGrid.Columns.Count];
            object[] state = new object[4];
            state[0] = mCssClass;
            state[1] = mIndex;
            state[2] = mVisible;

            for (int j = 0; j < Cells.Count; j++)
            {
                cells[j] = Cells[j].SaveViewState();
            }

            state[3] = cells;
            return state;
        }
        internal void LoadViewState(object savedState)
        {
            object[] state = (object[])(savedState);
            mCssClass = (string)state[0];
            mIndex = (int)state[1];
            mVisible = (bool)state[2];
            object[] cells = (object[])(state[3]);

            for (int j = 0; j < Cells.Count; j++)
            {
                Cells[j].LoadViewState(cells[j]);
            }
        }
	}
    /// <summary>
    /// Row comparer (needed to sort)
    /// </summary>
	public class GridRowComparer : IComparer    
	{ 
        /// <summary>
        /// Sort method
        /// </summary>
		public enum SortAscDesc 
		{ 
            /// <summary>
            /// Asc
            /// </summary>
			Asc, 
            /// <summary>
            /// Desc
            /// </summary>
			Desc 
		} 
		private int m_SortCell; 
		private SortAscDesc m_AscDesc; 
	 
	    /// <summary>
	    /// Creates a new GridRowComparer
	    /// </summary>
	    /// <param name="sortCell">Cell index</param>
        /// <param name="ascDesc">Sort method</param>
		public GridRowComparer(int sortCell, SortAscDesc ascDesc) 
		{ 
			m_SortCell = sortCell; 
			m_AscDesc = ascDesc; 
		} 
	
		#region IComparer Members 
	    /// <summary>
	    /// Compare two GridRows
	    /// </summary>
	    /// <param name="x">One GridRow</param>
        /// <param name="y">Other GridRow</param>
	    /// <returns>0 equal; 1 firs &gt; second; -1 first &lt; second</returns>
		public int Compare(object x, object y) 
		{ 
			GridCell c1 = ((GridRow)x).Cells[m_SortCell];
			GridCell c2 = ((GridRow)y).Cells[m_SortCell];
			
			int nRet = 0; 

			if (c1.GetType() == c2.GetType())
			{
                nRet = c1.Compare(c2);
                if (m_AscDesc == SortAscDesc.Desc)
                    return -nRet;
                return nRet;
			}

			
			nRet = c1.Value.ToString().CompareTo(c2.Value.ToString()); 
			if ( m_AscDesc == SortAscDesc.Desc ) 
				nRet = -nRet; 
			return nRet; 
		} 
	
		#endregion 
	} 
}
