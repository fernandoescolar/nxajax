using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Framework.Ajax.UI
{
    public interface IAjaxControllerContainer
    {
        IAjaxController AjaxController { get; }
    }
}
