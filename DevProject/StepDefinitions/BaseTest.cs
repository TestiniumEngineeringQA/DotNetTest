using Newtonsoft.Json;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Remote;
using TechTalk.SpecFlow;
using DevProject.Models;
using System.Reflection;


namespace DevProject.StepDefinitions
{
    [Binding]
    public class BaseTest
    {

        public static string BasePathConstants = "";
        public IWebDriver? Driver;
        public bool IsRemoteDriver;
        private readonly string _chrome = "chrome";
        public ChromeOptions ChromeOptions = new ChromeOptions();
        public Dictionary<string, KeyValuePair<string, string>> KeyValuePairs = new();
        private static string _baseExt = "*.json";


        [BeforeScenario]
        public void SetUp()
        {
            ChromeOptions.AddArgument("--start-maximized");
            if (Environment.GetEnvironmentVariable("key") == null)
            {
                Console.WriteLine("Test will be running in local");
                BasePathConstants = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ?? string.Empty, @"../../../") + "Constants";
            }
            else
            {
                Console.WriteLine("Test will be running in Testinium");
                BasePathConstants = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ?? string.Empty, @"../demospecflow/DevProject/") + "Constants";
            }
        }
        [Given(@"Wake up the driver")]
        public void DriverAwake()
        {
            if (Environment.GetEnvironmentVariable("key") == null)
            {
                Console.WriteLine("key null ChromeDriver is waking up");
                string workingDirectory = Environment.CurrentDirectory;
                var parent1 = Directory.GetParent(workingDirectory);
                var parent2 = parent1?.Parent;
                var parent3 = parent2?.Parent;
                string projectDirectory = parent3?.FullName ?? string.Empty;
                string driverPath = projectDirectory + "/Drivers";
                ChromeOptions option1 = new ChromeOptions();
                option1.AddArguments("disable-popup-blocking");
                option1.AddArguments("ignore-certificate-errors");
                option1.AddArguments("--ignore-ssl-errors=yes");
                option1.AddArguments("start-maximized");
                Driver = new ChromeDriver(driverPath, option1);
                IsRemoteDriver = false;
                Console.WriteLine("ChromeDriver woke");
            }
            else
            {
                Console.WriteLine("Remote ChromeDriver is waking up");
                ChromeOptions.AddArguments("disable-popup-blocking");
                ChromeOptions.AddArguments("ignore-certificate-errors");
                ChromeOptions.AddArguments("--ignore-ssl-errors=yes");
                Console.WriteLine("options added");
                ChromeOptions.AddAdditionalOption("testinium:browserName", _chrome);
                ChromeOptions.AddAdditionalOption("testinium:key", Environment.GetEnvironmentVariable("key"));
                Console.WriteLine(_chrome);
                Console.WriteLine(Environment.GetEnvironmentVariable("key"));
                Console.WriteLine("capability added");
                // Preprod Hub'ı
                Driver = new RemoteWebDriver(new Uri("http://172.25.1.12:4444/wd/hub"), ChromeOptions.ToCapabilities(), TimeSpan.FromSeconds(60));
                //Driver = new RemoteWebDriver(new Uri("http://172.25.1.159:4444/wd/hub"), ChromeOptions.ToCapabilities(), TimeSpan.FromSeconds(60));
                IsRemoteDriver = true;
                Console.WriteLine("ChromeDriver woke");
            }
        }

       [Given(@"Click ""(.*)""")]
       public void GivenClick(string selector)
       {
           throw new Exception("Fail olsun diye");
       }


        [AfterScenario]
        public void AfterScenario()
        {


            try
            {
                if (Driver != null)
                {
                    Driver.Quit();
                    Console.WriteLine("Driver Quited After Scenario");
                }
            }
            catch
            {
                Console.WriteLine("Driver is already closed");
            }
        }






        [Given(@"Get (.*) dictionary folders json")]
        public void CreateDictionary(string key)
        {
            KeyValuePairs = GetDictionary(key);

        }


        [Given(@"Add these two number (.*) ,(.*)")]
        public void CreateDictionary(string key, string key2)
        {



        }





        //
        [Given(@"Connect to DB via (.*) connectionString")]
        public void ConnecToOutDb(string connectionString)
        {

            //DB connection functions here
        }




        public Dictionary<string, KeyValuePair<string, string>> GetDictionary(string key)
        {
            Dictionary<string, KeyValuePair<string, string>> dic = new Dictionary<string, KeyValuePair<string, string>>();
            var txtFiles = Directory.EnumerateFiles(BasePathConstants + "/" + key, _baseExt);
            foreach (string currentFile in txtFiles)
            {
                var json = File.ReadAllText(currentFile);
                List<Element>? elements = JsonConvert.DeserializeObject<List<Element>>(json);
                if (elements != null)
                {
                    foreach (var item in elements)
                    {
                        dic.Add(item.key, new KeyValuePair<string, string>(item.type, item.value));
                    }
                }
            }
            Console.WriteLine("Total element count in the dictionary" + dic.Count);
            return dic;
        }


    }
}
