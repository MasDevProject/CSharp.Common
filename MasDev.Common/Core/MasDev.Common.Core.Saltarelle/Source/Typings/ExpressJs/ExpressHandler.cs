using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace NodeExpress
{
    public delegate void ExpressHandler(ExpressServerRequest request, ExpressServerResponse response);

    public delegate void ExpressChainedHandler(ExpressServerRequest request, ExpressServerResponse response, ExpressChain next);
}
