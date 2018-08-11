using System;

namespace ZokuChat.Services
{
    public interface IExceptionService
    {
		void ReportException(Exception e);
    }
}
