using System;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace NUnitTestProject1.Pages
{
    class TripSearchResultPage
    {
        private IWebDriver driver;

        [FindsBy(How = How.ClassName, Using = "SearchHeader")]
        public IWebElement searchHeader { get; set; }

        [FindsBy(How = How.ClassName, Using = "SearchSegments")]
        public IWebElement resultList { get; set; }

        [FindsBy(How = How.ClassName, Using = "CurrencySelect")]
        public IWebElement currencySelector { get; set; }

        [FindsBy(How = How.ClassName, Using = "SearchLayout__content")]
        public IWebElement pageContent { get; set; }

        // Функция проверяющая успешность поиска
        public bool SearchSucces()
        {
            if (pageContent.Text.Contains("не найдено"))
            {
                return false;
            }
            else if (pageContent.Text.Contains("Укажите пункт отправления"))
            {
                return false;
            }
            if (pageContent.Text.Contains("Не найдена"))
            {
                return false;
            }

            return true;
        }

        public void ShowPrice(string currency)
        {
            currencySelector.Click();

            driver.FindElement(By.XPath("//*[@data-value='" + currency + "']")).Click();
        }

        public string Title()
        {
            return searchHeader.Text;
        } 

        public string RuDayOfWeek(int day) 
        {
            DateTime dt = DateTime.Now;

            dt = dt.AddDays(day - (int)dt.DayOfWeek);

            return dt.ToString("dddd");
        }

        // Метод поиска рейса по времени и станции.
        public IWebElement SearchTrip(double time, string fromStation = "")
        {
            ReadOnlyCollection<IWebElement> resultAll = resultList.FindElements(By.ClassName("SearchSegment"));

            List<IWebElement> resultSuccessList = new List<IWebElement>();

            foreach (IWebElement result in resultAll)
            {
                double timeToTrip = Double.Parse(result.FindElement(By.XPath(".//div[contains(@class,'Time_important')]/span")).Text.Replace(":", ","));

                if (timeToTrip >= time) 
                {
                    resultSuccessList.Add(result);
                }
            }

            if (fromStation != "")
            {
                foreach (IWebElement result in resultSuccessList)
                {
                    if (result.FindElement(By.ClassName("SearchSegment__station")).Text == fromStation)
                    {
                        return result;
                    }
                }
            }

            if (resultSuccessList.Count > 0)
            {
                return resultSuccessList.First();
            }
            else
            {
                return null;
            }
        }

        public TripSearchResultPage(IWebDriver driver)
        {
            this.driver = driver;
            PageFactory.InitElements(driver, this);
        }
    }

}
