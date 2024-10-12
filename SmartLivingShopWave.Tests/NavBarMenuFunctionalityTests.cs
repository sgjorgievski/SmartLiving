using NUnit.Framework;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using NUnit.Framework.Internal;


namespace SmartLivingShopWave.Tests
{
    public class NavBarMenuFunctionalityTests
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
        public void TestNavbarAccessoriesMenuAndSubmenu()
        {
            // Locate the "Accessories" main menu item
            var accessoriesMenuItem = driver.FindElement(By.XPath("//*[@id=\"menu-item-80500\"]/a/span"));

            // Hover over the "Accessories" menu item to reveal the submenu
            var actions = new Actions(driver);
            actions.MoveToElement(accessoriesMenuItem).Perform();

            // Define the CSS selector for submenu items
            By submenuItemsCSS = By.CssSelector("#menu-item-80500 > div ul li a");

            // Wait for and get the submenu items
            IList<IWebElement> submenuItems = wait.Until(drv => drv.FindElements(submenuItemsCSS));

            // Check if submenu items are found
            if (submenuItems.Count == 0)
            {
                Console.WriteLine("No submenu items found.");
                return; // Exit the test if no submenu items are found
            }

            foreach (var submenuItem in submenuItems)
            {
                // Print if submenu item is displayed
                Console.WriteLine($"Submenu item '{submenuItem.Text}' is displayed");

                // Click on the submenu item
                submenuItem.Click();

                // Wait to observe the result of the click
                Thread.Sleep(2000);

                // Navigate back to the main page
                driver.Navigate().Back();

                // Wait for the main page to reload and re-hover over the main menu item
                wait.Until(drv => drv.FindElement(By.XPath("//*[@id=\"menu-item-80500\"]/a/span")).Displayed);
                actions.MoveToElement(accessoriesMenuItem).Perform();
                Thread.Sleep(1000); // Wait for submenu to be visible again

                // Re-locate the submenu items after navigating back
                submenuItems = wait.Until(drv => drv.FindElements(submenuItemsCSS));
            }
        }

        [Test]
        public void TestNavbarEyewearMenuAndSubmenu()
        {
            // Locate the "EyewearMenu" main menu item
            var eyewearMenuItem = driver.FindElement(By.XPath("//*[@id=\"menu-item-81333\"]/a/span"));

            // Hover over the "EyewearMenu" menu item to reveal the submenu
            var actions = new Actions(driver);
            actions.MoveToElement(eyewearMenuItem).Perform();

            // Define the CSS selector for submenu items
            By submenuItemsCSS = By.CssSelector("#menu-item-81333 > div ul li a");

            // Wait for and get the submenu items
            IList<IWebElement> submenuItems = wait.Until(drv => drv.FindElements(submenuItemsCSS));

            // Check if submenu items are found
            if (submenuItems.Count == 0)
            {
                Console.WriteLine("No submenu items found.");
                return; // Exit the test if no submenu items are found
            }

            foreach (var submenuItem in submenuItems)
            {
                // Print if submenu item is displayed
                Console.WriteLine($"Submenu item '{submenuItem.Text}' is displayed");

                // Click on the submenu item
                submenuItem.Click();

                // Wait to observe the result of the click
                Thread.Sleep(2000);

                // Navigate back to the main page
                driver.Navigate().Back();

                // Wait for the main page to reload and re-hover over the main menu item
                wait.Until(drv => drv.FindElement(By.XPath("//*[@id=\"menu-item-80500\"]/a/span")).Displayed);
                actions.MoveToElement(eyewearMenuItem).Perform();
                Thread.Sleep(1000); // Wait for submenu to be visible again

                // Re-locate the submenu items after navigating back
                submenuItems = wait.Until(drv => drv.FindElements(submenuItemsCSS));
            }
        }

