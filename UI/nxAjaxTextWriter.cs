/*
 * Licensing
 *  Mozilla Public License 1.1 (MPL 1.1)
 * 
 * Fernando Escolar Martínez-Berganza <fer.escolar@gmail.com>
 * 
 * Based on Mono System.Web.HtmlTextWriter
 */

using System;
using System.IO;
using System.Text;
using System.Collections;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;


namespace nxAjax.UI
{
    /// <summary>
    /// nxAjax Html and javascript code writer. It is based on Mono System.Web.HtmlTextWriter
    /// </summary>
    public class nxAjaxTextWriter : StringWriter
    {
        readonly static Hashtable _tagTable;
		readonly static Hashtable _attributeTable;
		readonly static Hashtable _styleTable;

        internal TextWriter TextWriter {
            get { return b; }
        }

        static nxAjaxTextWriter()
		{
			_tagTable = new Hashtable (tags.Length, StringComparer.OrdinalIgnoreCase);
			_attributeTable = new Hashtable (htmlattrs.Length, StringComparer.OrdinalIgnoreCase);
			_styleTable = new Hashtable (htmlstyles.Length, StringComparer.OrdinalIgnoreCase);

			foreach (HtmlTag tag in tags)
				_tagTable.Add (tag.name, tag);

			foreach (HtmlAttribute attr in htmlattrs)
				_attributeTable.Add (attr.name, attr);

			foreach (HtmlStyle style in htmlstyles)
				_styleTable.Add (style.name, style);
		}
        internal nxAjaxTextWriter() : this(new StringWriter())
		{
		}
        /// <summary>
        /// Contructor. Creates a new nxAjaxTextWriter object
        /// </summary>
        /// <param name="writer">source writer base</param>
		public nxAjaxTextWriter (TextWriter writer) : this (writer, DefaultTabString)
		{
		}

        /// <summary>
        /// Contructor. Creates a new nxAjaxTextWriter object
        /// </summary>
        /// <param name="writer">source writer base</param>
        /// <param name="tabString">tab string representation</param>
		public nxAjaxTextWriter (TextWriter writer, string tabString)
		{
			b = writer;
			tab_string = tabString;
		}

		internal static string StaticGetStyleName (System.Web.UI.HtmlTextWriterStyle styleKey)
		{
			if ((int) styleKey < htmlstyles.Length)
				return htmlstyles [(int) styleKey].name;

			return null;
		}

		
		protected static void RegisterAttribute (string name, HtmlTextWriterAttribute key)
		{
		}

		
		protected static void RegisterStyle (string name, HtmlTextWriterStyle key)
		{
		}

		
		protected static void RegisterTag (string name, HtmlTextWriterTag key)
		{
		}

        /// <summary>
        /// Adds a new attributte to render
        /// </summary>
        /// <param name="key">key attributte</param>
        /// <param name="value">new value</param>
        /// <param name="fEncode">with html encode</param>
		public virtual void AddAttribute (HtmlTextWriterAttribute key, string value, bool fEncode)
		{
			if (fEncode)
				value = HttpUtility.HtmlAttributeEncode (value);

			AddAttribute (GetAttributeName (key), value, key);
		}

        /// <summary>
        /// Adds a new attributte to render
        /// </summary>
        /// <param name="key">key attributte</param>
        /// <param name="value">new value</param>
		public virtual void AddAttribute (HtmlTextWriterAttribute key, string value)
		{
			if ((key != HtmlTextWriterAttribute.Name) && (key != HtmlTextWriterAttribute.Id))
				value = HttpUtility.HtmlAttributeEncode (value);

			AddAttribute (GetAttributeName (key), value, key);
		}

        /// <summary>
        /// Adds a new attributte to render
        /// </summary>
        /// <param name="name">attributte name</param>
        /// <param name="value">attributte value</param>
        /// <param name="fEncode">with html encode</param>
		public virtual void AddAttribute (string name, string value, bool fEncode)
		{
			if (fEncode)
				value = HttpUtility.HtmlAttributeEncode (value);

			AddAttribute (name, value, GetAttributeKey (name));
		}
        /// <summary>
        /// Adds a new attributte to render
        /// </summary>
        /// <param name="name">attributte name</param>
        /// <param name="value">attributte value</param>
		public virtual void AddAttribute (string name, string value)
		{
			HtmlTextWriterAttribute key = GetAttributeKey (name);

			if ((key != HtmlTextWriterAttribute.Name) && (key != HtmlTextWriterAttribute.Id))
				value = HttpUtility.HtmlAttributeEncode (value);

			AddAttribute (name, value, key);
		}
        /// <summary>
        /// Adds a new attributte to render
        /// </summary>
        /// <param name="name">attributte name</param>
        /// <param name="value">attributte value</param>
        /// <param name="key">attributte key</param>
		protected virtual void AddAttribute (string name, string value, HtmlTextWriterAttribute key)
		{
			NextAttrStack ();
			attrs [attrs_pos].name = name;
			attrs [attrs_pos].value = value;
			attrs [attrs_pos].key = key;
		}

        /// <summary>
        /// Adds a new style attributte to render
        /// </summary>
        /// <param name="name">attributte name</param>
        /// <param name="value">attributte value</param>
        /// <param name="key">attributte key</param>
		protected virtual void AddStyleAttribute (string name, string value, HtmlTextWriterStyle key)
		{
			NextStyleStack ();
			styles [styles_pos].name = name;
			value = HttpUtility.HtmlAttributeEncode (value);
			styles [styles_pos].value = value;
			styles [styles_pos].key = key;
		}

        /// <summary>
        /// Adds a new style attributte to render
        /// </summary>
        /// <param name="name">attributte name</param>
        /// <param name="value">attributte value</param>
		public virtual void AddStyleAttribute (string name, string value)
		{
			AddStyleAttribute (name, value, GetStyleKey (name));
		}
        /// <summary>
        /// Adds a new style attributte to render
        /// </summary>
        /// <param name="key">attributte key</param>
        /// <param name="value">attributte value</param>
		public virtual void AddStyleAttribute (HtmlTextWriterStyle key, string value)
		{
			AddStyleAttribute (GetStyleName (key), value, key);
		}
        /// <summary>
        /// Close the Writer
        /// </summary>
		public override void Close ()
		{
			b.Close ();
		}

