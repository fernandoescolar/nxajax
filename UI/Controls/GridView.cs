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

namespace Framework.Ajax.UI.Controls
{
	/// <summary>
	/// Grid (Table) Data View control
    /// <code>
    /// &lt;ajax:GridView runat="server"&gt;&lt;/ajax:GridView&gt;
    /// </code>
    /// <remarks>
    /// Template page type:
    /// &lt;!$BEGIN header&gt;
    ///  [header tags]
    /// &lt;!$END header&gt;
    /// 
    /// &lt;!$BEGIN content&gt;
    /// [content tags] &lt;$COLUMN{0}$&gt; [content tags]
    /// ...
    /// [content tags] &lt;$COLUMN{N}$&gt; [content tags]
    /// &lt;!$END content&gt;
    /// 
    /// &lt;!$BEGIN footer&gt;
    ///  [footer tags]
    /// &lt;!$END footer&gt;
    /// </remarks>
	/// </summary>
    [AspNetHostingPermission(SecurityAction.Demand, Level = AspNetHostingPermissionLevel.Minimal)]
    [AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    [Designer("Framework.Ajax.UI.Design.AjaxControlDesigner")]
    [ToolboxData("<{0}:GridView runat=\"server\"></{0}:GridView>")]
    [ParseChildren(true, "Columns")]
    [PersistChildren(false)]
    [DefaultProperty("Columns")]
    [ToolboxItem(true)]
    [ToolboxBitmap(typeof(System.Web.UI.WebControls.DataGrid))]
    public class GridView : AjaxControl, IChildAjaxControlContainer
	{
		#region Private Attributes
        protected Label mTitle;
        protected GridHeader mHeader;
        protected Pager mPager;

		private string rowCssClass, selectedRowCssClass, contentTemplateFileName;
		private bool showPager, showTitle, showHeader, sortable;
		private int pageSize, width, height;
		private GridColumnStyleCollection columns;
		private GridRowCollection rows;

		private GridCell selectedCell;
		private int selectedRow, selectedColumn, page=0;
		private int sortedBy = -1;
		private bool sortedAsc = true;
		#endregion

		#region Public Properties
        internal bool isInDesignMode { get { return DesignMode; } }
        public override string ID
        {
            get
            {
                return base.ID;
            }
            set
            {
                base.ID = value;
                mPager.ID = ID + "_Pager";
                mHeader.ID = ID + "_AH";
                foreach (GridRow r in rows)
                    foreach (GridCell c in r.Cells)
                        c.RefreshControl();
            }
        }
        /// <summary>
        /// Gets/Sets Table Title
        /// </summary>
		[Category("Appearance"), DefaultValue(""), Description("The title of control.")]
		public string Title
		{
			get { return mTitle.Text; }
            set { mTitle.Text = value; }
		}
        /// <summary>
        /// Gets/Sets Table title css class name
        /// </summary>
		[Category("Appearance"), DefaultValue(""), Description("Control Title CSS class name.")]
		public string TitleCssClass
		{
			get { return mTitle.CssClass; }
            set { mTitle.CssClass = value; }
		}
        /// <summary>
        /// Gets/Sets Table Header css class name
        /// </summary>
		[Category("Appearance"), DefaultValue(""), Description("Header CSS class name.")]
		public string HeaderCssClass
		{
			get { return mHeader.CssClass; }
            set { mHeader.CssClass = value; }
		}
        /// <summary>
        /// Gets/Sets Table Rows css class name
        /// </summary>
		[Category("Appearance"), DefaultValue(""), Description("Row CSS class name.")]
		public string RowCssClass
		{
			get { return rowCssClass; }
			set { rowCssClass = value; AjaxUpdate(); }
		}
        /// <summary>
        /// Gets/Sets Table selected row css class name
        /// </summary>
		[Category("Appearance"), DefaultValue(""), Description("Selected Row CSS class name.")]
		public string SelectedRowCssClass
		{
			get { return selectedRowCssClass; }
			set { selectedRowCssClass = value; AjaxUpdate(); }
		}
        /// <summary>
        /// Gets/Sets Pager css class name
        /// </summary>
		[Category("Appearance"), DefaultValue(""), Description("Pager CSS class name.")]
		public string PagerCssClass
		{
			get { return mPager.CssClass; }
            set { mPager.CssClass = value; }
		}
        /// <summary>
        /// Gets/Sets Pager selected item css class name
        /// </summary>
        [Category("Appearance"), DefaultValue(""), Description("Pager CSS class name.")]
        public string PagerSelectedItemCssClass
        {
            get { return mPager.SelectedItemCssClass; }
            set { mPager.SelectedItemCssClass = value; }
        }
        /// <summary>
        /// Gets/Sets Grid template file path
        /// </summary>
        [Category("Appearance"), DefaultValue(""), Description("Content template fileName.")]
        public string ContentTemplateFileName
        {
            get { return contentTemplateFileName; }
            set { contentTemplateFileName = value; }
        }
        /// <summary>
        /// Gets/Sets if shows control pager
        /// </summary>
		[Category("Appearance"), DefaultValue(true), Description("Will show pager control.")]
		public bool ShowPager
		{
			get { return showPager; }
			set { showPager = value; AjaxUpdate(); }
		}
        /// <summary>
        /// Gets/Sets if shows control header
        /// </summary>
		[Category("Appearance"), DefaultValue(true), Description("Will Show header line.")]
		public bool ShowHeader
		{
			get { return showHeader; }
			set { showHeader = value; AjaxUpdate(); }
		}
        /// <summary>
        /// Gets/Sets if shows table title
        /// </summary>
		[Category("Appearance"), DefaultValue(true), Description("Will Show the control title.")]
		public bool ShowTitle
		{
			get { return showTitle; }
			set { showTitle = value; }
		}
        /// <summary>
        /// Gets/Sets if table list is sortable
        /// </summary>
		[Category("Appearance"), DefaultValue(true), Description("Is sortable.")]
		public bool Sortable
		{
			get { return sortable; }
			set { sortable = value; AjaxUpdate(); }
		}
        /// <summary>
        /// Gets/Sets Pager page size
        /// </summary>
		[Category("Appearance"), DefaultValue(0), Description("Pager page size")]
		public int PageSize
		{
			get { return pageSize; }
			set { pageSize = value; }
		}
        /// <summary>
        /// Gets/Sets Table Width
        /// </summary>
		[Category("Appearance"), DefaultValue(-1), Description("Width.")]
		public int Width
		{
			get { return width; }
			set { width = value; AjaxUpdate(); }
		}
        /// <summary>
        /// Gets/Sets Table Height
        /// </summary>
		[Category("Appearance"), DefaultValue(-1), Description("Height.")]
		public int Height
		{
			get { return height; }
			set { height = value; AjaxUpdate(); }
		}
        /// <summary>
        /// Gets Grid ColumnStyles Collection
        /// </summary>
		[Category("Data"), DefaultValue(""), Description("Column Styles Items.")]
		public GridColumnStyleCollection Columns
		{
			get { return columns; }
		}
        /// <summary>
        /// Gets Grid Rows Collections
        /// </summary>
		[Category("Data"), DefaultValue(""), Description("Rows Items.")]
		public GridRowCollection Rows
		{
			get { return rows; }
		}
        /// <summary>
        /// Gets Selected GridCell
        /// </summary>
		public GridCell SelectedCell{
			get { return selectedCell; }
		}
        /// <summary>
        /// Gets/Sets Selected GridRow Index
        /// </summary>
		public int SelectedRow
		{
			get { return selectedRow; }
            set { selectedRow = value; }
		}
        /// <summary>
        /// Gets/Sets Selected GridColumnStyle Index
        /// </summary>
		public int SelectedColumn
		{
			get { return selectedColumn; }
            set { selectedColumn = value; }
		}
        /// <summary>
        /// Gets Sorted GridCell Index (into a Row)
        /// </summary>
        public int SortedIndex { get { return sortedBy; } }
        /// <summary>
        /// Gets Sorted Mode (Asc: true, Desc: false)
        /// </summary>
        public bool SortedAsc { get { return sortedAsc; } }
		#endregion

        /// <summary>
        /// Creates a new GridView control
        /// </summary>
		public GridView(): base("div")
		{
            mTitle = new Label();
            mHeader = new GridHeader(this);
            mPager = new Pager();
            mTitle.KeepState = false;
            mHeader.KeepState = false;
            mPager.KeepState = false;

            mPager.ServerChange += new AjaxEventHandler(mPager_ServerChange);

			rowCssClass ="";
			pageSize = width = height = -1;
			showPager = showTitle = showHeader = sortable = true;
			columns = new GridColumnStyleCollection(this);
			rows = new GridRowCollection(this);
			selectedCell = null;
			selectedRow = selectedColumn = -1;
            contentTemplateFileName = string.Empty;
		}

        protected void mPager_ServerChange(AjaxControl sender, string value)
        {
            page = int.Parse(value)-1;
            AjaxUpdate();
        }

		#region Renders
        protected internal void renderLoaingImg(AjaxTextWriter writer)
        {
            RenderLoadingImage(writer);
        }
        protected virtual void renderHTMLHidden(AjaxTextWriter writer, string id)
        {
            Hidden h = new Hidden();
            h.ID = id;
            h.Value = "0";
            h.RenderHTML(writer);
            h.Dispose();
        }
		public override void RenderHTML(AjaxTextWriter writer)
		{
            renderHTMLHidden(writer, ID + "_txtPosY");
            renderHTMLHidden(writer, ID + "_txtPosX");
            RenderBeginTag(writer);
            if (DesignMode)
			    RenderPage(writer);
            RenderEndTag(writer);
			hasChanged = false;
		}
        private void RenderPage(AjaxTextWriter writer)
		{
			int superW = 0;
			foreach(GridColumnStyle cs in columns)
				superW += cs.Width;
					
			/*Title*/
			if (showTitle)
                mTitle.RenderHTML(writer);
			
			/*Header*/
			if(showHeader)
			{
                mHeader.ID = ID + "_AH";
                mHeader.Style["position"] = "relative";
                mHeader.Style["overflow"] = "hidden";
                mHeader.Style["width"] = width.ToString() + "px";
                mHeader.RenderHTML(writer);
			}
			/*Content*/
            int inicio, final;
            if (this.pageSize > 0)
            {
                inicio = page * pageSize;
                final = inicio + pageSize;
                if (final > rows.Count)
                    final = rows.Count;

            }
            else
            {
                inicio = 0;
                final = rows.Count;
            }

            writer.WriteBeginTag("div");
            writer.WriteAttribute("id", ID + "_Content");
            writer.WriteAttribute("style", "overflow: auto; width: " + width + "px; height: " + height + "px");
            writer.WriteAttribute("onscroll", "DoScroll('" + ID + "');");
            writer.Write(AjaxTextWriter.TagRightChar);

            Templates contentTemplate = null;
            if (contentTemplateFileName != string.Empty)
            {
                contentTemplate = new Templates(Page.Server.MapPath(System.IO.Path.GetDirectoryName(contentTemplateFileName)), AjaxController.Lang);
                try
                {
                    contentTemplate.Load("pageTemplate", System.IO.Path.GetFileName(contentTemplateFileName));
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine("Problems with GridView Content Template File: " + ex.Message);
                }
            }

            bool contentRendered = false;
            if (contentTemplate != null)
                if (contentTemplate.IsLoaded)
                {
                    writer.Write(contentTemplate["pageTemplate"]["header"].ToString());

                    for (int i = inicio; i < final; i++)
                        rows[i].Render(writer, contentTemplate["pageTemplate"]["content"]);

                    writer.Write(contentTemplate["pageTemplate"]["footer"].ToString());

                    contentRendered = true;
                }

            if (!contentRendered)
            {
                writer.WriteBeginTag("table");
                writer.WriteAttribute("class", CssClass);
                writer.WriteAttribute("style", "width: " + superW + "px;");
                writer.Write(AjaxTextWriter.TagRightChar);

                for (int i = inicio; i < final; i++)
                    rows[i].Render(writer);

                writer.WriteEndTag("table");
            }

            writer.WriteEndTag("div");
			/*Pager*/
			if (showPager)
			{
                mPager.Page = Page;
                mPager.ID = ID + "_Pager";
                mPager.SetRange(1, Rounded(rows.Count, pageSize));
                mPager.SelectedValue = (page + 1).ToString();

                mPager.RenderHTML(writer);
			}
		}
        public override void RenderJS(AjaxTextWriter writer)
		{
            base.RenderJS(writer);
            if (hasChanged || !AjaxController.IsPostBack)
            {
                AjaxTextWriter jswriter = new AjaxTextWriter();
                RenderPage(jswriter);
                writer.Write("$('#" + ID + "').html('" + jswriter.ToString().Replace("'", "\\'").Replace("\r", "\\r").Replace("\n", "\\n") + "');");
                mPager.AjaxUpdate();
            }
			if (hasChanged)
			{
				writer.Write("PutScroll('" + ID + "');");
				hasChanged = false;
			}
            if (!AjaxController.IsPostBack && !hasChanged)
            {
                writer.Write("try { if (document.all) $('#" + ID + "_Content')[0].onscroll = DoScroll('" + ID + "'); } catch(e) {}");
            }

            int inicio, final;
            if (this.pageSize > 0)
            {
                inicio = page * pageSize;
                final = inicio + pageSize;
                if (final > rows.Count)
                    final = rows.Count;

            }
            else
            {
                inicio = 0;
                final = rows.Count;
            }
            for (int i = inicio; i < final; i++)
                rows[i].RenderJS(writer);

            if (showPager)
            {
                mPager.Page = Page;
                mPager.RenderJS(writer);
            }
		}
		private int Rounded(int cols, int size)
		{
			double d = (double)cols/(double)size;
			int i =  cols/size;
			if(d>(double)i)
				i++;
			return i;
		}
		#endregion

		#region Implements AjaxControl
		public override void RaiseEvent(string action, string value)
		{
			switch(action.ToLower())
			{
				case "onsort":
					if(sortedBy == int.Parse(value))
						sortedAsc = !sortedAsc;
					else
					{
						sortedBy = int.Parse(value);
						sortedAsc = true;
					}
                    Sort(sortedBy, sortedAsc);
					break;
				case "onselect":
                    PutSelectedPosition(value);
					if (selectedColumn>=0)
					{
                        AjaxUpdate();
                        try { columns[selectedColumn].RaiseEvent(this, value); }
                        catch (Exception ex) { System.Diagnostics.Debug.WriteLine("Error: " + ex.Message); }
					}
					break;
			}
		}

		protected override void AjaxLoadViewState(object savedState)
		{
			object[] state = (object[])(savedState);
			base.AjaxLoadViewState(state[0]);
			CssClass = (string)state[1];
			mTitle.ProtectedLoadViewState(state[2]);
			mHeader.ProtectedLoadViewState(state[3]);
			mPager.ProtectedLoadViewState(state[4]);
			rowCssClass = (string)state[5];
			selectedRowCssClass = (string)state[6];
			showPager = (bool)state[7];
			showTitle = (bool)state[8];
			showHeader = (bool)state[9];
			sortable = (bool)state[10];
			pageSize = (int)state[11];
			width = (int)state[12];
			height = (int)state[13];

            if (columns.Count <= 0 && state[22] != null)
            {
                columns = (GridColumnStyleCollection)state[22];
                foreach (GridColumnStyle cs in Columns)
                    cs.ResetColumn(this);
            }
            else
            {
                foreach (GridColumnStyle cs in Columns)
                    cs.ParentGridView = this;
            }
			columns.LoadViewState(state[14]);

            System.Data.DataTable dt = (System.Data.DataTable)state[15];
            setDataSource(dt);
            rows.LoadViewState(state[23]);

			selectedCell = (GridCell)state[16];
			selectedRow = (int)state[17];
			selectedColumn = (int)state[18];
			sortedBy = (int)state[19];
			sortedAsc = (bool)state[20];
			page = (int)state[21];

            AjaxNotUpdate();
		}

		protected override object AjaxSaveViewState()
		{
			object[] state = new object[24];
			state[0] = base.AjaxSaveViewState();
            state[1] = CssClass;
			state[2] = mTitle.ProtectedSaveViewState();
			state[3] = mHeader.ProtectedSaveViewState();
			state[4] = mPager.ProtectedSaveViewState();
			state[5] = rowCssClass;
			state[6] = selectedRowCssClass;
			state[7] = showPager;
			state[8] = showTitle;
			state[9] = showHeader;
			state[10] = sortable;
			state[11] = pageSize;
			state[12] = width;
			state[13] = height;
			state[14] = columns.SaveViewState();
			state[15] = getDataSource();
			state[16] = selectedCell;
			state[17] = selectedRow;
			state[18] = selectedColumn;
			state[19] = sortedBy;
			state[20] = sortedAsc;
			state[21] = page;
            state[22] = columns;
            state[23] = rows.SaveViewState();
		
			return state;
		}
        protected void PutSelectedPosition(string obj)
        { 
            string[] aux = obj.Split(';');
            if (aux.Length >= 2)
            { 
                if (selectedCell != null)
                    {try { this.Rows[selectedRow].Cells[selectedColumn].Selected = false; } catch { } }

                selectedRow = int.Parse(aux[0]);
				selectedColumn = int.Parse(aux[1]);
				try { selectedCell = rows[selectedRow].Cells[selectedColumn];} catch { }
                try { rows[selectedRow].Cells[selectedColumn].Selected = true; } catch { }
            }
        }
		public override void PutPostValue(string obj)
		{
            PutSelectedPosition(obj);
			string[] aux = obj.Split(';');
			if(aux.Length>=3)
                if (selectedCell != null)
                {
                    selectedCell.Value = aux[2];                 
                }
		}
		#endregion

        /// <summary>
        /// Sets datasource to fill the GridView
        /// </summary>
        /// <param name="dataTable">Data source</param>
		public void setDataSource(System.Data.DataTable dataTable)
		{
            int countRows = 0;
			rows.Clear();
			foreach(System.Data.DataRow dr in dataTable.Rows)
			{
                GridRow gr = new GridRow(this, countRows);
				foreach(GridColumnStyle cs in columns)
				{
                    if (cs.DataColumn != "")
                      gr.Cells.Add(cs.CreateGridCell(dr[cs.DataColumn]));
				}
				rows.Add(gr);
                countRows++;
			}
			AjaxUpdate();
		}
        /// <summary>
        /// Gets datasource witch the GridView is filled
        /// </summary>
        /// <returns>Data source</returns>
        public System.Data.DataTable getDataSource()
        {
            System.Data.DataTable dt = new System.Data.DataTable();
            foreach (GridColumnStyle cs in columns)
            {
                if (!dt.Columns.Contains(cs.DataColumn))
                    dt.Columns.Add(cs.DataColumn);
            }
            foreach (GridRow gr in rows)
            {
                System.Data.DataRow dr = dt.NewRow();
                for (int i=0; i< columns.Count; i++)
                {
                    dr[columns[i].DataColumn] = gr.Cells[i].Value;
                }
                dt.Rows.Add(dr);
            }
            return dt;
        }
        /// <summary>
        /// Creates a new GridRow formated by the GridView ColumnStyles
        /// </summary>
        /// <returns>New GridRow object</returns>
        public GridRow NewRow()
        {
            
            GridRow gr = new GridRow(this, 0);
            foreach (GridColumnStyle cs in columns)
            {
                if (cs.DataColumn != "")
                    gr.Cells.Add(cs.CreateGridCell());
            }
            return gr;
        }
        /// <summary>
        /// Sort the current GridView data
        /// </summary>
        /// <param name="columnNumber">GridColumnStyle Index</param>
        /// <param name="Asc">Sorted Mode (Asc: true, Desc: false)</param>
        public void Sort(int columnNumber, bool Asc)
        {
            sortedBy = columnNumber;
            sortedAsc = Asc;
            rows.Sort(sortedBy, sortedAsc);
            AjaxUpdate();
        }
        public AjaxControl FindInnerControl(string id)
        {
            if (mPager.ID == id)
                return mPager;

            foreach (GridRow gr in rows)
            {
                int i = 0;
                foreach (GridCell c in gr.Cells)
                {
                    if (c.InnerControl.ID == id)
                    {
                        selectedRow = gr.Index;
                        selectedColumn = i;
                        return c.InnerControl;
                    }
                    i++;
                }
            }
            return null;
        }
	}
}
