using System.Collections.Generic;


namespace MasDev.Rest.Push
{
	public interface IPushHttpContext : IHttpContext
	{
		IPushCallers Clients { get; }



		string ConnectionId { get; }
	}





	public interface IPushCallers
	{
		dynamic All { get; }



		dynamic Caller { get; }



		dynamic Others { get; }



		dynamic AllExcept (params string[] excludeConnectionIds);



		dynamic Client (string connectionId);



		dynamic Group (string groupName, params string[] excludeConnectionIds);



		dynamic Groups (IList<string> groupNames, params string[] excludeConnectionIds);



		dynamic OthersInGroup (string groupName);



		dynamic OthersInGroups (IList<string> groupNames);
	}





	public interface IPushClients
	{
		dynamic All { get; }



		dynamic AllExcept (params string[] excludeConnectionIds);



		dynamic Client (string connectionId);



		dynamic Group (string groupName, params string[] excludeConnectionIds);



		dynamic Groups (IList<string> groupNames, params string[] excludeConnectionIds);
	}
}

