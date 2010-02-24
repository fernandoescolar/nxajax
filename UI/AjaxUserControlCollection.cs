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

/// <summary>
/// nxAjax User Interface objects container
/// </summary>
namespace Framework.Ajax.UI
{
    /// <summary>
    /// An AjaxUserControl collection object
    /// </summary>
    public class AjaxUserControlCollection: System.Collections.CollectionBase
	{
        /// <summary>
        /// Adds a new AjaxUserControl
        /// </summary>
        /// <param name="ctrl"></param>
		public void Add(AjaxUserControl ctrl)
		{
			List.Add(ctrl);
		}
        /// <summary>
        /// Removes an existing AjaxUserControl
        /// </summary>
        /// <param name="ctrl"></param>
		public void Remove(AjaxUserControl ctrl)
		{
			List.Remove(ctrl);
		}
        /// <summary>
        /// Check if this collection contains a specified AjaxUserControl
        /// </summary>
        /// <param name="ctrl">AjaxUserControl to check</param>
        /// <returns>If the AjaxUserControl is contained</returns>
		public bool Contains(AjaxUserControl ctrl)
		{
			return List.Contains(ctrl);
		}
        /// <summary>
        /// Clear all AjaxUserControls contained
        /// </summary>
		public new void Clear()
		{
			List.Clear();
		}
        /// <summary>
        /// Returns the number of AjaxUserControls contained
        /// </summary>
		public new int Count
		{
			get { return List.Count; }
		}

        /// <summary>
        /// Get/Set the AjaxUserControl in one position
        /// </summary>
        /// <param name="pos">position in the collection</param>
        /// <returns>Selected AjaxUserControl</returns>
		public AjaxUserControl this[int pos]
		{
			get { return (AjaxUserControl)List[pos]; }
			set { List[pos] = value; }
		}
        /// <summary>
        /// Add one AjaxUserControl to a AjaxUserControlCollection
        /// </summary>
        /// <param name="root">The AjaxUserControl Collection</param>
        /// <param name="ctrl">Specified AjaxUserControl to Add</param>
        /// <returns>The AjaxUserControl Collection</returns>
        public static AjaxUserControlCollection operator +(AjaxUserControlCollection root, AjaxUserControl ctrl)
		{
            if (!(root & ctrl))
			    root.Add(ctrl);
			return root;
		}
        /// <summary>
        /// Removes an existing AjaxUserControl from a AjaxUserControlCollection
        /// </summary>
        /// <param name="root">The AjaxUserControl Collection</param>
        /// <param name="ctrl">Specified AjaxUserControl to remove</param>
        /// <returns>The AjaxUserControl Collection</returns>
        public static AjaxUserControlCollection operator -(AjaxUserControlCollection root, AjaxUserControl ctrl)
		{
			root.Remove(ctrl);
			return root;
		}
        /// <summary>
        /// Get the AjaxUserControl in one position
        /// </summary>
        /// <param name="root">The AjaxUserControl Collection</param>
        /// <param name="num">position in the collection</param>
        /// <returns>AjaxUserControl</returns>
        public static AjaxUserControl operator ^(AjaxUserControlCollection root, int num)
		{
			return root[num];
		}
        /// <summary>
        /// Get the AjaxUserControl in one position
        /// </summary>
        /// <param name="root">The AjaxUserControl Collection</param>
        /// <param name="num">position in the collection</param>
        /// <returns>AjaxUserControl</returns>
        public static AjaxUserControl operator |(AjaxUserControlCollection root, int num)
		{
			return root[num];
		}
        /// <summary>
        /// Check if this collection contains a specified AjaxUserControl
        /// </summary>
        /// <param name="root">The AjaxUserControl Collection</param>
        /// <param name="ctrl">AjaxUserControl to check</param>
        /// <returns>If the AjaxUserControl is contained</returns>
        /// <returns></returns>
        public static bool operator &(AjaxUserControlCollection root, AjaxUserControl ctrl)
		{
			return root.Contains(ctrl);
		}

        /// <summary>
        /// Creates a new empty AjaxUserControlCollection
        /// </summary>
        public AjaxUserControlCollection() : base() { }
    }
}
