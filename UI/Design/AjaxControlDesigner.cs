using System;
using System.Collections.Generic;
using System.Text;

namespace Framework.Ajax.UI.Design
{
    /// <summary>
    /// Provides a moderate level of fidelity for the AjaxControls in the VS.net IDE.
    /// </summary>
    public class AjaxControlDesigner : System.Web.UI.Design.ControlDesigner
    {
        /// <summary>
        /// Provides easy access the properties set in the IDE.
        /// </summary>
        protected AjaxControl myControl;

        /// <summary>
        /// Initializes the designer
        /// </summary>
        /// <param name="component"></param>
        public override void Initialize(System.ComponentModel.IComponent component)
        {
            // Make sure that this designer is attached to a AjaxControls
            if (component is AjaxControl)
            {
                base.Initialize(component);
                myControl = (AjaxControl)component;
            }
        }
        /// <summary>
        /// Writes the HTML used be VS.net to display the control at design-time.
        /// </summary>
        /// <returns></returns>
        public override string GetDesignTimeHtml()
        {
            try
            {
                AjaxTextWriter writer = new AjaxTextWriter();
                myControl.RenderHTML(writer);
                return writer.ToString();
            }
            catch(Exception ex)
            {
                // Display the error in VS.net, in Design view
                return String.Concat("<h3>Error</h3>Stack Trace:<br>", ex.StackTrace);
            }
        }

    }
}
