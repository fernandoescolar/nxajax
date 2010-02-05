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

namespace Framework.Ajax.UI.Controls
{
	/// <summary>
	/// ComboBox Items Collection
	/// </summary>
	[DefaultProperty("Text")]
	public class ComboBoxItemCollection : CollectionBase
	{
		private ComboBox parent;
		public ComboBoxItemCollection(ComboBox parent) : base(){ this.parent = parent; }
        internal ComboBoxItemCollection() : base() { this.parent = new ComboBox(); }

        /// <summary>
        /// Refresh control parent
        /// </summary>
        /// <param name="parent">Parent ComboBox</param>
        public void Refresh(ComboBox parent)
        {
            this.parent = parent;
        }
        /// <summary>
        /// Adds a new ComboBoxItem to collection
        /// </summary>
        /// <param name="ctrl">ComboBoxItem to add</param>
		public void Add(ComboBoxItem ctrl)
		{
			List.Add(ctrl);
			parent.AjaxUpdate();
		}
        /// <summary>
        /// Remove an existing ComboBoxItem from the collection
        /// </summary>
        /// <param name="ctrl">ComboBoxItem to remove</param>
		public void Remove(ComboBoxItem ctrl)
		{
			List.Remove(ctrl);
			parent.AjaxUpdate();
		}
        /// <summary>
        /// Checks if an specified ComboBoxItem is contained in the collection
        /// </summary>
        /// <param name="ctrl">ComboBoxItem to check</param>
        /// <returns></returns>
		public bool Contains(ComboBoxItem ctrl)
		{
			return List.Contains(ctrl);
		}
        /// <summary> 
        /// Clear all ComboBoxItems contained
        /// </summary>
		public new void Clear()
		{
			List.Clear();
            parent.SelectedIndex = -1;
			parent.AjaxUpdate();
		}
        /// <summary>
        /// Gets ComboBoxItem contained count
        /// </summary>
		public new int Count
		{
			get { return List.Count; }
		}

        /// <summary>
        /// Gets/Sets a ComboBoxItem in an specified position
        /// </summary>
        /// <param name="pos">position</param>
        /// <returns>Selected ComboBoxItem</returns>
		public ComboBoxItem this[int pos]
		{
            get { if (pos < 0) return null;  return (ComboBoxItem)List[pos]; }
            set { if (pos >= 0) List[pos] = value; }
		}
        /// <summary>
        /// Adds a new ComboBoxItem to collection
        /// </summary>
        /// <param name="root">ComboBox Items Collection</param>
        /// <param name="ctrl">ComboBoxItem to add</param>
        /// <returns>The current collection</returns>
		public static ComboBoxItemCollection operator+(ComboBoxItemCollection root, ComboBoxItem ctrl)
		{
			root.Add(ctrl);
			return root;
		}
        /// <summary>
        /// Remove an existing ComboBoxItem from the collection
        /// </summary>
        /// <param name="root">ComboBox Items Collection</param>
        /// <param name="ctrl">ComboBoxItem to remove</param>
        /// <returns>The current collection</returns>
		public static ComboBoxItemCollection operator-(ComboBoxItemCollection root, ComboBoxItem ctrl)
		{
			root.Remove(ctrl);
			return root;
		}
        /// <summary>
        /// Gets a ComboBoxItem in an specified position
        /// </summary>
        /// <param name="root">ComboBox Items Collection</param>
        /// <param name="num">position</param>
        /// <returns>Selected ComboBoxItem</returns>
		public static ComboBoxItem operator^(ComboBoxItemCollection root, int num)
		{
			return root[num];
		}
        /// <summary>
        /// Gets a ComboBoxItem in an specified position
        /// </summary>
        /// <param name="root">ComboBox Items Collection</param>
        /// <param name="num">position</param>
        /// <returns>Selected ComboBoxItem</returns>
		public static ComboBoxItem operator|(ComboBoxItemCollection root, int num)
		{
			return root[num];
		}
        /// <summary>
        /// Checks if an specified ComboBoxItem is contained in the collection
        /// </summary>
        /// <param name="root">ComboBox Items Collection</param>
        /// <param name="ctrl">ComboBoxItem to check</param>
        /// <returns>if ComboBoxItem is contained</returns>
		public static bool operator&(ComboBoxItemCollection root, ComboBoxItem ctrl)
		{
			return root.Contains(ctrl);
		}
	}
}
