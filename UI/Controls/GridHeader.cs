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
using System.ComponentModel;

namespace nxAjax.UI.Controls
{
    /// <summary>
    /// Grid Header Control
    /// </summary>
    [Designer("nxAjax.UI.Design.nxControlDesigner")]
    public class GridHeader : nxControl
    {
        protected GridView parentGrid;

        /// <summary>
        /// Creates a new Grid Header control
        /// </summary>
        /// <param name="parentGrid">Parent GridView</param>
        internal GridHeader(GridView parentGrid) : base("div")
        {
            this.parentGrid = parentGrid;
        }

        public override void RenderHTML(nxAjaxTextWriter writer)
        {
            int superW = 0;
            foreach (GridColumnStyle cs in parentGrid.Columns)
                superW += cs.Width;
            
            if (parentGrid.LoadingImg != string.Empty)
                superW += 16;

            string cssClass = this.CssClass;
            this.CssClass = parentGrid.CssClass;
            RenderBeginTag(writer);
            this.CssClass = cssClass;

            writer.WriteBeginTag("table");
            writer.WriteAttribute("ID", parentGrid.ID + "_Header");
            writer.WriteAttribute("cellspacing", "0");
            writer.WriteAttribute("rules", "all");
            writer.WriteAttribute("style", "position: relative; width: " + superW + "px");
            writer.Write(nxAjaxTextWriter.TagRightChar);

            writer.WriteBeginTag("tr");
            if (cssClass != string.Empty)
                writer.WriteAttribute("class", cssClass);
            writer.Write(nxAjaxTextWriter.TagRightChar);

            renderColumnHeaders(writer);

            if (parentGrid.LoadingImg != string.Empty)
            {
                writer.WriteBeginTag("td");
                if (parentGrid.LoadingImgCssClass != string.Empty)
                    writer.WriteAttribute("class", parentGrid.LoadingImgCssClass);
                writer.WriteAttribute("style", "width: 16px;");
                writer.Write(nxAjaxTextWriter.TagRightChar);
                parentGrid.renderLoaingImg(writer);
                writer.WriteEndTag("td");
            }

            writer.WriteEndTag("tr");
            writer.WriteEndTag("table");

            RenderEndTag(writer);
        }
        protected void renderColumnHeaders(nxAjaxTextWriter writer)
        {
            foreach (GridColumnStyle cs in parentGrid.Columns)
                if (cs.Visible)
                    cs.RenderHeaderCell(writer);
        }

        public override void PutPostValue(string obj)
        {
            
        }
    }
}
