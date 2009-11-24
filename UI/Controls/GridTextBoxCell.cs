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
    /// TextBox GridCell Type
    /// </summary>
    public class GridTextboxCell : GridCell
    {
        /// <summary>
        /// Raises on value changes
        /// <remarks>
        /// It is a server event
        /// </remarks>
        /// </summary>
        public event nxEventHandler ServerChange
        {
            add { ((TextBox)mControl).ServerChange += value; }
            remove { ((TextBox)mControl).ServerChange -= value; }
        }

        public override object Value
        {
            set { ((TextBox)mControl).Value = value.ToString(); }
            get { return ((TextBox)mControl).Value; }
        }
        /// <summary>
        /// Gets/Sets Maximun length (in characters) of the TextBox
        /// </summary>
        public int MaxLength
        {
            set { ((TextBox)mControl).MaxLength = value; }
            get { return ((TextBox)mControl).MaxLength; }
        }

        /// <summary>
        /// Gets/Sets TextBox Type
        /// </summary>
        public TextboxTypes TextboxType { get { return ((TextBox)mControl).TextboxType; } set { ((TextBox)mControl).TextboxType = value; } }

        internal GridTextboxCell(GridColumnStyle parentColumn) : base(parentColumn)
        {
            TextBox item = new TextBox();
            this.mControl = item;
            prepareControl();
        }
        internal GridTextboxCell(GridColumnStyle parentColumn, TextBox control) : base(parentColumn)
        {
            this.mControl = control;
            prepareControl();
        }
        internal GridTextboxCell(GridColumnStyle parentColumn, object value) : this(parentColumn)
        {
            this.Value = value;
        }

        public override int Compare(GridCell gc)
        {
            switch (TextboxType)
            {
                case TextboxTypes.SIGNED_INTEGER:
                case TextboxTypes.UNSIGNED_INTEGER:
                    int i1 = int.Parse(this.Value.ToString());
                    int i2 = int.Parse(gc.Value.ToString());
                    return i1.CompareTo(i2);
                case TextboxTypes.SIGNED_MONEY:
                case TextboxTypes.UNSIGNED_MONEY:
                    double d1 = double.Parse(this.Value.ToString());
                    double d2 = double.Parse(gc.Value.ToString());
                    return d1.CompareTo(d2);
                default:
                    return this.Value.ToString().CompareTo(gc.Value.ToString());
            }
        }
    }
}
