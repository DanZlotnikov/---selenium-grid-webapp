using OpenQA.Selenium;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SeleniumAutomationWebapp;


namespace SeleniumAutomationWebapp
{
    class Tests
    {
        public static void WebappSandboxSalesOrder(RemoteWebDriver webappDriver, RemoteWebDriver backofficeDriver)
        {
            HelperFunctions.GetToOrderCenter(webappDriver);
        }

        public static void WebappSandboxLogin(RemoteWebDriver webappDriver, Dictionary<String, String> userCredentials)
        {
            String username = userCredentials["username"];
            String password = userCredentials["password"];

            Exception error = null;
            bool TestSuccess = true;

            try
            {
                webappDriver.Navigate().GoToUrl(Consts.webappSandboxLoginPageUrl);
                webappDriver.Manage().Window.Maximize();
                webappDriver.FindElementByXPath("//input[@type='email']").SendKeys(username);
                webappDriver.FindElementByXPath("//input[@type='password']").SendKeys(password);
                HelperFunctions.SafeClick(webappDriver, "//button[@type='submit']");
                System.Threading.Thread.Sleep(4000);


            }
            catch (Exception e)
            {
                error = e;
                TestSuccess = false;
            }
            finally
            {
                Console.WriteLine(error);
            }

        }
    }
}
