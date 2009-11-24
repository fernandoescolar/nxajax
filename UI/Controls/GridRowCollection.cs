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
	/// GridRow Items Collection
	/// </summary>
	public class GridRowCollection : CollectionBase
	{
        protected GridView parentGrid;
        /// <summary>
        /// Creates a new GridRowCollection
        /// </summary>
        /// <param name="parentGrid">Parent GridView control</param>
		internal GridRowCollection(GridView parentGrid):base()
		{
            this.parentGrid = parentGrid;
		}
        /// <summary>
        /// Adds a GridRow in a specified position
        /// </summary>
        /// <param name="index">Specified position</param>
        /// <param name="ctrl">GridRow to added</param>
        public void Insert(int index, GridRow ctrl)
        {
            List.Insert(index, ctrl);
            ctrl.Index = index;
            ctrl.ParentGridView = parentGrid;
            for (int i = index; i < List.Count; i++)
                this[i].Index = i;
        }
        /// <summary>
        /// Adds a GridRow
        /// </summary>
        /// <param name="ctrl">GridRow to added</param>
        public void Add(GridRow ctrl)
		{
			List.Add(ctrl);
            ctrl.Index = List.Count - 1;
            ctrl.ParentGridView = parentGrid;
		}
        /// <summary>
        /// Removes an specified GridRow from collection
        /// </summary>
        /// <param name="ctrl">GridRow to added</param>
		public void Remove(GridRow ctrl)
		{
			List.Remove(ctrl);
            //reindex
            for (int i = 0; i < List.Count; i++)
                this[i].Index = i;
		}
        /// <summary>
        /// Checks if collection contains an specified GridRow
        /// </summary>
        /// <param name="ctrl">GridRow to check</param>
        /// <returns></returns>
		public bool Contains(GridRow ctrl)
		{
			return List.Contains(ctrl);
		}
        /// <summary> 
        /// Clear all GridRow contained
        /// </summary>
		public new void Clear()
		{
			List.Clear();
		}
        /// <summary>
        /// Gets GridRow contained count
        /// </summary>
		public new int Count
		{
			get { return List.Count; }
		}

        /// <summary>
        /// Gets/Sets a GridRow in an specified position
        /// </summary>
        /// <param name="pos">position</param>
        /// <returns>Selected GridRow</returns>
		public GridRow this[int pos]
		{
            get { if (pos < 0) return null;  return (GridRow)List[pos]; }
			set { if (pos >= 0) List[pos] = value; }
		}

        /// <summary>
        /// Sorts GridRow collection
        /// </summary>
        /// <param name="cell">Cell Index To sort</param>
        /// <param name="asc">Sort Method (Asc: true; Desc: false)</param>
		public void Sort(int cell, bool asc)
		{
			InnerList.Sort(new GridRowComparer(cell,(asc)? GridRowComparer.SortAscDesc.Asc : GridRowComparer.SortAscDesc.Desc));
            //reindex
            for (int i = 0; i < List.Count; i++)
                this[i].Index = i;
		}
        /// <summary>
        /// Adds a new GridRow to collection
        /// </summary>
        /// <param name="root">GridRow Items Collection</param>
        /// <param name="ctrl">GridRow to add</param>
        /// <returns>The current collection</returns>
		public static GridRowCollection operator+(GridRowCollection root, GridRow ctrl)
		{
			root.Add(ctrl);
			return root;
		}
        /// <summary>
        /// Remove an existing GridRow from the collection
        /// </summary>
        /// <param name="root">GridRow Items Collection</param>
        /// <param name="ctrl">GridRow to remove</param>
        /// <returns>The current collection</returns>
		public static GridRowCollection operator-(GridRowCollection root, GridRow ctrl)
		{
			root.Remove(ctrl);
			return root;
		}
        /// <summary>
        /// Gets a GridRow in an specified position
        /// </summary>
        /// <param name="root">GridRow Items Collection</param>
        /// <param name="num">position</param>
        /// <returns>Selected GridRow</returns>
		public static GridRow operator^(GridRowCollection root, int num)
		{
			return root[num];
		}
		public static GridRow operator|(GridRowCollection root, int num)
		{
			return root[num];
		}
        /// <summary>
        /// Checks if an specified GridRow is contained in the collection
        /// </summary>
        /// <param name="root">GridRow Items Collection</param>
        /// <param name="ctrl">GridRow to check</param>
        /// <returns>if GridRow is contained</returns>
		public static bool operator&(GridRowCollection root, GridRow ctrl)
		{
			return root.Contains(ctrl);
		}

        internal object SaveViewState()
        {
            object[] state = new object[List.Count];
            for (int i = 0; i < List.Count; i++)
            {
                state[i] = this[i].SaveViewState();
            }

            return state;
        }
        internal void LoadViewState(object savedState)
        {
            object[] state = (object[])(savedState);
            for (int i = 0; i < List.Count; i++)
            {
                this[i].LoadViewState(state[i]);
            }
        }
	}
}
