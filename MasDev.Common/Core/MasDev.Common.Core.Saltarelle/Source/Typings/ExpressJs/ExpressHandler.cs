namespace NodeExpress
{
	public delegate void ExpressHandler (ExpressServerRequest request, ExpressServerResponse response);

	public delegate void ExpressChainedHandler (ExpressServerRequest request, ExpressServerResponse response, ExpressChain next);
}
