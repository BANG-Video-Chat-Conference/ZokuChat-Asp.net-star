using System;

namespace ZokuChat.Exceptions
{
    public interface ISentryClient
    {
		bool IsConfigured { get; set; }

		void Capture(Exception e);

		void CaptureAsync(Exception e);
    }
}
