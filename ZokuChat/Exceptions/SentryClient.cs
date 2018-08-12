using System;
using Microsoft.Extensions.Options;
using SharpRaven;
using SharpRaven.Data;
using ZokuChat.Extensions;

namespace ZokuChat.Exceptions
{
	public class SentryClient : ISentryClient
	{
		private readonly RavenClient _client;

		public SentryClient(IOptions<ExceptionReporterOptions> optionsAccessor)
		{
			ExceptionReporterOptions options = optionsAccessor.Value;

			if (options.SentryProjectId > 0 && !options.SentryKey.IsNullOrWhitespace())
			{
				
				_client = new RavenClient($"https://{options.SentryKey}@sentry.io/{options.SentryProjectId}");
				IsConfigured = true;
			}
		}

		public bool IsConfigured { get; set; }

		public void Capture(Exception e)
		{
			_client.Capture(new SentryEvent(e));
		}

		public void CaptureAsync(Exception e)
		{
			_client.CaptureAsync(new SentryEvent(e));
		}
	}
}
