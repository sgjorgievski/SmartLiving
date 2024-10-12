using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using static System.Collections.Specialized.BitVector32;
using static System.Net.WebRequestMethods;

namespace SmartLivingShopWave.Tests
{
    public class FooterLinksAccessibilityTest
    {
        private ChromeDriver driver;
        private WebDriverWait wait;
        private readonly string screenshotDirectory = @"C:\Users\Pc\source\repos\Final Project\SmartLiving ShopFushion\Screenshoot";

        [OneTimeSetUp]
        public void BeforeAllTest()
        {
            driver = new ChromeDriver();
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
   
        }

        [SetUp]

        public void SetUp()
        {
            driver.Navigate().GoToUrl("https://smartliving.mk/mk/");
            driver.Manage().Window.Maximize();
            driver.FindElement(By.Id("cookie_action_close_header")).Click();
        }


        [Test]
        public void VerifyPlaceholderRegistration()
        {
            try
            {
                // Wait for the footer to be present
                IWebElement footer = wait.Until(d => d.FindElement(By.XPath("//footer")));

                // Wait for the placeholder registration section to be present and visible
                IWebElement placeholderSection = wait.Until(d => footer.FindElement(By.XPath("//*[@id='pre-foot-news']/div/h5/div/form")));
                Assert.That(placeholderSection.Displayed, Is.True, "Placeholder section is not displayed");

                // Locate the email input and registration button
                IWebElement emailInput = placeholderSection.FindElement(By.XPath(".//input[@type='email']"));
                IWebElement registrationButton = placeholderSection.FindElement(By.XPath(".//input[@type='submit']"));
                Thread.Sleep(3000);
                Assert.Multiple(() =>
                {
                    // Ensure email input and submit button are visible
                    Assert.That(emailInput.Displayed, Is.True, "Email input is not displayed");
                    Assert.That(registrationButton.Displayed, Is.True, "Submit button is not displayed");
                });

                // Enter an email and submit
                string testEmail = "nikolovski984@gmail.com";
                emailInput.SendKeys(testEmail);
                registrationButton.Click();
                Thread.Sleep(3000);
                // Take a screenshot
                if (driver is ITakesScreenshot screenshotDriver)
                {
                    Screenshot screenshot = screenshotDriver.GetScreenshot();
                    string screenshotFileName = $"Placeholder_Registration_{DateTime.Now:yyyy-MM-dd_HH-mm-ss}.png";
                    string screenshotPath = Path.Combine(screenshotDirectory, screenshotFileName);
                    screenshot.SaveAsFile(screenshotPath);
                }
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

        [Test]
        public void VerifySocialMediaIcons()
        {

            // Wait for the footer to be present
            IWebElement footer = wait.Until(d => d.FindElement(By.TagName("footer")));

            // Locate the social media section within the footer
            IWebElement socialMediaSection = wait.Until(d => footer.FindElement(By.XPath("/html/body/div[1]/div[2]/div/div[1]/div[4]/div/div/div[2]")));

            // Use JavaScript Executor to scroll to the social media section
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView({ behavior: 'smooth', block: 'center', inline: 'center' });", socialMediaSection);

            // Wait for the social media section to be visible
            wait.Until(d => socialMediaSection.Displayed);
            Assert.That(socialMediaSection.Displayed, Is.True, "Social media section is not displayed.");
            Thread.Sleep(3000);

            // Dictionary for the social media icons and their expected URLs
            var expectedIcons = new Dictionary<string, List<string>>
            {
            { "Facebook",  new List<string> { "https://www.facebook.com/SmartLivingSkopje/" } },
            { "Instagram", new List<string> { "https://www.instagram.com/smartliving/" } },
            { "Pinterest", new List<string> { "https://www.pinterest.com/smartliving74/", "https://www.pinterest.com/superblockstudio/" } }
        };
            foreach (var icon in expectedIcons)
            {
                try
                {
                    // Locate the icon by href
                    string iconXpath = $"//a[contains(@href, '{icon.Value.First()}')]";
                    IWebElement iconElement = wait.Until(d => socialMediaSection.FindElement(By.XPath(iconXpath)));

                    // Ensure the icon is displayed and clickable
                    wait.Until(d => iconElement.Displayed && iconElement.Enabled);

                    // Highlight the icon for visual feedback
                    ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].style.border='3px solid red'", iconElement);

                    // Log the href being clicked
                    Console.WriteLine($"Attempting to click {icon.Key} icon with href: {iconElement.GetAttribute("href")}");

                    // Click the icon
                    iconElement.Click();
                    Thread.Sleep(3000);

                    // Handle the new tab
                    if (driver.WindowHandles.Count > 1)
                    {
                        // Store the original tab handle
                        string originalTabHandle = driver.CurrentWindowHandle;

                        // Switch to the new tab
                        string newTabHandle = driver.WindowHandles.Last();
                        driver.SwitchTo().Window(newTabHandle);
                        Thread.Sleep(3000);

                        // Validate the actual URL
                        string actualUrl = driver.Url;
                        if (!icon.Value.Contains(actualUrl))
                        {
                            Console.WriteLine($"Discrepancy detected for {icon.Key}: Expected one of {string.Join(", ", icon.Value)}, but opened {actualUrl}");
                            Assert.Fail($"The URL for {icon.Key} did not match any expected URL.");
                        }

                        // Log success
                        Console.WriteLine($"{icon.Key} icon was clicked and navigated to the correct page.");

                        // Close the new tab/window
                        driver.Close();
                        Thread.Sleep(3000);

                        // Switch back to the original tab
                        driver.SwitchTo().Window(originalTabHandle);
                    }
                    else
                    {
                        // If no new tab opened, just log the information
                        Console.WriteLine($"No new tab was opened for {icon.Key}. This might require additional handling based on the application behavior.");
                    }

                    // Wait for the footer to be visible again
                    footer = wait.Until(d => d.FindElement(By.TagName("footer")));
                    socialMediaSection = wait.Until(d => footer.FindElement(By.XPath("/html/body/div[1]/div[2]/div/div[1]/div[4]/div/div/div[2]")));

                    // Ensure the social media section is displayed after navigating back
                    Assert.That(socialMediaSection.Displayed, Is.True, "Social media section is not displayed after navigating back.");

                    // Remove highlight from the icon
                    ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].style.border=''", iconElement);
                }
                catch (NoSuchElementException)
                {
                    // Log error if the icon is not found
                    Console.WriteLine($"Icon for {icon.Key} not found.");
                    Assert.Fail($"Icon for {icon.Key} not found.");
                }
                catch (WebDriverTimeoutException)
                {
                    // Log timeout error if the icon or page transition fails
                    Console.WriteLine($"Timeout occurred while interacting with {icon.Key} icon.");
                    Assert.Fail($"Failed to interact with {icon.Key} icon within the timeout period.");
                }
                catch (Exception ex)
                {
                    // Log any other errors that occur
                    Console.WriteLine($"Error interacting with {icon.Key} icon: {ex.Message}");
                    Assert.Fail($"Failed to interact with {icon.Key} icon.");
                }
            }
        }


