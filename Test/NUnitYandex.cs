using System;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using NUnitTestProject1.Pages;

namespace NUnitTestProject1.Test
{
    [TestFixture]
    public class NUnitYandex
    {
        IWebDriver driver;
 
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
        public void TestMethod1()
        {
            // Задаем переменные для тест-кейса
            string fromCity = "Екатеринбург";

            string fromStation = "Автовокзал Южный";

            string toCity = "Каменск-Уральский";

            int dayOfWeek = 6;

            string transport = "Автобус";

            double time = 12.00;

            string[] currencies = new string[] { "USD", "RUB" };

            MainPage MainPage = new MainPage(driver);

            MainPage.Navigate();

            MainPage.ClickMoreButton();

            MainPage.ClickSchedule();

            TripListPage TripListPage = new TripListPage(driver);

            // Указываем данные для поиска
            TripListPage.From(fromCity);

            TripListPage.To(toCity);

            TripListPage.Date(dayOfWeek);

            TripListPage.Transport(transport);

            TripListPage.Find();

            TripSearchResultPage TripSearchResultPage = new TripSearchResultPage(driver);

            // Проверяем нашлись ли рейсы
            Assert.True(TripSearchResultPage.SearchSucces(), "Рейсы не найдены");

            // Сверяем результаты поиска
            // Проверяем место отправления рейса
            Assert.True((TripSearchResultPage.Title()).Contains(fromCity), "Едем не из {0}", fromCity);

            // Проверяем место назначения рейса
            Assert.True((TripSearchResultPage.Title()).Contains(toCity), "Едем не в {0}", toCity);

            // Проверяем транспорт или компанию
            Assert.True((TripSearchResultPage.Title()).Contains(transport.ToLower()), "Едем не на {0}", transport);

            // Проверяем день недели
            Assert.True((TripSearchResultPage.Title()).Contains(TripSearchResultPage.RuDayOfWeek(dayOfWeek)), "День недели не {0}", TripSearchResultPage.RuDayOfWeek(dayOfWeek));

            TripBlock TripBlock = null;

            // Сохраняем данные рейса, если находится подходящий под условия, либо выводим сообщение об отсутствии рейсов и прерываем тест
            if (TripSearchResultPage.SearchTrip(time, fromStation) == null)
            {
                Console.WriteLine("Рейсы из {0} {1} после {2} отсутствуют.", fromCity, fromStation, time);
                return;
            }
            else
            {
                TripBlock = new TripBlock(TripSearchResultPage.SearchTrip(time, fromStation));
            }
            
            // Сохрянием цены на рейс в разных валютах, если они есть
            if (TripBlock.CurrencyExists())
            {
                foreach (string currency in currencies)
                {
                    TripSearchResultPage.ShowPrice(currency);

                    TripBlock.SetPrice(currency);
                }
            }
            else
            {
                Console.WriteLine("Цен нет.");
            }

            // Выводим время начала рейса
            Console.WriteLine(TripBlock.GetTimeStart());

            // Выводим цены
            TripBlock.PrintPrices();

            // Переходим на страницу детализации рейса
            driver.Url = TripBlock.GetDetailsLink();

            TripDetailsPage TripDetailsPage = new TripDetailsPage(driver);

            // Проверка заголовка
            // Проверяем место отправление рейса
            Assert.True(TripDetailsPage.GetTitle().Contains(TripBlock.GetRootPointStart()), "Едем не из {0}", TripBlock.GetRootPointStart());
            
            // Проверяем место прибытия рейса
            Assert.True(TripDetailsPage.GetTitle().Contains(TripBlock.GetRootPointFinish()), "Едем не в {0}", TripBlock.GetRootPointFinish());

            // Проверяем транспорт или компанию
            Assert.True(TripDetailsPage.GetTransport().Contains(TripBlock.GetTransport()), "Едем не на {0}", TripBlock.GetTransport());

            // Проверка таблицы
            // Проверяем время отправления
            Assert.True(TripDetailsPage.GetTimeStart().Contains(TripBlock.GetTimeStart()), "Отправляемся не в {0}", TripBlock.GetTimeStart());

            // Проверяем станцию отправления
            Assert.True(TripDetailsPage.GetDepartureStationId().Contains(TripBlock.GetDepartureStationId()), "Станции отправления не равны.");

            // Проверяем станцию прибытия
            Assert.True(TripDetailsPage.GetArrivalStationId().Contains(TripBlock.GetArrivalStationId()), "Станции прибытия не равны.");

            // Проверяем время прибытия
            Assert.True(TripDetailsPage.GetTimeFinish().Contains(TripBlock.GetTimeFinish()), "Приезжаем не в {0}", TripBlock.GetTimeFinish());

            // Проверяем время в пути
            Assert.True(TripDetailsPage.GetTimeInRoad().Contains(TripBlock.GetTimeInRoad()), "Время в пути не {0}", TripBlock.GetTimeInRoad());
        }
    }
}
