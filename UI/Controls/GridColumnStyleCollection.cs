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
    /// GridColumnStyle Items Collection
    /// </summary>
	public class GridColumnStyleCollection : CollectionBase
	{
        protected GridView parentGrid;

        /// <summary>
        /// Creates a new GridColumnStyle Collection
        /// </summary>
        /// <param name="parentGrid">Parent GridView control</param>
		public GridColumnStyleCollection(GridView parentGrid) : base()
		{
            this.parentGrid = parentGrid;
		}
        /// <summary>
        /// Adds a new GridColumnStyle to collection
        /// </summary>
        /// <param name="ctrl">GridColumnStyle to add</param>
		public void Add(GridColumnStyle ctrl)
		{
			List.Add(ctrl);
            ctrl.Index = List.Count - 1;
            ctrl.ParentGridView = parentGrid;
		}
        /// <summary>
        /// Remove an existing GridColumnStyle from the collection
        /// </summary>
        /// <param name="ctrl">GridColumnStyle to remove</param>
		public void Remove(GridColumnStyle ctrl)
		{
			List.Remove(ctrl);
            //reindex
            for (int i = 0; i < List.Count; i++)
                this[i].Index = i;
		}
        /// <summary>
        /// Checks if an specified GridColumnStyle is contained in the collection
        /// </summary>
        /// <param name="ctrl">GridColumnStyle to check</param>
        /// <returns></returns>
        public bool Contains(GridColumnStyle ctrl)
		{
			return List.Contains(ctrl);
		}
        /// <summary> 
        /// Clear all GridColumnStyle contained
        /// </summary>
		public new void Clear()
		{
			List.Clear();
		}
        /// <summary>
        /// Gets GridColumnStyle contained count
        /// </summary>
		public new int Count
		{
			get { return List.Count; }
		}

        /// <summary>
        /// Gets/Sets a GridColumnStyle in an specified position
        /// </summary>
        /// <param name="pos">position</param>
        /// <returns>Selected GridColumnStyle</returns>
		public GridColumnStyle this[int pos]
		{
			get { 
				try 
				{	
					return (GridColumnStyle)List[pos]; 
				}
				catch
				{
					return null;
				}
			}
			set { List[pos] = value; }
		}
        /// <summary>
        /// Adds a new GridColumnStyle to collection
        /// </summary>
        /// <param name="root">GridColumnStyle Items Collection</param>
        /// <param name="ctrl">GridColumnStyle to add</param>
        /// <returns>The current collection</returns>
		public static GridColumnStyleCollection operator+(GridColumnStyleCollection root, GridColumnStyle ctrl)
		{
			root.Add(ctrl);
			return root;
		}
        /// <summary>
        /// Remove an existing GridColumnStyle from the collection
        /// </summary>
        /// <param name="root">GridColumnStyle Items Collection</param>
        /// <param name="ctrl">GridColumnStyle to remove</param>
        /// <returns>The current collection</returns>
		public static GridColumnStyleCollection operator-(GridColumnStyleCollection root, GridColumnStyle ctrl)
		{
			root.Remove(ctrl);
			return root;
		}
        /// <summary>
        /// Gets a GridColumnStyle in an specified position
        /// </summary>
        /// <param name="root">GridColumnStyle Items Collection</param>
        /// <param name="num">position</param>
        /// <returns>Selected GridColumnStyle</returns>
		public static GridColumnStyle operator^(GridColumnStyleCollection root, int num)
		{
			return root[num];
		}
        /// <summary>
        /// Gets a GridColumnStyle in an specified position
        /// </summary>
        /// <param name="root">GridColumnStyle Items Collection</param>
        /// <param name="num">position</param>
        /// <returns>Selected GridColumnStyle</returns>
		public static GridColumnStyle operator|(GridColumnStyleCollection root, int num)
		{
			return root[num];
		}
        /// <summary>
        /// Checks if an specified GridColumnStyle is contained in the collection
        /// </summary>
        /// <param name="root">GridColumnStyle Items Collection</param>
        /// <param name="ctrl">GridColumnStyle to check</param>
        /// <returns>if GridColumnStyle is contained</returns>
		public static bool operator&(GridColumnStyleCollection root, GridColumnStyle ctrl)
		{
			return root.Contains(ctrl);
		}

        internal object SaveViewState()
        {
            object[] state = new object[List.Count];
            for (int i = 0; i < List.Count; i++)
                state[i] = this[i].SaveViewState();

            return state;
        }
        internal void LoadViewState(object savedState)
        {
            object[] state = (object[])(savedState);
            for (int i = 0; i < List.Count; i++)
                this[i].LoadViewState(state[i]);
        }
    }
}
