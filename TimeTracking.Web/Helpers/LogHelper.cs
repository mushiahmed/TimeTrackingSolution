using Newtonsoft.Json;
using System;
using System.IO;
using System.Text;
using System.Web.Hosting;

namespace TimeTracking.Web.Helpers
{
    public static class LogHelper
    {
        private static readonly object _sync = new object();

        public static void LogError(Exception ex, string className, string methodName, object additionalInfo = null)
        {
            try
            {
                var logsFolder = HostingEnvironment.MapPath("~/Logs") ?? Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Logs");
                if (string.IsNullOrEmpty(logsFolder))
                    return;

                if (!Directory.Exists(logsFolder))
                    Directory.CreateDirectory(logsFolder);

                var filePath = Path.Combine(logsFolder, DateTime.Now.ToString("yyyy-MM-dd") + ".log");
                var timeStamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");

                var sb = new StringBuilder();
                sb.AppendLine("--------------------------------------------------------------------------------");
                sb.AppendLine($"{timeStamp} | {className}.{methodName}");
                sb.AppendLine("Exception: " + ex.Message);
                sb.AppendLine("StackTrace:");
                sb.AppendLine(ex.StackTrace ?? string.Empty);

                if (ex.InnerException != null)
                {
                    sb.AppendLine("InnerException: " + ex.InnerException.Message);
                    sb.AppendLine(ex.InnerException.StackTrace ?? string.Empty);
                }

                if (additionalInfo != null)
                {
                    try
                    {
                        sb.AppendLine("AdditionalInfo: " + JsonConvert.SerializeObject(additionalInfo));
                    }
                    catch
                    {
                        sb.AppendLine("AdditionalInfo: (could not serialize)");
                    }
                }

                sb.AppendLine();

                lock (_sync)
                {
                    File.AppendAllText(filePath, sb.ToString(), Encoding.UTF8);
                }
            }
            catch
            { }
        }


        public static void LogMessage(string message, string className, string methodName, object additionalInfo = null)
        {
            try
            {
                var logsFolder = HostingEnvironment.MapPath("~/Logs") ?? Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Logs");
                if (string.IsNullOrEmpty(logsFolder))
                    return;

                if (!Directory.Exists(logsFolder))
                    Directory.CreateDirectory(logsFolder);

                var filePath = Path.Combine(logsFolder, DateTime.Now.ToString("yyyy-MM-dd") + ".log");
                var timeStamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");

                var sb = new StringBuilder();
                sb.AppendLine("--------------------------------------------------------------------------------");
                sb.AppendLine($"{timeStamp} | {className}.{methodName}");
                sb.AppendLine("Message: " + message);

                if (additionalInfo != null)
                {
                    try
                    {
                        sb.AppendLine("AdditionalInfo: " + JsonConvert.SerializeObject(additionalInfo));
                    }
                    catch
                    {
                        sb.AppendLine("AdditionalInfo: An Error Occurred while serializing additional info");
                    }
                }

                sb.AppendLine();

                lock (_sync)
                {
                    File.AppendAllText(filePath, sb.ToString(), Encoding.UTF8);
                }
            }
            catch
            {

            }
        }
    }
}