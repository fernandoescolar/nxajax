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
    /// EditableLabel GridCell Type
    /// </summary>
    public class GridEditableLabelCell : GridTextboxCell, IGridEditableCell
    {
        /// <summary>
        /// Raises on control enters in Edit Mode
        /// <remarks>
        /// It is a server event
        /// </remarks>
        /// </summary>
        public event AjaxEventHandler ServerEnterEditMode
        {
            add { ((InternalTextBox)mControl).ServerEnterEditMode += value; }
            remove { ((InternalTextBox)mControl).ServerEnterEditMode -= value; }
        }
        /// <summary>
        /// Raises on control exit from Edit Mode
        /// <remarks>
        /// It is a server event
        /// </remarks>
        /// </summary>
        public event AjaxEventHandler ServerExitEditMode
        {
            add { ((InternalTextBox)mControl).ServerExitEditMode += value; }
            remove { ((InternalTextBox)mControl).ServerExitEditMode -= value; }
        }

        public override object Value
        {
            set { ((InternalTextBox)mControl).Value = value.ToString(); }
            get { return ((InternalTextBox)mControl).Value; }
        }
        /// <summary>
        /// Gets if control is in edit mode
        /// </summary>
        public bool IsInEditMode
        {
            get { return ((InternalTextBox)mControl).EditModeOn; }
        }
        public void EnterOnEditMode()
        {
            ((InternalTextBox)mControl).EnterOnEditMode();
        }

        public void ExitFromEditMode()
        {
            ((InternalTextBox)mControl).ExitFromEditMode();
        }


        internal GridEditableLabelCell(GridColumnStyle parentColumn):  base(parentColumn, new InternalTextBox())
        {
         
        }
        internal GridEditableLabelCell(GridColumnStyle parentColumn, object value) : this(parentColumn)
        {
            this.Value = value;
        }
        /// <summary>
        /// Internal New EditableLabel.
        /// Quite different than original...
        /// </summary>
        internal class InternalTextBox : TextBox, IEditable
        { 
         protected Label mLabel;
            protected bool isInEditMode;

            public bool EditModeOn { get { return isInEditMode; } }

            #region Public Server Events
            public event AjaxEventHandler ServerEnterEditMode;
            public event AjaxEventHandler ServerExitEditMode;
            #endregion

            internal InternalTextBox() : base()
            {
                mLabel = new Label();
                isInEditMode = false;
            }

            protected void prepareLabel()
            {
                mLabel.Page = this.Page;
                mLabel.ID = this.ID + "_view";
                mLabel.Text = this.Value;
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
                if (isInEditMode)
                {
                    base.renderInitJavaScript(writer);
                }
            }
            #endregion

            public void EnterOnEditMode()
            {
                isInEditMode = true;
                if (ServerEnterEditMode != null)
                    ServerEnterEditMode(this, Value);
            }

            public void ExitFromEditMode()
            {
                isInEditMode = false;
                if (ServerExitEditMode != null)
                    ServerExitEditMode(this, Value);
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
