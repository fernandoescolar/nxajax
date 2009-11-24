/*
 * Licensing
 *  Mozilla Public License 1.1 (MPL 1.1)
 * 
 * Fernando Escolar Martínez-Berganza <fer.escolar@gmail.com>
 * 
 */
using System;
using System.Collections;
using System.ComponentModel;

namespace nxAjax.UI.Controls
{
	/// <summary>
	/// GridCells Collection
	/// </summary>
	public class GridCellCollection : CollectionBase
	{
        protected GridRow parentRow;

        /// <summary>
        /// Creates a new GridCells Collection
        /// </summary>
        /// <param name="parentRow">Parent GridRow</param>
		public GridCellCollection(GridRow parentRow):base()
		{
            this.parentRow = parentRow;
		}
        /// <summary>
        /// Adds a new GridCell to collection
        /// </summary>
        /// <param name="ctrl">GridCell to add</param>
		public void Add(GridCell ctrl)
		{
			List.Add(ctrl);
            ctrl.RowIndex = parentRow.Index;
		}
        /// <summary>
        /// Remove an existing GridCell from the collection
        /// </summary>
        /// <param name="ctrl">GridCell to remove</param>
		public void Remove(GridCell ctrl)
		{
			List.Remove(ctrl);
		}
        /// <summary>
        /// Checks if an specified GridCell is contained in the collection
        /// </summary>
        /// <param name="ctrl">GridCell to check</param>
        /// <returns></returns>
		public bool Contains(GridCell ctrl)
		{
			return List.Contains(ctrl);
		}
        /// <summary> 
        /// Clear all GridCell contained
        /// </summary>
		public new void Clear()
		{
			List.Clear();
		}
        /// <summary>
        /// Gets GridCell contained count
        /// </summary>
		public new int Count
		{
			get { return List.Count; }
		}

        /// <summary>
        /// Gets/Sets a GridCell in an specified position
        /// </summary>
        /// <param name="pos">position</param>
        /// <returns>Selected GridCell</returns>
		public GridCell this[int pos]
		{
			get { return (GridCell)List[pos]; }
			set { List[pos] = value; }
		}
        /// <summary>
        /// Adds a new GridCell to collection
        /// </summary>
        /// <param name="root">GridCell Items Collection</param>
        /// <param name="ctrl">GridCell to add</param>
        /// <returns>The current collection</returns>
		public static GridCellCollection operator+(GridCellCollection root, GridCell ctrl)
		{
			root.Add(ctrl);
			return root;
		}
        /// <summary>
        /// Remove an existing GridCell from the collection
        /// </summary>
        /// <param name="root">GridCell Items Collection</param>
        /// <param name="ctrl">GridCell to remove</param>
        /// <returns>The current collection</returns>
		public static GridCellCollection operator-(GridCellCollection root, GridCell ctrl)
		{
			root.Remove(ctrl);
			return root;
		}
        /// <summary>
        /// Gets a GridCell in an specified position
        /// </summary>
        /// <param name="root">GridCell Items Collection</param>
        /// <param name="num">position</param>
        /// <returns>Selected GridCell</returns>
		public static GridCell operator^(GridCellCollection root, int num)
		{
			return root[num];
		}
        /// <summary>
        /// Gets a GridCell in an specified position
        /// </summary>
        /// <param name="root">GridCell Items Collection</param>
        /// <param name="num">position</param>
        /// <returns>Selected GridCell</returns>
		public static GridCell operator|(GridCellCollection root, int num)
		{
			return root[num];
		}
        /// <summary>
        /// Checks if an specified GridCell is contained in the collection
        /// </summary>
        /// <param name="root">GridCell Items Collection</param>
        /// <param name="ctrl">GridCell to check</param>
        /// <returns>if GridCell is contained</returns>
		public static bool operator&(GridCellCollection root, GridCell ctrl)
		{
			return root.Contains(ctrl);
		}
	}
}
