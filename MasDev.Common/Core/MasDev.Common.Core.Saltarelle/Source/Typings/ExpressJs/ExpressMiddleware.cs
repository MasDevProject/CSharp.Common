using System;

namespace NodeExpress
{
	public delegate void ExpressMiddleware ();
	public delegate void ExplicitExpressMiddleware (ExpressServerRequest request, ExpressServerResponse response, Action next);
}