		protected virtual string EncodeAttributeValue (HtmlTextWriterAttribute attrKey, string value)
		{
			return HttpUtility.HtmlAttributeEncode (value);
		}

		protected string EncodeAttributeValue (string value, bool fEncode)
		{
			if (fEncode)
				return HttpUtility.HtmlAttributeEncode (value);
			return value;
		}

		protected string EncodeUrl (string url)
		{
			return HttpUtility.UrlPathEncode (url);
		}


		protected virtual void FilterAttributes ()
		{
			AddedAttr style_attr = new AddedAttr ();

			for (int i = 0; i <= attrs_pos; i++) {
				AddedAttr a = attrs [i];
				if (OnAttributeRender (a.name, a.value, a.key)) {
					if (a.key == HtmlTextWriterAttribute.Style) {
						style_attr = a;
						continue;
					}

					WriteAttribute (a.name, a.value, false);
				}
			}

			if (styles_pos != -1 || style_attr.value != null) {
				Write (SpaceChar);
				Write ("style");
				Write (EqualsDoubleQuoteString);


				for (int i = 0; i <= styles_pos; i++) {
					AddedStyle a = styles [i];
					if (OnStyleAttributeRender (a.name, a.value, a.key)) {
						if (a.key == HtmlTextWriterStyle.BackgroundImage) {
							a.value = String.Concat ("url(", HttpUtility.UrlPathEncode (a.value), ")");
						}
						WriteStyleAttribute (a.name, a.value, false);
					}
				}

				Write (style_attr.value);
				Write (DoubleQuoteChar);
			}

			styles_pos = attrs_pos = -1;
		}
        /// <summary>
        /// Flush the writer
        /// </summary>
		public override void Flush ()
		{
			b.Flush ();
		}

		protected HtmlTextWriterAttribute GetAttributeKey (string attrName)
		{
			object attribute = _attributeTable [attrName];
			if (attribute == null)
				return (HtmlTextWriterAttribute) (-1);

			return (HtmlTextWriterAttribute) ((HtmlAttribute) attribute).key;
		}

		protected string GetAttributeName (HtmlTextWriterAttribute attrKey)
		{
			if ((int) attrKey < htmlattrs.Length)
				return htmlattrs [(int) attrKey].name;

			return null;
		}

		protected HtmlTextWriterStyle GetStyleKey (string styleName)
		{
			object style = _styleTable [styleName];
			if (style == null)
				return (HtmlTextWriterStyle) (-1);

			return (HtmlTextWriterStyle) ((HtmlStyle) style).key;
		}

		protected string GetStyleName (HtmlTextWriterStyle styleKey)
		{
			return StaticGetStyleName (styleKey);
		}

		protected virtual HtmlTextWriterTag GetTagKey (string tagName)
		{
			object tag = _tagTable [tagName];
			if (tag == null)
				return HtmlTextWriterTag.Unknown;

			return (HtmlTextWriterTag) ((HtmlTag) tag).key;
		}

		internal static string StaticGetTagName (HtmlTextWriterTag tagKey)
		{
			if ((int) tagKey < tags.Length)
				return tags [(int) tagKey].name;

			return null;
		}


		protected virtual string GetTagName (HtmlTextWriterTag tagKey)
		{
			if ((int) tagKey < tags.Length)
				return tags [(int) tagKey].name;

			return null;
		}

		protected bool IsAttributeDefined (HtmlTextWriterAttribute key)
		{
			string value;
			return IsAttributeDefined (key, out value);
		}

		protected bool IsAttributeDefined (HtmlTextWriterAttribute key, out string value)
		{
			for (int i = 0; i <= attrs_pos; i++)
				if (attrs [i].key == key) {
					value = attrs [i].value;
					return true;
				}

			value = null;
			return false;
		}

		protected bool IsStyleAttributeDefined (HtmlTextWriterStyle key)
		{
			string value;
			return IsStyleAttributeDefined (key, out value);
		}

		protected bool IsStyleAttributeDefined (HtmlTextWriterStyle key, out string value)
		{
			for (int i = 0; i <= styles_pos; i++)
				if (styles [i].key == key) {
					value = styles [i].value;
					return true;
				}

			value = null;
			return false;
		}

		protected virtual bool OnAttributeRender (string name, string value, HtmlTextWriterAttribute key)
		{
			return true;
		}

		protected virtual bool OnStyleAttributeRender (string name, string value, HtmlTextWriterStyle key)
		{
			return true;
		}

		protected virtual bool OnTagRender (string name, HtmlTextWriterTag key)
		{
			return true;
		}


		protected virtual void OutputTabs ()
		{
			if (!newline)
				return;
			newline = false;

			for (int i = 0; i < Indent; i++)
				b.Write (tab_string);
		}



		protected string PopEndTag ()
		{
			if (tagstack_pos == -1)
				throw new InvalidOperationException ();

			string s = TagName;
			tagstack_pos--;
			return s;
		}

		protected void PushEndTag (string endTag)
		{
			NextTagStack ();
			TagName = endTag;
		}

		void PushEndTag (HtmlTextWriterTag t)
		{
			NextTagStack ();
			TagKey = t;
		}


		protected virtual string RenderAfterContent ()
		{
			return null;
		}

		protected virtual string RenderAfterTag ()
		{
			return null;
		}

		protected virtual string RenderBeforeContent ()
		{
			return null;
		}

		protected virtual string RenderBeforeTag ()
		{
			return null;
		}
        /// <summary>
        /// Render a begin tag
        /// </summary>
        /// <param name="tagName">tag name to render</param>
		public virtual void RenderBeginTag (string tagName)
		{
			bool ignore = !OnTagRender (tagName, GetTagKey (tagName));

			PushEndTag (tagName);
			TagIgnore = ignore;
			DoBeginTag ();
		}
        /// <summary>
        /// Render a begin tag
        /// </summary>
        /// <param name="tagKey">tag key object to render</param>
		public virtual void RenderBeginTag (HtmlTextWriterTag tagKey)
		{
			bool ignore = !OnTagRender (GetTagName (tagKey), tagKey);

			PushEndTag (tagKey);
			DoBeginTag ();
			TagIgnore = ignore;
		}

