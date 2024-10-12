    using NUnit.Framework;
    using OpenQA.Selenium.Chrome;
    using OpenQA.Selenium.Support.UI;
    using OpenQA.Selenium;
    using System.Threading;


namespace SmartLivingShopWave.Tests
{


    public class LanguageButtonsTests
    {
        private ChromeDriver driver;
        private WebDriverWait wait;


        [OneTimeSetUp]

        public void BeforeAllTest()
        {
            driver = new ChromeDriver();
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));

            driver.Navigate().GoToUrl("https://smartliving.mk/mk/");
            driver.Manage().Window.Maximize();
            Thread.Sleep(1000);
            driver.FindElement(By.Id("cookie_action_close_header")).Click();

        }

        [Test]

        public void TestLanguageButtonsClickByText()

        {
            var kidsMenu = wait.Until(drv => drv.FindElement(By.XPath("//*[@id=\"menu-item-81226\"]/a")));
            kidsMenu.Click();
           
            driver.FindElement(By.LinkText("EN")).Click();
            Thread.Sleep(2000);

            driver.FindElement(By.LinkText("MK")).Click();
            Thread.Sleep(2000);

            driver.FindElement(By.LinkText("SQ")).Click();
           
        }

        [Test]
        public void TestLanguageButtonsClickByXPath()

        {
            var carpetMenu = wait.Until(drv => drv.FindElement(By.XPath("//*[@id=\"menu-item-80525\"]/a")));
            carpetMenu.Click();

            //EN
            driver.FindElement(By.XPath("/html/body/div[1]/header/div/div[1]/div/div/div[1]/div/div/a[1]")).Click();
            Thread.Sleep(2000);

            //MK
            driver.FindElement(By.XPath("/html/body/div[1]/header/div/div[1]/div/div/div[1]/div/div/a[2]")).Click();
            Thread.Sleep(2000);

            //SQ
            driver.FindElement(By.XPath("/html/body/div[1]/header/div/div[1]/div/div/div[1]/div/div/a[3]")).Click();
           
        }

        [TearDown]
        public void TearDown()
        {

            driver.Manage().Cookies.DeleteAllCookies();
        }

        [OneTimeTearDown]
        public void AfterAllTests()
        {
            // Clean up and close the browser
            if (driver != null)
            {
                driver.Quit();
                driver.Dispose();

            }

        }
    }
}