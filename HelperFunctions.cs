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
    class HelperFunctions
    {
        /// <summary>
        /// Delegate a function into an object
        /// </summary>
        /// <param name="webappDriver"></param>
        /// <param name="backofficeDriver"></param>
        public delegate void Delegator(RemoteWebDriver webappDriver, RemoteWebDriver backofficeDriver);

        /// <summary>
        /// Wraps 
        /// </summary>
        /// <param name="delegatedFunction"></param>
        /// <param name="remoteWebDriver"></param>
        public static void BasicTestWrapper(HelperFunctions.Delegator delegatedFunction, RemoteWebDriver webappDriver, RemoteWebDriver backofficeDriver)
        {
            Exception error = null;
            bool testSuccess = true;

            try
            {
                delegatedFunction(webappDriver, backofficeDriver);
            }
            catch (Exception e)
            {
                error = e;
                testSuccess = false;
            }
            finally
            {
                Console.WriteLine(error);
            }
        }

        /// <summary>
        /// Gets from home page to cart(Sales Order > Store1 > Default Catalog)
        /// </summary>
        /// <param name="webappDriver"></param>
        public static void GetToOrderCenter(RemoteWebDriver webappDriver)
        {
            webappDriver.Navigate().GoToUrl(Consts.webappSandboxHomePageUrl);

            SafeClick(webappDriver, "//div[@id='mainCont']/app-home-page/footer/div/div[2]/div/div");

            SafeClick(webappDriver, "//div[@id='viewsContainer']/app-custom-list/virtual-scroll/div[2]/div[1]/app-custom-form/fieldset/div/app-custom-field-generator/app-custom-button/a/span");

            System.Threading.Thread.Sleep(5000);

            SafeClick(webappDriver, "//div[@id='actionBar']/div/ul[3]/li/a/span");

            SafeClick(webappDriver, "//div[@id='actionBar']/div/ul[3]/li/ul/li/span");

            SafeClick(webappDriver, "//div[@id='container']/div[2]");
        }

        /// <summary>
        /// Clicks an element in the web page. Uses retry logic for a specified amount of tries. One second buffer between tries.
        /// </summary>
        /// <param name="webappDriver"></param>
        /// <param name="elementXPath"></param>
        public static void SafeClick(RemoteWebDriver webappDriver, string elementXPath)
        {
            IWebElement element; 

            int retryCount = 0;
            while (retryCount < Consts.maxRetryCount)
            {
                try
                {
                    System.Threading.Thread.Sleep(1000);
                    element = webappDriver.FindElementByXPath(elementXPath);
                    Highlight(webappDriver, element);
                    element.Click();
                    return;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    retryCount++;
                    continue;
                }
            }

            string errorMessage = string.Format("Click action failed for element at XPath: {0}", elementXPath);
            RetryException error = new RetryException(errorMessage);
            throw error;
        }

        /// <summary>
        /// Highlights an element in the webpage
        /// </summary>
        /// <param name="driver"></param>
        /// <param name="element"></param>
        public static void Highlight(IWebDriver driver, IWebElement element)
        {
            var jsDriver = (IJavaScriptExecutor)driver;
            string highlightJavascript = @"arguments[0].style.cssText = ""background: yellow; border-width: 2px; border-style: solid; border-color: red"";";
            jsDriver.ExecuteScript(highlightJavascript, new object[] { element });

            System.Threading.Thread.Sleep(750);
            string originalStyleJavascript = @"arguments[0].style.cssText = ""background: none; border-width: 1px; border-style: solid; border-color: transparent"";";
            jsDriver.ExecuteScript(originalStyleJavascript, new object[] { element });
        }

    }
}
