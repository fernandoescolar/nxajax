/*
 * Licensing
 *  Mozilla Public License 1.1 (MPL 1.1)
 * 
 * Fernando Escolar Martínez-Berganza <fer.escolar@gmail.com>
 * 
 */
using System;
using System.Web;
using System.Collections.Generic;
using System.Text;
using System.Web.UI;
using System.ComponentModel;
using System.Drawing;
using System.Security.Permissions;

namespace nxAjax.UI.Controls
{
    /// <summary>
    /// Pager control (used by GridView)
    /// <code>
    /// &lt;ajax:Pager runat="server"&gt;&lt;/ajax:Pager&gt;
    /// </code>
    /// </summary>
    [AspNetHostingPermission(SecurityAction.Demand, Level = AspNetHostingPermissionLevel.Minimal)]
    [AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    [Designer("nxAjax.UI.Design.nxControlDesigner")]
    [DefaultEventAttribute("ServerChange")]
    [ToolboxData("<{0}:Pager runat=\"server\"></{0}:Pager>")]
    [ToolboxItem(true)]
    [ToolboxBitmap(typeof(System.Web.UI.WebControls.Panel))]
    public class Pager : Menu
    {
        protected int initIndex, finishIndex;

        /// <summary>
        /// Creates a new pagination info
        /// </summary>
        /// <param name="init">Firts page number</param>
        /// <param name="finish">End page number</param>
        public void SetRange(int init, int finish)
        {
            this.initIndex = init;
            this.finishIndex = finish;

            Items.Clear();
            for(int i=init; i<=finish; i++)
            {
                Items.Add(new MenuItem(i.ToString() , i.ToString()));
            }
        }
        /// <summary>
        /// Creates a new Pager control
        /// </summary>
        public Pager() : base()
        {
            initIndex = finishIndex = 0;
        }

        protected override void LoadViewState(object savedState)
        {
            object[] state = (object[])(savedState);
            base.LoadViewState(state[0]);
            initIndex = (int)state[1];
            finishIndex = (int)state[2];
        }
        protected override object SaveViewState()
        {
            object[] state = new object[3];
            state[0] = base.SaveViewState();
            state[1] = initIndex;
            state[2] = finishIndex;
            return state;
        }

    }
}
