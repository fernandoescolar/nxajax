/*
 * Licensing
 *  Mozilla Public License 1.1 (MPL 1.1)
 * 
 * Fernando Escolar Martínez-Berganza <fer.escolar@gmail.com>
 * 
 */

using System;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Specialized;

namespace nxAjax
{
	/// <summary>
    /// Multilanguage system.
	/// It loads from a system file.
	/// </summary>
	public class Language
	{
		private StringDictionary vars;
		private string id, name;
        
        /// <summary>
        /// Language ID
        /// </summary>
        public string Id { get { return id; } }

        /// <summary>
        /// Language Name
        /// </summary>
		public string Name { get { return name; } }
        /// <summary>
        /// Gets a literal
        /// </summary>
        /// <param name="key">key</param>
        /// <returns>Literal value for this key</returns>
		public string this[string key] { get { return vars[key]; } }
        /// <summary>
        /// All keys in the language file.
        /// </summary>
		public ICollection Keys { get{ return vars.Keys; } }
        /// <summary>
        /// Creates a new Language
        /// </summary>
        /// <param name="filename">Filename</param>
		public Language(string filename)
		{
			vars = new StringDictionary();
			StreamReader reader = new StreamReader(filename, GetFileEncoding(filename), true);
			bool info = false;
			bool dic = false;
			while(true)
			{
				string line = reader.ReadLine();
				if (line==null)
					break;
				if (line.Trim()=="")
					continue;
				if (line[0]=='#')
					continue;
				switch(line.ToUpper().Trim())
				{
					case "[INFO]":
						info = true;
						break;
					case "[DICTIONARY]":
						dic = true;
						break;
					default:
						if (dic)
						{
							int pos = line.IndexOf("=");
							string key = line.Substring(0, pos);
							//System.Text.Encoding srcEnc = System.Text.Encoding.GetEncoding(line.Substring(pos+1, line.Length - (pos+1)));
							//byte[] aux = System.Text.Encoding.Default.GetBytes(line.Substring(pos+1, line.Length - (pos+1)));
							string val = line.Substring(pos+1, line.Length - (pos+1));
							vars.Add(key, val);
						}
						else if(info)
						{
							int pos = line.IndexOf("=");
							string key = line.Substring(0, pos);
							string val = line.Substring(pos+1, line.Length - (pos+1));
							if(key=="name")
								this.name = val;
							if(key=="id")
								this.id = val;
						}
						break;
				}
				
			}
			reader.Close();
		}
        /// <summary>
        /// Detects the byte order mark of a file and returns
        /// an appropriate encoding for the file.
        /// </summary>
        /// <param name="srcFile"></param>
        /// <returns></returns>
        private Encoding GetFileEncoding(string srcFile)
        {
            // *** Use Default of Encoding.Default (Ansi CodePage)
            Encoding enc = Encoding.Default;
            // *** Detect byte order mark if any - otherwise assume default
            byte[] buffer = new byte[5];
            FileStream file = new FileStream(srcFile, FileMode.Open);
            file.Read(buffer, 0, 5);
            file.Close();

            if (buffer[0] == 0xef && buffer[1] == 0xbb && buffer[2] == 0xbf)
                enc = Encoding.UTF8;
            else if (buffer[0] == 0xfe && buffer[1] == 0xff)
                enc = Encoding.Unicode;
            else if (buffer[0] == 0 && buffer[1] == 0 && buffer[2] == 0xfe && buffer[3] == 0xff)
                enc = Encoding.UTF32;
            else if (buffer[0] == 0x2b && buffer[1] == 0x2f && buffer[2] == 0x76)
                enc = Encoding.UTF7;

            return enc;
        }
	}
}
