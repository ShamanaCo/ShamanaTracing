using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Shamana.Tracing.Core.Web.Models
{
    public interface IWebModel
    {
        string PublicProperty
        {
            get;
            set;
        }

        string PublicMethod();

        void PublicArgs(string arg0, int arg1);

        void PublicArgs(out string arg0);
    }
}