using System;
using System.Collections.Generic;
using System.Text;

namespace nxAjax.UI.Controls
{
    public class GridEditableComboColumn : GridColumnStyle
    {
        protected ComboBoxItemCollection mItems = new ComboBoxItemCollection();

        public ComboBoxItemCollection Items { get { return mItems; } }

        public event nxEventHandler ServerChange;
        public event nxEventHandler ServerExitEditMode;
        public event nxEventHandler ServerEnterEditMode;

        public override GridCell CreateGridCell()
        {
            GridEditableComboLabelCell item = new GridEditableComboLabelCell(this);

            foreach (ComboBoxItem i in mItems)
                item.Items.Add(i);

            if (ServerChange != null)
                item.ServerChange += new nxEventHandler(item_ServerChange);
            if (ServerEnterEditMode != null)
                item.ServerEnterEditMode += new nxEventHandler(item_ServerEnterEditMode);
            if (ServerExitEditMode != null)
                item.ServerExitEditMode += new nxEventHandler(item_ServerExitEditMode);

            return item;
        }
        public override GridCell CreateGridCell(object value)
        {
            GridEditableComboLabelCell item = (GridEditableComboLabelCell)CreateGridCell();
            item.Value = value;

            return item;
        }

        protected void item_ServerChange(nxControl sender, string value)
        {
            if (ServerChange != null)
                ServerChange(sender, value);
        }
        protected void item_ServerExitEditMode(nxControl sender, string value)
        {
            if (ServerExitEditMode != null)
                ServerExitEditMode(sender, value);
        }

        protected void item_ServerEnterEditMode(nxControl sender, string value)
        {
            if (ServerEnterEditMode != null)
                ServerEnterEditMode(sender, value);
        }
    }
}
