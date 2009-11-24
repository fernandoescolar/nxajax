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
	/// TreeNode Items Collection
	/// </summary>
	[DefaultProperty("Text")]
	public class TreeNodeCollection : CollectionBase
	{
        /// <summary>
        /// Creates a new TreeNode Collection
        /// </summary>
		public TreeNodeCollection() : base(){  }

        /// <summary>
        /// Adds a new TreeNode to collection
        /// </summary>
        /// <param name="ctrl">TreeNode to add</param>
		public void Add(TreeNode ctrl)
		{
			List.Add(ctrl);
		}
        /// <summary>
        /// Remove an existing TreeNode from the collection
        /// </summary>
        /// <param name="ctrl">TreeNode to remove</param>
		public void Remove(TreeNode ctrl)
		{
			List.Remove(ctrl);
		}
        /// <summary>
        /// Checks if an specified TreeNode is contained in the collection
        /// </summary>
        /// <param name="ctrl">TreeNode to check</param>
        /// <returns></returns>
		public bool Contains(TreeNode ctrl)
		{
			return List.Contains(ctrl);
		}
        /// <summary> 
        /// Clear all TreeNode contained
        /// </summary>
		public new void Clear()
		{
			List.Clear();
		}
        /// <summary>
        /// Gets TreeNode contained count
        /// </summary>
		public new int Count
		{
			get { return List.Count; }
		}
        /// <summary>
        /// Search Contained TreeNode
        /// </summary>
        /// <param name="value">TreeNode value</param>
        /// <returns>Founded TreeNode</returns>
		public TreeNode Search(string value)
		{
			for(int i = 0; i<this.Count; i++)
			{
				if(this[i].Value == value)
					return this[i];
				else
				{
					TreeNode n = this[i].Nodes.Search(value);
					if (n != null)
						return n;
				}
			}
			return null;
		}
        /// <summary>
        /// Gets/Sets a TreeNode in an specified position
        /// </summary>
        /// <param name="pos">position</param>
        /// <returns>Selected TreeNode</returns>
		public TreeNode this[int pos]
		{
			get { return (TreeNode)List[pos]; }
			set { List[pos] = value; }
		}
        /// <summary>
        /// Adds a new TreeNode to collection
        /// </summary>
        /// <param name="root">TreeNode Items Collection</param>
        /// <param name="ctrl">TreeNode to add</param>
        /// <returns>The current collection</returns>
		public static TreeNodeCollection operator+(TreeNodeCollection root, TreeNode ctrl)
		{
			root.Add(ctrl);
			return root;
		}
        /// <summary>
        /// Remove an existing TreeNode from the collection
        /// </summary>
        /// <param name="root">TreeNode Items Collection</param>
        /// <param name="ctrl">TreeNode to remove</param>
        /// <returns>The current collection</returns>
		public static TreeNodeCollection operator-(TreeNodeCollection root, TreeNode ctrl)
		{
			root.Remove(ctrl);
			return root;
		}
        /// <summary>
        /// Gets a TreeNode in an specified position
        /// </summary>
        /// <param name="root">TreeNode Items Collection</param>
        /// <param name="num">position</param>
        /// <returns>Selected TreeNode</returns>
		public static TreeNode operator^(TreeNodeCollection root, int num)
		{
			return root[num];
		}
        /// <summary>
        /// Gets a TreeNode in an specified position
        /// </summary>
        /// <param name="root">TreeNode Items Collection</param>
        /// <param name="num">position</param>
        /// <returns>Selected TreeNode</returns>
		public static TreeNode operator|(TreeNodeCollection root, int num)
		{
			return root[num];
		}
        /// <summary>
        /// Checks if an specified TreeNode is contained in the collection
        /// </summary>
        /// <param name="root">TreeNode Items Collection</param>
        /// <param name="ctrl">TreeNode to check</param>
        /// <returns>if TreeNode is contained</returns>
		public static bool operator&(TreeNodeCollection root, TreeNode ctrl)
		{
			return root.Contains(ctrl);
		}
	}
}
