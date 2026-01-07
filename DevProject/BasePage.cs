using OpenQA.Selenium;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Support.UI;
using TechTalk.SpecFlow;
using DevProject.Configurations;
using NUnit.Framework;
using System.Threading;
using System.Linq;
using DevProject.StepDefinitions;


namespace DevProject
{
    [Binding]
    public class BasePage
    {


        public BaseTest _baseTest;
        public IWebDriver? Driver => _baseTest.Driver;
        public Dictionary<string, KeyValuePair<string, string>> KeyValuePairs => _baseTest.KeyValuePairs;
        private IJavaScriptExecutor? _javaScriptExecutor;
        public BasePage(BaseTest baseTest)
        {

            _baseTest = baseTest;
        }


        [Given(@"Click (.*) Element By (.*)")]
        public void ClickElement(string key, string type)
        {
            if (Driver == null)
                throw new InvalidOperationException("Driver is not initialized.");
            IWebElement textAreaElement;
            if ("id".Equals(type))
                textAreaElement = Driver.FindElement(By.Id(key));
            else if ("name".Equals(type))
                textAreaElement = Driver.FindElement(By.Name(key));
            else if ("xpath".Equals(type))
                textAreaElement = Driver.FindElement(By.XPath(key));
            else
                throw new ArgumentException("The type value of element is not valid");
            textAreaElement.Click();





            ///
        





        }



        [Given(@"Driver Navigate to (.*)")]
        public void navigate(string key)
        {
            Driver?.Navigate().GoToUrl(key);
        }

        [Given(@"Send (.*) key to (.*) type (.*) element")]
        public void SendKeys(string text, string type, string key)
        {
            if (Driver == null)
                throw new InvalidOperationException("Driver is not initialized.");
            IWebElement textAreaElement;
            if ("id".Equals(type))
                textAreaElement = Driver.FindElement(By.Id(key));
            else if ("name".Equals(type))
                textAreaElement = Driver.FindElement(By.Name(key));
            else if ("xpath".Equals(type))
                textAreaElement = Driver.FindElement(By.XPath(key));
            else
                throw new ArgumentException("The type value of element is not valid");
            textAreaElement.SendKeys(text);
        }


        [Given(@"Check (.*) type (.*) element is equasls (.*)")]
        public void GivenCheckIf(string key, string type, string assertKey)
        {
            if (Driver == null)
                throw new InvalidOperationException("Driver is not initialized.");
            IWebElement errorMessage;
            if ("id".Equals(type))
                errorMessage = Driver.FindElement(By.Id(key));
            else if ("name".Equals(type))
                errorMessage = Driver.FindElement(By.Name(key));
            else if ("xpath".Equals(type))
                errorMessage = Driver.FindElement(By.XPath(key));
            else
                throw new ArgumentException("The type value of element is not valid");
            Assert.AreEqual(assertKey, errorMessage.Text.Trim());
        }









        //==================================================================NewSteps=====================================================================
        public void goToUrl(string uri)
        {
            Driver?.Navigate().GoToUrl(uri);

        }
        public void clickBP(string key)
        {
            Thread.Sleep(1000);
            findElement(key).Click();
        }
        public void clickStale(string key)
        {
            Thread.Sleep(1000);
            findElementStale(key).Click();
        }
        public void clickDD(string key, string key2)
        {
            findElement(key).Click();
            findElement(key2).Click();
        }

        public void fillTextBox(string key, string text)
        {
            findElement(key).SendKeys(text);
        }

        public void fillPasswordBox(string key, string passId)
        {
            Console.WriteLine(KeyValuePairs[passId].Value);
           
        }





        [Given(@"secret Send (.*) key to (.*) element")]
        public void fillPasswordBoxSentence(string text, string key)
        {
            fillPasswordBox(key, text);
        }

        public void fileUpload()
        {
            var allowsDetection = Driver as IAllowsFileDetection;
            if (allowsDetection != null)
            {
                allowsDetection.FileDetector = new LocalFileDetector();
            }
        }

        public void checkElementIsDisplayed(string key)
        {
            Assert.IsTrue(findElement(key).Displayed, key + "' not found");
        }
        public void checkElement(string key)
        {
            Assert.AreNotEqual(findElement(key), null + "not found");
        }

        public void browserMaximize()
        {
            Driver?.Manage().Window.Maximize();
        }
        public void goBack()
        {
            Driver?.Navigate().Back();
        }

        public void forward()
        {
            Driver?.Navigate().Forward();
        }
        public void leftPress(string key, string text)
        {
            findElement(key).SendKeys(Keys.Home);
            fillTextBox(key, text);

        }

        public void refreshPage()
        {
            Driver?.Navigate().Refresh();
        }
        public void switchToLastWindow()

        {
            Driver?.SwitchTo().Window(Driver.WindowHandles.Last());
        }

        public void pageContainsText(string text)

        {
            Assert.IsTrue(Driver.PageSource.Contains(text), "page contains " + text + " value");
        }

        public void elementContainsText(string key, string text)

        {
            Assert.AreEqual(findElement(key).Text.ToString(), text, "text not match..");
        }


