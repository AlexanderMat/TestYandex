using System;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace NUnitTestProject1.Pages
{
    class TripBlock
    {
        private IWebElement tripBlock;

        private string timeStart;

        private string timeFinish;

        private string duration;

        private string transport;

        // Название точек отправления и прибытия рейса
        private string pointOfRootArrival;

        private string pointOfRootDeparture;

        // Т.к. название одних и тех же станций варьируется, логичней использовать ИД станций 
        private string stationDepartureId;

        private string stationArrivalId;

        private Dictionary<string, string> prices = new Dictionary<string,string>();

        [FindsBy(How = How.ClassName, Using = "Time_important")]
        public IWebElement elementTimeStart { get; set; }

        [FindsBy(How = How.XPath, Using = ".//div[contains(@class,'SearchSegment__times')]/div[3]")]
        public IWebElement elementTimefinish { get; set; }

        [FindsBy(How = How.ClassName, Using = "SearchSegment__duration")]
        public IWebElement elementDuration { get; set; }

        [FindsBy(How = How.ClassName, Using = "SearchSegment__transport")]
        public IWebElement elementTransport { get; set; }

        [FindsBy(How = How.ClassName, Using = "SearchSegment__stations")]
        public IWebElement pointsTargetTitle { get; set; }

        [FindsBy(How = How.ClassName, Using = "SearchSegment__title")]
        public IWebElement pointsRootTitle { get; set; }

        [FindsBy(How = How.ClassName, Using = "Price")]
        public IWebElement elementPrice { get; set; }

        [FindsBy(How = How.ClassName, Using = "SearchSegment__link")]
        public IWebElement tripTitle { get; set; }

        [FindsBy(How = How.ClassName, Using = "SearchSegment__scheduleAndPrices")]
        public IWebElement pricesBlock { get; set; }

        public string GetTimeStart()
        {
            return timeStart;
        }

        public string GetTimeFinish()
        {
            return timeFinish;
        }

        public string GetTransport()
        {
            return transport;
        }

        public string GetRootPointStart()
        {
            return pointOfRootDeparture;
        }

        public string GetRootPointFinish()
        {
            return pointOfRootArrival;
        }

        public string GetDepartureStationId()
        {
            return stationDepartureId;
        }

        public string GetArrivalStationId()
        {
            return stationArrivalId;
        }

        public string GetTimeInRoad()
        {
            return duration;
        }

        public bool CurrencyExists()
        {
            if (pricesBlock.Text.Contains("Р"))
            {
                return true;
            }
            else
            {
                return false;
            } 
        }

        public void SetPrice(string currency)
        {
            if (prices.ContainsKey(currency))
            {
                prices.Remove(currency);
            }

            prices.Add(currency, GetValuePrice());
        }

        public void PrintPrices()
        {
            foreach (var price in prices)
            {
                Console.WriteLine(price.Value);
            }
        }

        private string GetValuePrice()
        {
            return elementPrice.Text;
        }

        public string GetDetailsLink()
        {
            return tripTitle.GetAttribute("href");
        }

        public TripBlock(IWebElement TripBlock)
        {
            this.tripBlock = TripBlock;

            PageFactory.InitElements(TripBlock, this);

            timeStart = elementTimeStart.Text;

            timeFinish = elementTimefinish.Text;

            duration = elementDuration.Text;

            transport = elementTransport.Text;

            string[] points = pointsRootTitle.Text.Split('—');
            pointOfRootDeparture = points[0].Trim();
            pointOfRootArrival = points[1].Trim();
            
            stationDepartureId = pointsTargetTitle.FindElement(By.XPath(".//div[contains(@class,'SearchSegment__station')][1]/a")).GetAttribute("href");

            stationArrivalId = pointsTargetTitle.FindElement(By.XPath(".//div[contains(@class,'SearchSegment__station')][2]/a")).GetAttribute("href");
        }
    }
}
