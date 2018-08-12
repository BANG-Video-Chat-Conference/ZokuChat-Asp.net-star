using System;
using ZokuChat.Exceptions;
namespace ZokuChat.Services
{
	public class ExceptionService : IExceptionService
	{
		private readonly ISentryClient _sentryClient;

		public ExceptionService(ISentryClient sentryClient)
		{
			_sentryClient = sentryClient;
		}

		public void ReportException(Exception e)
		{
			if (_sentryClient.IsConfigured)
			{
				_sentryClient.CaptureAsync(e);
			}
		}
	}
}