		void WriteIfNotNull (string s)
		{
			if (s != null)
				Write (s);
		}


		void DoBeginTag ()
		{
			WriteIfNotNull (RenderBeforeTag ());
			if (!TagIgnore) {
				WriteBeginTag (TagName);
				FilterAttributes ();

				HtmlTextWriterTag key = (int) TagKey < tags.Length ? TagKey : HtmlTextWriterTag.Unknown;

				switch (tags [(int) key].tag_type) {
					case TagType.Inline:
						Write (TagRightChar);
						break;
					case TagType.Block:
						Write (TagRightChar);
						WriteLine ();
						Indent++;
						break;
					case TagType.SelfClosing:
						Write (SelfClosingTagEnd);
						break;
				}
			}
			
			// FIXME what do i do for self close here?
			WriteIfNotNull (RenderBeforeContent ());
		}

        /// <summary>
        /// Render the end of the current tag
        /// </summary>
		public virtual void RenderEndTag ()
		{
			
			// FIXME what do i do for self close here?
			WriteIfNotNull (RenderAfterContent ());

			if (!TagIgnore) {
				HtmlTextWriterTag key = (int) TagKey < tags.Length ? TagKey : HtmlTextWriterTag.Unknown;

				switch (tags [(int) key].tag_type) {
					case TagType.Inline:
						WriteEndTag (TagName);
						break;
					case TagType.Block:
						Indent--;
						WriteLineNoTabs ("");
						WriteEndTag (TagName);

						break;
					case TagType.SelfClosing:
						// NADA
						break;
				}
			}
			
			WriteIfNotNull (RenderAfterTag ());

			PopEndTag ();
		}

        /// <summary>
        /// Write an attribute
        /// </summary>
        /// <param name="name">Attribute name</param>
        /// <param name="value">Attribute value</param>
        /// <param name="fEncode">Html Encode</param>
		public virtual void WriteAttribute (string name, string value, bool fEncode)
		{
			Write (SpaceChar);
			Write (name);
			if (value != null) {
				Write (EqualsDoubleQuoteString);
				value = EncodeAttributeValue (value, fEncode);
				Write (value);
				Write (DoubleQuoteChar);
			}
		}

        /// <summary>
        /// Write the begin of a tag
        /// <remarks>Without '>' character</remarks>
        /// </summary>
        /// <param name="tagName">tag name to begin</param>
		public virtual void WriteBeginTag (string tagName)
		{
			Write (TagLeftChar);
			Write (tagName);
		}
        /// <summary>
        /// Write the end of a tag
        /// </summary>
        /// <param name="tagName">tag name to end</param>
		public virtual void WriteEndTag (string tagName)
		{
			Write (EndTagLeftChars);
			Write (tagName);
			Write (TagRightChar);
		}
        /// <summary>
        /// Write full tag begin
        /// <remarks>With '>' character</remarks>
        /// </summary>
        /// <param name="tagName"></param>
		public virtual void WriteFullBeginTag (string tagName)
		{
			Write (TagLeftChar);
			Write (tagName);
			Write (TagRightChar);
		}
        /// <summary>
        /// Write one style attribute
        /// </summary>
        /// <param name="name">Attribute name</param>
        /// <param name="value">Attribute value</param>
		public virtual void WriteStyleAttribute (string name, string value)
		{
			WriteStyleAttribute (name, value, false);
		}
        /// <summary>
        /// Write one style attribute
        /// </summary>
        /// <param name="name">Attribute name</param>
        /// <param name="value">Attribute value</param>
        /// <param name="fEncode">Html encode</param>
		public virtual void WriteStyleAttribute (string name, string value, bool fEncode)
		{
			Write (name);
			Write (StyleEqualsChar);
			Write (EncodeAttributeValue (value, fEncode));
			Write (SemicolonChar);
		}
        /// <summary>
        /// Write one delimited buffer
        /// </summary>
        /// <param name="buffer">buffer</param>
        /// <param name="index">start intex</param>
        /// <param name="count">count write</param>
		public override void Write (char [] buffer, int index, int count)
		{
			OutputTabs ();
			b.Write (buffer, index, count);
		}
        /// <summary>
        /// Wrie one double value
        /// </summary>
        /// <param name="value">double value</param>
		public override void Write (double value)
		{
			OutputTabs ();
			b.Write (value);
		}
        /// <summary>
        /// Write one char value
        /// </summary>
        /// <param name="value">char value</param>
		public override void Write (char value)
		{
			OutputTabs ();
			b.Write (value);
		}
        /// <summary>
        /// Write all chars in a buffer
        /// </summary>
        /// <param name="buffer">buffer</param>
		public override void Write (char [] buffer)
		{
			OutputTabs ();
			b.Write (buffer);
		}
        /// <summary>
        /// Write one int value
        /// </summary>
        /// <param name="value">int value</param>
		public override void Write (int value)
		{
			OutputTabs ();
			b.Write (value);
		}
        /// <summary>
        /// Write one object with a System.String.Format
        /// </summary>
        /// <param name="format">format</param>
        /// <param name="arg0">object to write</param>
		public override void Write (string format, object arg0)
		{
			OutputTabs ();
			b.Write (format, arg0);
		}
        /// <summary>
        /// Write two object with a System.String.Format
        /// </summary>
        /// <param name="format">format</param>
        /// <param name="arg0">object to write</param>
        /// <param name="arg1">object to write</param>
		public override void Write (string format, object arg0, object arg1)
		{
			OutputTabs ();
			b.Write (format, arg0, arg1);
		}
        /// <summary>
        /// Write some object with a System.String.Format
        /// </summary>
        /// <param name="format">format</param>
        /// <param name="args">object to write</param>
		public override void Write (string format, params object [] args)
		{
			OutputTabs ();
			b.Write (format, args);
		}
        /// <summary>
        /// Write one string value
        /// </summary>
        /// <param name="s">string value</param>
		public override void Write (string s)
		{
			OutputTabs ();
			b.Write (s);
		}
        /// <summary>
        /// Write one long value
        /// </summary>
        /// <param name="value">long value</param>
		public override void Write (long value)
		{
			OutputTabs ();
			b.Write (value);
		}
        /// <summary>
        /// Write one object like a string
        /// </summary>
        /// <param name="value">object value</param>
		public override void Write (object value)
		{
			OutputTabs ();
			b.Write (value);
		}
        /// <summary>
        /// Write one float value
        /// </summary>
        /// <param name="value"></param>
		public override void Write (float value)
		{
			OutputTabs ();
			b.Write (value);
		}
        /// <summary>
        /// Write one boolean value
        /// </summary>
        /// <param name="value">boolean value</param>
		public override void Write (bool value)
		{
			OutputTabs ();
			b.Write (value);
		}
        /// <summary>
        /// Write one attribute
        /// </summary>
        /// <param name="name">Attribute name</param>
        /// <param name="value">Attribute value</param>
		public virtual void WriteAttribute (string name, string value)
		{
			WriteAttribute (name, value, false);
		}
        /// <summary>
        /// Write one char in a single line
        /// </summary>
        /// <param name="value">char value</param>
		public override void WriteLine (char value)
		{
			OutputTabs ();
			b.WriteLine (value);
			newline = true;
		}
        /// <summary>
        /// Write one long value in a single line
        /// </summary>
        /// <param name="value">long value</param>
		public override void WriteLine (long value)
		{
			OutputTabs ();
			b.WriteLine (value);
			newline = true;
		}
        /// <summary>
        /// Write one object string representation value in a single line
        /// </summary>
        /// <param name="value">object value</param>
		public override void WriteLine (object value)
		{
			OutputTabs ();
			b.WriteLine (value);
			newline = true;
		}

