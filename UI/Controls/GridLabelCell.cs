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
    /// GridLabelCell Types
    /// </summary>
    public enum GridLabelCellType
    {
        /// <summary>
        /// Text Value
        /// </summary>
        Text,
        /// <summary>
        /// Int Value
        /// </summary>
        Int,
        /// <summary>
        /// Double Value
        /// </summary>
        Double,
        /// <summary>
        /// DateTime Value
        /// </summary>
        DateTime,
        /// <summary>
        /// Literal HTML code
        /// </summary>
        Html
    }
    /// <summary>
    /// Label GridCell Type
    /// </summary>
    public class GridLabelCell : GridCell
    {
        protected string mFormat;
        protected GridLabelCellType mGridLabelCellType;

        /// <summary>
        /// Gets/Sets GridLabelCell Type
        /// </summary>
        public GridLabelCellType GridLabelCellType
        {
            get { return mGridLabelCellType; }
            set { mGridLabelCellType = value; }
        }
        public override object Value
        {
            set 
            {
                try
                {
                    switch (mGridLabelCellType)
                    {
                        case GridLabelCellType.Int:
                            int i1 = int.Parse(value.ToString());
                            if (mFormat != string.Empty)
                                ((Label)mControl).Text = i1.ToString(mFormat);
                            else
                                ((Label)mControl).Text = i1.ToString();
                            break;
                        case GridLabelCellType.Double:
                            double d1 = double.Parse(value.ToString());
                            if (mFormat != string.Empty)
                                ((Label)mControl).Text = d1.ToString(mFormat);
                            else
                                ((Label)mControl).Text = d1.ToString();

                            break;
                        case GridLabelCellType.DateTime:
                            DateTime dt1 = DateTime.Parse(value.ToString());
                            if (mFormat != string.Empty)
                                ((Label)mControl).Text = dt1.ToString(mFormat);
                            else
                                ((Label)mControl).Text = dt1.ToString();
                            break;
                        case GridLabelCellType.Html:
                            ((Label)mControl).InnerHtml = value.ToString();
                            break;
                        default:
                            ((Label)mControl).Text = value.ToString();
                            break;
                    }
                }
                catch (FormatException fex)
                {
                    System.Diagnostics.Debug.WriteLine(fex.Message);
                    if (value == null)
                        ((Label)mControl).Text = "";
                    else
                        ((Label)mControl).Text = value.ToString();
                }
            }
            get {
                try
                {
                    switch (mGridLabelCellType)
                    {
                        case GridLabelCellType.Int:
                            return int.Parse(((Label)mControl).Text);
                        case GridLabelCellType.Double:
                            return double.Parse(((Label)mControl).Text);
                        case GridLabelCellType.DateTime:
                            return DateTime.Parse(((Label)mControl).Text);
                        case GridLabelCellType.Html:
                            return ((Label)mControl).InnerHtml;
                        default:
                            return ((Label)mControl).Text;
                    }
                }
                catch (FormatException fex)
                {
                    System.Diagnostics.Debug.WriteLine(fex.Message);
                    return ((Label)mControl).Text;
                }
            }
        }
        /// <summary>
        /// Gets/Sets string format
        /// </summary>
        public string Format { get { return mFormat; } set { mFormat = value; } }

        internal GridLabelCell(GridColumnStyle parentColumn) : base(parentColumn)
        {
            this.mFormat = string.Empty;
            mGridLabelCellType = GridLabelCellType.Text;
            this.mControl = new Label();
            prepareControl();
        }
        internal GridLabelCell(GridColumnStyle parentColumn, object value) : this(parentColumn)
        {
            this.Value = value;
        }
        internal GridLabelCell(GridColumnStyle parentColumn, GridLabelCellType type) : base(parentColumn)
        {
            this.mFormat = string.Empty;
            mGridLabelCellType = type;
            this.mControl = new Label();
            prepareControl();
        }
        internal GridLabelCell(GridColumnStyle parentColumn, GridLabelCellType type, string format) : base(parentColumn)
        {
            this.mFormat = format;
            mGridLabelCellType = type;
            this.mControl = new Label();
            prepareControl();
        }
        public override int Compare(GridCell gc)
        {
            switch (mGridLabelCellType)
            { 
                case GridLabelCellType.Int:
                    int i1 = int.Parse(Value.ToString());
                    int i2 = int.Parse(gc.Value.ToString());
                    return i1.CompareTo(i2);
                case GridLabelCellType.Double:
                    double d1 = double.Parse(Value.ToString());
                    double d2 = double.Parse(gc.Value.ToString());
                    return d1.CompareTo(d2);
                case GridLabelCellType.DateTime:
                    DateTime dt1 = DateTime.Parse(Value.ToString());
                    DateTime dt2 = DateTime.Parse(gc.Value.ToString());
                    return dt1.CompareTo(dt2);
                default:
                    return this.Value.ToString().CompareTo(gc.Value.ToString());
            }
        }
    }
}
