using System.Runtime.Remoting.Proxies;
using System.Runtime.Remoting.Messaging;
using System.Reflection;
using System;
using MasDev.Rest.Auth;
using System.Linq;
using System.Collections.Generic;
using MasDev.IO.Http;
using System.Threading.Tasks;


namespace MasDev.Rest.Proxy
{
	public class RestModuleProxy<T> : RealProxy where T : class, IRestModule, new()
	{
		readonly T _instance;

		readonly string[] _propertiesNames;



		RestModuleProxy (T instance) : base (typeof(T))
		{
			_instance = instance;

			var l = new HashSet<string> ();
			foreach (var p in typeof(T).GetRuntimeProperties ()) {
				if (p.SetMethod != null)
					l.Add (p.SetMethod.Name);
				if (p.GetMethod != null)
					l.Add (p.GetMethod.Name);
			}
			_propertiesNames = l.ToArray ();
		}



		public static T Create (T instance)
		{
			return (T)new RestModuleProxy<T> (instance).GetTransparentProxy ();
		}




		public override IMessage Invoke (IMessage msg)
		{
			var methodCall = msg as IMethodCallMessage;
			if (methodCall == null || IsProperty (methodCall))
				return methodCall.UnProxedCall (_instance);
				
			var method = (MethodInfo)methodCall.MethodBase;
			//var debug = string.Format ("******************************** {0}.{1}: ", method.DeclaringType.Name, method.Name);

			var handleTransactions = method.GetCustomAttribute<HandleTransactionsAttribute> ();
			var authorize = method.GetCustomAttribute<AuthorizeAttribute> ();
			var anonymous = method.GetCustomAttribute<AllowAnonymousAttribute> ();

			var shouldAuthorize = authorize != null || (RestConfiguration.AuthOptions.AuthorizeByDefault && anonymous == null);
			var uow = _instance.Repositories.SharedUnitOfWork;
			if (shouldAuthorize) {
				var authorizedRoles = authorize == null ? null : authorize.Roles;
				try {
					Authorize (authorizedRoles);
				} catch (Exception e) {
					if (uow.IsStarted) {
						uow.Rollback ();
						//Console.WriteLine (debug + "uow rollbacked after auth exception");
					}
					return new ReturnMessage (e, methodCall);
				}
			}

			var result = methodCall.UnProxedCall (_instance);
			if (handleTransactions == null || !uow.IsStarted) {
				//Console.WriteLine (debug + "not marked as [HandleTransactions] or !uow.IsStarted");
				return result;
			}
				
			var methodResult = result as ReturnMessage;

			if (methodResult == null) {
				//Console.WriteLine (debug + " result is not a ReturnMessage");
				return result;
			}
				
			try {
				if (methodResult.Exception == null) {
					var actualResult = methodResult.ReturnValue as Task;

					if (actualResult == null) {
						uow.Commit ();
						//Console.WriteLine (debug + "committed uow (sync)");
					} else {
						actualResult.GetAwaiter ().UnsafeOnCompleted (() => {
							if (actualResult.Exception == null) {
								uow.Commit ();
								//Console.WriteLine (debug + "committed uow (async)");
								return;
							}

							uow.Rollback ();
							//Console.WriteLine (debug + "uow rollbacked after generic exception (async)");
						});
					}

				} else {
					uow.Rollback ();
					//Console.WriteLine (debug + "uow rollbacked after generic exception (sync)");
				}
			} catch (Exception e) {
				return new ReturnMessage (e, msg as IMethodCallMessage);
			}

			return result;
		}



		bool IsProperty (IMethodCallMessage methodCall)
		{
			var name = methodCall.MethodName;
			return _propertiesNames.Any (p => p == name);
		}



		void Authorize (int? roles = null)
		{
			var context = _instance.HttpContext;
			if (!context.RequestHeaders.ContainsKey (AuthorizationHeader.Name))
				throw new UnauthorizedException ("Missing authorization header");

			var headerValues = context.RequestHeaders [AuthorizationHeader.Name].ToList ();
			if (headerValues.Count < 0 || headerValues.Count > 1)
				throw new UnauthorizedException ("Bad authorization header");

			var authManager = _instance.AuthManager;
			var headerValue = headerValues.Single ();
			var headerToken = headerValue.Replace (authManager.TokenType, string.Empty).Trim ();
			var token = authManager.Deserialize (headerToken);
			_instance.CurrentCredentials = token == null ? null : token.Credentials;
			_instance.CredentialsScope = authManager.Authenticate (token, roles, _instance.Repositories.CredentialsRepository);
		}
	}
}

