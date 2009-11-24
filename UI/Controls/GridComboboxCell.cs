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
    public class GridComboboxCell : GridCell
    {
        /// <summary>
        /// Raises on Control selected item change
        /// <remarks>Is a server event</remarks>
        /// </summary>
        public event nxEventHandler ServerChange
        {
            add { ((ComboBox)mControl).ServerChange += value; }
            remove { ((ComboBox)mControl).ServerChange -= value; }
        }

        public override object Value
        {
            set { ((ComboBox)mControl).SelectedValue = value.ToString(); }
            get { return ((ComboBox)mControl).SelectedValue; }
        }
        /// <summary>
        /// ComboBox Item Collection
        /// </summary>
        public ComboBoxItemCollection Items { get { return ((ComboBox)mControl).Items; } }

        /// <summary>
        /// Creates a new GridComboboxCell
        /// </summary>
        /// <param name="parentColumn">Parent GridColumnStyle</param>
        public GridComboboxCell(GridColumnStyle parentColumn) : base(parentColumn)
        {
            ComboBox item = new ComboBox();
            this.mControl = item;
            prepareControl();
        }
        /// <summary>
        /// Creates a new GridComboboxCell
        /// </summary>
        /// <param name="parentColumn">Parent GridColumnStyle</param>
        /// <param name="control">Inner ComboBox control</param>
        internal GridComboboxCell(GridColumnStyle parentColumn, ComboBox control) : base(parentColumn)
        {
            this.mControl = control;
            prepareControl();
        }
        /// <summary>
        /// Creates a new GridComboboxCell
        /// </summary>
        /// <param name="parentColumn">Parent GridColumnStyle</param>
        /// <param name="value">Initial value</param>
        public GridComboboxCell(GridColumnStyle parentColumn, object value) : this(parentColumn)
        {
            this.Value = value;
        }

        public override int Compare(GridCell gc)
        {
            return this.Value.ToString().CompareTo(gc.Value.ToString());
        }
    }
}