        /// <summary>
        /// Write one double value in a single line
        /// </summary>
        /// <param name="value">double value</param>
		public override void WriteLine (double value)
		{
			OutputTabs ();
			b.WriteLine (value);
			newline = true;
		}
        /// <summary>
        /// Write one delimited buffer in a single line
        /// </summary>
        /// <param name="buffer">buffer object</param>
        /// <param name="index">start index</param>
        /// <param name="count">write count</param>
		public override void WriteLine (char [] buffer, int index, int count)
		{
			OutputTabs ();
			b.WriteLine (buffer, index, count);
			newline = true;
		}
        /// <summary>
        /// Write all characters in one buffer in a single line
        /// </summary>
        /// <param name="buffer">buffer object</param>
		public override void WriteLine (char [] buffer)
		{
			OutputTabs ();
			b.WriteLine (buffer);
			newline = true;
		}
        /// <summary>
        /// Write one bool value in a single line
        /// </summary>
        /// <param name="value"></param>
		public override void WriteLine (bool value)
		{
			OutputTabs ();
			b.WriteLine (value);
			newline = true;
		}
        /// <summary>
        /// Write an empty single line
        /// </summary>
		public override void WriteLine ()
		{
			OutputTabs ();
			b.WriteLine ();
			newline = true;
		}
        /// <summary>
        /// Write an int value in a single line
        /// </summary>
        /// <param name="value">int value</param>
		public override void WriteLine (int value)
		{
			OutputTabs ();
			b.WriteLine (value);
			newline = true;
		}
        /// <summary>
        /// Write two objects with a System.String.Format 
        /// </summary>
        /// <param name="format">format</param>
        /// <param name="arg0">object value</param>
        /// <param name="arg1">object value</param>
		public override void WriteLine (string format, object arg0, object arg1)
		{
			OutputTabs ();
			b.WriteLine (format, arg0, arg1);
			newline = true;
		}
        /// <summary>
        /// Write one object with a System.String.Format 
        /// </summary>
        /// <param name="format">format</param>
        /// <param name="arg0">object value</param>
		public override void WriteLine (string format, object arg0)
		{
			OutputTabs ();
			b.WriteLine (format, arg0);
			newline = true;
		}
        /// <summary>
        /// Write some objects with a System.String.Format 
        /// </summary>
        /// <param name="format">format</param>
        /// <param name="args">object values</param>
		public override void WriteLine (string format, params object [] args)
		{
			OutputTabs ();
			b.WriteLine (format, args);
			newline = true;
		}
        /// <summary>
        /// Write an unsigned integer in a single line
        /// </summary>
        /// <param name="value"></param>
		[CLSCompliant (false)]
		public override void WriteLine (uint value)
		{
			OutputTabs ();
			b.WriteLine (value);
			newline = true;
		}
        /// <summary>
        /// Write a string in a single line
        /// </summary>
        /// <param name="s"></param>
		public override void WriteLine (string s)
		{
			OutputTabs ();
			b.WriteLine (s);
			newline = true;
		}
        /// <summary>
        /// Write a float value in a single line
        /// </summary>
        /// <param name="value"></param>
		public override void WriteLine (float value)
		{
			OutputTabs ();
			b.WriteLine (value);
			newline = true;
		}
        /// <summary>
        /// Write an string value in a single line without tabs spacing
        /// </summary>
        /// <param name="s">string value</param>
		public void WriteLineNoTabs (string s)
		{
			b.WriteLine (s);
			newline = true;
		}
        /// <summary>
        /// Gets text encoding
        /// </summary>
		public override Encoding Encoding {
			get {
				return b.Encoding;
			}
		}

		int indent;
        /// <summary>
        /// Gets/Sets Indent position
        /// </summary>
		public int Indent {
			get {
				return indent;
			}
			set {
				indent = value;
			}
		}
        /// <summary>
        /// Gets/Sets Source writer base
        /// </summary>
		public System.IO.TextWriter InnerWriter {
			get {
				return b;
			}
			set {
				b = value;
			}
		}
        /// <summary>
        /// Gets/Sets New line string representation
        /// </summary>
		public override string NewLine {
			get {
				return b.NewLine;
			}
			set {
				b.NewLine = value;
			}
		}

		protected HtmlTextWriterTag TagKey {
			get {
				if (tagstack_pos == -1)
					throw new InvalidOperationException ();

				return tagstack [tagstack_pos].key;
			}
			set {
				tagstack [tagstack_pos].key = value;
				tagstack [tagstack_pos].name = GetTagName (value);
			}
		}

