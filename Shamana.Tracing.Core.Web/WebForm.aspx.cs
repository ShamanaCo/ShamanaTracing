using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Shamana.Tracing.Core.Web.Models;

namespace Shamana.Tracing.Core.Web
{
    public partial class WebForm : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var webModel = TransactionProxy<IWebModel>.Create(this.Context, new WebModel());
            webModel.PublicArgs(string.Empty, 0);
            webModel.PublicMethod();
            webModel.PublicProperty = string.Empty;
            string test;
            webModel.PublicArgs(out test);
        }
    }
}