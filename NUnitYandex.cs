using System;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace NUnitTestProject
{
    [TestFixture]
    public class NUnitYandex
    {
        string fromCity = "Екатеринбург";
        string toCity = "Каменск-Уральский";
        string timeString;
        int timeInt;
        IWebElement voyage;
        IWebDriver driver;

        // Данные о рейсе
        string timeStart;
        string timeFinish;
        string timeInRoad;
        string priceRubles;
        string priceDollars;
        string transporter;
        string pointOfDeparture;
        string pointArrival;

        [SetUp]
        public void Initializer()
        {
            driver = new FirefoxDriver();
        }

        [TearDown]
        public void EndTest()
        {
            driver.Close();
        }

        [Test]
        public void TripTest()
        {
            driver.Url = "http://yandex.ru";

            driver.FindElement(By.LinkText("ещё")).Click();

            driver.FindElement(By.LinkText("Расписания")).Click();

            // Указываем откуда
            driver.FindElement(By.Name("fromName")).Clear();
            driver.FindElement(By.Name("fromName")).SendKeys(fromCity);

            // Указываем куда едем
            driver.FindElement(By.Name("toName")).SendKeys(toCity);

            // Выясняем число ближайшей субботы
            DateTime date = DateTime.Today;

            if ((int)date.DayOfWeek > 6)
            {
                date = date.AddDays(6);
            }
            else if ((int)date.DayOfWeek < 6)
            {
                date = date.AddDays(6 - (int)date.DayOfWeek);
            }

            // Указываем субботу
            driver.FindElement(By.Name("when")).SendKeys(((int)date.Day).ToString());

            // Указываем Автобус
            driver.FindElement(By.XPath("//input[@value='bus']")).Click();

            // Кликаем Найти
            driver.FindElement(By.ClassName("y-button_islet-rasp-search")).Click();

            // Убеждаемся в правильности выдачи результатов
            string header1 = driver.FindElement(By.ClassName("SearchHeader__threadTitle")).Text;
            string header2 = driver.FindElement(By.ClassName("SearchHeader__dateSubtitle")).Text;
            string tableHeader = header1 + header2;

            Assert.True(tableHeader.Contains(fromCity), "Не оттуда!");
            Assert.True(tableHeader.Contains(toCity), "Не туда!");
            Assert.True(tableHeader.Contains("суббота"), "Не в субботу!");
            Assert.True(tableHeader.Contains("автобус"), "Нет автобуса!");

            // Сохраняем данные рейса

            // Получаем массив объектов, где отправление происходит из Екатеринбург, Южный автовокзал.
            // Если нет таких объектов, выводим сообщение.
            // -
            // Внутри каждого объекта в массиве находим время, вычленяем часы, и сравниваем с 12.
            // Если больше, то присваиваем целевой переменной
            // -
            // Если нет такого объекта, выводим сообщение.

            // Находим все рейсы из Екб, ЮА
            ReadOnlyCollection<IWebElement> results = driver.FindElements(By.XPath("//span[contains(text(), 'Екатеринбург, Южный автовокзал')]/../../../../.."));

            if (results == null)
            {
                Console.WriteLine("Рейсов из Екатеринбург, Южный автовокзал нет!");
            }
            else
            {
                foreach (IWebElement result in results)
                {
                    timeString = result.FindElement(By.XPath(".//div[contains(@class,'Time_important')]/span")).Text;
                    timeInt = Int32.Parse(timeString.Substring(0, 2));
                    if (timeInt >= 12)
                    {
                        voyage = result;
                        break;
                    }
                }
            }

            if (voyage == null)
            {
                Console.WriteLine("Рейсов после 12 нет!");
            }

            // Сохраняем данные о рейсе
            timeStart = voyage.FindElement(By.XPath(".//div[contains(@class,'Time_important')]/span")).Text;
            timeFinish = voyage.FindElement(By.XPath(".//div[contains(@class,'SearchSegment__times')]/div[3]")).Text;
            timeInRoad = voyage.FindElement(By.XPath(".//div[contains(@class,'SearchSegment__duration')]/span")).Text;
            priceRubles = voyage.FindElement(By.ClassName("Price")).Text;
            driver.FindElement(By.ClassName("CurrencySelect")).Click();
            driver.FindElement(By.XPath("//*[@data-value='USD']")).Click();
            priceDollars = voyage.FindElement(By.ClassName("Price")).Text;
            transporter = voyage.FindElement(By.ClassName("SearchSegment__model")).Text;

            string[] points = voyage.FindElement(By.ClassName("SearchSegment__title")).Text.Split('—');
            pointOfDeparture = points[0].Trim();
            pointArrival = points[1].Trim();

            Console.WriteLine("Время отправления: " + timeStart);
            Console.WriteLine("Цена в рублях: " + priceRubles);
            Console.WriteLine("Цена в долларах: " + priceDollars);

            // Обычный метод клика возвращал ошибку из-за перекрытия видимости элемента
            driver.Url = voyage.FindElement(By.ClassName("SearchSegment__link")).GetAttribute("href");

            // Проверяем заголовок
            string voyageTitle = driver.FindElement(By.ClassName("b-page-title__title")).Text;

            Assert.True(voyageTitle.Contains(fromCity), "Не оттуда!");
            Assert.True(voyageTitle.Contains(toCity), "Не туда!");
            Assert.True(voyageTitle.Contains("автобус"), "Нет автобуса!");
            
            // Проверяем транспорт
            Assert.True(driver.FindElement(By.ClassName("b-page-title__text")).Text.Contains(transporter), "Транспорт не тот.");

            IWebElement tableRow = driver.FindElement(By.ClassName("b-timetable__row_type_start"));

            // Проверяем время отправления
            Assert.True(tableRow.FindElement(By.XPath(".//*[@class='b-timetable__time']/span/strong")).Text.Equals(timeStart), "Время отправления не то.");

            // Проверяем пункт отправления
            Assert.True(tableRow.FindElement(By.XPath(".//*[@class='b-timetable__city']/a")).Text.Equals(pointOfDeparture), "Едем не оттуда.");

            tableRow = driver.FindElement(By.ClassName("b-timetable__row_type_end"));

            // Проверяем пункт назначения
            Assert.True(tableRow.FindElement(By.XPath(".//*[@class='b-timetable__city']/a")).Text.Equals(pointArrival), "Едем не туда.");

            // Проверяем время прибытия
            Assert.True(tableRow.FindElement(By.XPath(".//*[@class='b-timetable__time']/span/strong")).Text.Equals(timeFinish), "Время прибытия не то.");

            // Проверяем время в пути
            Assert.True(tableRow.FindElement(By.ClassName("b-timetable__cell_position_last")).Text.Equals(timeInRoad), "Время в пути не то.");
        }
    }
}