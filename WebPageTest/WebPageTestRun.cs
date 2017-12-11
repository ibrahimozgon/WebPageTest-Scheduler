using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Net.Mail;
using log4net;
using NUnit.Framework;
using OpenQA.Selenium;

namespace WebPageTest
{
    [TestFixture]
    public class WebPageTestRun
    {
        private ILog _log;
        private const string WebpageTestUrl = "https://www.webpagetest.org/";
        [SetUp]
        public void SetUp()
        {
            log4net.Config.XmlConfigurator.Configure();
            _log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            _log.WarnFormat("Selenium Basladi");
        }

        [Test]
        public void Run()
        {
            var pages = ConfigurationManager.AppSettings.Get("urls").Split(';');
            var results = pages.Select(TestPage).ToList();

            SendMail(results);
        }

        private PerformanceTestResult TestPage(string url)
        {
            try
            {
                var commands = new Commands(WebpageTestUrl);
                _log.WarnFormat("Browser acildi");
                commands.GoTo(WebpageTestUrl);
                FillPageForm(commands, url);
                var result = GetTestResult(commands);
                result.TestedPageUrl = url;
                result.Date = DateTime.Now.ToString(CultureInfo.InvariantCulture);
                commands.Dispose();
                return result;
            }
            catch (Exception e)
            {
                _log.Error(e);
                return null;
            }
        }

        private static void FillPageForm(Commands commands, string url)
        {
            const string urlInput = "//input[@name='url']";
            commands.SendKeys(LocatorType.XPath, urlInput, url);
            commands.Click(LocatorType.Id, "advanced_settings");
            commands.SelectDropDownElement(By.Name("location"), DropdownSelector.Value,
                "Dulles:Chrome.DSL");
            commands.SendKeys(LocatorType.Id, "number_of_tests", "1");
            commands.SelectDropDownElement(By.Name("where"), DropdownSelector.Value, "Istanbul_loc");
            commands.Click(LocatorType.ClassName, "start_test");
        }

        private static PerformanceTestResult GetTestResult(Commands commands)
        {
            commands.WaitForVisible(LocatorType.XPath, "//div[@id='optimization']", TimeSpan.FromMinutes(10));
            var firstViewTime = commands.GetText(LocatorType.XPath, "//td[@id='LoadTime']");
            var url = commands.GetDriverUrl();
            var screenshotPath = commands.TakeScreenshotAndGetPath();
            return new PerformanceTestResult
            {
                ResultUrl = url,
                FirstViewTime = firstViewTime,
                ScreenshotPath = screenshotPath,
            };
        }

        private static void SendMail(IEnumerable<PerformanceTestResult> results)
        {
            var mail = new MailMessage();
            var smtpServer = new SmtpClient(ConfigurationManager.AppSettings["mail.smtp"]);
            mail.From = new MailAddress(ConfigurationManager.AppSettings["mail.from"]);
            mail.IsBodyHtml = true;
            var emailList = ConfigurationManager.AppSettings["mail.to"];

            foreach (var email in emailList.Split(';'))
            {
                mail.To.Add(email);
            }

            mail.Subject = "Webpage Hız Testi";
            foreach (var result in results.Where(t => t != null))
            {
                var screenshotAttachment = new Attachment(result.ScreenshotPath);
                mail.Attachments.Add(screenshotAttachment);

                mail.Body += $"Test edilen Sayfa: <a href=\"{result.TestedPageUrl}\">{result.TestedPageUrl}</a>" +
                            $"<br>Sonuş sayfası: <a href=\"{result.ResultUrl}\">{result.ResultUrl}</a>" +
                            $"<br>Saat: {result.Date}" +
                            $"<br>Süre: {result.FirstViewTime}<br><hr><br>";
            }

            smtpServer.Port = int.Parse(ConfigurationManager.AppSettings["mail.port"]);
            smtpServer.Credentials = new System.Net.NetworkCredential(ConfigurationManager.AppSettings["mail.from"],
                ConfigurationManager.AppSettings["mail.password"]);
            smtpServer.EnableSsl = ConfigurationManager.AppSettings["mail.ssl"] == "1";

            smtpServer.Send(mail);
        }


    }
}
