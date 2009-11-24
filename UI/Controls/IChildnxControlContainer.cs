/*
 * Licensing
 *  Mozilla Public License 1.1 (MPL 1.1)
 * 
 * Fernando Escolar Martínez-Berganza <fer.escolar@gmail.com>
 * 
 */

namespace nxAjax.UI.Controls
{
    /// <summary>
    /// The object is a nxControl container. It has inner child nxControls.
    /// </summary>
    public interface IChildnxControlContainer
    {
        /// <summary>
        /// Find an inner nxControl by ID
        /// </summary>
        /// <param name="id">nxControl ID</param>
        /// <returns>nxControl founded</returns>
        nxControl FindInnerControl(string id);
    }
}