        [Test]
        public void TestNavbarLightingsMenuAndSubmenu()
        {
            // Locate the "Lightings" main menu item
            var lightingsMenuItem = driver.FindElement(By.XPath("//*[@id=\"menu-item-81216\"]/a/span"));

            // Hover over the "Lightings" menu item to reveal the submenu
            var actions = new Actions(driver);
            actions.MoveToElement(lightingsMenuItem).Perform();

            // Define the CSS selector for submenu items
            By submenuItemsCSS = By.CssSelector("#menu-item-81216 > div ul li a");

            // Wait for and get the submenu items
            IList<IWebElement> submenuItems = wait.Until(drv => drv.FindElements(submenuItemsCSS));

            // Check if submenu items are found
            if (submenuItems.Count == 0)
            {
                Console.WriteLine("No submenu items found.");
                return; // Exit the test if no submenu items are found
            }

            foreach (var submenuItem in submenuItems)
            {
                // Print if submenu item is displayed
                Console.WriteLine($"Submenu item '{submenuItem.Text}' is displayed");

                // Click on the submenu item
                submenuItem.Click();

                // Wait to observe the result of the click
                Thread.Sleep(2000);

                // Navigate back to the main page
                driver.Navigate().Back();

                // Wait for the main page to reload and re-hover over the main menu item
                wait.Until(drv => drv.FindElement(By.XPath("//*[@id=\"menu-item-80500\"]/a/span")).Displayed);
                actions.MoveToElement(lightingsMenuItem).Perform();
                Thread.Sleep(1000); // Wait for submenu to be visible again

                // Re-locate the submenu items after navigating back
                submenuItems = wait.Until(drv => drv.FindElements(submenuItemsCSS));
            }
        }

        [Test]
        public void TestNavbarTablewareMenuAndSubmenu()
        {
            // Locate the "Tableware" main menu item
            var tablewareMenuItem = driver.FindElement(By.XPath("//*[@id=\"menu-item-80528\"]/a/span"));

            // Hover over the "Tableware" menu item to reveal the submenu
            var actions = new Actions(driver);
            actions.MoveToElement(tablewareMenuItem).Perform();

            // Define the CSS selector for submenu items
            By submenuItemsCSS = By.CssSelector("#menu-item-80528 > div ul li a");

            // Wait for and get the submenu items
            IList<IWebElement> submenuItems = wait.Until(drv => drv.FindElements(submenuItemsCSS));

            // Check if submenu items are found
            if (submenuItems.Count == 0)
            {
                Console.WriteLine("No submenu items found.");
                return; // Exit the test if no submenu items are found
            }

            foreach (var submenuItem in submenuItems)
            {
                // Print if submenu item is displayed
                Console.WriteLine($"Submenu item '{submenuItem.Text}' is displayed");

                // Click on the submenu item
                submenuItem.Click();

                // Wait to observe the result of the click
                Thread.Sleep(2000);

                // Navigate back to the main page
                driver.Navigate().Back();

                // Wait for the main page to reload and re-hover over the main menu item
                wait.Until(drv => drv.FindElement(By.XPath("//*[@id=\"menu-item-80528\"]/a/span")).Displayed);
                actions.MoveToElement(tablewareMenuItem).Perform();
                Thread.Sleep(1000); // Wait for submenu to be visible again

                // Re-locate the submenu items after navigating back
                submenuItems = wait.Until(drv => drv.FindElements(submenuItemsCSS));
            }
        }


        [Test]

        public void TestCarpetMenuItemsInteraction()
        {

            // Locate and click the Carpet menu item
            IWebElement carpetMenu = wait.Until(drv => drv.FindElement(By.XPath("//*[@id=\"menu-item-80525\"]/a/span")));
            carpetMenu.Click();

            // Locate the dropdown element
            var dropdownElement = wait.Until(drv => drv.FindElement(By.Name("orderby")));

            // Click on the dropdown to open it
            dropdownElement.Click();

            // Create a SelectElement instance
            var selectElement = new SelectElement(dropdownElement);

            // Select the option by visible text
            selectElement.SelectByText("Сортирај по просечна оценка");

            // Wait for the dropdown to process the selection 
            Thread.Sleep(3000);

            // Locate and click the CarpetNora item
            IWebElement carpetNora = wait.Until(drv => drv.FindElement(By.XPath("/html/body/div[1]/div[1]/div/div/div/div[3]/div[2]/div[2]/div/div[3]/h3/a")));
            carpetNora.Click();

        }

        [Test]

