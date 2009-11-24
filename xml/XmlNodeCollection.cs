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

namespace nxAjax.xml
{
	/// <summary>
	/// Not used XmlNodeCollection.
	/// </summary>
    [Obsolete("Used System.Xml")] 
	[DefaultProperty("Text")]
	public class XmlNodeCollection : CollectionBase
	{
		private XmlNode parent;

        /// <summary>
        /// All childnodes
        /// </summary>
        /// <param name="parent"></param>
		public XmlNodeCollection(XmlNode parent): base()	{ this.parent = parent; }

        /// <summary>
        /// Adds a XmlNode
        /// </summary>
        /// <param name="ctrl">XmlNode</param>
		public void Add(XmlNode ctrl)
		{
			ctrl.Parent = parent;
			List.Add(ctrl);
		}
        /// <summary>
        /// Removes a XmlNode
        /// </summary>
        /// <param name="ctrl">XmlNode</param>
		public void Remove(XmlNode ctrl)
		{
			List.Remove(ctrl);
		}
        /// <summary>
        /// Determines if contains an especific XmlNode
        /// </summary>
        /// <param name="ctrl">XmlNode</param>
        /// <returns></returns>
		public bool Contains(XmlNode ctrl)
		{
			return List.Contains(ctrl);
		}
        /// <summary>
        /// Clear all XmlNodes
        /// </summary>
		public new void Clear()
		{
			List.Clear();
		}

        /// <summary>
        /// Gets number of XmlNodes
        /// </summary>
		public new int Count
		{
			get { return List.Count; }
		}

        /// <summary>
        /// Gets/Sets one XmlNode by position
        /// </summary>
        /// <param name="pos"></param>
        /// <returns></returns>
		public XmlNode this[int pos]
		{
			get { return (XmlNode)List[pos]; }
			set { List[pos] = value; }
		}

        /// <summary>
        /// Add XmlNode to Collection
        /// </summary>
        /// <param name="root">Xml node collection</param>
        /// <param name="ctrl">XmlNode to add</param>
        /// <returns>The xml node collection</returns>
		public static XmlNodeCollection operator+(XmlNodeCollection root, XmlNode ctrl)
		{
			root.Add(ctrl);
			return root;
		}
        /// <summary>
        /// Removes a XmlNode to Collection
        /// </summary>
        /// <param name="root">Xml node collection</param>
        /// <param name="ctrl">XmlNode to remove</param>
        /// <returns>The xml node collection</returns>
		public static XmlNodeCollection operator-(XmlNodeCollection root, XmlNode ctrl)
		{
			root.Remove(ctrl);
			return root;
		}
        /// <summary>
        /// Gets an XmlNode in a position
        /// </summary>
        /// <param name="root">Xml node collection</param>
        /// <param name="num">position</param>
        /// <returns>XmlNode in the position</returns>
		public static XmlNode operator^(XmlNodeCollection root, int num)
		{
			return root[num];
		}
        /// <summary>
        /// Gets an XmlNode in a position
        /// </summary>
        /// <param name="root">Xml node collection</param>
        /// <param name="num">position</param>
        /// <returns>XmlNode in the position</returns>
		public static XmlNode operator|(XmlNodeCollection root, int num)
		{
			return root[num];
		}
        /// <summary>
        /// Determines if contains an especific XmlNode
        /// </summary>
        /// <param name="root">Xml node collection</param>
        /// <param name="ctrl">XmlNode</param>
        /// <returns></returns>
		public static bool operator&(XmlNodeCollection root, XmlNode ctrl)
		{
			return root.Contains(ctrl);
		}
	}
}
