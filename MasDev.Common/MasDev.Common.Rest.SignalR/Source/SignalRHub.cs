using MasDev.Common.Rest.Proxy;
using System.Threading.Tasks;


namespace MasDev.Common.Rest.SignalR
{
	public class SignalRHub<TPushManager> : HubPushHttpContext where TPushManager : PushManager, new()
	{
		public TPushManager Manager { get; private set; }



		public SignalRHub ()
		{
			Manager = RestModuleProxy<TPushManager>.Create (new TPushManager ());
			Manager.HttpContext = this;
		}



		public override async Task OnConnected ()
		{
			await Manager.OnConnected ();
			await base.OnConnected ();
		}



		public override async Task OnDisconnected (bool stopCalled)
		{
			await Manager.OnDisconnected (stopCalled);
			await base.OnDisconnected (stopCalled);
		}



		public override async Task OnReconnected ()
		{
			await Manager.OnReconnected ();
			await base.OnReconnected ();
		}



		protected override void Dispose (bool disposing)
		{
			Manager.Dispose ();
			base.Dispose (disposing);
		}
	}
}

