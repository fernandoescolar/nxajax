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
    /// Edit Modes
    /// </summary>
    public enum EditMode
    {
        /// <summary>
        /// On Click Event
        /// </summary>
        OnClick,
        /// <summary>
        /// On Custom Event
        /// </summary>
        Custom
    }
    /// <summary>
    /// Exit from edit modes
    /// </summary>
    public enum ExitEditMode
    {
        /// <summary>
        /// On blur event
        /// </summary>
        OnBlur,
        /// <summary>
        /// On blur or return key pressed events
        /// </summary>
        OnBlurOrReturn,
        /// <summary>
        /// On custom event
        /// </summary>
        Custom
    }
    /// <summary>
    /// It is an Editable object
    /// </summary>
    public interface IEditable
    {
        /// <summary>
        /// Raises on enter in edit mode
        /// <remarks>
        /// It is a server event
        /// </remarks>
        /// </summary>
        event AjaxEventHandler ServerEnterEditMode;
        /// <summary>
        /// Raises on exit in edit mode
        /// <remarks>
        /// It is a server event
        /// </remarks>
        /// </summary>
        event AjaxEventHandler ServerExitEditMode;
      
        /// <summary>
        /// Forces the control enter on edit mode
        /// </summary>
        void EnterOnEditMode();
        /// <summary>
        /// Forces the control exit from edit mode
        /// </summary>
        void ExitFromEditMode();
    }
    /// <summary>
    /// It is an editable Grid Cell control
    /// </summary>
    public interface IGridEditableCell : IEditable
    {
        /// <summary>
        /// Gets if control is in edit mode
        /// </summary>
        bool IsInEditMode { get; }
    }
    /// <summary>
    /// It is an Editable control
    /// </summary>
    public interface IEditableControl : IEditable
    {
        /// <summary>
        /// Gets/Sets Edit Mode
        /// </summary>
        EditMode EditMode { get; set; }
        /// <summary>
        /// Gets/Sets Exit Edit Mode
        /// </summary>
        ExitEditMode ExitEditMode { get; set; }
    }
}
