/*
 * Licensing
 *  Mozilla Public License 1.1 (MPL 1.1)
 * 
 * Fernando Escolar Martínez-Berganza <fer.escolar@gmail.com>
 * 
 */
using System;

namespace nxAjax.UI.Controls
{
	/// <summary>
	/// Base Grid Cell Control
	/// </summary>
	public abstract class GridCell
	{
        /// <summary>
        /// Contained nxControl
        /// </summary>
        protected nxControl mControl;
        protected int mWidth, mHeight, mRowIndex;
        protected GridColumnStyle parentColumn;
        protected bool mSelected;

        /// <summary>
        /// Gets/Sets Value attributte
        /// </summary>
        public abstract object Value { set; get; }
        /// <summary>
        /// Gets Inner nxControl
        /// </summary>
        public nxControl InnerControl { get { return mControl; } }
        /// <summary>
        /// Gets/Sets Css class name
        /// </summary>
        public string CssClass { get { return mControl.CssClass; } set { mControl.CssClass = value; } }
        /// <summary>
        /// Gets/Sets Cell width
        /// </summary>
        public int Width { get { return mWidth; } set { mWidth = value; } }
        /// <summary>
        /// Gets/Sets Cell height
        /// </summary>
        public int Height { get { return mHeight; } set { mHeight = value; } }
        /// <summary>
        /// Gets/Sets Cell row index
        /// </summary>
        public int RowIndex { get { return mRowIndex; } set { mRowIndex = value; prepareControl(); } }
        /// <summary>
        /// Gets/Sets if cell is selected
        /// </summary>
        public bool Selected { get { return mSelected; } set { mSelected = value; } }
        /// <summary>
        /// Gets Parent column style
        /// </summary>
        public GridColumnStyle ParentColumnStyle { get { return parentColumn; } internal set { parentColumn = value; } }
        /// <summary>
        /// Gets Css Style collection
        /// </summary>
        public System.Web.UI.CssStyleCollection Style { get { return mControl.Style; } }

        /// <summary>
        /// Creates a new GridCell
        /// </summary>
        /// <param name="parentColumn">Parent GridColumnStyle object</param>
        internal GridCell(GridColumnStyle parentColumn)
		{
            this.parentColumn = parentColumn;
            mWidth = mHeight = 0;
		}
        //internal GridCell(GridColumnStyle parentColumn, object value): this(parentColumn)
        //{
			
        //}
        /// <summary>
        /// Refresh GridCell InnerControl
        /// </summary>
        public void RefreshControl()
        {
            prepareControl();
        }
        protected void prepareControl()
        {
            mControl.ID = parentColumn.ParentGridView.ID + "_" + RowIndex + "_" + parentColumn.Index;
            mControl.Page = parentColumn.ParentGridView.Page;
            mControl.PostBackMode = PostBackMode.Async;
            mControl.LoadingImgID = parentColumn.ParentGridView.LoadingImgID;
        }
        /// <summary>
        /// Renders GridCell InnerControl
        /// </summary>
        /// <param name="writer">Tag writer</param>
        public void RenderControl(nxAjaxTextWriter writer)
        {
            if (!parentColumn.DesignMode)
                if (parentColumn.hasJavacript())
                {
                    writer.WriteBeginTag("a");
                    writer.WriteAttribute("href", "javascript:" + parentColumn.getJavaScript(this));
                    writer.Write(nxAjaxTextWriter.TagRightChar);
                }

            mControl.RenderHTML(writer);

            if (!parentColumn.DesignMode)
                if (parentColumn.hasJavacript())
                    writer.WriteEndTag("a");
        }
        /// <summary>
        /// Renders only GridCell InnerControl
        /// </summary>
        /// <param name="writer">Tag writer</param>
        public virtual void RenderSingle(nxAjaxTextWriter writer)
        {
            RenderControl(writer);
        }
        /// <summary>
        /// Renders all GridCell
        /// </summary>
        /// <param name="writer">Tag writer</param>
		public virtual void RenderCell(nxAjaxTextWriter writer)
		{
            if (!parentColumn.Visible)
                return;
            
            writer.WriteBeginTag("td");
			
			string auxcss="";
            if (parentColumn != null)
			{
                auxcss = (mControl.CssClass != "") ? mControl.CssClass : parentColumn.CssClass;
                mWidth = (mWidth > 0) ? mWidth : parentColumn.Width;
                mHeight = (mHeight > 0) ? mHeight : parentColumn.Height;
			}
            if (auxcss != null && auxcss != string.Empty)
                writer.WriteAttribute("class", auxcss);

            string style = string.Empty;
            if (mWidth > 0)
                style += "width:" + mWidth + "px;";
            if (mHeight > 0)
                style += "height:" + mHeight + "px;";

            if (style != string.Empty)
                writer.WriteAttribute("style", style);

            writer.Write(nxAjaxTextWriter.TagRightChar);

            RenderSingle(writer);

            writer.WriteEndTag("td");
		}
        /// <summary>
        /// Renders Control Javascript functions
        /// </summary>
        /// <param name="writer">Javascript writer</param>
        public virtual void RenderCellJS(nxAjaxTextWriter writer)
        {
            if (!parentColumn.Visible)
                return;

            mControl.RenderJS(writer);
        }
        internal object SaveViewState()
        {
            object[] state = new object[4];
            state[0] = mSelected;
            state[1] = mControl.ProtectedSaveViewState();

            return state;
        }
        internal void LoadViewState(object savedState)
        {
            object[] state = (object[])(savedState);
            mSelected = (bool)state[0];
            mControl.ProtectedLoadViewState(state[1]);
            mControl.AjaxNotUpdate();
        }
        /// <summary>
        /// Compare GridCell with other
        /// </summary>
        /// <param name="gc">GridCell to compare</param>
        /// <returns>0 equals, -1 less, 1 greater</returns>
        public abstract int Compare(GridCell gc);
	}
}
