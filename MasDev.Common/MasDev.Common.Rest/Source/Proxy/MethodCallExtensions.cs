using System;
using System.Runtime.Remoting.Messaging;
using System.Reflection;


namespace MasDev.Common.Rest.Proxy
{
	public static class MethodCallExtensions
	{
		public static IMessage UnProxedCall (this IMethodCallMessage methodCall, object instance)
		{
			var method = (MethodInfo)methodCall.MethodBase;
			try
			{
				var result = method.Invoke (instance, methodCall.InArgs);
				return new ReturnMessage (result, null, 0, methodCall.LogicalCallContext, methodCall);
			} catch (Exception e)
			{
				if (e is TargetInvocationException && e.InnerException != null)
				{
					return new ReturnMessage (e.InnerException, methodCall);
				}
				return new ReturnMessage (e, methodCall);
			}
		}
	}
}

