using System;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using System.Collections.Generic;
using System.Collections;
using System.Collections.ObjectModel;
using System.Text;

namespace NUnitTestProject1.Pages
{
    class TripDetailsPage
    {
        private IWebDriver driver;

        private string stationDepartureId;

        private string stationArrivalId;

        [FindsBy(How = How.ClassName, Using = "b-page-title__title")]
        public IWebElement elementTitle { get; set; }

        [FindsBy(How = How.ClassName, Using = "b-timetable__row_type_start")]
        public IWebElement rowStart { get; set; }

        [FindsBy(How = How.ClassName, Using = "b-timetable__row_type_end")]
        public IWebElement rowEnd { get; set; }
        
        [FindsBy(How = How.ClassName, Using = "b-page-title__text")]
        public IWebElement transport { get; set; }

        public string GetTitle()
        {
            return elementTitle.Text.ToString();
        }

        public string GetTimeStart()
        {
            return rowStart.FindElement(By.ClassName("b-timetable__cell_type_departure")).Text;
        }

        public string GetPointStart()
        {
            return rowStart.FindElement(By.XPath(".//*[@class='b-timetable__city']/a")).Text;
        }

        public string GetTimeFinish()
        {
            return rowEnd.FindElement(By.XPath(".//*[@class='b-timetable__time']/span/strong")).Text;
        }

        public string GetPointFinish()
        {
            return rowEnd.FindElement(By.XPath(".//*[@class='b-timetable__city']/a")).Text;
        }

        public string GetTimeInRoad()
        {
            return rowEnd.FindElement(By.ClassName("b-timetable__cell_position_last")).Text;
        }

        public string GetTransport()
        {
            return transport.Text;
        }

        public string GetDepartureStationId()
        {
            return stationDepartureId;
        }

        public string GetArrivalStationId()
        {
            return stationArrivalId;
        }

        public TripDetailsPage(IWebDriver driver)
        {
            this.driver = driver;
            PageFactory.InitElements(driver, this);

            stationDepartureId = rowStart.FindElement(By.XPath(".//a[contains(@class,'b-link')]")).GetAttribute("href") + "/";

            stationArrivalId = rowEnd.FindElement(By.XPath(".//a[contains(@class,'b-link')]")).GetAttribute("href") + "/";
        }
    }
}
