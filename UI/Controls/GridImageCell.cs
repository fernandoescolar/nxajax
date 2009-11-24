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

namespace nxAjax.UI.Controls
{
    /// <summary>
    /// Image GridCell Type
    /// </summary>
    public class GridImageCell : GridCell
    {
        public override object Value
        {
            set { ((ImageButton)mControl).Src = value.ToString(); }
            get { return ((ImageButton)mControl).Src; }
        }
        /// <summary>
        /// Gets/Sets image "alt" property
        /// </summary>
        public string Alt
        {
            set { ((ImageButton)mControl).Alt = value; }
            get { return ((ImageButton)mControl).Alt; }
        }

        /// <summary>
        /// Raises on image is clicked
        /// <remarks>
        /// It is a server event
        /// </remarks>
        /// </summary>
        public event nxEventHandler ServerClick
        {
            add { ((ImageButton)mControl).ServerClick += value; }
            remove { ((ImageButton)mControl).ServerClick -= value; }
        }

        internal GridImageCell(GridColumnStyle parentColumn) : base(parentColumn)
        {
            ImageButton item = new ImageButton();
            this.mControl = item;
            prepareControl();
        }
        internal GridImageCell(GridColumnStyle parentColumn, object value) : this(parentColumn)
        {
            this.Value = value;
        }
        public override int Compare(GridCell gc)
        {
            return this.Value.ToString().CompareTo(gc.Value.ToString());
        }
    }
}
