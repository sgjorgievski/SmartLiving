using OpenQA.Selenium.Chrome;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using System.IO;
using System.Collections.ObjectModel;

namespace SmartLivingShopWave.Tests
{
    public class SearchbarTest
    {
        private ChromeDriver driver;
        private WebDriverWait wait;
        private Actions action;


        [OneTimeSetUp]
        public void BeforeAllTest()
        {
            driver = new ChromeDriver();
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            action = new Actions(driver);
        }

        [SetUp]

        public void SetUp()
        {
            driver.Navigate().GoToUrl("https://smartliving.mk/mk/");
            driver.Manage().Window.Maximize();
            driver.FindElement(By.Id("cookie_action_close_header")).Click();
        }


        [Test]

        public void VerifySearchWithValidInput()
        {
            //locate navmenu kids
            var kidsMenu = wait.Until(drv => drv.FindElement(By.XPath("//*[@id=\"menu-item-81226\"]/a")));
            kidsMenu.Click();

            //locate element racoon
            var racoonItem = wait.Until(drv => drv.FindElement(By.XPath("/html/body/div[1]/div[1]/div/div/div/div[3]/div[2]/div[8]/div/div[3]/h3/a")));
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView({ behavior: 'smooth', block: 'center' });", racoonItem, 60);
            racoonItem.Click();

            // Enter a query
            driver.FindElement(By.Name("s")).SendKeys("Ракун за перници" + Keys.Return);

            //wait results
            var results = wait.Until(drv => drv.FindElement(By.XPath("/html/body/div[1]/div[1]/div[2]/div/div/div[3]/div[2]/div[1]/div/div[3]/h3/a")));

            // Verify results
            Assert.Multiple(() =>
            {
                Assert.That(results.Displayed, "Search results are not displayed.");
                Assert.That(results.Text, Does.Contain("Ракун за перници"), "Search results do not contain expected text.");
            });
            string resultsText = results.Text;
            Console.WriteLine("Search Results: " + resultsText);

        }

        [Test]
        public void VerifySerchWithInvalidInput()
        {
            // Enter a query
            var searchInput = driver.FindElement(By.Name("s"));
            searchInput.SendKeys("Сонце" + Keys.Return);

            // Wait for the results
            var results = wait.Until(drv => drv.FindElement(By.XPath("/html/body/div[1]/div[1]/div[2]/div/div/div[3]/div")));

            // Verify results
            Assert.Multiple(() =>
            {
                Assert.That(results.Displayed, "Search results are not displayed.");
                Assert.That(results.Text, Does.Contain("Нема производи беа пронајдени за појавување на вашиот избор"), "Search results do not contain expected text.");
            });
            string resultsText = results.Text;
            Console.WriteLine("Search Results: " + resultsText);

        }

        [Test]
        public void VerifySerchWithEmptyInput()
        {

            // Enter a query
            var searchInput = driver.FindElement(By.Name("s"));
            searchInput.Click();
            searchInput.SendKeys(Keys.Return);

            var messageElement = wait.Until(drv => drv.FindElement(By.XPath("//input[@name='s' and @required]")));

            // Verify results
            Assert.Multiple(() =>
            {
                Assert.That(messageElement.GetAttribute("validationMessage"), Is.EqualTo("Please fill out this field."), "The expected validation message is not displayed.");
            });

            Console.WriteLine("Validation Message: " + messageElement.GetAttribute("validationMessage"));
        }


