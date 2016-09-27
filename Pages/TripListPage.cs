using System;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

namespace NUnitTestProject1.Pages
{
    class TripListPage
    {
        private IWebDriver driver;

        [FindsBy(How = How.Name, Using = "fromName")]
        public IWebElement fromName { get; set; }

        [FindsBy(How = How.Name, Using = "toName")]
        public IWebElement toName { get; set; }

        [FindsBy(How = How.Name, Using = "when")]
        public IWebElement when { get; set; }

        [FindsBy(How = How.ClassName, Using = "transport-selector")]
        public IWebElement transportBar { get; set; }

        [FindsBy(How = How.ClassName, Using = "y-button_islet-rasp-search")]
        public IWebElement findButton { get; set; }

        public void From(string city)
        {
            fromName.Clear();
            fromName.SendKeys(city);
        }

        public void To(string city)
        {
            toName.Clear();
            toName.SendKeys(city);
        }

        // На входе - день недели, на выходе число, ближайшего заданного дня недели
        public void Date(int dayOfWeek)
        {
            DateTime date = DateTime.Today;

            if ((int)date.DayOfWeek > dayOfWeek)
            {
                date = date.AddDays((7 - (int)date.DayOfWeek) + dayOfWeek);
            }
            else if ((int)date.DayOfWeek < dayOfWeek)
            {
                date = date.AddDays(dayOfWeek - (int)date.DayOfWeek);
            }

            when.Clear();
            when.SendKeys(((int)date.Day).ToString());
        }

        public void Transport(string transport)
        {
            transportBar.FindElement(By.XPath(".//label[contains(text(), '" + transport + "')]")).Click();
        }

        public void Find()
        {
            findButton.Click();
        }

        public TripListPage(IWebDriver driver)
        {
            this.driver = driver;
            PageFactory.InitElements(driver, this);
        }

    }
}
