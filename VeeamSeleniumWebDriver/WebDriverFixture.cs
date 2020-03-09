using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;

namespace VeeamSeleniumWebDriver
{
    public class WebDriverFixture : IDisposable
    {
        public WebDriverFixture()
        {
            _webDrivers.Add(WebDriverType.Chrome, new ChromeDriver("."));
        }

        private readonly Dictionary<WebDriverType, IWebDriver> _webDrivers = new Dictionary<WebDriverType, IWebDriver>();
        private bool _disposed = false;
        
        public IWebDriver this[WebDriverType webDriver]
        {
            get { return _webDrivers[webDriver]; }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            foreach (var wd in _webDrivers.Values)
            {
                wd.Close();
            }

            _disposed = true;
        }

        ~WebDriverFixture()
        {
            Dispose(false);
        }
    }

    public enum WebDriverType
    {
        Chrome
    }
}
