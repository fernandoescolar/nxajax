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
namespace nxAjax.UI
{
    /// <summary>
    /// An nxUserControl collection object
    /// </summary>
    public class nxUserControlCollection: System.Collections.CollectionBase
	{
        /// <summary>
        /// Adds a new nxUserControl
        /// </summary>
        /// <param name="ctrl"></param>
		public void Add(nxUserControl ctrl)
		{
			List.Add(ctrl);
		}
        /// <summary>
        /// Removes an existing nxUserControl
        /// </summary>
        /// <param name="ctrl"></param>
		public void Remove(nxUserControl ctrl)
		{
			List.Remove(ctrl);
		}
        /// <summary>
        /// Check if this collection contains a specified nxUserControl
        /// </summary>
        /// <param name="ctrl">nxUserControl to check</param>
        /// <returns>If the nxUserControl is contained</returns>
		public bool Contains(nxUserControl ctrl)
		{
			return List.Contains(ctrl);
		}
        /// <summary>
        /// Clear all nxUserControls contained
        /// </summary>
		public new void Clear()
		{
			List.Clear();
		}
        /// <summary>
        /// Returns the number of nxUserControls contained
        /// </summary>
		public new int Count
		{
			get { return List.Count; }
		}

        /// <summary>
        /// Get/Set the nxUserControl in one position
        /// </summary>
        /// <param name="pos">position in the collection</param>
        /// <returns>Selected nxUserControl</returns>
		public nxUserControl this[int pos]
		{
			get { return (nxUserControl)List[pos]; }
			set { List[pos] = value; }
		}
        /// <summary>
        /// Add one nxUserControl to a nxUserControlCollection
        /// </summary>
        /// <param name="root">The nxUserControl Collection</param>
        /// <param name="ctrl">Specified nxUserControl to Add</param>
        /// <returns>The nxUserControl Collection</returns>
        public static nxUserControlCollection operator +(nxUserControlCollection root, nxUserControl ctrl)
		{
			root.Add(ctrl);
			return root;
		}
        /// <summary>
        /// Removes an existing nxUserControl from a nxUserControlCollection
        /// </summary>
        /// <param name="root">The nxUserControl Collection</param>
        /// <param name="ctrl">Specified nxUserControl to remove</param>
        /// <returns>The nxUserControl Collection</returns>
        public static nxUserControlCollection operator -(nxUserControlCollection root, nxUserControl ctrl)
		{
			root.Remove(ctrl);
			return root;
		}
        /// <summary>
        /// Get the nxUserControl in one position
        /// </summary>
        /// <param name="root">The nxUserControl Collection</param>
        /// <param name="num">position in the collection</param>
        /// <returns>nxUserControl</returns>
        public static nxUserControl operator ^(nxUserControlCollection root, int num)
		{
			return root[num];
		}
        /// <summary>
        /// Get the nxUserControl in one position
        /// </summary>
        /// <param name="root">The nxUserControl Collection</param>
        /// <param name="num">position in the collection</param>
        /// <returns>nxUserControl</returns>
        public static nxUserControl operator |(nxUserControlCollection root, int num)
		{
			return root[num];
		}
        /// <summary>
        /// Check if this collection contains a specified nxUserControl
        /// </summary>
        /// <param name="root">The nxUserControl Collection</param>
        /// <param name="ctrl">nxUserControl to check</param>
        /// <returns>If the nxUserControl is contained</returns>
        /// <returns></returns>
        public static bool operator &(nxUserControlCollection root, nxUserControl ctrl)
		{
			return root.Contains(ctrl);
		}

        /// <summary>
        /// Creates a new empty nxUserControlCollection
        /// </summary>
        public nxUserControlCollection() : base() { }
    }
}
