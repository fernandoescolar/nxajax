/*
 * Licensing
 *  Mozilla Public License 1.1 (MPL 1.1)
 * 
 * Fernando Escolar Martínez-Berganza <fer.escolar@gmail.com>
 * 
 */
using System;
using System.Collections;

namespace nxAjax.UI.Controls
{
	/// <summary>
	/// MenuItems Collection
	/// </summary>
	public class MenuItemCollection : CollectionBase
	{
		private MenuItem parent;

        /// <summary>
        /// Creates a new MenuItemCollection
        /// </summary>
        /// <param name="parent">Parent MenuItem</param>
		internal MenuItemCollection(MenuItem parent): base()	{ this.parent = parent; }

        /// <summary>
        /// Adds a new MenuItem to collection
        /// </summary>
        /// <param name="ctrl">MenuItem to add</param>
		public void Add(MenuItem ctrl)
		{
			ctrl.Parent = parent;
			List.Add(ctrl);
		}
        /// <summary>
        /// Remove an existing MenuItem from the collection
        /// </summary>
        /// <param name="ctrl">MenuItem to remove</param>
		public void Remove(MenuItem ctrl)
		{
			List.Remove(ctrl);
		}
        /// <summary>
        /// Checks if an specified MenuItem is contained in the collection
        /// </summary>
        /// <param name="ctrl">MenuItem to check</param>
        /// <returns></returns>
		public bool Contains(MenuItem ctrl)
		{
			return List.Contains(ctrl);
		}
        /// <summary> 
        /// Clear all MenuItem contained
        /// </summary>
		public new void Clear()
		{
			List.Clear();
		}
        /// <summary>
        /// Gets MenuItem contained count
        /// </summary>
		public new int Count
		{
			get { return List.Count; }
		}

        /// <summary>
        /// Gets/Sets a MenuItem in an specified position
        /// </summary>
        /// <param name="pos">position</param>
        /// <returns>Selected MenuItem</returns>
		public MenuItem this[int pos]
		{
			get { return (MenuItem)List[pos]; }
			set { List[pos] = value; }
		}
        /// <summary>
        /// Adds a new MenuItem to collection
        /// </summary>
        /// <param name="root">MenuItem Items Collection</param>
        /// <param name="ctrl">MenuItem to add</param>
        /// <returns>The current collection</returns>
		public static MenuItemCollection operator+(MenuItemCollection root, MenuItem ctrl)
		{
			root.Add(ctrl);
			return root;
		}
        /// <summary>
        /// Remove an existing MenuItem from the collection
        /// </summary>
        /// <param name="root">MenuItem Items Collection</param>
        /// <param name="ctrl">MenuItem to remove</param>
        /// <returns>The current collection</returns>
		public static MenuItemCollection operator-(MenuItemCollection root, MenuItem ctrl)
		{
			root.Remove(ctrl);
			return root;
		}
        /// <summary>
        /// Gets a MenuItem in an specified position
        /// </summary>
        /// <param name="root">MenuItem Items Collection</param>
        /// <param name="num">position</param>
        /// <returns>Selected MenuItem</returns>
		public static MenuItem operator^(MenuItemCollection root, int num)
		{
			return root[num];
		}
        /// <summary>
        /// Gets a MenuItem in an specified position
        /// </summary>
        /// <param name="root">MenuItem Items Collection</param>
        /// <param name="num">position</param>
        /// <returns>Selected MenuItem</returns>
		public static MenuItem operator|(MenuItemCollection root, int num)
		{
			return root[num];
		}
        /// <summary>
        /// Checks if an specified MenuItem is contained in the collection
        /// </summary>
        /// <param name="root">MenuItem Items Collection</param>
        /// <param name="ctrl">MenuItem to check</param>
        /// <returns>if MenuItem is contained</returns>
		public static bool operator&(MenuItemCollection root, MenuItem ctrl)
		{
			return root.Contains(ctrl);
		}
	}
}