		protected string TagName {
			get {
				if (tagstack_pos == -1)
					throw new InvalidOperationException ();

				return tagstack [tagstack_pos].name;
			}
			set {
				tagstack [tagstack_pos].name = value;
				tagstack [tagstack_pos].key = GetTagKey (value);
				if (tagstack [tagstack_pos].key != HtmlTextWriterTag.Unknown)
					tagstack [tagstack_pos].name = GetTagName (tagstack [tagstack_pos].key);
			}
		}

		bool TagIgnore {
			get {
				if (tagstack_pos == -1)
					throw new InvalidOperationException ();

				return tagstack [tagstack_pos].ignore;
			}

			set {
				if (tagstack_pos == -1)
					throw new InvalidOperationException ();
				
				tagstack [tagstack_pos].ignore = value;
			}
		}

		TextWriter b;
		string tab_string;
		bool newline;

		//
		// These emulate generic Stack <T>, since we can't use that ;-(. _pos is the current
		// element.IE, you edit blah [blah_pos]. I *really* want generics, sigh.
		//
		AddedStyle [] styles;
		AddedAttr [] attrs;
		AddedTag [] tagstack;

		int styles_pos = -1, attrs_pos = -1, tagstack_pos = -1;

		struct AddedTag {
			public string name;
			public HtmlTextWriterTag key;
			public bool ignore;
		}


		struct AddedStyle {
			public string name;
			public HtmlTextWriterStyle key;
			public string value;
		}


		struct AddedAttr {
			public string name;
			public HtmlTextWriterAttribute key;
			public string value;
		}

		void NextStyleStack ()
		{
			if (styles == null)
				styles = new AddedStyle [16];

			if (++styles_pos < styles.Length)
				return;

			int nsize = styles.Length * 2;
			AddedStyle [] ncontents = new AddedStyle [nsize];

			Array.Copy (styles, ncontents, styles.Length);
			styles = ncontents;
		}

		void NextAttrStack ()
		{
			if (attrs == null)
				attrs = new AddedAttr [16];

			if (++attrs_pos < attrs.Length)
				return;

			int nsize = attrs.Length * 2;
			AddedAttr [] ncontents = new AddedAttr [nsize];

			Array.Copy (attrs, ncontents, attrs.Length);
			attrs = ncontents;
		}

		void NextTagStack ()
		{
			if (tagstack == null)
				tagstack = new AddedTag [16];

			if (++tagstack_pos < tagstack.Length)
				return;

			int nsize = tagstack.Length * 2;
			AddedTag [] ncontents = new AddedTag [nsize];

			Array.Copy (tagstack, ncontents, tagstack.Length);
			tagstack = ncontents;
		}
        /// <summary>
        /// Gets Default tab string value
        /// </summary>
		public const string DefaultTabString = "\t";
        /// <summary>
        /// Gets Default double quoted char value
        /// </summary>
		public const char DoubleQuoteChar = '"';
        /// <summary>
        /// Gets Default end tag left string value
        /// </summary>
		public const string EndTagLeftChars = "</";
        /// <summary>
        /// Gets Default equals char symbol
        /// </summary>
		public const char EqualsChar = '=';
        /// <summary>
        /// Gets Default equals with double quote string
        /// </summary>
		public const string EqualsDoubleQuoteString = "=\"";
        /// <summary>
        /// Gets Default self tag closing string value
        /// </summary>
		public const string SelfClosingChars = " /";
        /// <summary>
        /// Gets Default self tag closing tag and end string value
        /// </summary>
		public const string SelfClosingTagEnd = " />";
        /// <summary>
        /// Gets Default style attribute separator char value
        /// </summary>
		public const char SemicolonChar = ';';
        /// <summary>
        /// Gets default single quote char value
        /// </summary>
		public const char SingleQuoteChar = '\'';
        /// <summary>
        /// Gets Default slash char value
        /// </summary>
		public const char SlashChar = '/';
        /// <summary>
        /// Gets Default space char value
        /// </summary>
		public const char SpaceChar = ' ';
        /// <summary>
        /// Gets Default style attribute equals char value
        /// </summary>
		public const char StyleEqualsChar = ':';
        /// <summary>
        /// Gets Default tag left char value
        /// </summary>
		public const char TagLeftChar = '<';
        /// <summary>
        /// Gets Default tag right char value
        /// </summary>
		public const char TagRightChar = '>';

		enum TagType {
			Block,
			Inline,
			SelfClosing,
		}


		sealed class HtmlTag {
			readonly public HtmlTextWriterTag key;
			readonly public string name;
			readonly public TagType tag_type;

			public HtmlTag (HtmlTextWriterTag k, string n, TagType tt)
			{
				key = k;
				name = n;
				tag_type = tt;
			}
		}

		sealed class HtmlStyle {
			readonly public HtmlTextWriterStyle key;
			readonly public string name;

			public HtmlStyle (HtmlTextWriterStyle k, string n)
			{
				key = k;
				name = n;
			}
		}


		sealed class HtmlAttribute {
			readonly public HtmlTextWriterAttribute key;
			readonly public string name;

			public HtmlAttribute (HtmlTextWriterAttribute k, string n)
			{
				key = k;
				name = n;
			}
		}

