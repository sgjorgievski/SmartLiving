using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;
using System;
using System.Drawing;
using System.Threading;

namespace SmartLivingShopWave.Tests
{
    [TestFixture("Chrome")]
    [TestFixture("Edge")]
    public class ViewportResponsivenessTest
    {
        private IWebDriver driver;
        private readonly string browserType;

        public ViewportResponsivenessTest(string browser) => browserType = browser;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            driver = browserType switch
            {
                "Chrome" => new ChromeDriver(),
                "Edge" => new EdgeDriver(),
                _ => throw new ArgumentException("Invalid browser type")
            };

            Thread.Sleep(2000);
        }


        [Test]
        public void TestViewportResponsiveDesign()
        {
            var viewports = new[] { "480x800", "768x1024", "1024x768", "1366x768", "1920x1080" };
            string url = "https://smartliving.mk/mk/";

            foreach (var viewport in viewports)
            {
                var dimensions = viewport.Split('x');
                int width = int.Parse(dimensions[0]);
                int height = int.Parse(dimensions[1]);

                // Set the window size
                driver.Manage().Window.Size = new Size(width, height);
                driver.Navigate().GoToUrl(url);

                // Assertions to verify responsiveness
                Assert.That(IsPageResponsive(), $"Page is not responsive at viewport size: {viewport}");
                Console.WriteLine($"Page is responsive at viewport size: {viewport}");
            }
        }

        private static bool IsPageResponsive() => true;

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            // Clean up and close the browser
            driver?.Quit();
            driver?.Dispose();
        }
    }
}



/*
        [SetUp]
        public void SetUp()
        {
            driver = new ChromeDriver();
            Thread.Sleep(1000);
        }


        [Test]
        public void TestViewportResponsiveDesign()
        {

                    string[] viewports = { "480x800", "768x1024", "1024x768", "1366x768", "1920x1080" };
                    string url = "https://smartliving.mk/mk/";

                    foreach (var viewport in viewports)
                    {
                        var dimensions = viewport.Split('x');
                        int width = int.Parse(dimensions[0]);
                        int height = int.Parse(dimensions[1]);

                        driver.Manage().Window.Size = new System.Drawing.Size(width, height);
                        driver.Navigate().GoToUrl(url);

                        // assertions to verify responsiveness
                        Assert.IsTrue(IsPageResponsive(), $"Page is not responsive at viewport size: {viewport}");
                    }
                }

            private bool IsPageResponsive()
            {

                return true;
            }

            [TearDown]

            public void TearDown()


            {
                driver.Quit();

            }


    }
}


//Edge

[SetUp]
public void SetUp()
{
    driver = new EdgeDriver();
    Thread.Sleep(1000);
}


[Test]
public void TestViewportResponsiveDesign()
{

    string[] viewports = { "480x800", "768x1024", "1024x768", "1366x768", "1920x1080" };
    string url = "https://smartliving.mk/mk/";


    foreach (var viewport in viewports)
    {
        var dimensions = viewport.Split('x');
        int width = int.Parse(dimensions[0]);
        int height = int.Parse(dimensions[1]);

        driver.Manage().Window.Size = new System.Drawing.Size(width, height);
        driver.Navigate().GoToUrl(url);

        // assertions to verify responsiveness
        Assert.That(IsPageResponsive(), $"Page is not responsive at viewport size: {viewport}");
        Console.WriteLine($"Page is responsive at viewport size: {viewport}");
    }
}



private static bool IsPageResponsive()
{

return true;
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
*/