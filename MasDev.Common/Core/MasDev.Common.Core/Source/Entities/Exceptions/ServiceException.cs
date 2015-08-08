using System;


namespace MasDev.Common
{
	public abstract class ServiceException : Exception
	{
		public ServiceExceptionContent Content { get; set; }

		protected ServiceException (ServiceExceptionContent content)
		{
			Content = content;
		}

		protected ServiceException (object content = null)
		{
			Content = new ServiceExceptionContent (null, content);
		}

		protected ServiceException (int additionalInformation, object content = null)
		{
			Content = new ServiceExceptionContent (additionalInformation, content);
		}
	}

	public sealed class ServiceExceptionContent
	{
		public int? AdditionalInformation { get; private set; }

		public object Content { get; private set; }

		public ServiceExceptionContent (int? additionalInformation, object content)
		{
			AdditionalInformation = additionalInformation;
			Content = content;
		}
	}
}