		static HtmlTag [] tags = {
			new HtmlTag (HtmlTextWriterTag.Unknown,    "",                  TagType.Block),
			new HtmlTag (HtmlTextWriterTag.A,          "a",                 TagType.Inline),
			new HtmlTag (HtmlTextWriterTag.Acronym,    "acronym",           TagType.Inline),
			new HtmlTag (HtmlTextWriterTag.Address,    "address",           TagType.Block),
			new HtmlTag (HtmlTextWriterTag.Area,       "area",              TagType.Block),
			new HtmlTag (HtmlTextWriterTag.B,          "b",                 TagType.Inline),
			new HtmlTag (HtmlTextWriterTag.Base,       "base",              TagType.SelfClosing),
			new HtmlTag (HtmlTextWriterTag.Basefont,   "basefont",          TagType.SelfClosing),
			new HtmlTag (HtmlTextWriterTag.Bdo,        "bdo",               TagType.Inline),
			new HtmlTag (HtmlTextWriterTag.Bgsound,    "bgsound",           TagType.SelfClosing),
			new HtmlTag (HtmlTextWriterTag.Big,        "big",               TagType.Inline),
			new HtmlTag (HtmlTextWriterTag.Blockquote, "blockquote",        TagType.Block),
			new HtmlTag (HtmlTextWriterTag.Body,       "body",              TagType.Block),
			new HtmlTag (HtmlTextWriterTag.Br,         "br",                TagType.Block),
			new HtmlTag (HtmlTextWriterTag.Button,     "button",            TagType.Inline),
			new HtmlTag (HtmlTextWriterTag.Caption,    "caption",           TagType.Block),
			new HtmlTag (HtmlTextWriterTag.Center,     "center",            TagType.Block),
			new HtmlTag (HtmlTextWriterTag.Cite,       "cite",              TagType.Inline),
			new HtmlTag (HtmlTextWriterTag.Code,       "code",              TagType.Inline),
			new HtmlTag (HtmlTextWriterTag.Col,        "col",               TagType.SelfClosing),
			new HtmlTag (HtmlTextWriterTag.Colgroup,   "colgroup",          TagType.Block),
			new HtmlTag (HtmlTextWriterTag.Dd,         "dd",                TagType.Inline),
			new HtmlTag (HtmlTextWriterTag.Del,        "del",               TagType.Inline),
			new HtmlTag (HtmlTextWriterTag.Dfn,        "dfn",               TagType.Inline),
			new HtmlTag (HtmlTextWriterTag.Dir,        "dir",               TagType.Block),
			new HtmlTag (HtmlTextWriterTag.Div,        "div",               TagType.Block),
			new HtmlTag (HtmlTextWriterTag.Dl,         "dl",                TagType.Block),
			new HtmlTag (HtmlTextWriterTag.Dt,         "dt",                TagType.Inline),
			new HtmlTag (HtmlTextWriterTag.Em,         "em",                TagType.Inline),
			new HtmlTag (HtmlTextWriterTag.Embed,      "embed",             TagType.SelfClosing),
			new HtmlTag (HtmlTextWriterTag.Fieldset,   "fieldset",          TagType.Block),
			new HtmlTag (HtmlTextWriterTag.Font,       "font",              TagType.Inline),
			new HtmlTag (HtmlTextWriterTag.Form,       "form",              TagType.Block),
			new HtmlTag (HtmlTextWriterTag.Frame,      "frame",             TagType.SelfClosing),
			new HtmlTag (HtmlTextWriterTag.Frameset,   "frameset",          TagType.Block),
			new HtmlTag (HtmlTextWriterTag.H1,         "h1",                TagType.Block),
			new HtmlTag (HtmlTextWriterTag.H2,         "h2",                TagType.Block),
			new HtmlTag (HtmlTextWriterTag.H3,         "h3",                TagType.Block),
			new HtmlTag (HtmlTextWriterTag.H4,         "h4",                TagType.Block),
			new HtmlTag (HtmlTextWriterTag.H5,         "h5",                TagType.Block),
			new HtmlTag (HtmlTextWriterTag.H6,         "h6",                TagType.Block),
			new HtmlTag (HtmlTextWriterTag.Head,       "head",              TagType.Block),
			new HtmlTag (HtmlTextWriterTag.Hr,         "hr",                TagType.SelfClosing),
			new HtmlTag (HtmlTextWriterTag.Html,       "html",              TagType.Block),
			new HtmlTag (HtmlTextWriterTag.I,          "i",                 TagType.Inline),
			new HtmlTag (HtmlTextWriterTag.Iframe,     "iframe",            TagType.Block),
			new HtmlTag (HtmlTextWriterTag.Img,        "img",               TagType.SelfClosing),
			new HtmlTag (HtmlTextWriterTag.Input,      "input",             TagType.SelfClosing),
			new HtmlTag (HtmlTextWriterTag.Ins,        "ins",               TagType.Inline),
			new HtmlTag (HtmlTextWriterTag.Isindex,    "isindex",           TagType.SelfClosing),
			new HtmlTag (HtmlTextWriterTag.Kbd,        "kbd",               TagType.Inline),
			new HtmlTag (HtmlTextWriterTag.Label,      "label",             TagType.Inline),
			new HtmlTag (HtmlTextWriterTag.Legend,     "legend",            TagType.Block),
			new HtmlTag (HtmlTextWriterTag.Li,         "li",                TagType.Inline),
			new HtmlTag (HtmlTextWriterTag.Link,       "link",              TagType.SelfClosing),
			new HtmlTag (HtmlTextWriterTag.Map,        "map",               TagType.Block),
			new HtmlTag (HtmlTextWriterTag.Marquee,    "marquee",           TagType.Block),
			new HtmlTag (HtmlTextWriterTag.Menu,       "menu",              TagType.Block),
			new HtmlTag (HtmlTextWriterTag.Meta,       "meta",              TagType.SelfClosing),
			new HtmlTag (HtmlTextWriterTag.Nobr,       "nobr",              TagType.Inline),
			new HtmlTag (HtmlTextWriterTag.Noframes,   "noframes",          TagType.Block),
			new HtmlTag (HtmlTextWriterTag.Noscript,   "noscript",          TagType.Block),
			new HtmlTag (HtmlTextWriterTag.Object,     "object",            TagType.Block),
			new HtmlTag (HtmlTextWriterTag.Ol,         "ol",                TagType.Block),
			new HtmlTag (HtmlTextWriterTag.Option,     "option",            TagType.Block),
			new HtmlTag (HtmlTextWriterTag.P,          "p",                 TagType.Inline),
			new HtmlTag (HtmlTextWriterTag.Param,      "param",             TagType.Block),
			new HtmlTag (HtmlTextWriterTag.Pre,        "pre",               TagType.Block),
			new HtmlTag (HtmlTextWriterTag.Q,          "q",                 TagType.Inline),
			new HtmlTag (HtmlTextWriterTag.Rt,         "rt",                TagType.Block),
			new HtmlTag (HtmlTextWriterTag.Ruby,       "ruby",              TagType.Block),
			new HtmlTag (HtmlTextWriterTag.S,          "s",                 TagType.Inline),
			new HtmlTag (HtmlTextWriterTag.Samp,       "samp",              TagType.Inline),
			new HtmlTag (HtmlTextWriterTag.Script,     "script",            TagType.Block),
			new HtmlTag (HtmlTextWriterTag.Select,     "select",            TagType.Block),
			new HtmlTag (HtmlTextWriterTag.Small,      "small",             TagType.Block),
			new HtmlTag (HtmlTextWriterTag.Span,       "span",              TagType.Inline),
			new HtmlTag (HtmlTextWriterTag.Strike,     "strike",            TagType.Inline),
			new HtmlTag (HtmlTextWriterTag.Strong,     "strong",            TagType.Inline),
			new HtmlTag (HtmlTextWriterTag.Style,      "style",             TagType.Block),
			new HtmlTag (HtmlTextWriterTag.Sub,        "sub",               TagType.Inline),
			new HtmlTag (HtmlTextWriterTag.Sup,        "sup",               TagType.Inline),
			new HtmlTag (HtmlTextWriterTag.Table,      "table",             TagType.Block),
			new HtmlTag (HtmlTextWriterTag.Tbody,      "tbody",             TagType.Block),
			new HtmlTag (HtmlTextWriterTag.Td,         "td",                TagType.Inline),
			new HtmlTag (HtmlTextWriterTag.Textarea,   "textarea",          TagType.Inline),
			new HtmlTag (HtmlTextWriterTag.Tfoot,      "tfoot",             TagType.Block),
			new HtmlTag (HtmlTextWriterTag.Th,         "th",                TagType.Inline),
			new HtmlTag (HtmlTextWriterTag.Thead,      "thead",             TagType.Block),
			new HtmlTag (HtmlTextWriterTag.Title,      "title",             TagType.Block),
			new HtmlTag (HtmlTextWriterTag.Tr,         "tr",                TagType.Block),
			new HtmlTag (HtmlTextWriterTag.Tt,         "tt",                TagType.Inline),
			new HtmlTag (HtmlTextWriterTag.U,          "u",                 TagType.Inline),
			new HtmlTag (HtmlTextWriterTag.Ul,         "ul",                TagType.Block),
			new HtmlTag (HtmlTextWriterTag.Var,        "var",               TagType.Inline),
			new HtmlTag (HtmlTextWriterTag.Wbr,        "wbr",               TagType.SelfClosing),
			new HtmlTag (HtmlTextWriterTag.Xml,        "xml",               TagType.Block),
		};