        [Test]
        public void VerifyTheFooterLinks()
        {
            // Wait for the footer to be present
            IWebElement footer = wait.Until(d => d.FindElement(By.TagName("footer")));

            // Locate the footer links section within the footer
            IWebElement footerLinksSection = wait.Until(d => footer.FindElement(By.XPath("/html/body/div[1]/footer/div[1]")));

            // Use JavaScript Executor to scroll to the footer links section
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView({ behavior: 'smooth', block: 'center', inline: 'center' });", footerLinksSection);

            // Wait for the footer links section to be visible
            wait.Until(d => footerLinksSection.Displayed);
            Assert.That(footerLinksSection.Displayed, Is.True, "Footer links section is not displayed.");
            Thread.Sleep(3000); //  to ensure visibility

            // Find all links within the footer links section
            IReadOnlyCollection<IWebElement> footerLinks = footerLinksSection.FindElements(By.TagName("a"));

            foreach (var link in footerLinks)
            {
                try
                {
                    // Check if the link has a non-empty href attribute
                    string href = link.GetAttribute("href");
                    if (string.IsNullOrEmpty(href))
                    {
                        Console.WriteLine("Link with empty href found");
                        continue; // Skip empty hrefs
                    }

                    // Highlight the link (optional for visual debugging)
                    ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].style.border='3px solid red'", link);

                    // Click the link and verify it navigates to the expected page
                    string currentUrl = driver.Url;
                    link.Click();

                    // Wait for the new page to load
                    wait.Until(d => !driver.Url.Equals(currentUrl));

                    // Log the actual URL for debugging
                    string actualUrl = driver.Url;
                    Console.WriteLine($"Navigated to: {actualUrl}");

                    // Check if the URL contains the expected href (handle potential redirection)
                    if (actualUrl.Contains(href))
                    {
                        Console.WriteLine($"Successfully navigated to the expected URL: {actualUrl}");
                    }
                    else
                    {
                        Console.WriteLine($"Navigation did not lead to the correct URL: Expected '{href}', but was '{actualUrl}'");
                    }

                    // Close the new tab/window if opened
                    if (driver.WindowHandles.Count > 1)
                    {
                        // Store the original tab handle
                        string originalTabHandle = driver.CurrentWindowHandle;

                        // Switch to the new tab
                        string newTabHandle = driver.WindowHandles.Last();
                        driver.SwitchTo().Window(newTabHandle);
                        driver.Close(); // Close the new tab/window

                        // Switch back to the original tab
                        driver.SwitchTo().Window(originalTabHandle);
                    }

                    // Navigate back to the main page
                    driver.Navigate().Back();

                    // Re-locate the footer and links after navigation to handle StaleElementReferenceException
                    footer = wait.Until(d => d.FindElement(By.TagName("footer")));
                    footerLinksSection = wait.Until(d => footer.FindElement(By.XPath("/html/body/div[1]/footer/div[1]")));
                    footerLinks = footerLinksSection.FindElements(By.TagName("a"));
                }
                catch (StaleElementReferenceException)
                {
                    // Log error if the element becomes stale
                    Console.WriteLine("Stale element reference. The element is no longer in the DOM.");

                    // Re-locate the footer and links section to recover from stale element
                    footer = wait.Until(d => d.FindElement(By.TagName("footer")));
                    footerLinksSection = wait.Until(d => footer.FindElement(By.XPath("/html/body/div[1]/footer/div[1]")));
                    footerLinks = footerLinksSection.FindElements(By.TagName("a"));
                }
                catch (Exception ex)
                {
                    // Special handling for Google Maps links
                    string href = link.GetAttribute("href");
                    if (href != null && (href.Contains("maps.google.com") || href.Contains("map.app.goo.gl")))
                    {
                        Console.WriteLine($"Error interacting with Google Maps link: {ex.Message}");
                    }
                    else
                    {
                        Console.WriteLine($"Error interacting with the link: {ex.Message}");
                    }
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