        public void ArtObjectMenuInteraction()
        {
            // Hover over ArtObject Menu
            var artHoverMenu = wait.Until(drv => drv.FindElement(By.XPath("//*[@id='menu-item-81235']/a")));
            action.MoveToElement(artHoverMenu).Perform();

            // Select submenu
            var sculptureSubMenu = wait.Until(drv => drv.FindElement(By.XPath("//*[@id='menu-item-81237']/a")));
            sculptureSubMenu.Click();

            // Select max displayed product on page
            var productsPerPage = wait.Until(drv => drv.FindElement(By.XPath("/html/body/div[1]/div[1]/div/div/div/div[3]/div[2]/div[2]/a[4]/span")));
            productsPerPage.Click();
            Thread.Sleep(3000);

            // Scroll to and click on the elephant item
            var elephantItem = wait.Until(drv => drv.FindElement(By.LinkText("KARE Deco Bowl Happy Elephant 20x18cm")));

            // JavaScript to scroll to the element with an offset
            ((IJavaScriptExecutor)driver).ExecuteScript(@"
             var element = arguments[0];
             var offset = arguments[1]; // Offset in pixels
             var elementPosition = element.getBoundingClientRect();
             var scrollPosition = window.pageYOffset || document.documentElement.scrollTop;
             window.scrollTo({
             top: elementPosition.top + scrollPosition - offset,
             behavior: 'smooth'
             });
             ", elephantItem,20);
            elephantItem.Click();
            Thread.Sleep(4000);

            // Select the first image
            var firstImage = wait.Until(drv => drv.FindElement(By.XPath("//*[@id=\"product-78755\"]/div[1]/div[2]/div/div/div[1]/div/div[1]/div/figure/div/div[1]/figure/a/img")));
            firstImage.Click();

            // Navigate through images
            var nextClick = wait.Until(drv => drv.FindElement(By.XPath("/html/body/div[15]/div[2]/div[2]/button[2]")));
            for (int i = 0; i < 5; i++)
            {
                nextClick.Click();
                Thread.Sleep(2000); // Wait for image change
            }

            // Close the image gallery
            var closeGallery = wait.Until(drv => drv.FindElement(By.XPath("/html/body/div[15]/div[2]/div[2]/div[1]/button[1]")));
            closeGallery.Click();
        }

        [Test]

        public void FurnitureMenuInteraction()
        {

            // List of items to interact with
            var menuItems = new List<(string mainMenuXPath, string submenuXPath, string secondLevelSubmenuXPath, string itemToClickXPath)>
            {
                (
                    "//*[@id='menu-item-81249']/a", // Main menu XPath
                    "//*[@id='menu-item-80491']/a", // Submenu XPath
                    "//*[@id='menu-item-80466']/a", // Second-level submenu XPath
                    "/html/body/div[1]/div[1]/div/div/div/div[4]/div[2]/div[2]/div/div[2]/div[1]/a/img" // Item to click XPath
                ),

                (
                    "//*[@id='menu-item-81249']/a", // Main menu XPath
                    "//*[@id='menu-item-80475']/a", // Submenu XPath
                    "//*[@id='menu-item-80490']/a", // Second-level submenu XPath
                    "/html/body/div[1]/div[1]/div/div/div/div[3]/div[2]/div[6]/div/div[3]/h3/a" // Item to click XPath
                ),


            };
            for (int i = 0; i < menuItems.Count; i++)
            {
                var (mainMenuXPath, submenuXPath, secondLevelSubmenuXPath, itemToClickXPath) = menuItems[i];

                try
                {
                    // Hover over the main menu
                    var mainMenu = wait.Until(drv => drv.FindElement(By.XPath(mainMenuXPath)));
                    action.MoveToElement(mainMenu).Perform();

                    // Hover over the submenu
                    var submenu = wait.Until(drv => drv.FindElement(By.XPath(submenuXPath)));
                    action.MoveToElement(submenu).Perform();
                    Thread.Sleep(2000); // Wait for submenu to be visible

                    // Click on the second-level submenu item
                    var secondLevelSubmenuItem = wait.Until(drv => drv.FindElement(By.XPath(secondLevelSubmenuXPath)));
                    secondLevelSubmenuItem.Click();
                    Thread.Sleep(2000);

                    // Click on a specific link
                    var itemToClick = wait.Until(drv => drv.FindElement(By.XPath(itemToClickXPath)));
                    itemToClick.Click();
                    Thread.Sleep(3000);

                    // Check if this is the last item
                    if (i == menuItems.Count - 1)
                    {
                        //Console.WriteLine($"Finished interaction with the last item: {itemToClickXPath}");
                        break; // Exit the loop after processing the last item
                    }

                    // navigate back and repeat
                    driver.Navigate().Back();
                    Thread.Sleep(2000);

                    // Navigate back to the main menu page
                    driver.Navigate().GoToUrl("https://smartliving.mk/mk/");
                    Thread.Sleep(2000);

                    // Re-hover over the main menu to re-select submenus
                    mainMenu = wait.Until(drv => drv.FindElement(By.XPath(mainMenuXPath)));
                    action.MoveToElement(mainMenu).Perform();
                    Thread.Sleep(2000);

                }
                catch (NoSuchElementException ex)
                {
                    Console.WriteLine($"Element not found: {ex.Message}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An error occurred: {ex.Message}");
                }
            }

            Console.WriteLine("All interactions completed.");
        }


        [Test]

        public void KidsMenuInteraction()
        {
            var action = new Actions(driver);

            // Locate and click the Kids Menu item
            var kidsMenu = wait.Until(drv => drv.FindElement(By.XPath("//*[@id=\"menu-item-81226\"]/a/span")));
            kidsMenu.Click();

            // Locate and click the Shop Grid View
            var shopGridView = wait.Until(drv => drv.FindElement(By.XPath("/html/body/div[1]/div[1]/div/div/div/div[2]/div[2]/div[3]/a[1]")));
            shopGridView.Click();
            Thread.Sleep(2000);

            // Hover over the first item
            var lampItem = wait.Until(drv => drv.FindElement(By.XPath("/html/body/div[1]/div[1]/div/div/div/div[3]/div[2]/div[2]/div/div[2]/div[1]/a/img")));
            action.MoveToElement(lampItem).Perform();
            Thread.Sleep(2000);

            // Hover over the second item
            var decoItem = wait.Until(drv => drv.FindElement(By.XPath("/html/body/div[1]/div[1]/div/div/div/div[3]/div[2]/div[4]/div/div[2]/div[1]/a/img")));
            action.MoveToElement(decoItem).Perform();
            Thread.Sleep(2000);

            // Scroll to and click on the angel item
            var angelItem = wait.Until(drv => drv.FindElement(By.LinkText("КАРЕ Деко Фигурина Ангел прасе 20см")));
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView({ behavior: 'smooth', block: 'center' });", angelItem);
            Thread.Sleep(3000);

            // Hover over the mirror item
            var rinoItem = wait.Until(drv => drv.FindElement(By.XPath("/html/body/div[1]/div[1]/div/div/div/div[3]/div[2]/div[16]/div/div[3]/h3/a")));
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView({ behavior: 'smooth', block: 'center' });", rinoItem);
            Thread.Sleep(3000);

            var uniItem = wait.Until(drv => drv.FindElement(By.XPath("/html/body/div[1]/div[1]/div/div/div/div[3]/div[2]/div[13]/div/div[3]/h3/a")));
            action.MoveToElement(decoItem).Perform();
            Thread.Sleep(2000);

            // Scroll to and click the load button for more images
            var loadButton = wait.Until(drv => drv.FindElement(By.XPath("/html/body/div[1]/div[1]/div/div/div/div[3]/div[3]/a")));
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView({ behavior: 'smooth', block: 'center' });", loadButton);
            try
            {
                loadButton.Click();
            }
            catch (ElementClickInterceptedException)
            {
                ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", loadButton);
            }
            Thread.Sleep(3000);

            // Scroll to and click on the bear item
            var bearItem = wait.Until(drv => drv.FindElement(By.XPath("/html/body/div[1]/div[1]/div/div/div/div[3]/div[2]/div[28]/div/div[3]/h3/a")));
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView({ behavior: 'smooth', block: 'center' });", bearItem, 20);

            // Click on the bear item
            try
            {
                bearItem.Click();
            }
            catch (ElementClickInterceptedException)
            {
                ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", bearItem, 20);
            }
        }

        [Test]

        public void VerifyPulloutFiltersFunctionalityfromNavMenu ()
        {
            // Locate the NavbarMenuNew, clcik and pullout filter need to be displayed
            var navMenuNew = driver.FindElement(By.XPath("//*[@id=\"menu-item-80608\"]/a/span"));
            navMenuNew.Click();

            // Wait for the filter panel to be visible
            var filterPanel = wait.Until(drv => drv.FindElement(By.XPath("//*[@id=\"post-80615\"]/div/div/div[1]/div/div/div"))); 
            Assert.That(filterPanel.Displayed, "Filter panel is not displayed after clicking the menu item.");
            Console.WriteLine(filterPanel.Text);
            Thread.Sleep(2000);

            // Interact with category filter 
            var categoryFilter = filterPanel.FindElement(By.CssSelector("form .title-text"));
           if (categoryFilter.Text.Trim() == "КАТЕГОРИИ")
            {
                // The text matches
                Console.WriteLine("Found the categoryFilter with text: " + categoryFilter.Text);

                //Java script for clicking

                Console.WriteLine("Attempting to click categoryFilter");
                //((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", categoryFilter);
                categoryFilter.Click();
                Thread.Sleep(3000);
            }
            else
            {
                // The text does not match
                Console.WriteLine("The text is different: " + categoryFilter.Text);
                return;//if text doesn't match
            }
           
            var categoryOption = wait.Until(drv => drv.FindElement(By.XPath("//article[@id='post-80615']/div/div/div/div/div/div/div/form/div/div[2]/div/ul/li[5]/ul/li[4]/ul/li[6]/a")));
            //((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", categoryOption);
            categoryOption.Click();
            Thread.Sleep(2000);

            // Interact with the filter by field 
            var filterByField = wait.Until(drv => drv.FindElement(By.XPath("//*[@id=\"post-80615\"]//form/div[2]/div[1]/span"))); 
            filterByField.Click();
            Thread.Sleep(2000);
            var filterByOption = wait.Until(drv => drv.FindElement(By.XPath("//*[@id=\"post-80615\"]//div/form//ul/li[1]/a/span[2]"))); 
            filterByOption.Click();
            Thread.Sleep(2000);

            // Interact with the filter for stock status 
            var stockStatusFilter = filterPanel.FindElement(By.XPath("//*[@id=\"post-80615\"]//form/div[3]/div[1]/span")); 
            stockStatusFilter.Click();
            var stockStatusOption = filterPanel.FindElement(By.XPath("//*[@id=\"post-80615\"]//form//div[3]//div[2]//ul/li[2]/a")); 
            stockStatusOption.Click();
            Thread.Sleep(2000);

            // Interact with the price filter 
            var priceFilter = filterPanel.FindElement(By.XPath("//*[@id=\"post-80615\"]//form//div[4]/div[1]/span"));
            priceFilter.Click();
            Thread.Sleep(2000);
            var priceFilterMin = filterPanel.FindElement(By.XPath("//*[@id=\"post-80615\"]//form//div[4]//div[2]//span[1]")); 
            var priceFilterMax = filterPanel.FindElement(By.XPath("//*[@id=\"post-80615\"]//form//div[4]//div[2]//div[1]//span[2]"));
            
            // Create an Actions object 
            Actions actions = new(driver);

            // Drag the minimum price slider to the desired value 
            actions.ClickAndHold(priceFilterMin)
                   .MoveByOffset(15, 0) 
                   .Release()
                   .Perform();

            // Wait for the filter to update
            Thread.Sleep(2000);

            // Drag the maximum price slider to the desired value 
            actions.ClickAndHold(priceFilterMax)
                   .MoveByOffset(-20, 0) 
                   .Release()
                   .Perform();

            // Wait for the filter to update
            Thread.Sleep(2000);

            // Interact with the filter for sort by
            var stockByFilter = filterPanel.FindElement(By.XPath("//*[@id=\"post-80615\"]//form//div[5]//div[1]//span"));
            stockByFilter.Click();
            var stockByOption = filterPanel.FindElement(By.XPath("//*[@id=\"post-80615\"]//form//div[5]//div[2]//ul/li[5]/a"));
            stockByOption.Click();
            Thread.Sleep(2000);

            // Click on the filter button to apply filters
            var filterButton = wait.Until(drv => drv.FindElement(By.XPath("//*[@id=\"post-80615\"]//form//div[6]//button"))); 
            filterButton.Click();

            // Verify dispalyed results
            var results = wait.Until(drv => drv.FindElement(By.XPath("//div[contains(@class, 'wd-products-element')]")));
            Assert.That(results.Displayed, "Results are not displayed after applying the filter.");
            Console.WriteLine("Results are displayed");
            Thread.Sleep(1000);

            //Clear filters
            var clearFilterButton = wait.Until(drv => drv.FindElement(By.XPath("/html/body/div[1]/div[1]/div/div/div/div[3]/div[1]/a")));
            clearFilterButton.Click();
 
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