		static HtmlAttribute [] htmlattrs = {
			new HtmlAttribute (HtmlTextWriterAttribute.Accesskey,         "accesskey"),
			new HtmlAttribute (HtmlTextWriterAttribute.Align,             "align"),
			new HtmlAttribute (HtmlTextWriterAttribute.Alt,               "alt"),
			new HtmlAttribute (HtmlTextWriterAttribute.Background,        "background"),
			new HtmlAttribute (HtmlTextWriterAttribute.Bgcolor,           "bgcolor"),
			new HtmlAttribute (HtmlTextWriterAttribute.Border,            "border"),
			new HtmlAttribute (HtmlTextWriterAttribute.Bordercolor,       "bordercolor"),
			new HtmlAttribute (HtmlTextWriterAttribute.Cellpadding,       "cellpadding"),
			new HtmlAttribute (HtmlTextWriterAttribute.Cellspacing,       "cellspacing"),
			new HtmlAttribute (HtmlTextWriterAttribute.Checked,           "checked"),
			new HtmlAttribute (HtmlTextWriterAttribute.Class,             "class"),
			new HtmlAttribute (HtmlTextWriterAttribute.Cols,              "cols"),
			new HtmlAttribute (HtmlTextWriterAttribute.Colspan,           "colspan"),
			new HtmlAttribute (HtmlTextWriterAttribute.Disabled,          "disabled"),
			new HtmlAttribute (HtmlTextWriterAttribute.For,               "for"),
			new HtmlAttribute (HtmlTextWriterAttribute.Height,            "height"),
			new HtmlAttribute (HtmlTextWriterAttribute.Href,              "href"),
			new HtmlAttribute (HtmlTextWriterAttribute.Id,                "id"),
			new HtmlAttribute (HtmlTextWriterAttribute.Maxlength,         "maxlength"),
			new HtmlAttribute (HtmlTextWriterAttribute.Multiple,          "multiple"),
			new HtmlAttribute (HtmlTextWriterAttribute.Name,              "name"),
			new HtmlAttribute (HtmlTextWriterAttribute.Nowrap,            "nowrap"),
			new HtmlAttribute (HtmlTextWriterAttribute.Onchange,          "onchange"),
			new HtmlAttribute (HtmlTextWriterAttribute.Onclick,           "onclick"),
			new HtmlAttribute (HtmlTextWriterAttribute.ReadOnly,          "readonly"),
			new HtmlAttribute (HtmlTextWriterAttribute.Rows,              "rows"),
			new HtmlAttribute (HtmlTextWriterAttribute.Rowspan,           "rowspan"),
			new HtmlAttribute (HtmlTextWriterAttribute.Rules,             "rules"),
			new HtmlAttribute (HtmlTextWriterAttribute.Selected,          "selected"),
			new HtmlAttribute (HtmlTextWriterAttribute.Size,              "size"),
			new HtmlAttribute (HtmlTextWriterAttribute.Src,               "src"),
			new HtmlAttribute (HtmlTextWriterAttribute.Style,             "style"),
			new HtmlAttribute (HtmlTextWriterAttribute.Tabindex,          "tabindex"),
			new HtmlAttribute (HtmlTextWriterAttribute.Target,            "target"),
			new HtmlAttribute (HtmlTextWriterAttribute.Title,             "title"),
			new HtmlAttribute (HtmlTextWriterAttribute.Type,              "type"),
			new HtmlAttribute (HtmlTextWriterAttribute.Valign,            "valign"),
			new HtmlAttribute (HtmlTextWriterAttribute.Value,             "value"),
			new HtmlAttribute (HtmlTextWriterAttribute.Width,             "width"),
			new HtmlAttribute (HtmlTextWriterAttribute.Wrap,              "wrap"),
			new HtmlAttribute (HtmlTextWriterAttribute.Abbr,              "abbr"),
			new HtmlAttribute (HtmlTextWriterAttribute.AutoComplete,      "autocomplete"),
			new HtmlAttribute (HtmlTextWriterAttribute.Axis,              "axis"),
			new HtmlAttribute (HtmlTextWriterAttribute.Content,           "content"),
			new HtmlAttribute (HtmlTextWriterAttribute.Coords,            "coords"),
			new HtmlAttribute (HtmlTextWriterAttribute.DesignerRegion,    "_designerregion"),
			new HtmlAttribute (HtmlTextWriterAttribute.Dir,               "dir"),
			new HtmlAttribute (HtmlTextWriterAttribute.Headers,           "headers"),
			new HtmlAttribute (HtmlTextWriterAttribute.Longdesc,          "longdesc"),
			new HtmlAttribute (HtmlTextWriterAttribute.Rel,               "rel"),
			new HtmlAttribute (HtmlTextWriterAttribute.Scope,             "scope"),
			new HtmlAttribute (HtmlTextWriterAttribute.Shape,             "shape"),
			new HtmlAttribute (HtmlTextWriterAttribute.Usemap,            "usemap"),
			new HtmlAttribute (HtmlTextWriterAttribute.VCardName,         "vcard_name"),
		};

