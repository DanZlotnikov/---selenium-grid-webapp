using System;
using static SeleniumAutomationWebapp.HelperFunctions;

namespace SeleniumAutomationWebapp
{
    class GlobalSettings
    {
        public static string successLogFilePath;
        public static string performanceLogFilePath;
        public static string finalizedPerformanceLogFilePath;

        public static void InitLogFiles()
        {
            DateTime dateTime = DateTime.Now;

            successLogFilePath = CreateNewLog("success", "chrome", dateTime);
            performanceLogFilePath = CreateNewLog("performance", "chrome", dateTime);
            finalizedPerformanceLogFilePath = CreateNewLog("finalizedPerformance", "chrome", dateTime);
        }
    }  
}
