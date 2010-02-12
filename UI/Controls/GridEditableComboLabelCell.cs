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
    /// EditableComboLabel GridCell Type
    /// </summary>
    public class GridEditableComboLabelCell : GridComboboxCell, IGridEditableCell
    {
        /// <summary>
        /// Raises on control enters in Edit Mode
        /// <remarks>
        /// It is a server event
        /// </remarks>
        /// </summary>
        public event AjaxEventHandler ServerEnterEditMode
        {
            add { ((InternalComboBox)mControl).ServerEnterEditMode += value; }
            remove { ((InternalComboBox)mControl).ServerEnterEditMode -= value; }
        }
        /// <summary>
        /// Raises on control exit from Edit Mode
        /// <remarks>
        /// It is a server event
        /// </remarks>
        /// </summary>
        public event AjaxEventHandler ServerExitEditMode
        {
            add { ((InternalComboBox)mControl).ServerExitEditMode += value; }
            remove { ((InternalComboBox)mControl).ServerExitEditMode -= value; }
        }


        internal GridEditableComboLabelCell(GridColumnStyle parentColumn) : base(parentColumn, new InternalComboBox())
        {

        }
        /// <summary>
        /// Gets if control is in edit mode
        /// </summary>
        public bool IsInEditMode
        {
            get { return ((InternalComboBox)mControl).EditModeOn; }
        }

        /// <summary>
        /// Forces control enter in edit mode
        /// </summary>
        public void EnterOnEditMode()
        {
            ((InternalComboBox)mControl).EnterOnEditMode();
        }

        /// <summary>
        /// Forces control exit from edit mode
        /// </summary>
        public void ExitFromEditMode()
        {
            ((InternalComboBox)mControl).ExitFromEditMode();
        }

        internal GridEditableComboLabelCell(GridColumnStyle parentColumn, object value) : this(parentColumn)
        {
            this.Value = value;
        }
        /// <summary>
        /// Internal new EditableComboLabel
        /// Quite different than original...
        /// </summary>
        internal class InternalComboBox : ComboBox, IEditable
        {
            protected Label mLabel;
            protected bool isInEditMode;

            public bool EditModeOn { get { return isInEditMode; } }

            #region Public Server Events
            public event AjaxEventHandler ServerEnterEditMode;
            public event AjaxEventHandler ServerExitEditMode;
            #endregion

            internal InternalComboBox() : base()
            {
                mLabel = new Label();
                isInEditMode = false;
            }

            protected void prepareLabel()
            {
                mLabel.Page = this.Page;
                mLabel.ID = this.ID + "_view";
                mLabel.Text = this.SelectedText;
                mLabel.CssClass = CssClass;
                mLabel.Disabled = Disabled;
                mLabel.Visible = Visible;
                mLabel.Display = Display;
                mLabel.Style.Add("cursor", "text");
            }

            #region Renders
            public override void RenderHTML(AjaxTextWriter writer)
            {
                if (!isInEditMode)
                {
                    prepareLabel();
                    mLabel.RenderHTML(writer);
                }
                else
                {
                    base.RenderHTML(writer);
                }
            }
            public override void RenderJS(AjaxTextWriter writer)
            {
                return;
            }
            #endregion

            public void EnterOnEditMode()
            {
                isInEditMode = true;
                if (ServerEnterEditMode != null)
                    ServerEnterEditMode(this, SelectedValue);
            }

            public void ExitFromEditMode()
            {
                isInEditMode = false;
                if (ServerExitEditMode != null)
                    ServerExitEditMode(this, SelectedValue);
            }

            protected override void AjaxLoadViewState(object savedState)
            {
                object[] state = (object[])(savedState);
                base.AjaxLoadViewState(state[0]);
                isInEditMode = (bool)state[1];
                mLabel.ProtectedLoadViewState(state[2]);
            }
            protected override object AjaxSaveViewState()
            {
                object[] state = new object[3];
                state[0] = base.AjaxSaveViewState();
                state[1] = isInEditMode;
                state[2] = mLabel.ProtectedSaveViewState();
                return state;
            }
        }
    }
}
