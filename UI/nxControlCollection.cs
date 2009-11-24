/*
 * Licensing
 *  Mozilla Public License 1.1 (MPL 1.1)
 * 
 * Fernando Escolar Martínez-Berganza <fer.escolar@gmail.com>
 * 
 */

using System;
using System.Collections;

namespace nxAjax.UI
{
	/// <summary>
    /// nxControl Collection object
	/// </summary>
	public class nxControlCollection : System.Collections.CollectionBase
	{
        /// <summary>
        /// Adds a new nxControl
        /// </summary>
        /// <param name="ctrl"></param>
		public void Add(nxControl ctrl)
		{
			List.Add(ctrl);
		}
        /// <summary>
        /// Removes an existing nxControl
        /// </summary>
        /// <param name="ctrl"></param>
		public void Remove(nxControl ctrl)
		{
			List.Remove(ctrl);
		}
        /// <summary>
        /// Check if this collection contains a specified nxControl
        /// </summary>
        /// <param name="ctrl">nxControl to check</param>
        /// <returns>If the nxControl is contained</returns>
		public bool Contains(nxControl ctrl)
		{
			return List.Contains(ctrl);
		}
        /// <summary>
        /// Clear all nxControls contained
        /// </summary>
		public new void Clear()
		{
			List.Clear();
		}
        /// <summary>
        /// Returns the number of nxControls contained
        /// </summary>
		public new int Count
		{
			get { return List.Count; }
		}

        /// <summary>
        /// Get/Set the nxControl in one position
        /// </summary>
        /// <param name="pos">position in the collection</param>
        /// <returns>Selected nxControl</returns>
		public nxControl this[int pos]
		{
			get { return (nxControl)List[pos]; }
			set { List[pos] = value; }
		}
        /// <summary>
        /// Add one nxControl to a nxControlCollection
        /// </summary>
        /// <param name="root">The nxControl Collection</param>
        /// <param name="ctrl">Specified nxControl to Add</param>
        /// <returns>The nxControl Collection</returns>
		public static nxControlCollection operator+(nxControlCollection root, nxControl ctrl)
		{
			root.Add(ctrl);
			return root;
		}
        /// <summary>
        /// Removes an existing nxControl from a nxUserControlCollection
        /// </summary>
        /// <param name="root">The nxControl Collection</param>
        /// <param name="ctrl">Specified nxControl to remove</param>
        /// <returns>The nxControl Collection</returns>
		public static nxControlCollection operator-(nxControlCollection root, nxControl ctrl)
		{
			root.Remove(ctrl);
			return root;
		}
        /// <summary>
        /// Get the nxControl in one position
        /// </summary>
        /// <param name="root">The nxControl Collection</param>
        /// <param name="num">position in the collection</param>
        /// <returns>nxControl</returns>
		public static nxControl operator^(nxControlCollection root, int num)
		{
			return root[num];
		}
        /// <summary>
        /// Get the nxControl in one position
        /// </summary>
        /// <param name="root">The nxControl Collection</param>
        /// <param name="num">position in the collection</param>
        /// <returns>nxControl</returns>
		public static nxControl operator|(nxControlCollection root, int num)
		{
			return root[num];
		}
        /// <summary>
        /// Check if this collection contains a specified nxControl
        /// </summary>
        /// <param name="root">The nxControl Collection</param>
        /// <param name="ctrl">nxControl to check</param>
        /// <returns>If the nxControl is contained</returns>
        /// <returns></returns>
		public static bool operator&(nxControlCollection root, nxControl ctrl)
		{
			return root.Contains(ctrl);
		}
        /// <summary>
        /// Creates a new empty nxControlCollection
        /// </summary>
		public nxControlCollection():base() { }
	}
}
