/*
 * Licensing
 *  Mozilla Public License 1.1 (MPL 1.1)
 * 
 * Fernando Escolar Martínez-Berganza <fer.escolar@gmail.com>
 * 
 */
using System;
using System.Collections.Specialized;

namespace nxAjax.xml
{
	/// <summary>
    /// XmlNode Object (Replaced by System.Xml.XmlNode)
	/// </summary>
    [Obsolete("Used System.Xml")] 
	public class XmlNode
	{
		protected string innerText;
		protected XmlNode myParent;
		protected XmlNodeCollection myChildNodes;
		protected StringDictionary myAttributes;
		protected string myValue, myName;

        /// <summary>
        /// Parent XmlNode
        /// </summary>
		public XmlNode Parent { get { return this.myParent; } set { this.myParent = value; } }
        /// <summary>
        /// Containde childnodes
        /// </summary>
		public XmlNodeCollection ChildNodes { get { return this.myChildNodes; } }
        /// <summary>
        /// Contained Attributtes
        /// </summary>
		public StringDictionary Attributes { get { return this.myAttributes; } }
        /// <summary>
        /// Value
        /// </summary>
		public string Value { get { return this.myValue; } set { this.myValue = value; } }
        /// <summary>
        /// Tag Name
        /// </summary>
		public string Name { get { return this.myName; } set { this.myName = value; } }
        /// <summary>
        /// Inner Xml
        /// </summary>
        public string InnerXml {
            get {
                string str = myValue;
                foreach (XmlNode n in myChildNodes)
                    str += n.InnerXml;
                return str;
            }
        }
        /// <summary>
        /// Creates a new empty XmlNode
        /// </summary>
		protected XmlNode()
		{
			innerText = myName = myValue = "";
			myChildNodes = new XmlNodeCollection(this);
			myAttributes = new StringDictionary();
		}
        /// <summary>
        /// Creates a new XmlNode with tag name and objects
        /// </summary>
        /// <param name="name">tag name</param>
        /// <param name="array">objects</param>
		internal XmlNode(string name, string[] array) : this()
		{
			if (array.Length>1)
			{
				bool escaneando = true;
				int i=0;
				while(escaneando)
				{
					int pos = XmlDocument.estaEnLista(array, "/" + array[i]);
					if (pos > 0)
					{
						string[] arr = new string[pos-i-1];
						Array.Copy(array, i+1, arr, 0, arr.Length);
						i = pos+1;
						myChildNodes.Add(new XmlNode(this, array[i], arr));
					}
					else
					{
						i++;
					}
				}
			}
			else
			{
				this.myName = name;
				this.myValue = array[0];
				this.innerText = this.myValue;
			}
			for (int j=0; j< array.Length; j++)
				this.innerText += array[0];
		}
        /// <summary>
        /// Creates a new XmlNode with tag name, parent and objects
        /// </summary>
        /// <param name="parent">parent node</param>
        /// <param name="name">tag name</param>
        /// <param name="array">objects</param>
		internal XmlNode(XmlNode parent, string name, string[] array) : this(name, array)
		{
			this.myParent = parent;
		}
	}
}
