/*
 * Licensing
 *  Mozilla Public License 1.1 (MPL 1.1)
 * 
 * Fernando Escolar Martínez-Berganza <fer.escolar@gmail.com>
 * 
 */

using System;
using System.IO;
using System.Collections;
using System.Collections.Specialized;

namespace nxAjax
{
	/// <summary>
    /// Template manager used in one nxPage.
	/// Gestor de las plantillas usadas por las nxPage.
	/// </summary>
	public class Templates
	{
		private string sPath;
		private TemplateDictionary td;
		private Language lang;
        private bool isLoaded;

        /// <summary>
        /// Gets if template has been loaded
        /// </summary>
        public bool IsLoaded { get { return isLoaded; } }
		
        /// <summary>
        /// Gets template page by key name
        /// </summary>
        /// <param name="key">key name</param>
        /// <returns>The template page</returns>
		public TemplatePage this[string key]
		{
			get { return (TemplatePage) td[key]; }
			set { td[key] = value; }
		}

        /// <summary>
        /// Creates a new Templates from a path with a language
        /// </summary>
        /// <param name="Path">Folder path</param>
        /// <param name="lang">Language object</param>
		public Templates(string Path, Language lang)
		{
			this.sPath = Path + (Path.EndsWith("\\")? "" : "\\");	
			td = new TemplateDictionary();
			this.lang = lang;
            this.isLoaded = false;
		}
        /// <summary>
        /// Load one template page from a file with one name
        /// </summary>
        /// <param name="name">Template page key name</param>
        /// <param name="fileName">Filename (without folder path)</param>
		public void Load(string name, string fileName)
		{
			TemplatePage page = new TemplatePage(Path.Combine(sPath, fileName), lang);
			td.Add(name, page);
            isLoaded = true;
		}
        /// <summary>
        /// Converts one template page in a string
        /// </summary>
        /// <param name="page">key name</param>
        /// <returns>result string</returns>
		public string ToString(string page)
		{
			return td[page].ToString();
		}

	}
	/// <summary>
    /// One Page of a template
	/// Representa al archivo de plantilla de una página.
	/// </summary>
	public class TemplatePage
	{
		private string text;
		private string sPath;
		private StringDictionary vars;
		private TemplateDictionary td;
		private Language lang;

        /// <summary>
        /// Gets Template Keys
        /// </summary>
        public ICollection Keys
        {
            get { return td.Keys; }
        }
        
        /// <summary>
        /// Get one contained template page
        /// </summary>
        /// <param name="key">key name</param>
        /// <returns>template page</returns>
		public TemplatePage this[string key]
		{
			get { return (TemplatePage) td[key]; }
			set { td[key] = value; }
		}
        /// <summary>
        /// Creates a new template page.
        /// </summary>
        /// <param name="name">Name of the page</param>
        /// <param name="text">contained text</param>
        /// <param name="lang">Language Object</param>
		public TemplatePage(string name, string text, Language lang)
		{
			this.lang = lang;
			this.text = text;
			this.sPath = "";
			td = new TemplateDictionary();
			load();
		}
        /// <summary>
        /// Creates a new template page from file
        /// </summary>
        /// <param name="Path">filename</param>
        /// <param name="lang">Language object</param>
		public TemplatePage(string Path, Language lang)
		{
			this.lang = lang;
			this.sPath = Path;
			td = new TemplateDictionary();
			StreamReader reader = new StreamReader(sPath);
			text = reader.ReadToEnd();
			reader.Close();
			load();
		}
        /// <summary>
        /// Returns if an specified key is contained in the keys list
        /// </summary>
        /// <param name="key">key name</param>
        /// <returns></returns>
        public bool ContainsKey(string key)
        {
            return td.Contains(key);
        }
		private void load()
		{
			vars = new StringDictionary();
			int position = 0;
			do
			{
				int start = text.IndexOf("<!$BEGIN ", position);
				if (start<0)
					break;
				int end = text.IndexOf(">", start);
				string pageName = text.Substring(start+9, end-start-9);
				int end2 = text.IndexOf("<!$END " + pageName + ">", position);
				string pageText = text.Substring(end+1, end2 - end - 1);
				td.Add(pageName, new TemplatePage(pageName, pageText, lang));
				position = end2 + ("<!$END " + pageName + ">").Length;
				text = text.Remove(start, position-start);
				position = 0;
			}while(position>=0);
			position = 0;
			do
			{
				int start = text.IndexOf("<$", position);
				if (start<0)
					break;
				int end = text.IndexOf("$>", start);
				try{ vars.Add(text.Substring(start+2, end-start-2), ""); }
				catch{}
				position = end;
			}while(position>=0);
		}
        /// <summary>
        /// Allocate one key value
        /// </summary>
        /// <param name="key">key name</param>
        /// <param name="val">new value</param>
		public void Allocate(string key, string val)
		{
			try
			{
				vars[key] = val;
			}
			catch{}
		}
        /// <summary>
        /// Add value to one key
        /// </summary>
        /// <param name="key">key name</param>
        /// <param name="val">new value</param>
		public void Add(string key, string val)
		{
			try
			{
				vars[key] += val;
			}
			catch{}
		}

        /// <summary>
        /// Convert template result in a string
        /// </summary>
        /// <returns></returns>
		public override string ToString()
		{
			string temp = text;
			foreach(string key in vars.Keys)
			{
				temp = temp.Replace("<$" + key.ToUpper() + "$>", vars[key]);
			}
			if (lang != null)
				foreach(string key in lang.Keys)
				{
					temp = temp.Replace("<&" + key.ToLower() + "&>", lang[key]);
				}
			return temp;
		}

	}
	/// <summary>
    /// Dictionary where it keep the replaced parts.
	/// Diccionario donde se almacenan las cadenas para ser sustituidas.
	/// </summary>
	public class TemplateDictionary : DictionaryBase
	{
        /// <summary>
        /// Get/Set Contained tamplate pages
        /// </summary>
        /// <param name="key">key name</param>
        /// <returns>Template page</returns>
		public TemplatePage this[string key]
		{
			get { return (TemplatePage) this.Dictionary[key]; }
			set { this.Dictionary[key] = value; }
		}
        /// <summary>
        /// Add new Template page
        /// </summary>
        /// <param name="key">key name</param>
        /// <param name="page">template page object</param>
		public void Add(string key, TemplatePage page)
		{
			this.Dictionary.Add(key, page);
		}
        /// <summary>
        /// Gets if contains one key value
        /// </summary>
        /// <param name="key">key name</param>
        /// <returns>if is contained</returns>
		public bool Contains(string key)
		{
			return this.Dictionary.Contains(key);
		}
        /// <summary>
        /// Gets all key names
        /// </summary>
		public ICollection Keys
		{
			get {return this.Dictionary.Keys;}
		}
	}

}
