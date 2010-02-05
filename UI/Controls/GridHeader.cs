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

namespace Framework.Ajax.UI.Controls
{
    /// <summary>
    /// Grid Header Control
    /// </summary>
    [Designer("Framework.Ajax.UI.Design.AjaxControlDesigner")]
    public class GridHeader : AjaxControl
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

        public override void RenderHTML(AjaxTextWriter writer)
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
            writer.Write(AjaxTextWriter.TagRightChar);

            writer.WriteBeginTag("tr");
            if (cssClass != string.Empty)
                writer.WriteAttribute("class", cssClass);
            writer.Write(AjaxTextWriter.TagRightChar);

            renderColumnHeaders(writer);

            if (parentGrid.LoadingImg != string.Empty)
            {
                writer.WriteBeginTag("td");
                if (parentGrid.LoadingImgCssClass != string.Empty)
                    writer.WriteAttribute("class", parentGrid.LoadingImgCssClass);
                writer.WriteAttribute("style", "width: 16px;");
                writer.Write(AjaxTextWriter.TagRightChar);
                parentGrid.renderLoaingImg(writer);
                writer.WriteEndTag("td");
            }

            writer.WriteEndTag("tr");
            writer.WriteEndTag("table");

            RenderEndTag(writer);
        }
        protected void renderColumnHeaders(AjaxTextWriter writer)
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
