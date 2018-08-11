using System;

namespace ZokuChat.Services
{
	public class ExceptionService : IExceptionService
	{
		public void ReportException(Exception e)
		{
			// Do nothing for now, later integrate with Sentry or Rollbar
		}
	}
}
