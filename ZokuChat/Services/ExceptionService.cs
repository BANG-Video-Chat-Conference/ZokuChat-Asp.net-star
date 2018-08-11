using System;
using Microsoft.Extensions.Options;
using SharpRaven;
using SharpRaven.Data;
using ZokuChat.Extensions;

namespace ZokuChat.Services
{
	public class ExceptionService : IExceptionService
	{
		private readonly RavenClient _client;
		private readonly bool _isClientConfigured;

		public ExceptionService(IOptions<ExceptionReporterOptions> optionsAccessor)
		{
			ExceptionReporterOptions options = optionsAccessor.Value;

			if (options.SentryProjectId > 0 && !options.SentryKey.IsNullOrWhitespace())
			{
				_client = new RavenClient($"https://{options.SentryKey}@sentry.io/{options.SentryProjectId}");
				_isClientConfigured = true;
			}
		}

		public void ReportException(Exception e)
		{
			if (_isClientConfigured)
			{
				_client.CaptureAsync(new SentryEvent(e));
			}
		}
	}
}
