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
    class Consts
    {
        public const string webappSandboxLoginPageUrl = "https://app.sandbox.pepperi.com/#/";
        public const string webappSandboxHomePageUrl = "https://app.sandbox.pepperi.com/#/HomePage";

        public const int maxRetryCount = 15;
    }
}
