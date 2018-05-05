using System;

namespace Shamana.Tracing.Core.Web.Models
{
    public class MarshalModel : MarshalByRefObject, IWebModel
    {
        private string PrivateProperty
        {
            get;
            set;
        }

        protected string ProtectedProperty
        {
            get;
            set;
        }

        public string PublicProperty
        {
            get;
            set;
        }

        public void InitMethod()
        {
            this.PublicProperty = string.Empty;
            this.ProtectedMethod();
            this.PrivateMethod();
        }

        public void PublicArgs(string arg0, int arg1)
        {
            this.InitMethod();
        }

        public void PublicArgs(out string arg0)
        {
            this.InitMethod();
            arg0 = string.Empty;
        }

        public string PublicMethod()
        {
            this.InitMethod();
            return string.Empty;
        }

        protected void ProtectedMethod()
        {
            this.ProtectedProperty = string.Empty;
        }

        private void PrivateMethod()
        {
            this.PrivateProperty = string.Empty;
        }
    }
}