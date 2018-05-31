using System;
using System.Collections.Generic;
using System.Text;

namespace SeleniumAutomationWebapp
{
    class RetryException: Exception
    {
        public RetryException()
        {
        }

        public RetryException(string message)
            : base(message)
        {
        }

        public RetryException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
