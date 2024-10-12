using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium.Support.UI;
using static System.Collections.Specialized.BitVector32;

namespace SmartLivingShopWave.Tests
{
    public class ScreenshotTest
    {
        private ChromeDriver driver;
        public string screenshotDirectory = @"C:\Users\Pc\source\repos\Final Project\SmartLiving ShopFushion\Screenshoot";


        [SetUp]

        public void SetUp()
        {
            driver = new ChromeDriver();
            driver.Navigate().GoToUrl("https://smartliving.mk/mk/");
            driver.Manage().Window.Maximize();
            driver.FindElement(By.Id("cookie_action_close_header")).Click();
        }
  

        [Test]
        public void TestElementScreenshoot()

        {
            //hover second menu
            var artMenu = driver.FindElement(By.XPath("//*[@id=\"menu-item-81235\"]/a/span"));
            var action = new Actions(driver);
            action.MoveToElement(artMenu).Perform();
            Thread.Sleep(3000);

            //select third dropdown menu
            var wallDecoSubMenu = driver.FindElement(By.XPath("//*[@id=\"menu-item-82366\"]/a"));
            wallDecoSubMenu.Click();
            Thread.Sleep(3000);

            //select peacock
            var peacockItem = driver.FindElement(By.XPath("//div[4]//div[2]//div[5]//div[3]//h3/a"));
            peacockItem.Click();

            //select first image
            var firstPicture = driver.FindElement(By.XPath("//*[@id=\"product-85166\"]//div[1]//figure//a//img"));
            firstPicture.Click();
            Thread.Sleep(2000);

            //go throw all images
            var nextClick = driver.FindElement(By.XPath("/html/body/div[15]/div[2]/div[2]/button[2]"));
            Thread.Sleep(3000);

            for (int i = 0; i < 5; i++)
            {
                //screenshot of image no.6
                nextClick.Click();
                Thread.Sleep(2000);

                // Take a screenshot
                if (driver is ITakesScreenshot screenshotDriver)
                {

                    Screenshot screenshot = screenshotDriver.GetScreenshot();
                    string screenshotFileName = $"{driver.Title}_{DateTime.Now.ToShortDateString()}_.png";
                    string screenshotPath = Path.Combine(screenshotDirectory, screenshotFileName);
                    screenshot.SaveAsFile(screenshotPath);
                    Console.WriteLine(screenshotFileName);
                    //screenshot.SaveAsFile(driver.Title + "_" + DateTime.Now.ToShortDateString() + "Screenshot.png"); without directory in bin
                                       
                }
                else
                {
                    throw new InvalidOperationException("The WebDriver does not support taking screenshots.");
                }

                
            }
        }


        [TearDown]
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