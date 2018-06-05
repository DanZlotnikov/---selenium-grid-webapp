using OpenQA.Selenium.Remote;
using System;
using System.Collections.Generic;
using static SeleniumAutomationWebapp.HelperFunctions;
using static SeleniumAutomationWebapp.Tests;

namespace SeleniumAutomationWebapp
{
    class TestOnChrome
    {
        public static RemoteWebDriver webappDriver, backofficeDriver;

        /// <summary>
        ///  Sets up testing requirements
        /// </summary>
        public static void SetUp()
        {

            DesiredCapabilities capability = DesiredCapabilities.Chrome();
            webappDriver = new RemoteWebDriver(new Uri("http://localhost:4444/wd/hub"), capability, TimeSpan.FromSeconds(600));
            backofficeDriver = new RemoteWebDriver(new Uri("http://localhost:4444/wd/hub"), capability, TimeSpan.FromSeconds(600));

            GlobalSettings.InitLogFiles();
        }
        /// <summary>
        /// Tears down testing environment
        /// </summary>
        public static void TearDown()
        {
            webappDriver.Quit();
            backofficeDriver.Quit();
            WriteToFinalizedPerformanceLog();
        }

        /// <summary>
        /// Runs test cases
        /// </summary>
        public static void TestSuite()
        {
            WebappSandboxLogin(webappDriver, new Dictionary<string, string>
            {
                {"username", "automation@pepperitest.com"},
                {"password", "123456"}
            });

            Delegator delegatedFunction = WebappSandboxSalesOrder;
            BasicTestWrapper(delegatedFunction, webappDriver, backofficeDriver);
        }

        public static void RunTests()
        {
            SetUp(); 
            TestSuite();
            TearDown();
        }

        static void Main(string[] args)
        {
            RunTests();
        }   
    }
}
