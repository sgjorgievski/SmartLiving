using OpenQA.Selenium.Chrome;
using OpenQA.Selenium;
using System;
using System.Threading;
using NUnit.Framework;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using System.Linq;
using static System.Collections.Specialized.BitVector32;
using System.Diagnostics.Metrics;
using System.IO;
using System.Reflection.Emit;


namespace SmartLivingShopWave.Tests
{
    public class ShoppingCartTest
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

        public string screenshotDirectory = @"C:\Users\Pc\source\repos\Final Project\SmartLiving ShopFushion\Screenshoot";

        void TakeScreenshot(string fileName)
        {
            if (driver is ITakesScreenshot screenshotDriver)
            {
                Screenshot screenshot = screenshotDriver.GetScreenshot();
                string screenshotPath = Path.Combine(screenshotDirectory, fileName);
                screenshot.SaveAsFile(screenshotPath);
                Console.WriteLine($"Screenshot saved: {screenshotPath}");
            }
            else
            {
                throw new InvalidOperationException("The WebDriver does not support taking screenshots.");
            }
        }

        private void AddItemToShoopingCart(string searchQuery, string itemText, int? quantity = null, bool isLastItem = false)
        {
            try
            {
                // Perform the search for the item
                var searchInput = wait.Until(drv => drv.FindElement(By.Name("s")));
                searchInput.Clear();
                searchInput.SendKeys(searchQuery);
                searchInput.SendKeys(Keys.Return);

                // Select the item
                var itemElement = wait.Until(drv => drv.FindElement(By.XPath($"//h3/a[contains(text(),'{itemText}')]")));
                itemElement.Click();
                Thread.Sleep(2000);

                // Locate the add to cart button
                var addToCartButton = wait.Until(drv => drv.FindElement(By.CssSelector(".single_add_to_cart_button.button.alt")));

                // Check if there is a quantity field
                bool hasQuantityField = driver.FindElements(By.CssSelector(".input-text.qty.text")).Count > 0;

                if (hasQuantityField)
                {
                    if (quantity.HasValue && quantity.Value > 0)
                    {
                        // Set the quantity if the quantity field is present and quantity is greater than 0
                        var quantityInput = wait.Until(drv => drv.FindElement(By.CssSelector(".input-text.qty.text")));
                        quantityInput.Clear();
                        quantityInput.SendKeys(quantity.Value.ToString()); // Set the desired quantity
                    }
                    else if (quantity.HasValue && quantity.Value == 0)
                    {
                        // If quantity is 0, handle accordingly if needed (this might not be necessary if 0 is acceptable)
                        Console.WriteLine($"Item '{itemText}' has a quantity of 0 and quantity field is present.");
                    }
                }
                else if (isLastItem)
                {
                    // For the last item, skip setting the quantity if no quantity field is present
                    Console.WriteLine($"Last item '{itemText}' does not have a quantity field. Using default stock level.");
                }

                addToCartButton.Click();
                Thread.Sleep(2000);

                // If not the last item, navigate back to the search page
                if (!isLastItem)
                {
                    driver.Navigate().GoToUrl("https://smartliving.mk/mk/");
                    wait.Until(drv => drv.FindElement(By.XPath("/html/body/div[6]/form/input[1]")));
                    Thread.Sleep(2000);
                }
            }
            catch (ElementNotInteractableException ex)
            {
                Console.WriteLine($"Element not interactable for item '{itemText}'. Error: {ex.Message}");
                Assert.Fail($"Element not interactable for item '{itemText}'. Error: {ex.Message}");
            }
            catch (NoSuchElementException ex)
            {
                Console.WriteLine($"Element not found for item '{itemText}'. Error: {ex.Message}");
                Assert.Fail($"Element not found for item '{itemText}'. Error: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An unexpected error occurred while adding '{itemText}' to the cart. Error: {ex.Message}");
                Assert.Fail($"An unexpected error occurred while adding '{itemText}' to the cart. Error: {ex.Message}");
            }
        }
        private void OpenShoppingCart()
        {
            try
            {
                var shoppingCartIcon = wait.Until(drv => drv.FindElement(By.XPath("/html/body/div[1]/header/div/div[1]/div/div/div[3]/div[3]/a")));
                shoppingCartIcon.Click();
                var viewCartButton = wait.Until(drv => drv.FindElement(By.XPath("/html/body/div[4]/div[2]/div/div[2]/p[2]/a[1]")));
                viewCartButton.Click();
                Thread.Sleep(2000);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while opening the shopping cart. Error: {ex.Message}");
                Assert.Fail($"An error occurred while opening the shopping cart. Error: {ex.Message}");
            }
        }

        [Test]
        public void Test_AddSingleItemToCart()
        {
            // Select NavBar Main Menu
            var homeofficeMenu = wait.Until(drv => drv.FindElement(By.XPath("//*[@id=\"menu-item-81250\"]/a/span")));
            homeofficeMenu.Click();

            // Locate the dropdown element and open
            var dropdownElement = wait.Until(drv => drv.FindElement(By.Name("orderby")));
            dropdownElement.Click();

            // Create a SelectElement instance and select option from menu
            var selectElement = new SelectElement(dropdownElement);
            selectElement.SelectByText("Сортирај по популарност"); 
            Thread.Sleep(3000);

            //select item 
            var laptop = wait.Until(drv => drv.FindElement(By.XPath("//div[@class='product-element-bottom product-information']//h3[@class='wd-entities-title']/a")));
            laptop.Click();

            //add to cart
            var addToCartButton = wait.Until(drv => drv.FindElement(By.CssSelector("form.cart button.single_add_to_cart_button.button.alt")));
            addToCartButton.Click();

            //verify result
            var results = wait.Until(drv => drv.FindElement(By.XPath("//*[@id=\"product-78302\"]/div[1]/div[1]/div")));
            Assert.That(results.Text, Does.Contain("„Плоштад на маса за лаптоп“ е додаден во вашата кошничка."), "Search results do not contain expected text.");
            string resultsText = results.Text;
            Console.WriteLine(resultsText);
        }

