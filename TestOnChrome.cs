using OpenQA.Selenium;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeleniumAutomationWebapp
{
    class TestOnChrome
    {
        public static RemoteWebDriver webappDriver, backofficeDriver;
        public static void SetUp()
        {

            DesiredCapabilities capability = DesiredCapabilities.Chrome();
            webappDriver = new RemoteWebDriver(new Uri("http://localhost:4444/wd/hub"), capability, TimeSpan.FromSeconds(600));
            backofficeDriver = new RemoteWebDriver(new Uri("http://localhost:4444/wd/hub"), capability, TimeSpan.FromSeconds(600));
        }

        public static void TearDown()
        {
            webappDriver.Close();
            backofficeDriver.Close();
        }

        public static void TestSuite()
        {
            Tests.WebappSandboxLogin(webappDriver, new Dictionary<string, string>
            {
                {"username", "automation@pepperitest.com"},
                {"password", "123456"}
            });

            HelperFunctions.Delegator delegatedFunction = Tests.WebappSandboxSalesOrder;
            HelperFunctions.BasicTestWrapper(delegatedFunction, webappDriver, backofficeDriver);
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