        public By generateElementBy(string by, string value)
        {
            Console.WriteLine("generateElementBy by: " + by + " value: " + value);
            By byElement = null;
            if (by.Equals(ElementType.id))
            {
                byElement = By.Id(value);
            }
            else if (by.Equals(ElementType.name))
            {
                byElement = By.Name(value);
            }
            else if (by.Equals(ElementType.className))
            {
                byElement = By.ClassName(value);
            }
            else if (by.Equals(ElementType.cssSelector))
            {
                byElement = By.CssSelector(value);
            }
            else if (by.Equals(ElementType.xpath))
            {
                byElement = By.XPath(value);
            }
            else if (by.Equals(ElementType.linkText))
            {
                byElement = By.LinkText(value);
            }
            else
            {
                Assert.Fail("No such selector.");
            }
            return byElement;
        }

        public IWebElement findElement(string key)
        {
            By by = generateElementBy(KeyValuePairs[key].Key, KeyValuePairs[key].Value);
            WebDriverWait wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(AppConficReader.GetElementLoadTimeout()));
            IWebElement webElement = wait.Until(driver => driver.FindElement(by));
            return webElement;
        }
        public IList<IWebElement> findElements(string key)
        {
            By by = generateElementBy(KeyValuePairs[key].Key, KeyValuePairs[key].Value);
            WebDriverWait wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(AppConficReader.GetElementLoadTimeout()));
            IList<IWebElement> webElements = wait.Until(driver => driver.FindElements(by));
            return webElements;
        }
        public IWebElement findElementStale(string key)
        {
            By by = generateElementBy(KeyValuePairs[key].Key, KeyValuePairs[key].Value);
            DefaultWait<IWebDriver> wait = new DefaultWait<IWebDriver>(Driver);
            wait.Timeout = TimeSpan.FromSeconds(AppConficReader.GetElementLoadTimeout());
            wait.PollingInterval = TimeSpan.FromMilliseconds(250);
            wait.IgnoreExceptionTypes(typeof(StaleElementReferenceException));
            wait.Message = "StaleElementReferenceException";
            IWebElement webElement = wait.Until(driver => {
                var el = driver.FindElement(by);
                return (el != null && el.Enabled && el.Displayed) ? el : null;
            });
            return webElement;
        }


        public void scrollElement(IWebElement element)
        {
            _javaScriptExecutor = (IJavaScriptExecutor)Driver;
            _javaScriptExecutor.ExecuteScript("arguments[0].scrollIntoView({ behaviour: 'smooth', block: 'center', inline: 'center'});", element);
        }

        public void clickJS(string key)
        {
            By by = generateElementBy(KeyValuePairs[key].Key, KeyValuePairs[key].Value);
            WebDriverWait wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(AppConficReader.GetElementLoadTimeout()));
            IWebElement webElement = wait.Until(driver => driver.FindElement(by));
            scrollElement(webElement);
            _javaScriptExecutor = (IJavaScriptExecutor)Driver;
            _javaScriptExecutor.ExecuteScript("arguments[0].click();", webElement);

        }

        public void clearElement(string key)
        {
            findElement(key).Clear();
        }
        ////sentences
      

        [Given(@"Click (.*) element")]
        public void clickElementSentence(string key)
        {
            clickBP(key);
        }

        [Given(@"Stale click (.*) element")]
        public void clickElementStaleSentence(string key)
        {
            clickStale(key);
        }

        [Given(@"Click via JS (.*) element")]
        public void clickElementJS(string key)
        {
            clickJS(key);
        }

        [Given(@"Click (.*),(.*) elements with order")]
        public void clickElementSentence(string key, string key2)
        {
            clickDD(key, key2);
        }

        [Given(@"Check (.*) element is displayed")]
        public void checkElementIsDisplayedSentence(string key)
        {
            checkElementIsDisplayed(key);
        }

        [Given(@"Check (.*) element is not null")]
        public void checkElementIsNull(string key)
        {
            checkElement(key);
        }

        [Given(@"Send (.*) key to (.*) element")]
        public void fillTextBoxSentence(string text, string key)
        {
            fillTextBox(key, text);
        }
        
       
        [Given(@"Go to (.*) Url")]
        public void goToUrlSentence(string url)
        {
            goToUrl(url);
            Console.WriteLine(url + " is opened");
        }


        [Given(@"Wait (.*) seconds")]
        public void waitBySecond(int second)
        {
            Thread.Sleep(second * 1000);
        }

        [Given(@"Go back")]
        public void goBackSentence()
        {
            goBack();
        }

        [Given(@"Go forward")]
        public void forwardSentence()
        {
            forward();
        }

        [Given(@"Reflesh page")]
        public void refreshPageSentence()
        {
            refreshPage();
        }

        [Given(@"Switch to last tab")]
        public void switchToLastWindowSentence()
        {
            switchToLastWindow();
        }

        [Given(@"File Upload")]
        public void fileUploadSentence()
        {
            fileUpload();
        }

        [Given(@"Page contains (.*) text")]
        public void pageContainsTextSentence(string text)
        {
            pageContainsText(text);
        }

        [Given(@"(.*) element contains (.*) text")]
        public void elementContainsTextSentence(string key, string text)
        {
            elementContainsText(key, text);
        }


        [Given(@"Driver Quit")]
        public void driverClose()
        {
            Driver?.Quit();
            Console.WriteLine("Driver Quited");
        }


 
        [Given(@"Clear (.*) element")]
        public void clearElementSentence(string key)
        {
            clearElement(key);
            Console.WriteLine(key + "element is cleared");
        }





    }
}