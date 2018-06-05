using Newtonsoft.Json;
using OpenQA.Selenium;
using OpenQA.Selenium.Remote;
using System;
using System.IO;
using System.Threading;
using static SeleniumAutomationWebapp.Consts;

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
        /// Wraps tests with a try-catch-except logic that creates logs
        /// </summary>
        /// <param name="delegatedFunction"></param>
        /// <param name="remoteWebDriver"></param> 
        public static void BasicTestWrapper(Delegator delegatedFunction, RemoteWebDriver webappDriver, RemoteWebDriver backofficeDriver)
        {
            string functionName = delegatedFunction.Method.Name.ToString();

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
                WriteToSuccessLog(functionName, testSuccess, error);
            }
        }

        /// <summary>
        /// Gets from home page to cart(Sales Order > Store1 > Default Catalog)
        /// </summary>
        /// <param name="webappDriver"></param>
        public static void GetToOrderCenter(RemoteWebDriver webappDriver)
        {
            webappDriver.Navigate().GoToUrl(webappSandboxHomePageUrl);

            // Accounts
            SafeClick(webappDriver, "//div[@id='mainCont']/app-home-page/footer/div/div[2]/div/div");

            // First account
            SafeClick(webappDriver, "//div[@id='viewsContainer']/app-custom-list/virtual-scroll/div[2]/div[1]/app-custom-form/fieldset/div/app-custom-field-generator/app-custom-button/a/span");


            Thread.Sleep(5000);

            // Plus button
            SafeClick(webappDriver, "//div[@id='actionBar']/div/ul[3]/li/a/span");

            // Sales Order
            SafeClick(webappDriver, "//div[@id='actionBar']/div/ul[3]/li/ul/li/span");

            /*      
            // Origin account
            SafeClick(webappDriver, "//body/app-root/div/app-accounts-home-page/object-chooser-modal/div/div/div/div/div/div/app-custom-list/virtual-scroll/div/div[2]/app-custom-form/fieldset/div");

            //Done
            SafeClick(driver, "//div[@id='mainCont']/app-accounts-home-page/object-chooser-modal/div/div/div/div[3]/div[2]");
            */
            
            // Default Catalog
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
                    element = webappDriver.FindElementByXPath(elementXPath);
                    Highlight(webappDriver, element);
                    element.Click();

                    return;
                }
                catch (Exception e)
                {
                    Thread.Sleep(1000);
                    retryCount++;
                    continue;
                }
            }

            string errorMessage = string.Format("Click action failed for element at XPath: {0}", elementXPath);
            RetryException error = new RetryException(errorMessage);
            throw error;
        }

        /// <summary>
        /// Sends keys to an element in the web page. Uses retry logic for a specified amount of tries. One second buffer between tries.
        /// </summary>
        /// <param name="webappDriver"></param>
        /// <param name="elementXPath"></param>
        /// <param name="KeysToSend"></param>
        public static void SafeSendKeys(RemoteWebDriver webappDriver, string elementXPath, string KeysToSend)
        {
            IWebElement element;

            int retryCount = 0;
            while (retryCount < maxRetryCount)
            {
                try
                {
                    element = webappDriver.FindElementByXPath(elementXPath);
                    Highlight(webappDriver, element);
                    element.SendKeys(KeysToSend);

                    return;
                }
                catch (Exception e)
                {
                    Thread.Sleep(1000);
                    retryCount++;
                    continue;
                }
            }

            string errorMessage = string.Format("SendKeys action failed for element at XPath: {0}", elementXPath);
            RetryException error = new RetryException(errorMessage);
            throw error;
        }

        /// <summary>
        /// Gets value from an element in the web page. Uses retry logic for a specified amount of tries. One second buffer between tries.
        /// </summary>
        /// <param name="webappDriver"></param>
        /// <param name="elementXPath"></param>
        /// <param name="attribute"></param>
        /// <returns></returns>
        public static dynamic SafeGetValue(RemoteWebDriver webappDriver, string elementXPath, string attribute)
        {
            IWebElement element;

            int retryCount = 0;
            while (retryCount < maxRetryCount)
            {
                try
                {
                    element = webappDriver.FindElementByXPath(elementXPath);
                    Highlight(webappDriver, element);
                    var value = element.GetAttribute(attribute);

                    return value;
                }
                catch (Exception e)
                {
                    Thread.Sleep(1000);
                    retryCount++;
                    continue;
                }
            }

            string errorMessage = string.Format("SendKeys action failed for element at XPath: {0}", elementXPath);
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
            // Highlight element
            var jsDriver = (IJavaScriptExecutor)driver;
            string highlightJavascript = @"arguments[0].style.cssText = ""background: yellow; border-width: 2px; border-style: solid; border-color: red"";";
            jsDriver.ExecuteScript(highlightJavascript, new object[] { element });

            // Restore element css to original 
            System.Threading.Thread.Sleep(500);
            string originalStyleJavascript = @"arguments[0].style.cssText = ""background: none; border-width: 1px; border-style: solid; border-color: transparent"";";
            jsDriver.ExecuteScript(originalStyleJavascript, new object[] { element });
        }

        /// <summary>
        /// Creates a new log file to the grid_logs folder
        /// </summary>
        /// <param name="logType"></param>
        /// <param name="browserName"></param>
        /// <param name="dateTime"></param>
        /// <returns> Log file path </returns>. 
        public static string CreateNewLog(string logType, string browserName, DateTime dateTime)
        {
            string strDateTime =  dateTime.ToString("yyyy-MM-dd HH:mm:ss.fff").Replace('\\', '-').Replace(' ', '_').Replace(':', '-');
            string path = string.Format("C:\\Users\\Dan.Z\\Desktop\\grid_logs\\CS\\{0}_{1}_{2}.json", logType, browserName, strDateTime);
            using (StreamWriter streamWriter = File.AppendText(path))
            {
                streamWriter.Write("");
            }
            return path;
        }

        /// <summary>
        /// Creates and returns a success json string for a test
        /// </summary>
        public static string CreateSuccessJson(string testName, bool success, Exception error)
        {
            // Create an object to dump - dynamic means it can be manipulated as I wish (like python)

            dynamic successObject = new
            {
                testName = testName,
                success = success,
                error = (error == null) ? null : error.Message
            };

            var json = JsonConvert.SerializeObject(successObject);
            return json;
        }
        
        /// <summary>
        /// Writes a new test success string to the success log file
        /// </summary>
        /// <param name="testName"></param>
        /// <param name="success"></param>
        /// <param name="error"></param>
        public static void WriteToSuccessLog(string testName, bool success, Exception error)
        {
            string jsonSuccessString = CreateSuccessJson(testName, success, error);
            using (StreamWriter streamWriter = File.AppendText(GlobalSettings.successLogFilePath))
            {
                streamWriter.WriteLine(jsonSuccessString);
            }
        }
    }
}
