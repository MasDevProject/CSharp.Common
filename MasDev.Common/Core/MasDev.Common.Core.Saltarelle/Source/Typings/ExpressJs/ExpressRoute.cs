using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace NodeExpress
{
    [Imported]
    [IgnoreNamespace]
    [ModuleName("express")]
    public class ExpressRoute
    {

        private ExpressRoute()
        {
        }

       /* [IntrinsicProperty]
        public object[] Callbacks
        {
            get
            {
                return null;
            }
        }

        [IntrinsicProperty]
        public object[] Keys
        {
            get
            {
                return null;
            }
        }*/

        [IntrinsicProperty]
        public string Method
        {
            get
            {
                return null;
            }
        }

        [IntrinsicProperty]
        public string Path
        {
            get
            {
                return null;
            }
        }

        [IntrinsicProperty]
        public string RegExp
        {
            get
            {
                return null;
            }
        }
    }
}
