using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Framework.Ajax.UI
{
    public interface IAjaxController
    {
        AjaxControlCollection AjaxControls { get; }
        AjaxUserControlCollection AjaxUserControls { get; }
        bool IsPostBack { get; }
        string PageUrl { get; }
        Language Lang { get; }
        ViewStateMode ViewStateMode { get; }

        void AddControl(System.Web.UI.Control control);

        void ExecuteJavascript(string lines);
        void DocumentAlert(string msg);
        void DocumentRedirect(string url);

        void WriteAjaxFormBegin(string id, AjaxTextWriter writer);
        void WriteAjaxFormEnd(AjaxTextWriter writer);

        string GetPostbackJavascript();
        string GetPostBackAjaxEvent(AjaxControl ctrl, string _event);
        string GetPostBackWithValueAjaxEvent(AjaxControl ctrl, string _event, string value);

        
    }
}
