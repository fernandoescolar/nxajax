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

namespace Framework.Ajax.UI.Controls
{
    /// <summary>
    /// Checkbox GridCell Type
    /// </summary>
    public class GridCheckBoxCell : GridCell
    {
        /// <summary>
        /// Raises on control is clicked
        /// <remarks>
        /// It is a server event
        /// </remarks>
        /// </summary>
        public event AjaxEventHandler ServerClick
        {
            add { ((CheckBox)mControl).ServerClick += value; }
            remove { ((CheckBox)mControl).ServerClick -= value; }
        }

        public override object Value
        {
            set { ((CheckBox)mControl).Checked = bool.Parse(value.ToString()); }
            get { return ((CheckBox)mControl).Checked; }
        }
        /// <summary>
        /// Creates a new GridCheckBoxCell
        /// </summary>
        /// <param name="parentColumn">Parent GridColumnStyle</param>
        internal GridCheckBoxCell(GridColumnStyle parentColumn) : base(parentColumn)
        {
            CheckBox item = new CheckBox();
            this.mControl = item;
            prepareControl();
        }
        internal GridCheckBoxCell(GridColumnStyle parentColumn, CheckBox control) : base(parentColumn)
        {
            this.mControl = control;
            prepareControl();
        }
        /// <summary>
        /// Creates a new GridCheckBoxCell
        /// </summary>
        /// <param name="parentColumn">Parent GridColumnStyle</param>
        /// <param name="value">initial control value</param>
        internal GridCheckBoxCell(GridColumnStyle parentColumn, object value) : this(parentColumn)
        {
            this.Value = value;
        }

        public override int Compare(GridCell gc)
        {
            bool b1 = bool.Parse(Value.ToString());
            bool b2 = bool.Parse(gc.Value.ToString());

            return b1.CompareTo(b2);
        }
    }
}
