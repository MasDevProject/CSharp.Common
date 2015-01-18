using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace NodeExpress
{
    public delegate void ExpressParameterHandler(object request, object response, ExpressChain next, string value);
}
