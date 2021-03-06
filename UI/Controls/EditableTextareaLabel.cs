﻿/*
 * Licensing
 *  Mozilla Public License 1.1 (MPL 1.1)
 * 
 * Fernando Escolar Martínez-Berganza <fer.escolar@gmail.com>
 * 
 */
using System;
using System.Web;
using System.Web.UI;
using System.Drawing;
using System.ComponentModel;
using System.Security.Permissions;

namespace Framework.Ajax.UI.Controls
{
    /// <summary>
    /// nxAjax framework Editable Textarea Label control. It is a label, but when you enter in edit mode, it is a Textarea.
    /// <code>
    /// &lt;ajax:EditableTextareaLabel runat="server"&gt;&lt;/ajax:EditableTextareaLabel&gt;
    /// </code>
    /// </summary>
    [AspNetHostingPermission(SecurityAction.Demand, Level = AspNetHostingPermissionLevel.Minimal)]
    [AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    [Designer("Framework.Ajax.UI.Design.AjaxControlDesigner")]
    [ToolboxData("<{0}:EditableTextareaLabel runat=\"server\"></{0}:EditableTextareaLabel>")]
    [ToolboxItem(true)]
    [ToolboxBitmap(typeof(System.Web.UI.WebControls.TextBox))]
    public class EditableTextareaLabel : TextArea, IEditableControl
    {
        #region Private Attributes
        protected Label mLabel;
        private EditMode editMode;
        private ExitEditMode exitEditMode;
        #endregion

        #region Public Properties
        [Category("Appearance"), DefaultValue(EditMode.OnClick), Description("Edit mode.")]
        public EditMode EditMode { get { return editMode; } set { editMode = value; } }
        [Category("Appearance"), DefaultValue(ExitEditMode.OnBlur), Description("Edit mode.")]
        public ExitEditMode ExitEditMode { get { return exitEditMode; } set { exitEditMode = value; } }
        #endregion

        #region Public Server Events
        public event AjaxEventHandler ServerEnterEditMode;
        public event AjaxEventHandler ServerExitEditMode;
        #endregion

        public EditableTextareaLabel() : base() 
        { 
            editMode = EditMode.OnClick;
            exitEditMode = ExitEditMode.OnBlur;
            mLabel = new Label();
            mLabel.KeepState = false;
        }
        protected void prepareLabel()
        {
            mLabel.Page = this.Page;
            mLabel.ID = this.ID + "_view";
            mLabel.InnerHtml = this.Value.Replace("\n", "<br/>");
            mLabel.CssClass = CssClass;
            mLabel.Disabled = Disabled;
            mLabel.Visible = Visible;
            mLabel.Display = Display;
            mLabel.Style.Add("cursor", "text");
        }

        #region Renders
        public override void RenderHTML(AjaxTextWriter writer)
        {
            prepareLabel();
            if (mLabel.InnerHtml == "")
                mLabel.InnerHtml = "<i><font color=\"green\">Edit</font></i>";

            if (editMode == EditMode.OnClick)
            {
                mLabel.Attributes.Add("onclick", "__editable.ParseToEditMode('" + ID + "');");
                if (ServerEnterEditMode != null)
                    mLabel.Attributes["onclick"] += AjaxController.GetPostBackAjaxEvent(this, "onentereditmode");
            }
            mLabel.RenderHTML(writer);

            Style.Add("display", "none");
            base.RenderHTML(writer);
            Style.Remove("display");
        }
        public override void RenderJS(AjaxTextWriter writer)
        {
            prepareLabel();
            base.RenderJS(writer);

            if (!AjaxController.IsPostBack)
            {
                if (exitEditMode != ExitEditMode.Custom)
                {
                    if (ServerEnterEditMode == null)
                        writer.Write("__editable.AddOnBlurExit('" + ID + "');");
                    else
                        writer.Write("__editable.AddOnBlurExit('" + ID + "', '" + AjaxController.GetPostBackAjaxEvent(this, "onexitfrommode").Replace("'", "\\'") + "');");
                }
                if (exitEditMode == ExitEditMode.OnBlurOrReturn)
                    writer.Write("__editable.AddOnReturnExit('" + ID + "');");
                
                mLabel.RenderJS(writer);
            }
            if (hasChanged)
            {
                if (mLabel.InnerHtml == "")
                    mLabel.InnerHtml = "<i><font color=\"green\">Edit</font></i>";
                mLabel.RenderJS(writer);
            }
        }
        #endregion

        public void EnterOnEditMode()
        {
            AjaxController.ExecuteJavascript("__editable.ParseToEditMode('" + ID + "');");
            if (ServerEnterEditMode != null)
                ServerEnterEditMode(this, Value);
        }

        public void ExitFromEditMode()
        {
            AjaxController.ExecuteJavascript("__editable.ExitFromEditMode('" + ID + "');");
            if (ServerExitEditMode != null)
                ServerExitEditMode(this, Value);
        }

        public override void RaiseEvent(string action, string value)
        {
            base.RaiseEvent(action, value);

            if (value.ToLower() == "<i><font color=\"green\">edit</font></i>")
                this.Value = string.Empty;
            else
                this.Value = value;

            switch (action.ToLower())
            {
                case "onentereditmode":
                    if (ServerEnterEditMode != null)
                        ServerEnterEditMode(this, value);
                    break;
                case "onexitfrommode":
                    if (ServerExitEditMode != null)
                        ServerExitEditMode(this, value);
                    break;
            }
        }

        protected override void AjaxLoadViewState(object savedState)
        {
            object[] state = (object[])(savedState);
            base.AjaxLoadViewState(state[0]);
            editMode = (EditMode)state[1];
            mLabel.ProtectedLoadViewState(state[2]);
        }
        protected override object AjaxSaveViewState()
        {
            object[] state = new object[3];
            state[0] = base.AjaxSaveViewState();
            state[1] = editMode;
            state[2] = mLabel.ProtectedSaveViewState();
            return state;
        }
    }
}
