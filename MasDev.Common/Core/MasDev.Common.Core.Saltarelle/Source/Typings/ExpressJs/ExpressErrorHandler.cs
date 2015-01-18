using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace NodeExpress
{

    public delegate void ExpressErrorHandler(Exception error, ExpressServerRequest request, ExpressServerResponse response);


    public delegate void ExpressChainedErrorHandler(Exception error, ExpressServerRequest request, ExpressServerResponse response, ExpressChain next);
}