        [Test]
        public void VerifySearchResultsForAlbanianLang()
        {
            // Select Albanian language
            driver.FindElement(By.LinkText("SQ")).Click();

            // Locate and hover over the "Accessories" main menu item
            var accessoriesMenuItem = driver.FindElement(By.XPath("//*[@id=\"menu-item-80500\"]/a/span"));
            action.MoveToElement(accessoriesMenuItem).Perform();

            // Click on the "Accessories" submenu
            var aksesore = wait.Until(drv => drv.FindElement(By.XPath("//*[@id=\"menu-item-80524\"]")));
            aksesore.Click();

            // Step 1: Open and view each item using absolute XPaths
            var itemXpaths = new List<(string XPath, string SearchWord)>
            {
            ("/html/body/div[1]/div[1]/div/div/div/div[3]/div[2]/div[4]/div/div[3]/h3/a", "Jastëk dyshemeje Shaila Zi 60x60x8cm"),
            ("/html/body/div[1]/div[1]/div/div/div/div[3]/div[2]/div[12]/div/div[3]/h3/a", "Rakun e jastëkut"),
            ("/html/body/div[1]/div[1]/div/div/div/div[3]/div[2]/div[13]/div/div[3]/h3/a", "Kufi jastëk")
            };


            // Base category URL for going back
            string categoryUrl = "https://smartliving.mk/sq/product-category/accessories/pillows/";

            // Open and view each item
            foreach (var (XPath, SearchWord) in itemXpaths)
            {
                // Locate the item by absolute XPath
                var itemLink = wait.Until(drv => drv.FindElement(By.XPath(XPath)));

                // Scroll to the item before clicking
                ((IJavaScriptExecutor)driver).ExecuteScript("var elem = arguments[0]; var rect = elem.getBoundingClientRect(); window.scrollTo({ top: rect.top + window.scrollY - (window.innerHeight / 2) + (rect.height / 2), behavior: 'smooth' });", itemLink);
                Thread.Sleep(2000);
                itemLink.Click();

                // Go back to the category page after viewing
                driver.Navigate().GoToUrl(categoryUrl);
            }

            // Step 2: Verify search bar functionality
            foreach (var (_, SearchWord) in itemXpaths)
            {
                // Perform the search for the item
                var searchInput = wait.Until(drv => drv.FindElement(By.Name("s")));
                searchInput.Clear();
                searchInput.SendKeys(SearchWord + Keys.Enter);

                // Wait for search results to load
                Thread.Sleep(2000);

                // Check for "no results" message
                var noResultsMessage = driver.FindElements(By.XPath("/html/body/div[1]/div[1]/div[2]/div/div/div[3]/div"));
                if (noResultsMessage.Count > 0 && noResultsMessage[0].Text.StartsWith("Nuk ka produkte që përputhet me përzgjedhjen tuaj"))
                {
                    Console.WriteLine($"No results found for: '{SearchWord}'. Message: '{noResultsMessage[0].Text}'");
                }
                else
                {
                    // If no message, try clicking the first displayed item
                    var firstDisplayedItem = driver.FindElement(By.XPath("/html/body/div[1]/div[1]/div[2]/div/div/div[3]/div[2]/div[1]/div/div[3]/h3/a"));
                    string displayedItemText = firstDisplayedItem.Text.Trim();
                    firstDisplayedItem.Click();

                    // Compare the displayed item text with the search term
                    if (displayedItemText.Equals(SearchWord.Trim(), StringComparison.OrdinalIgnoreCase))
                    {
                        Console.WriteLine($"Search successful: '{SearchWord}' matches the displayed item: '{displayedItemText}'.");
                    }
                    else
                    {
                        Console.WriteLine($"Search failed: '{SearchWord}' does not match the displayed item: '{displayedItemText}'.");
                    }

                    // Go back to the search results
                    driver.Navigate().Back();
                }
            }
        }


        [Test]
        public void VerifySearchResultsForEnglishLang()
        {
            // Select English language
            driver.FindElement(By.LinkText("EN")).Click();

            // Collection of search terms
            List<string> searchWords = new()
            {
            "Okkia Sunglasses Alessia Optical White",
            "Okkia Sunglasses Tokyo Optical",
            "Bench Tibor Walnut Black",
            "Bookcase Zoi White"
            };

            foreach (string word in searchWords)
            {
                // Perform the search for the item
                var searchInput = wait.Until(drv => drv.FindElement(By.Name("s")));
                searchInput.Clear();
                searchInput.SendKeys(word + Keys.Enter);
                Console.WriteLine($"Searched for: {word}");

                try
                {
                    // Wait for the products container to appear
                    var productsContainer = wait.Until(drv => drv.FindElement(By.XPath("//div[contains(@class, 'container')]")));
                    //Console.WriteLine(productsContainer.GetAttribute("outerHTML")); // Log HTML for verification

                    // Locate displayed product items within the products container
                    var displayedItems = productsContainer.FindElements(By.XPath("/html/body/div[1]/div[1]/div[2]/div/div/div[3]/div[2]/div[1]/div/div[3]/h3/a"));


                    // Create a list of displayed item texts
                    var displayedItemsTexts = displayedItems.Select(item => item.Text.Trim()).ToList();

                    // Check if any of the displayed items match the search term
                    bool itemFound = displayedItemsTexts.Any(text => text.Contains(word, StringComparison.OrdinalIgnoreCase));

                    if (itemFound)
                    {
                        Console.WriteLine($"Search successful: '{word}' - Item is displayed.");
                        foreach (var text in displayedItemsTexts.Where(text => text.Contains(word, StringComparison.OrdinalIgnoreCase)))
                        {
                            Console.WriteLine($"Item found: {text}");
                        }
                    }
                    else
                    {
                        Console.WriteLine($"Search failed: '{word}' - No items displayed that match the search term.");
                    }
                }
                catch (NoSuchElementException ex)
                {
                    Console.WriteLine($"Element not found: {ex.Message}");
                }
            }
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

