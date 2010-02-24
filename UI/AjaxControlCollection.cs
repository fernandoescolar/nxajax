/*
 * Licensing
 *  Mozilla Public License 1.1 (MPL 1.1)
 * 
 * Fernando Escolar Martínez-Berganza <fer.escolar@gmail.com>
 * 
 */

using System;
using System.Collections;

namespace Framework.Ajax.UI
{
	/// <summary>
    /// AjaxControl Collection object
	/// </summary>
	public class AjaxControlCollection : System.Collections.CollectionBase
	{
        /// <summary>
        /// Adds a new AjaxControl
        /// </summary>
        /// <param name="ctrl"></param>
		public void Add(AjaxControl ctrl)
		{
			List.Add(ctrl);
		}
        /// <summary>
        /// Removes an existing AjaxControl
        /// </summary>
        /// <param name="ctrl"></param>
		public void Remove(AjaxControl ctrl)
		{
			List.Remove(ctrl);
		}
        /// <summary>
        /// Check if this collection contains a specified AjaxControl
        /// </summary>
        /// <param name="ctrl">AjaxControl to check</param>
        /// <returns>If the AjaxControl is contained</returns>
		public bool Contains(AjaxControl ctrl)
		{
			return List.Contains(ctrl);
		}
        /// <summary>
        /// Clear all AjaxControls contained
        /// </summary>
		public new void Clear()
		{
			List.Clear();
		}
        /// <summary>
        /// Returns the number of AjaxControls contained
        /// </summary>
		public new int Count
		{
			get { return List.Count; }
		}

        /// <summary>
        /// Get/Set the AjaxControl in one position
        /// </summary>
        /// <param name="pos">position in the collection</param>
        /// <returns>Selected AjaxControl</returns>
		public AjaxControl this[int pos]
		{
			get { return (AjaxControl)List[pos]; }
			set { List[pos] = value; }
		}
        /// <summary>
        /// Add one AjaxControl to a AjaxControlCollection
        /// </summary>
        /// <param name="root">The AjaxControl Collection</param>
        /// <param name="ctrl">Specified AjaxControl to Add</param>
        /// <returns>The AjaxControl Collection</returns>
		public static AjaxControlCollection operator+(AjaxControlCollection root, AjaxControl ctrl)
		{
            if (!(root & ctrl))
			    root.Add(ctrl);
			return root;
		}
        /// <summary>
        /// Removes an existing AjaxControl from a AjaxUserControlCollection
        /// </summary>
        /// <param name="root">The AjaxControl Collection</param>
        /// <param name="ctrl">Specified AjaxControl to remove</param>
        /// <returns>The AjaxControl Collection</returns>
		public static AjaxControlCollection operator-(AjaxControlCollection root, AjaxControl ctrl)
		{
			root.Remove(ctrl);
			return root;
		}
        /// <summary>
        /// Get the AjaxControl in one position
        /// </summary>
        /// <param name="root">The AjaxControl Collection</param>
        /// <param name="num">position in the collection</param>
        /// <returns>AjaxControl</returns>
		public static AjaxControl operator^(AjaxControlCollection root, int num)
		{
			return root[num];
		}
        /// <summary>
        /// Get the AjaxControl in one position
        /// </summary>
        /// <param name="root">The AjaxControl Collection</param>
        /// <param name="num">position in the collection</param>
        /// <returns>AjaxControl</returns>
		public static AjaxControl operator|(AjaxControlCollection root, int num)
		{
			return root[num];
		}
        /// <summary>
        /// Check if this collection contains a specified AjaxControl
        /// </summary>
        /// <param name="root">The AjaxControl Collection</param>
        /// <param name="ctrl">AjaxControl to check</param>
        /// <returns>If the AjaxControl is contained</returns>
        /// <returns></returns>
		public static bool operator&(AjaxControlCollection root, AjaxControl ctrl)
		{
			return root.Contains(ctrl);
		}
        /// <summary>
        /// Creates a new empty AjaxControlCollection
        /// </summary>
		public AjaxControlCollection():base() { }
	}
}
