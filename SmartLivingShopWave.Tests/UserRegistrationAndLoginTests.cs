using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using System;
using System.Threading;

namespace SmartLivingShopWave.Tests
{
    public class UserRegistrationTests
    {
        private ChromeDriver driver;
        private WebDriverWait wait;
        


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

        public void TestUserRegistration()
        {

            // Navigate to the Registration Page
            driver.FindElement(By.XPath("/html/body/div[1]/header/div/div[1]/div/div/div[3]/div[1]/a/span[2]")).Click();
            wait.Until(d => d.FindElement(By.XPath("/html/body/div[5]/div[3]/a"))).Click();
            Thread.Sleep(2000);

            // Scroll the registration button into view using JavaScript
            IWebElement slider = driver.FindElement(By.XPath("/html"));
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(true);", slider);

            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].style.left = '70px';", slider);
            Thread.Sleep(3000);

            // Case 1: Register without entering email
            Console.WriteLine("Test Case 1: Register without entering email.");

            // Re-locate email field and register button
            IWebElement emailField = wait.Until(d => d.FindElement(By.XPath("//*[@id='reg_email']")));
            IWebElement registerButton = wait.Until(d => d.FindElement(By.XPath("//*[@id='customer_login']/div[2]/form/p[4]/button")));

            emailField.Clear(); // Ensure that email field is empty
            registerButton.Click();

            // Verify appropriate error message for empty email
            try
            {
                IWebElement errorMessage = driver.FindElement(By.XPath("//*[@id=\"post-9\"]/div/div/div[1]/ul"));
                if (errorMessage.Displayed)
                {
                    Console.WriteLine("грешка:Ве молиме наведете валидна адреса за е-пошта.");
                }
                else
                {
                    Console.WriteLine("Error message is not displayed for empty email.");
                }
            }
            catch (NoSuchElementException)
            {
                Console.WriteLine("Error message element not found for empty email.");
            }


            // Case 2: Register with a valid email
            Console.WriteLine("Test Case 2: Register with a valid email.");

            // Re-locate elements
            emailField = wait.Until(d => d.FindElement(By.XPath("//*[@id='reg_email']"))); // Re-locate the email field
            registerButton = wait.Until(d => d.FindElement(By.XPath("//*[@id='customer_login']/div[2]/form/p[4]/button"))); // Re-locate the register button
            Thread.Sleep(3000);

            string validEmail = "talevski81@gmail.com";
            emailField.SendKeys(validEmail);
            registerButton.Click();

            // Verify the success message for valid email registration
            try
            {
                IWebElement successMessage = wait.Until(d => d.FindElement(By.XPath("//*[@id=\"post-9\"]/div/div/div[1]/div")));
                if (successMessage.Displayed)
                {
                    Console.WriteLine("Ви благодариме за вашата регистрација. Вашата сметка мора да се активира пред да можете да се најавите. Ве молиме проверете ја вашата е-пошта.");
                }
                else
                {
                    Console.WriteLine("Registration was not successful. Success message is not displayed.");
                }
            }
            catch (NoSuchElementException)
            {
                Console.WriteLine("Success message element not found for valid email registration.");
            }
            catch (TimeoutException)
            {
                Console.WriteLine("Timeout while waiting for the success message.");
            }
        }


        [Test]

        public void TestUserLogin()

        {
            // Navigate to the Login Page
            driver.FindElement(By.XPath("/html/body/div[1]/header/div/div[1]/div/div/div[3]/div[1]/a/span[1]")).Click();

            //Perform Login
            IWebElement usernameField = driver.FindElement(By.Id("username"));
            IWebElement passwordField = driver.FindElement(By.Id("password"));
            IWebElement loginButton = driver.FindElement(By.XPath("/html/body/div[5]/form/p[3]/button"));

            // Input login details
            usernameField.SendKeys("nikolovskis984@gmail.com");
            passwordField.SendKeys("Newerashop#1");
            loginButton.Click();

            //Verify Login Success
            try
            {
                IWebElement userProfile = driver.FindElement(By.XPath("/html/body/div[1]/header/div/div[1]/div/div/div[3]/div[1]/a/span[2]"));
                if (userProfile.Displayed)
                {
                    Console.WriteLine("Login was successful.");
                }
                else
                {
                    Console.WriteLine("Login was not successful. User profile is not displayed.");
                }
            }
            catch (NoSuchElementException)
            {
                Console.WriteLine("Login was not successful. User profile element not found.");
            }
        }

        [Test]
        public void TestForgotPassword()
        {
            // Navigate to the Login Page
            driver.FindElement(By.XPath("/html/body/div[1]/header/div/div[1]/div/div/div[3]/div[1]/a/span[2]")).Click();
            Thread.Sleep(3000);

            // Locate and click the "Forgot Password" link
            IWebElement forgotPasswordLink = driver.FindElement(By.XPath("/html/body/div[5]/form/p[4]/a"));
            forgotPasswordLink.Click();

            // Wait for the Forgot Password page or modal to be visible
            WebDriverWait wait = new(driver, TimeSpan.FromSeconds(10));
            wait.Until(d => d.FindElement(By.XPath("//*[@id=\"post-9\"]/div/div/form")));

            // Input email address for password recovery
            IWebElement emailField = driver.FindElement(By.XPath("//*[@id=\"user_login\"]"));
            IWebElement submitButton = driver.FindElement(By.XPath("//*[@id=\"post-9\"]/div/div/form/p[3]/button"));

            // Enter email and click Submit
            string email = "nikolovskis984@gmail.com";
            emailField.SendKeys(email);
            submitButton.Click();

            // Verify the success message or response
            try
            {
                // Adjust the locator according to the actual success message or confirmation element
                IWebElement successMessage = driver.FindElement(By.XPath("//*[@id=\"post-9\"]/div/div/div"));
                if (successMessage.Displayed)
                {
                    Console.WriteLine("Испратена е е-пошта за ресетирање на лозинката.");
                }
                else
                {
                    Console.WriteLine("Password reset request was not successful.");
                }
            }
            catch (NoSuchElementException)
            {
                Console.WriteLine("Password reset request was not successful.");
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





