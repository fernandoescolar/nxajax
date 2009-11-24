/*
 * Licensing
 *  Mozilla Public License 1.1 (MPL 1.1)
 * 
 * Fernando Escolar Martínez-Berganza <fer.escolar@gmail.com>
 * 
 */
using System;
using System.Collections;

/// <summary>
/// Obsolete Xml package. Replaced by System.Xml
/// </summary>
namespace nxAjax.xml
{
	/// <summary>
	/// XmlDocument Object (Replaced by System.Xml.XmlDocument)
	/// </summary>
    [Obsolete("Used System.Xml")] 
	public class XmlDocument
	{
		protected XmlNodeCollection myChildNodes;

        /// <summary>
        /// Gets one XmlNode by tag
        /// </summary>
        /// <param name="tag">tag name</param>
        /// <returns></returns>
		public XmlNode this[string tag]
		{
			get { 
				for (int i=0; i<myChildNodes.Count; i++)
					if (myChildNodes[i].Name == tag)	  
						return myChildNodes[i];
				return null;
			}
		}
        /// <summary>
        /// Gets all the childnodes
        /// </summary>
		public XmlNodeCollection ChildNodes { get { return this.myChildNodes; } }

        /// <summary>
        /// Creates a new XmlDocument
        /// </summary>
		public XmlDocument()
		{
			myChildNodes = new XmlNodeCollection(null);
		}
        /// <summary>
        /// Loads document from xml string
        /// </summary>
        /// <param name="xml">xml code string</param>
        /// <returns>XmlDocument</returns>
		public static XmlDocument LoadFromString(string xml)
		{
			XmlDocument doc = new XmlDocument();
			ArrayList lista = new ArrayList();
			

			bool escaneando = true;
			int a = -1;
			int b = -1;

			while(escaneando)
			{
				a = xml.IndexOf("<", 0);
				int z = xml.IndexOf(">", 0);


				if (b>0 && a>0 && a-b>0)
				{
					string texto = xml.Substring(b+1, b-a-1);
					string aux = texto.Replace("\n", "").Replace("\t", "").Replace(" ", "").Replace("\r", "");
					if (aux.Length>0)
						lista.Add(texto);
				}



				if (a>0 && z>0)
				{
					string s = xml.Substring(a+1, a-z-1);	
					if(s.ToLower().IndexOf("</" + s + ">")>0)
					{
						lista.Add(s);
					}

					b = z;
				}
				else
				{
					escaneando = false;
				}
			}
			
			escaneando = true;
			int i=0;
			while(escaneando)
			{
				int pos = estaEnLista(lista, (string)lista[i]);
				if (pos > 0)
				{
					string[] arr = new string[pos-i-1];
					lista.CopyTo(i+1, arr, 0, pos-i-1);
					i = pos+1;
					doc.myChildNodes.Add(new XmlNode((string)lista[i], arr));
				}
				else
				{
					i++;
				}
			}
			
			return doc;
		}
		internal static int estaEnLista(ArrayList lista, string name)
		{
			string[] aux = new string[lista.Count];
			lista.CopyTo(0, aux, 0, lista.Count);
			return estaEnLista(aux, name);
		}
		internal static int estaEnLista(string[] lista, string name)
		{
			for (int i = 0; i<lista.Length; i++)
				if (((string)lista[i])==name)
					return i;
	
			return -1;
		}
	}
}
