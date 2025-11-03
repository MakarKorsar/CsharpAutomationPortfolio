using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using WebDriverManager;
using WebDriverManager.DriverConfigs.Impl;
using System;
using System.Threading;
using System.Threading.Tasks; // Только для демонстрации

namespace CsharpTests.Tests
{
    public class UITests
    {
        private IWebDriver _driver;

        [SetUp]
        public void Setup()
        {
            // Используем WebDriverManager для автоматической загрузки драйвера
            new DriverManager().SetUpDriver(new ChromeConfig());
            _driver = new ChromeDriver(); 
            _driver.Manage().Window.Maximize();
            _driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);
        }

        [Test]
        public async Task Test_Google_Search_Title()
        {
            _driver.Navigate().GoToUrl("https://www.google.com");
            
            // Проверка наличия поля поиска
            IWebElement searchBox = _driver.FindElement(By.Name("q"));

await Task.Delay(2000); // Задержка для демонстрации асинхронности
            searchBox.SendKeys("C# automation with Selenium");
            await Task.Delay(2000);
            searchBox.SendKeys(Keys.Enter);
            
            // Проверяем, что заголовок содержит искомый текст
            await Task.Delay(2000); // Ждем загрузки результатов (для демонстрации)
            Assert.That(_driver.Title, Does.Contain("C# automation with Selenium"));
        }

        [TearDown]
public void Teardown()
        {
            // Проверяем, что драйвер был инициализирован, и корректно его закрываем.
            if (_driver != null)
            {
                // Закрытие браузера и освобождение всех ресурсов.
                // Вызов Quit() обычно достаточен для Selenium, 
                // но анализатор NUnit1032 требует, чтобы ресурс был Disposed.
                _driver.Quit(); 
                _driver.Dispose(); // Добавляем вызов Dispose для удовлетворения NUnit
            }
        }
    }
}