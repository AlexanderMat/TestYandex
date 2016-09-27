using System;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

namespace NUnitTestProject1.Pages
{
    public class MainPage
    {
        private IWebDriver driver;
        private string url = @"https://www.yandex.ru/";

        [FindsBy(How = How.LinkText, Using = "ещё")]
        public IWebElement MoreButton { get; set; }

        [FindsBy(How = How.LinkText, Using = "Расписания")]
        public IWebElement Schedule { get; set; }

        public void ClickMoreButton()
        {
            MoreButton.Click();
        }

        public void ClickSchedule()
        {
            Schedule.Click();
        }

        public void Navigate()
        {
            this.driver.Navigate().GoToUrl(this.url);
        }

        public MainPage(IWebDriver driver)
        {
            this.driver = driver;
            PageFactory.InitElements(driver, this);
        }
    }
}
