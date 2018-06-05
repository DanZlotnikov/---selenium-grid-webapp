using OpenQA.Selenium.Remote;
using System;
using System.Collections.Generic;
using static SeleniumAutomationWebapp.HelperFunctions;
using static SeleniumAutomationWebapp.Consts;

namespace SeleniumAutomationWebapp
{
    class Tests
    {

        public static void WebappSandboxLogin(RemoteWebDriver webappDriver, Dictionary<String, String> userCredentials)
        {
            String username = userCredentials["username"];
            String password = userCredentials["password"];

            Exception error = null;
            bool testSuccess = true;

            try
            {
                // Login page
                webappDriver.Navigate().GoToUrl(webappSandboxLoginPageUrl);
                webappDriver.Manage().Window.Maximize();

                // Input credentials
                SafeSendKeys(webappDriver, "//input[@type='email']", username);
                SafeSendKeys(webappDriver, "//input[@type='password']", password);

                // Login button
                SafeClick(webappDriver, "//button[@type='submit']");
            }

            catch (Exception e)
            {
                error = e;
                testSuccess = false;
            }

            finally
            {
                WriteToSuccessLog("WebappSandboxLogin", testSuccess, error);
            }

        }
        public static void WebappSandboxSalesOrder(RemoteWebDriver webappDriver, RemoteWebDriver backofficeDriver)
        {
            GetToOrderCenter(webappDriver);
        }

    }
}