		static HtmlStyle [] htmlstyles = {
			new HtmlStyle (HtmlTextWriterStyle.BackgroundColor,    "background-color"),
			new HtmlStyle (HtmlTextWriterStyle.BackgroundImage,    "background-image"),
			new HtmlStyle (HtmlTextWriterStyle.BorderCollapse,     "border-collapse"),
			new HtmlStyle (HtmlTextWriterStyle.BorderColor,        "border-color"),
			new HtmlStyle (HtmlTextWriterStyle.BorderStyle,        "border-style"),
			new HtmlStyle (HtmlTextWriterStyle.BorderWidth,        "border-width"),
			new HtmlStyle (HtmlTextWriterStyle.Color,              "color"),
			new HtmlStyle (HtmlTextWriterStyle.FontFamily,         "font-family"),
			new HtmlStyle (HtmlTextWriterStyle.FontSize,           "font-size"),
			new HtmlStyle (HtmlTextWriterStyle.FontStyle,          "font-style"),
			new HtmlStyle (HtmlTextWriterStyle.FontWeight,         "font-weight"),
			new HtmlStyle (HtmlTextWriterStyle.Height,             "height"),
			new HtmlStyle (HtmlTextWriterStyle.TextDecoration,     "text-decoration"),
			new HtmlStyle (HtmlTextWriterStyle.Width,              "width"),
			new HtmlStyle (HtmlTextWriterStyle.ListStyleImage,     "list-style-image"),
			new HtmlStyle (HtmlTextWriterStyle.ListStyleType,      "list-style-type"),
			new HtmlStyle (HtmlTextWriterStyle.Cursor,             "cursor"),
			new HtmlStyle (HtmlTextWriterStyle.Direction,          "direction"),
			new HtmlStyle (HtmlTextWriterStyle.Display,            "display"),
			new HtmlStyle (HtmlTextWriterStyle.Filter,             "filter"),
			new HtmlStyle (HtmlTextWriterStyle.FontVariant,        "font-variant"),
			new HtmlStyle (HtmlTextWriterStyle.Left,               "left"),
			new HtmlStyle (HtmlTextWriterStyle.Margin,             "margin"),
			new HtmlStyle (HtmlTextWriterStyle.MarginBottom,       "margin-bottom"),
			new HtmlStyle (HtmlTextWriterStyle.MarginLeft,         "margin-left"),
			new HtmlStyle (HtmlTextWriterStyle.MarginRight,        "margin-right"),
			new HtmlStyle (HtmlTextWriterStyle.MarginTop,          "margin-top"),
			new HtmlStyle (HtmlTextWriterStyle.Overflow,           "overflow"),
			new HtmlStyle (HtmlTextWriterStyle.OverflowX,          "overflow-x"),
			new HtmlStyle (HtmlTextWriterStyle.OverflowY,          "overflow-y"),
			new HtmlStyle (HtmlTextWriterStyle.Padding,            "padding"),
			new HtmlStyle (HtmlTextWriterStyle.PaddingBottom,      "padding-bottom"),
			new HtmlStyle (HtmlTextWriterStyle.PaddingLeft,        "padding-left"),
			new HtmlStyle (HtmlTextWriterStyle.PaddingRight,       "padding-right"),
			new HtmlStyle (HtmlTextWriterStyle.PaddingTop,         "padding-top"),
			new HtmlStyle (HtmlTextWriterStyle.Position,           "position"),
			new HtmlStyle (HtmlTextWriterStyle.TextAlign,          "text-align"),
			new HtmlStyle (HtmlTextWriterStyle.VerticalAlign,      "vertical-align"),
			new HtmlStyle (HtmlTextWriterStyle.TextOverflow,       "text-overflow"),
			new HtmlStyle (HtmlTextWriterStyle.Top,                "top"),
			new HtmlStyle (HtmlTextWriterStyle.Visibility,         "visibility"),
			new HtmlStyle (HtmlTextWriterStyle.WhiteSpace,         "white-space"),
			new HtmlStyle (HtmlTextWriterStyle.ZIndex,             "z-index"),
		};
        /// <summary>
        /// Checks if an attribute is a valid form attribute
        /// </summary>
        /// <param name="attribute">Attribute name</param>
        /// <returns></returns>
		public virtual bool IsValidFormAttribute (string attribute)
		{
			return true;
		}

		/// <summary>
		/// Writes "&lt;br/&gt;" tag
		/// </summary>
		public virtual void WriteBreak ()
		{
			string br = GetTagName (HtmlTextWriterTag.Br);
			WriteBeginTag (br);
			Write (SelfClosingTagEnd);
		}
        /// <summary>
        /// Write an string value encoded
        /// </summary>
        /// <param name="text">string value</param>
		public virtual void WriteEncodedText (string text)
		{
			Write (HttpUtility.HtmlEncode (text));
		}
		/// <summary>
		/// Initialize render
		/// </summary>
		public virtual void BeginRender ()
		{
		}
        /// <summary>
        /// Finish render
        /// </summary>
		public virtual void EndRender ()
		{
		}

        public override string ToString()
        {
            return b.ToString();
        }
    }
}
