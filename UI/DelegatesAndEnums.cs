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

namespace Framework.Ajax.UI
{
    /// <summary>
    /// Post Back Modes
    /// </summary>
    public enum PostBackMode
    {
        /// <summary>
        /// Syncronous
        /// </summary>
        Sync,
        /// <summary>
        /// Asyncronous
        /// </summary>
        Async
    }
    /// <summary>
    /// Display Types
    /// </summary>
    public enum DisplayType
    {
        /// <summary>
        /// Not Set
        /// </summary>
        NotSet,
        /// <summary>
        /// No display
        /// </summary>
        None,
        /// <summary>
        /// In Block
        /// </summary>
        Block,
        /// <summary>
        /// In line
        /// </summary>
        Inline
    }

    /// <summary>
    /// Standar delegate event handler
    /// </summary>
    /// <param name="sender">AjaxControl sender</param>
    /// <param name="value">value param</param>
    public delegate void AjaxEventHandler(AjaxControl sender, string value);
    /// <summary>
    /// Standar GridView delegate event handler
    /// </summary>
    /// <param name="sender">AjaxControl sender</param>
    /// <param name="column">current column</param>
    /// <param name="row">current row</param>
    /// <param name="value">value param</param>
    public delegate void AjaxGridEventHandler(AjaxControl sender, int column, int row, string value);
}
