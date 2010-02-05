/*
 * Licensing
 *  Mozilla Public License 1.1 (MPL 1.1)
 * 
 * Fernando Escolar Martínez-Berganza <fer.escolar@gmail.com>
 * 
 */

namespace Framework.Ajax.UI.Controls
{
    /// <summary>
    /// The object is a AjaxControl container. It has inner child AjaxControls.
    /// </summary>
    public interface IChildAjaxControlContainer
    {
        /// <summary>
        /// Find an inner AjaxControl by ID
        /// </summary>
        /// <param name="id">AjaxControl ID</param>
        /// <returns>AjaxControl founded</returns>
        AjaxControl FindInnerControl(string id);
    }
}
