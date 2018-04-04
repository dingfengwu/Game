using Game.Base.Domain.Customers;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Game.Base.Logging
{
    public static class LoggingExtension
    {
        public static void Error(this ILogger @this,string message)
        {
            @this.LogError(message);
        }

        public static void Error(this ILogger @this,string message,Exception ex)
        {
            @this.LogError(ex, message);
        }

        public static void Error(this ILogger @this, string message, Exception ex, Customer customer)
        {
            //待处理...
            @this.LogError(ex, message);
        }

        public static void Information(this ILogger @this, string message, Exception ex, Customer customer)
        {
            @this.LogInformation(ex, message);
        }
        public static void Warning(this ILogger @this, string message)
        {
            @this.LogWarning(message);
        }
    }
}