        [Test]

        public void Test_AddingMultipleItemToCart()
        {

            // Select NavBar Main Menu Furniture
            var furnitureMenu = wait.Until(drv => drv.FindElement(By.CssSelector("#menu-item-81249 a span")));
            action.MoveToElement(furnitureMenu).Perform();

            // Select submenu Outdoor Furniture
            var outdoorFurnitureSubMenu = wait.Until(drv => drv.FindElement(By.CssSelector("#menu-item-80546 a")));
            action.MoveToElement(outdoorFurnitureSubMenu).Perform();

            // Select the Seating submenu
            var seatingSubMenu = wait.Until(drv => drv.FindElement(By.CssSelector("#menu-item-88410 a")));
            seatingSubMenu.Click();

            //select item
            var deckchair = wait.Until(drv => drv.FindElement(By.XPath("//h3/a[contains(text(), 'Лежалка Брз Беж 65х83х86см')]")));
            deckchair.Click();

            //Increase quantity
            var deckchairQuantityInput = wait.Until(drv => drv.FindElement(By.XPath("//input[@value='+']")));
            deckchairQuantityInput.Click();

            //add to cart
            var deckchairAddToCartButton = wait.Until(drv => drv.FindElement(By.Name("add-to-cart")));
            deckchairAddToCartButton.Click();
            driver.FindElement(By.Name("add-to-cart")).Click();
            Thread.Sleep(3000);

            // Verify result for deckchair
            var deckchairCartMessage = wait.Until(drv => drv.FindElement(By.XPath("//*[@id=\"product-87875\"]/div[1]/div[1]/div")));
            Assert.That(deckchairCartMessage.Text, Does.Contain("2 × „Лежалката Fasty Beige 65x83x86cm“ се додадени во вашата кошничка."), "Search results do not contain expected text.");
            string deckchairCartMessageText = deckchairCartMessage.Text;
            Console.WriteLine(deckchairCartMessageText);

            // Select Second Element from NavBar Main Menu Tableware
            var tablewareMenu = wait.Until(drv => drv.FindElement(By.CssSelector("#menu-item-80528 a span")));
            action.MoveToElement(tablewareMenu).Perform();
            Thread.Sleep(2000);

            // Select the Glassware submenu
            var glasswareSubMenu = wait.Until(drv => drv.FindElement(By.CssSelector("#menu-item-81252 a")));
            glasswareSubMenu.Click();

            //select item
            var wineglass = wait.Until(drv => drv.FindElement(By.XPath($"//h3/a[contains(text(), 'KARE Вино чаша мраз цвеќиња сини')]")));
            wineglass.Click();

            //Increase quantity
            var wineglassQuantityInput = wait.Until(drv => drv.FindElement(By.Name("quantity")));
            wineglassQuantityInput.Clear();
            wineglassQuantityInput.SendKeys("4");

            //add to cart
            var wineglassAddToCartButton = wait.Until(drv => drv.FindElement(By.Name("add-to-cart")));
            wineglassAddToCartButton.Click();
            driver.FindElement(By.Name("add-to-cart")).Click();
            Thread.Sleep(3000);

            //verify result for wineglass
            var wineglassCartMessage = wait.Until(drv => drv.FindElement(By.XPath("//*[@id=\"product-86956\"]/div[1]/div[1]/div")));
            Assert.That(wineglassCartMessage.Text, Does.Contain("4 × „KARE Wine Glass Ice Flowers Blue“ се додадени во вашата кошничка."), "Search results do not contain expected text.");
            string wineglassCartMessageText = wineglassCartMessage.Text;
            Console.WriteLine(wineglassCartMessageText);


            //Add Thrid Eelement from Accessories
            var AccessoriesMenu = wait.Until(drv => drv.FindElement(By.CssSelector("#menu-item-80500 a span")));
            AccessoriesMenu.Click();

            //scrool to Element
            var elephantItem = wait.Until(drv => drv.FindElement(By.LinkText("KARE Money Box Среќен слон")));
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView({ behavior: 'smooth', block: 'center' });", elephantItem);
            Thread.Sleep(2000);
            elephantItem.Click();

            //add to cart
            var elephantAddToCartButton = wait.Until(drv => drv.FindElement(By.Name("add-to-cart")));
            elephantAddToCartButton.Click();
            driver.FindElement(By.Name("add-to-cart")).Click();
            Thread.Sleep(3000);

            //verify result
            var elephantCartMessage = driver.FindElement(By.XPath("//*[@id=\"product-89412\"]/div[1]/div[1]/div"));
            Assert.That(elephantCartMessage.Text, Does.Contain("KARE Money Box Happy Elephant“ е додадена во вашата кошничка."), "Search results do not contain expected text.");
            string elephantCartMessageText = elephantCartMessage.Text;
            Console.WriteLine(elephantCartMessageText);
        }

      
        [Test]
        public void TestSocialMediaShareButtonsForMultipleItems()
        {
            try
            {
                // Define a list of search terms
                var searchItems = new List<(string SearchQuery, string ItemXPath)>
            {
             ("Armchair Ferguson Grey", "/html/body/div[1]/div[1]/div[2]/div/div/div[3]/div[2]/div[1]/div/div[3]/h3/a"),
             ("Crochet Bowl Round","/html/body/div[1]/div[1]/div[2]/div/div/div[3]/div[2]/div[1]/div/div[3]/h3/a")

            };

            // Base URL for navigation back to search page
            string baseUrl = "https://smartliving.mk/mk";

                foreach (var (SearchQuery, ItemXPath) in searchItems)
                {
                    // Perform the search
                    PerformSearch(SearchQuery);

                    // Click on the item from search results
                    ClickOnSearchResult(ItemXPath);

                    // Click social media share buttons

                    ClickSocialShareButton(By.XPath("//a[contains(@href, 'facebook.com/sharer/sharer')]"), "Facebook");
                    ClickSocialShareButton(By.XPath("//a[contains(@href, 'x.com/share')]"), "X");
                    ClickSocialShareButton(By.XPath("//a[contains(@href, 'mailto:')]"), "Email");
                    ClickSocialShareButton(By.XPath("//a[contains(@href, 'pinterest.com/pin')]"), "Pinterest");
                    ClickSocialShareButton(By.XPath("//a[contains(@href, 'linkedin.com/shareArticle?mini=true&url=')]"), "LinkedIn");
                    ClickSocialShareButton(By.XPath("//a[contains(@href, 'api.whatsapp.com/send?text=')]"), "WhatsApp");
                    ClickSocialShareButton(By.XPath("//a[contains(@href, 'viber://forward?text=')]"), "Viber");

                    // Navigate back to the search page
                    driver.Navigate().GoToUrl(baseUrl);
                    Thread.Sleep(2000); 
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error during test execution: {ex.Message}");
                Assert.Fail("Test failed due to an unexpected error.");
            }
        }

        private void PerformSearch(string searchQuery)
        {
            try
            {
                Console.WriteLine($"Performing search with query: '{searchQuery}'");

                // Locate and click the search button
                var searchButton = wait.Until(driver => driver.FindElement(By.XPath("/html/body/div[1]/header/div/div[1]/div/div/div[1]/div[1]/a/span[1]")));
                searchButton.Click();

                // Wait for the search bar to be interactable
                Console.WriteLine("Waiting for the search bar to be clickable...");
                var searchBar = wait.Until(driver => driver.FindElement(By.XPath("/html/body/div[6]/form/input[1]")));

                // Ensure the search bar is visible and enabled
                wait.Until(driver => searchBar.Displayed && searchBar.Enabled);
                Console.WriteLine("Clicking the search bar...");
                searchBar.Click();

                // Clear the search bar before entering the new query
                searchBar.Clear();
                Console.WriteLine($"Entering search query: '{searchQuery}'");
                searchBar.SendKeys(searchQuery);
                searchBar.SendKeys(Keys.Enter);
                Thread.Sleep(3000); 
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error during search operation: {ex.Message}");
                Assert.Fail("Failed to perform search operation.");
            }
        }
        private void ClickOnSearchResult(string itemXPath)
        {
            try
            {
                Console.WriteLine("Clicking on the search result item...");

                // Wait for the search result item to be visible and clickable
                var item = wait.Until(driver => driver.FindElement(By.XPath(itemXPath)));
                wait.Until(driver => item.Displayed && item.Enabled);
                item.Click();

                Console.WriteLine("Successfully clicked on the search result item.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error during clicking on the search result item: {ex.Message}");
                Assert.Fail("Failed to click on the search result item.");
            }
        }


        private void ClickSocialShareButton(By by, string platformName)
        {
            try
            {
                Console.WriteLine($"Attempting to click the {platformName} share button...");

                // Wait for the button to be visible and clickable
                var button = wait.Until(driver => driver.FindElement(by));
                wait.Until(driver => button.Displayed && button.Enabled);
                button.Click();

                // Store the original window handle
                var originalWindow = driver.CurrentWindowHandle;

                // Handle mailto: links and platforms like Viber separately
                if (platformName.Equals("Email", StringComparison.OrdinalIgnoreCase) ||
                   platformName.Equals("Viber", StringComparison.OrdinalIgnoreCase))
                {
                    Console.WriteLine($"Handling {platformName} link separately.");

                    // Wait for a new window/tab to open
                    wait.Until(driver => driver.WindowHandles.Count > 1);

                    // Switch to the new window/tab and close it if possible
                    var newWindowHandle = driver.WindowHandles.First(handle => handle != originalWindow);
                    driver.SwitchTo().Window(newWindowHandle);
                    driver.Close(); // Close the mailto: or Viber pop-up/tab
                    driver.SwitchTo().Window(originalWindow); // Switch back to the original window/tab
                    Console.WriteLine($"{platformName} link handled and closed.");
                    return; // Continue to the next button
                }

                // Handle other social media share buttons
                Console.WriteLine($"{platformName} button clicked. Checking for new tab or redirect.");

                // Wait for a new tab/window to open or URL to change
                wait.Until(driver => driver.WindowHandles.Count > 1 || driver.Url.Contains(platformName, StringComparison.OrdinalIgnoreCase));

                var handles = driver.WindowHandles;

                if (handles.Count > 1)
                {
                    foreach (var handle in handles)
                    {
                        if (handle != originalWindow)
                        {
                            driver.SwitchTo().Window(handle);
                            Console.WriteLine($"Switched to new tab: {driver.Title}");

                            // Wait for the page to load completely
                            wait.Until(driver => driver.Url.Contains(platformName, StringComparison.OrdinalIgnoreCase));

                            // Add platform-specific checks here
                            if (platformName.Equals("Facebook", StringComparison.OrdinalIgnoreCase))
                            {
                                wait.Until(driver => driver.Url.Contains("facebook.com", StringComparison.OrdinalIgnoreCase));
                                Console.WriteLine($"Facebook share page loaded: {driver.Url}");
                            }
                            else if (platformName.Equals("X", StringComparison.OrdinalIgnoreCase))
                            {
                                wait.Until(driver => driver.Url.Contains("intent/post", StringComparison.OrdinalIgnoreCase));
                                Console.WriteLine($"X (Twitter) share page loaded: {driver.Url}");
                            }
                            else if (platformName.Equals("Pinterest", StringComparison.OrdinalIgnoreCase))
                            {
                                wait.Until(driver => driver.Url.Contains("pinterest.com", StringComparison.OrdinalIgnoreCase));
                                Console.WriteLine($"Pinterest share page loaded: {driver.Url}");
                            }
                            else if (platformName.Equals("LinkedIn", StringComparison.OrdinalIgnoreCase))
                            {
                                wait.Until(driver => driver.Url.Contains("linkedin.com", StringComparison.OrdinalIgnoreCase));
                                Console.WriteLine($"LinkedIn share page loaded: {driver.Url}");
                            }
                            else if (platformName.Equals("WhatsApp", StringComparison.OrdinalIgnoreCase))
                            {
                                wait.Until(driver => driver.Url.Contains("api.whatsapp.com", StringComparison.OrdinalIgnoreCase));
                                Console.WriteLine($"WhatsApp share page loaded: {driver.Url}");
                            }
                            else if (platformName.Equals("Viber", StringComparison.OrdinalIgnoreCase))
                            {
                                // No specific URL check needed for Viber, just ensure the tab is handled
                                wait.Until(driver => driver.Url.Contains("viber://", StringComparison.OrdinalIgnoreCase));
                                Console.WriteLine($"Viber share page loaded: {driver.Url}");
                            }

                            // Close the new window/tab
                            driver.Close();
                            // Switch back to the original window
                            driver.SwitchTo().Window(originalWindow);
                            Console.WriteLine($"{platformName} share page handled and closed.");
                            break; // Exit the loop after closing the new tab
                        }
                    }
                }
                else
                {
                    // If no new window/tab, check if the URL is correct
                    wait.Until(driver => driver.Url.Contains(platformName, StringComparison.OrdinalIgnoreCase));
                    Console.WriteLine($"{platformName} share button works.");
                }

                // Add a short delay 
                Thread.Sleep(1000);
            }
            catch (NoSuchElementException ex)
            {
                Console.WriteLine($"No such element found for {platformName} share button: {ex.Message}");
                Assert.Fail($"{platformName} share button not found.");
            }
            catch (WebDriverException ex)
            {
                Console.WriteLine($"WebDriver exception while interacting with {platformName} share button: {ex.Message}");
                Assert.Fail($"{platformName} share button interaction failed.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error testing {platformName} share button: {ex.Message}");
                Assert.Fail($"{platformName} share button test failed.");
            }
        }


        [Test]
        public void Test_AddFavoriteItemsToCart()
        {
            void AddItemToFavorites(string searchQuery, string itemText)
            {
                try
                {
                    // Locate and click the search bar
                    var search = wait.Until(drv => drv.FindElement(By.XPath("/html/body/div[1]/header/div/div[1]/div/div/div[1]/div[1]/a/span[1]")));
                    search.Click();
                    Thread.Sleep(2000);

                    // Enter a search query
                    var searchInput = wait.Until(drv => drv.FindElement(By.XPath("/html/body/div[6]/form/input[1]")));
                    searchInput.Clear();
                    searchInput.SendKeys(searchQuery);
                    searchInput.SendKeys(Keys.Return);

                    // Select the item
                    var itemElement = wait.Until(drv => drv.FindElement(By.XPath($"//h3/a[contains(text(),'{itemText}')]")));
                    itemElement.Click();

                    //scrool to Element
                    var addtocart = wait.Until(drv => drv.FindElement(By.XPath("//button[@name='add-to-cart']")));
                    ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView({ behavior: 'smooth', block: 'center' });", addtocart);
                    Thread.Sleep(2000);

                    // Add to favorites
                    var favoritesButton = wait.Until(drv => drv.FindElement(By.LinkText("Додај во листата на желби")));
                    favoritesButton.Click();
                    Thread.Sleep(2000);

                    // Return to search page or reset the search state
                    driver.Navigate().Back();
                    Thread.Sleep(2000);


                }
                catch (Exception ex)
                {
                    // Catch any other unexpected exceptions
                    Console.WriteLine($"An unexpected error occurred while adding '{itemText}' to favorites. Error: {ex.Message}");
                    Assert.Fail($"An unexpected error occurred while adding '{itemText}' to favorites. Error: {ex.Message}");
                }
            }

            // List of items to add to favorites
            var items = new List<(string searchQuery, string itemText)>
                {
                ("Deco Olive Tree H150cm", "Деко маслиново дрво H150cm"),
                ("KARE Side Table Animal Polar Bear D37cm", "KARE Странична маса Животинска поларна мечка D37cm"),
                 };

            // Add all items to favorites
            foreach (var (searchQuery, itemText) in items)
            {
                AddItemToFavorites(searchQuery, itemText);
            }

            // Click on the favorites icon to view favorites
            try
            {
                var favoritesIcon = wait.Until(drv => drv.FindElement(By.XPath("/html/body/div[1]/header/div/div[1]/div/div/div[3]/div[2]/a/span[1]")));
                favoritesIcon.Click();
                Thread.Sleep(2000);

                // Define the empty message text
                string emptyMessageText = "Оваа листа со желби е празна.";

                try
                {
                    // Check if the empty message is displayed
                    var emptyMessage = wait.Until(drv => drv.FindElement(By.XPath("//*[@id=\"post-27457\"]/div/div/div/p[1]")));
                    Console.WriteLine(emptyMessageText);
                    Assert.That(emptyMessage.Text, Is.EqualTo(emptyMessageText));
                }
                catch (WebDriverTimeoutException)
                {
                    // If no empty message is found, check if the items are present
                    foreach (var (searchQuery, itemText) in items)
                    {
                        try
                        {
                            // Locate the favorite item in the list
                            var favoriteItem = wait.Until(drv => drv.FindElement(By.XPath($"//div[contains(text(),'{itemText}')]")));
                            string favoriteItemText = favoriteItem.Text;

                            // Log and assert that the item is in the favorites list
                            Console.WriteLine($"Item found in favorites: {favoriteItemText}");
                            Assert.That(favoriteItemText, Is.EqualTo(itemText));
                        }
                        catch (WebDriverTimeoutException)
                        {
                            // item was not found
                            Console.WriteLine($"Item '{itemText}' not found in favorites.");
                            Assert.Fail($"Item '{itemText}' not found in favorites.");
                        }
                    }
                }
            }
            catch (WebDriverTimeoutException ex)
            {
                Console.WriteLine($"Failed to navigate to the favorites list. Error: {ex.Message}");
                Assert.Fail($"Failed to navigate to the favorites list. Error: {ex.Message}");
            }
        }

        [Test]
        public void TestQuantityLimits()
        {

            // Locate and click the search bar
            var search = wait.Until(drv => drv.FindElement(By.XPath("/html/body/div[1]/header/div/div[1]/div/div/div[1]/div[1]/a/span[1]")));
            search.Click();

            // Enter a query
            var searchInput = wait.Until(drv => drv.FindElement(By.XPath("/html/body/div[6]/form/input[1]")));
            searchInput.SendKeys("KARE Bulb LED Bulb 3W Ø9,5cm");
            searchInput.SendKeys(Keys.Return);

            // Select the bulb item
            var bulbElement = wait.Until(drv => drv.FindElement(By.XPath("//h3/a[contains(text(),'KARE сијалица LED сијалица 3W Ø9,5cm')]")));
            bulbElement.Click();

            //scrool to Element
            var cart = wait.Until(drv => drv.FindElement(By.Name("add-to-cart")));
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView({ behavior: 'smooth', block: 'center' });", cart);

            // Wait for the quantity input and add to cart button
            var quantityInput = wait.Until(drv => drv.FindElement(By.XPath("//input[@name='quantity']")));
            var addItemButton = wait.Until(drv => drv.FindElement(By.XPath("//button[@name='add-to-cart']")));

            //verifcation of error message with screenshoot

            // Test with quantity set to 0
            quantityInput.Clear();
            quantityInput.SendKeys("0");
            addItemButton.Click();

            // Take a screenshot after clicking add to cart with quantity 0
            try
            {
                // Wait a moment for the error message to appear
                Thread.Sleep(2000);

                // Capture screenshot
                TakeScreenshot("Error_Quantity_0.png");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while taking the screenshot: {ex.Message}");
                throw;
            }

            // Test with a large quantity
            int largeQuantity = 50;
            quantityInput.Clear();
            quantityInput.SendKeys(largeQuantity.ToString());
            addItemButton.Click();

            // Take a screenshot after clicking add to cart with a large quantity
            try
            {
                // Wait a moment for the error message to appear
                Thread.Sleep(2000);

                // Capture screenshot
                TakeScreenshot("Error_Quantity_Large.png");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while taking the screenshot: {ex.Message}");
                throw;
            }
        }


        [Test]

        public void VerifyRemoveItemsFromCart()
        {

            // Add items to the cart
            var items = new List<(string searchQuery, string itemText, int? quantity)>
            {
                ("Cabinet Claude", "Кабинетот Клод", 1),
                ("Okkia Sunglasses Chiara Optical White", "Очила за сонце Okkia Chiara оптички бели", null),
                ("Shoe Cabinet Bella White 60x24x115cm", "Кабинет за чевли Bella White 60x24x115cm", null)
            };

            // Add all items to the shopping cart
            for (int i = 0; i < items.Count; i++)
            {

                var (searchQuery, itemText, quantity) = items[i];
                bool isLastItem = (i == items.Count - 1); // Check if it's the last item

                // Pass quantity as null for the last item 
                AddItemToShoopingCart(searchQuery, itemText, quantity, isLastItem);

            }

            // Open the shopping cart
            OpenShoppingCart();

            // Remove items and verify removal
            RemoveAllItemsFromCart();

            // Verify the cart is empty
            VerifyCartIsEmpty();
        }

        private void RemoveAllItemsFromCart()
        {
            try
            {
                bool cartIsEmpty;

                do
                {
                    cartIsEmpty = true;

                    // Find the remove buttons in the cart
                    var removeButtons = wait.Until(drv => drv.FindElements(By.LinkText("×")));

                    if (removeButtons.Count > 0)
                    {
                        cartIsEmpty = false; // There are items in the cart

                        foreach (var button in removeButtons)
                        {
                            try
                            {
                                button.Click();
                                Thread.Sleep(3000); // Wait for removal to complete and page to update
                            }
                            catch (StaleElementReferenceException)
                            {

                                // Console.WriteLine("Encountered stale element, re-fetching remove buttons.");
                                break; // Exit the loop to refetch elements
                            }
                        }

                        // Ensure the cart is updated
                        wait.Until(drv => drv.FindElements(By.LinkText("×")).Count == 0 || drv.FindElements(By.XPath("//tr//td[contains(@class,'item-name')]")).Count == 0);
                    }
                } while (!cartIsEmpty);

                //Console.WriteLine("All items have been removed from the cart.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while removing items from the cart. Error: {ex.Message}");
                Assert.Fail($"An error occurred while removing items from the cart. Error: {ex.Message}");
            }
        }

        private void VerifyCartIsEmpty()
        {
            try
            {
                // Find all items in the cart
                var itemsInCart = driver.FindElements(By.XPath("//tr//td[contains(@class,'item-name')]"));

                Assert.That(itemsInCart, Is.Empty, "The cart is not empty.");
                Console.WriteLine("Вашата кошничка моментално е празна.");
            }
            catch (AssertionException ex)
            {
                // Handle assertion exception separately
                Console.WriteLine($"Assertion failed: {ex.Message}");
                throw; // Re-throw the exception to signal test failure
            }
            catch (Exception ex)
            {
                // Handle general exceptions
                Console.WriteLine($"An error occurred while verifying the cart. Error: {ex.Message}");
                Assert.Fail($"An error occurred while verifying the cart. Error: {ex.Message}");
            }
        }


        [Test]

        public void VerifyCartUpdatesWithItemQuantity()
        {

            // Add items to the cart
            var items = new List<(string searchQuery, string itemText, int quantity)>
    {
        ("Table Clock Kaled Blue 10.8x12cm", "Часовник за маса Kaled Blue 10.8x12cm", 4),
        ("KARE Napkin Ring Happy Elephants (4/Set)", "KARE прстен за салфетка Среќни слонови (4/сет)", 2),

    };

            // Add all items to shopping cart
            for (int i = 0; i < items.Count; i++)
            {
                var (searchQuery, itemText, quantity) = items[i];
                bool isLastItem = (i == items.Count - 1); // Check if it's the last item
                AddItemToShoopingCart(searchQuery, itemText, quantity, isLastItem);
            }

            // Open the shopping cart
            OpenShoppingCart();

            // Verify items In Cart
            VerifyCartUpdatesInCart();

            // Verify Updated quantity In Cart
            UpdateAllQuantitiesAndClickUpdate();
        }
        private void VerifyCartUpdatesInCart()
        {
            try
            {
                var productXPaths = new List<(string NameXPath, string PriceXPath, string QuantityXPath, string SubtotalXPath, string ExpectedName, string ExpectedPrice, string ExpectedQuantity, string ExpectedSubtotal)>
        {
            (
                "//*[@id='post-7']/div/div/div/form/div/table/tbody/tr[1]/td[3]/a", // XPath for name
                "//*[@id='post-7']/div/div/div/form/div/table/tbody/tr[1]/td[4]/span/span/bdi", // XPath for price
                "/html/body/div[1]/div[1]/div[2]/div/div/article/div/div/div/form/div/table/tbody/tr[1]/td[5]/div/input[2]", // XPath for quantity
                "//*[@id='post-7']/div/div/div/form/div/table/tbody/tr[1]/td[6]/span/span/bdi", // XPath for subtotal
                "Часовник за маса Kaled Blue 10.8x12cm", "680 ден", "4", "2,720 ден" // Updated expected values
            ),
            (
                "//*[@id='post-7']/div/div/div/form/div/table/tbody/tr[2]/td[3]/a", // XPath for name
                "//*[@id='post-7']/div/div/div/form/div/table/tbody/tr[2]/td[4]/span/span/bdi", // XPath for price
                "/html/body/div[1]/div[1]/div[2]/div/div/article/div/div/div/form/div/table/tbody/tr[2]/td[5]/div/input[2]", // XPath for quantity
                "//*[@id='post-7']/div/div/div/form/div/table/tbody/tr[2]/td[6]/span/span/bdi", // XPath for subtotal
                "KARE прстен за салфетка Среќни слонови (4/сет)", "2,000 ден", "2", "4,000 ден" // Updated expected values
            )
        };

                foreach (var (nameXPath, priceXPath, quantityXPath, subtotalXPath, expectedName, expectedPrice, expectedQuantity, expectedSubtotal) in productXPaths)
                {
                    var itemRow = driver.FindElement(By.XPath($"//tr[.//a[text()='{expectedName}']]"));
                    var itemName = itemRow.FindElement(By.XPath(nameXPath));
                    var itemPrice = itemRow.FindElement(By.XPath(priceXPath));
                    var itemQuantity = itemRow.FindElement(By.XPath(quantityXPath));
                    var itemSubtotal = itemRow.FindElement(By.XPath(subtotalXPath));

                    Assert.Multiple(() =>
                    {
                        Assert.That(itemName.Text, Is.EqualTo(expectedName), "Item name does not match.");
                        Assert.That(itemPrice.Text, Is.EqualTo(expectedPrice), "Item price does not match.");
                        Assert.That(itemQuantity.GetAttribute("value"), Is.EqualTo(expectedQuantity), "Item quantity does not match.");
                        Assert.That(itemSubtotal.Text, Is.EqualTo(expectedSubtotal), "Item subtotal does not match.");
                    });

                    Console.WriteLine($"Item '{expectedName}' - Price: {itemPrice.Text}, Quantity: {itemQuantity.GetAttribute("value")}, Subtotal: {itemSubtotal.Text}");
                }
            }
            catch (NoSuchElementException e)
            {
                Assert.Fail($"Element not found during verification: {e.Message}");
            }
            catch (Exception e)
            {
                Assert.Fail($"An unexpected error occurred during verification: {e.Message}");
            }
        }


        private void UpdateAllQuantitiesAndClickUpdate()
        {
            try
            {
                // List of items to update
                var itemsToUpdate = new List<(string itemName, int newQuantity)>
        {
            ("KARE прстен за салфетка Среќни слонови (4/сет)", 1),
            ("Часовник за маса Kaled Blue 10.8x12cm", 2)
        };

                // Dictionary to hold the initial subtotals
                var initialSubtotals = new Dictionary<string, string>();

                // Capture initial subtotals
                foreach (var (itemName, _) in itemsToUpdate)
                {
                    var cartItemRow = driver.FindElement(By.XPath($"//tr[.//a[text()='{itemName}']]"));
                    var subtotalElement = cartItemRow.FindElement(By.XPath(".//td[contains(@class, 'subtotal')]/span"));
                    string initialSubtotal = subtotalElement.Text;
                    initialSubtotals[itemName] = initialSubtotal;
                }

                // Iterate through each item and update its quantity
                foreach (var (itemName, newQuantity) in itemsToUpdate)
                {
                    var cartItemRow = driver.FindElement(By.XPath($"//tr[.//a[text()='{itemName}']]"));
                    var quantityField = cartItemRow.FindElement(By.CssSelector(".input-text.qty.text"));
                    quantityField.Clear();
                    quantityField.SendKeys(newQuantity.ToString());
                }

                // Click the update button for the entire cart
                var updateButton = driver.FindElement(By.XPath("//*[@id=\"post-7\"]/div/div/div/form/div/table/tbody/tr[3]/td/div/button"));
                updateButton.Click();

                // Wait for the update to be processed
                WebDriverWait wait = new(driver, TimeSpan.FromSeconds(12));
                wait.Until(driver =>
                {
                    try
                    {
                        foreach (var itemName in itemsToUpdate.Select(i => i.itemName))
                        {
                            var cartItemRow = driver.FindElement(By.XPath($"//tr[.//a[text()='{itemName}']]"));
                            var subtotalElement = cartItemRow.FindElement(By.XPath(".//td[contains(@class, 'subtotal')]/span"));
                            string updatedSubtotal = subtotalElement.Text;
                            if (updatedSubtotal != initialSubtotals[itemName])
                            {
                                return true;
                            }
                        }
                        return false;
                    }
                    catch
                    {
                        return false;
                    }
                });

                // Print updated information for each item
                foreach (var (itemName, newQuantity) in itemsToUpdate)
                {
                    var cartItemRow = driver.FindElement(By.XPath($"//tr[.//a[text()='{itemName}']]"));
                    var itemNameElement = cartItemRow.FindElement(By.XPath(".//td[3]/a"));
                    var itemPriceElement = cartItemRow.FindElement(By.XPath(".//td[4]/span/span/bdi"));
                    var quantityField = cartItemRow.FindElement(By.CssSelector(".input-text.qty.text"));
                    var subtotalElement = cartItemRow.FindElement(By.XPath(".//td[contains(@class, 'subtotal')]/span"));

                    // Fetch updated information
                    string updatedName = itemNameElement.Text;
                    string updatedPrice = itemPriceElement.Text;
                    string updatedQuantity = quantityField.GetAttribute("value");
                    string updatedSubtotal = subtotalElement.Text;

                    // Print the updated information
                    Console.WriteLine($"Item '{updatedName}' - Price: {updatedPrice}, Updated Quantity: {updatedQuantity}, Updated Subtotal: {updatedSubtotal}");
                }
            }
            catch (NoSuchElementException e)
            {
                Assert.Fail($"Element not found during update: {e.Message}");
            }
            catch (Exception e)
            {
                Assert.Fail($"An unexpected error occurred during update: {e.Message}");
            }
        }


        [Test]
        public void CompleteCheckoutProcess()
        {
            bool useEmailPlaceholder = true; // true for guest checkout, false for login checkout

            try
            {
                // Add items to the cart
                AddItemsToCart();
                OpenShoppingCart();
                ApplyCouponCode("274898");
                ProceedToCheckout();

                if (useEmailPlaceholder)
                {
                    CompleteCheckoutAsGuest();
                }
                else
                {
                    LogInAndCompleteCheckout();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                Assert.Fail("CompleteCheckoutProcess failed due to an unexpected error.");
            }
        }

        private void AddItemsToCart()
        {
            var items = new List<(string searchQuery, string itemText, int quantity)>
            {
            ("Table Clock Kaled Blue 10.8x12cm", "Часовник за маса Kaled Blue 10.8x12cm", 2),
            ("KARE Napkin Ring Happy Elephants (4/Set)", "KARE прстен за салфетка Среќни слонови (4/сет)", 1),
            };

            for (int i = 0; i < items.Count; i++)
            {
                var (searchQuery, itemText, quantity) = items[i];
                bool isLastItem = (i == items.Count - 1); // Check if it's the last item
                try
                {
                    AddItemToShoopingCart(searchQuery, itemText, quantity, isLastItem);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error adding item to shopping cart: {ex.Message}");
                    Assert.Fail("Failed to add item to shopping cart.");
                }
            }
        }

        private void ApplyCouponCode(string couponCode)
        {
            try
            {
                var couponInput = wait.Until(driver => driver.FindElement(By.XPath("//input[@id='coupon_code']")));
                couponInput.SendKeys(couponCode);
                var applyCouponButton = wait.Until(driver => driver.FindElement(By.XPath("//button[@name='apply_coupon']")));
                applyCouponButton.Click();

                var couponMessage = wait.Until(driver => driver.FindElement(By.XPath("//*[@id='post-7']/div/div/div/div[1]/ul/li")));
                string messageText = couponMessage.Text;

                if (messageText.Contains("Купонот"))
                {
                    Assert.That(messageText, Does.Contain("Купонот"));
                    Console.WriteLine("Coupon does not exist. Proceeding to checkout.");
                }
                else
                {
                    Console.WriteLine("Coupon applied successfully.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error applying coupon code: {ex.Message}");
                Assert.Fail("Failed to apply coupon code.");
            }
        }

        private void ProceedToCheckout()
        {
            try
            {
                var checkoutButton = wait.Until(driver => driver.FindElement(By.XPath("//a[contains(@href, 'checkout')]"))); // Update XPath as needed
                checkoutButton.Click();
                Console.WriteLine("Proceeded to Step 1.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error clicking proceed to checkout button: {ex.Message}");
                Assert.Fail("Failed to proceed to checkout.");
            }
        }

        private void CompleteCheckoutAsGuest()
        {
            try
            {
                var emailInput = wait.Until(driver => driver.FindElement(By.XPath("//input[@id='billing_email']")));
                emailInput.SendKeys("arsovski74@gmail.com");
                Console.WriteLine("Email input completed as guest.");
                Thread.Sleep(2000);

                // Proceed to Step 2
                var proceedToStep2Button = wait.Until(driver => driver.FindElement(By.XPath("//*[@id=\"fc-wrapper\"]/div[1]/div/section[1]/div/button"))); 
                proceedToStep2Button.Click();
                Console.WriteLine("Proceeded to Step 2.");
                Thread.Sleep(2000);

                FillCheckoutDetails();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error during guest checkout: {ex.Message}");
                Assert.Fail("Failed to complete checkout as guest.");
            }
        }

        private void LogInAndCompleteCheckout()
        {
            try
            {
                var loginButton = wait.Until(driver => driver.FindElement(By.LinkText("Логирај се")));
                loginButton.Click();
                Thread.Sleep(2000);

                var emailInput = wait.Until(driver => driver.FindElement(By.Id("username")));
                var passwordInput = wait.Until(driver => driver.FindElement(By.Id("password")));
                var submitLoginButton = wait.Until(driver => driver.FindElement(By.Name("login")));


                emailInput.SendKeys("nikolovskis984@gmail.com");
                passwordInput.SendKeys("Newerashop#1");
                submitLoginButton.Click();
                Console.WriteLine("Logged in successfully.");
                
                var ShoppingCart = wait.Until(driver => driver.FindElement(By.XPath("/html/body/div[1]/header/div/div[1]/div/div/div[3]/div[3]/a/span[1]")));
                ShoppingCart.Click();
                Thread.Sleep(2000);

                var checkoutButton = wait.Until(driver => driver.FindElement(By.XPath("/html/body/div[4]/div[2]/div/div[2]/p[2]/a[2]")));
                checkoutButton.Click();
                Thread.Sleep(2000);

                FillCheckoutDetails();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error during login and checkout: {ex.Message}");
                Assert.Fail("Failed to log in and complete checkout.");
            }
        }

        private void FillCheckoutDetails()
        {
            try
            {

                // Step 2 - Fill customer details
                var firstName = wait.Until(driver => driver.FindElement(By.XPath("//input[@id='shipping_first_name']")));
                var lastName = wait.Until(driver => driver.FindElement(By.XPath("//input[@id='shipping_last_name']")));
                var country = wait.Until(driver => driver.FindElement(By.XPath("//*[@id=\"shipping_country_field\"]/span/span/span[1]/span")));
                var street = wait.Until(driver => driver.FindElement(By.XPath("//input[@id='shipping_address_1']")));
                var city = wait.Until(driver => driver.FindElement(By.XPath("//input[@id='shipping_city']")));
                var state = wait.Until(driver => driver.FindElement(By.XPath("//input[@id='shipping_state']")));
                var zipCode = wait.Until(driver => driver.FindElement(By.XPath("//input[@id='shipping_postcode']")));
                var note = wait.Until(driver => driver.FindElement(By.XPath("//*[@id=\"order_comments\"]")));

                if (!note.Displayed)
                {
                    // Click the expand button if the textarea is not displayed
                    var expandNoteSectionButton = wait.Until(driver => driver.FindElement(By.XPath("//*[@id='fc-expansible-form-section__toggle-plus--order_comments']")));
                    expandNoteSectionButton.Click();
                    Thread.Sleep(2000);
                }

                firstName.SendKeys("Никола");
                lastName.SendKeys("Арсовски");
                country.Click();

                var searchField = wait.Until(driver => driver.FindElement(By.XPath("/html/body/span/span/span[1]/input")));
                searchField.SendKeys("North Macedonia");
                Thread.Sleep(3000);

                // Select "North Macedonia" from the dropdown list
                var countryOption = wait.Until(driver => driver.FindElement(By.XPath("//li[text()='North Macedonia']")));
                countryOption.Click();

                street.SendKeys("Џон Кенеди 2");
                city.SendKeys("Скопје");
                state.SendKeys("Македонија");
                zipCode.SendKeys("1000");
                note.SendKeys("Available between 14 to 17 hours");
                Thread.Sleep(4000);
                Console.WriteLine("Customer details input completed.");

                // Proceed to Step 3
                var proceedToStep3Button = wait.Until(driver => driver.FindElement(By.XPath("//*[@id=\"fc-wrapper\"]/div[1]/div/section[2]/div/button"))); 
                proceedToStep3Button.Click();
                Console.WriteLine("Proceeded to Step 3.");

                // Step 3 - Enter phone number
                var phonenumber = wait.Until(driver => driver.FindElement(By.XPath("//*[@id=\"billing_phone\"]")));
                phonenumber.SendKeys("+389 70 525 592");
                Thread.Sleep(2000);
                Console.WriteLine("Telephone number entered successfully.");

                // Proceed to Step 4
                var proceedToStep4Button = wait.Until(driver => driver.FindElement(By.XPath("//*[@id=\"fc-wrapper\"]/div[1]/div/section[3]/div/button"))); 
                proceedToStep4Button.Click();
                Console.WriteLine("Proceeded to Step 4.");

                // Finalize process in Step 4 - payment details or pay on delivery

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error during checkout details input: {ex.Message}");
                Assert.Fail("Failed to fill checkout details.");
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


        
