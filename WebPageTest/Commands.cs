using System;
using System.IO;
using System.Reactive.Linq;
using System.Threading;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Support.Extensions;
using OpenQA.Selenium.Support.UI;

namespace WebPageTest
{
    public class Commands : IDisposable
    {
        private IWebDriver _driver;

        public void OpenDriver(string url)
        {
            var options = new ChromeOptions();
            options.AddArgument("--start-maximized");
            var driver = new ChromeDriver(options);
            _driver = driver;
            GoTo(url);
        }

        public void WaitForVisible(LocatorType by, string locator, TimeSpan? waitTime = null)
        {
            var wait = new WebDriverWait(_driver, waitTime ?? new TimeSpan(0, 0, 30));

            switch (by)
            {
                case LocatorType.XPath:
                    wait.Until(ExpectedConditions.ElementIsVisible(By.XPath(locator)));
                    break;
                case LocatorType.CssSelector:
                    wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector(locator)));
                    break;
                case LocatorType.LinkText:
                    wait.Until(ExpectedConditions.ElementIsVisible(By.LinkText(locator)));
                    break;
                case LocatorType.Id:
                    wait.Until(ExpectedConditions.ElementIsVisible(By.Id(locator)));
                    break;
                case LocatorType.Name:
                    wait.Until(ExpectedConditions.ElementIsVisible(By.Name(locator)));
                    break;
                case LocatorType.TagName:
                    wait.Until(ExpectedConditions.ElementIsVisible(By.TagName(locator)));
                    break;
                case LocatorType.ClassName:
                    wait.Until(ExpectedConditions.ElementIsVisible(By.ClassName(locator)));
                    break;
                default:
                    wait.Until(ExpectedConditions.ElementIsVisible(By.XPath(locator)));
                    break;
            }
        }
        public void SendKeys(LocatorType by, string locator, string text)
        {
            IWebElement element;
            switch (by)
            {
                case LocatorType.XPath:
                    element = _driver.FindElement(By.XPath(locator));
                    break;
                case LocatorType.CssSelector:
                    element = _driver.FindElement(By.CssSelector(locator));
                    break;
                case LocatorType.LinkText:
                    element = _driver.FindElement(By.LinkText(locator));
                    break;
                case LocatorType.Id:
                    element = _driver.FindElement(By.Id(locator));
                    break;
                case LocatorType.Name:
                    element = _driver.FindElement(By.Name(locator));
                    break;
                case LocatorType.TagName:
                    element = _driver.FindElement(By.TagName(locator));
                    break;
                case LocatorType.ClassName:
                    element = _driver.FindElement(By.ClassName(locator));
                    break;
                default:
                    element = _driver.FindElement(By.XPath(locator));
                    break;
            }
            Highlight(element);
            element.Clear();
            element.SendKeys(text);
        }
        public void Highlight(IWebElement context)
        {
            var rc = (RemoteWebElement)context;
            var driver = (IJavaScriptExecutor)rc.WrappedDriver;
            const string script =
                @"arguments[0].style.cssText = ""border-width: 5px; border-style: solid; border-color: red""; ";
            driver.ExecuteScript(script, rc);
            Observable.Timer(new TimeSpan(0, 0, 0, 0, 50)).Subscribe(p =>
            {
                const string clear =
                    @"arguments[0].style.cssText = ""border-width: 0px; border-style: solid; border-color: red""; ";
                try
                {
                    driver.ExecuteScript(clear, rc);
                }
                catch (StaleElementReferenceException e)
                {

                }
            });
        }
        public void Click(LocatorType by, string locator)
        {
            IWebElement element;
            switch (by)
            {
                case LocatorType.XPath:
                    element = _driver.FindElement(By.XPath(locator));
                    break;
                case LocatorType.CssSelector:
                    element = _driver.FindElement(By.CssSelector(locator));
                    break;
                case LocatorType.LinkText:
                    element = _driver.FindElement(By.LinkText(locator));
                    break;
                case LocatorType.Id:
                    element = _driver.FindElement(By.Id(locator));
                    break;
                case LocatorType.Name:
                    element = _driver.FindElement(By.Name(locator));
                    break;
                case LocatorType.TagName:
                    element = _driver.FindElement(By.TagName(locator));
                    break;
                case LocatorType.ClassName:
                    element = _driver.FindElement(By.ClassName(locator));
                    break;
                default:
                    element = _driver.FindElement(By.XPath(locator));
                    break;
            }
            Highlight(element);
            element.Click();
        }

        public string GetDriverUrl()
        {
            return _driver.Url;
        }
        public string GetText(LocatorType by, string locator)
        {
            IWebElement element;
            switch (by)
            {
                case LocatorType.XPath:
                    element = _driver.FindElement(By.XPath(locator));
                    break;
                case LocatorType.CssSelector:
                    element = _driver.FindElement(By.CssSelector(locator));
                    break;
                case LocatorType.LinkText:
                    element = _driver.FindElement(By.LinkText(locator));
                    break;
                case LocatorType.Id:
                    element = _driver.FindElement(By.Id(locator));
                    break;
                case LocatorType.Name:
                    element = _driver.FindElement(By.ClassName(locator));
                    break;
                case LocatorType.TagName:
                    element = _driver.FindElement(By.TagName(locator));
                    break;
                case LocatorType.ClassName:
                    element = _driver.FindElement(By.ClassName(locator));
                    break;
                default:
                    element = _driver.FindElement(By.XPath(locator));
                    break;
            }
            return element.Text;
        }

        public void Dispose()
        {
            _driver?.Close();
            _driver?.Quit();
            _driver?.Dispose();
        }

        public Screenshot TakeScreenShot()
        {
            return _driver.TakeScreenshot();
        }

        public string TakeScreenshotAndGetPath()
        {
            var screenshot = TakeScreenShot();
            var filepath = Environment.CurrentDirectory + "\\Images\\" + Guid.NewGuid() + ".png";
            var thumbDirectory = Directory.GetParent(filepath);

            if (!thumbDirectory.Exists)
                thumbDirectory.Create();

            screenshot.SaveAsFile(filepath, ScreenshotImageFormat.Png);
            return filepath;
        }

        public string SelectDropDownElement(By by, DropdownSelector selector, string value)
        {
            Wait(500);
            var element = _driver.FindElement(by);
            Highlight(element);
            var selectElement = new SelectElement(element);

            switch (selector)
            {
                case DropdownSelector.Text:
                    Wait(750);
                    selectElement.SelectByText(value);
                    return value;
                case DropdownSelector.Value:
                    Wait(750);
                    selectElement.SelectByValue(value);
                    return value;
                default:
                    return "Selector seçimi doğru yapılmadı";
            }
        }
        public void Wait(int milliseconds)
        {
            Thread.Sleep(milliseconds);
        }

        public void GoTo(string url)
        {
            _driver.Navigate().GoToUrl(url);
        }
    }
}