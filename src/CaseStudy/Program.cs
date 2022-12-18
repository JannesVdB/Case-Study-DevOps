using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System.Collections.ObjectModel;

namespace CaseStudy
{
    class Program
    {
        private static IWebDriver driver;
        private static readonly string path = @"D:\School\Hoge school\2 ITF\Devops\C#";
        static void Main(string[] args)
        {
            Console.WriteLine("Which site do you want to scrape: ");
            Console.WriteLine("Option 1: Youtube");
            Console.WriteLine("Option 2: ICT Job");
            Console.WriteLine("Option 3: The Spruce");
            int scrapeOption = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("Which data do you want to scrape: ");
            string topic = Console.ReadLine();
            Console.WriteLine("Do you want your data to be saved in a: ");
            Console.WriteLine("Option 1: CSV-file");
            Console.WriteLine("Option 2: JSON-file");
            Console.WriteLine("Option 3: CSV-file & JSON-file");
            Console.WriteLine("Option 4: I wish to not save my data");
            int fileOption = Convert.ToInt32(Console.ReadLine());
           
            driver = new ChromeDriver();
            driver.Manage().Window.Maximize();
            
            if ( scrapeOption == 1)
            {
                Youtube(topic, fileOption);
            } 
            else if ( scrapeOption == 2)
            {
                ICTJob(topic, fileOption);
            } 
            else if ( scrapeOption == 3)
            {
                TheSpruce(topic, fileOption);
            }

            driver.Quit();
        }

        static void Youtube(string searchString, int file)
        {
            string url = "https://www.youtube.com/results?search_query=" + searchString + "&sp=CAI%253D";
            string acceptAllElement = "//*[@id=\"content\"]/div[2]/div[6]/div[1]/ytd-button-renderer[2]/yt-button-shape/button";
            string videoElement = "ytd-video-renderer.style-scope.ytd-item-section-renderer";
            string titleLinkElement = "#video-title";
            string viewsElement = ".//*[@id=\"metadata-line\"]/span[1]";
            string uploaderElement = "a.yt-simple-endpoint.style-scope.yt-formatted-string";

            string outputFileCSV = String.Format("{0}\\{1}", path, "YoutubeVideos.csv");
            string outputFileJSON = String.Format("{0}\\{1}", path, "YoutubeVideos.json");

            int videoCount = 1;

            driver.Navigate().GoToUrl(url);

            Thread.Sleep(2000);

            var acceptAll = driver.FindElement(By.XPath(acceptAllElement));
            acceptAll.Click();

            Thread.Sleep(3000);

            ReadOnlyCollection<IWebElement> videos = driver.FindElements(By.CssSelector(videoElement));

            Dictionary<int, List<string>> videoInfo = new Dictionary<int, List<string>>();

            Console.WriteLine("\n");
            Console.WriteLine("We scraped the 5 most recent " + searchString + "videos for you. \n");

            foreach (IWebElement video in videos)
            {
                if (videoCount < 6)
                {
                    string videoTitle, videoLink, videoViews, videoUploader;

                    IWebElement titleLink = video.FindElement(By.CssSelector(titleLinkElement));
                    videoTitle = titleLink.Text;
                    videoLink = titleLink.GetAttribute("href");

                    IWebElement views = video.FindElement(By.XPath(viewsElement));
                    videoViews = views.Text;

                    IWebElement uploader = video.FindElement(By.CssSelector(uploaderElement));
                    videoUploader = uploader.GetAttribute("innerHTML");

                    Console.WriteLine("Video #" + videoCount);
                    Console.WriteLine("Video title: " + videoTitle);
                    Console.WriteLine("Amount of views: " + videoViews);
                    Console.WriteLine("Uploader: " + videoUploader);
                    Console.WriteLine("Link to channel: " + videoLink + "\n");

                    List<string> videoData = new List<string>() { videoTitle, videoViews, videoUploader, videoLink };
                    videoInfo.Add(videoCount, videoData);

                    videoCount++;
                }
            }

            if (file == 1)
            {
                Util.CSVFile(videoInfo, outputFileCSV);
            }
            else if (file == 2)
            {
                Video.JSONFile(videoInfo, outputFileJSON);
            } 
            else if (file == 3)
            {
                Util.CSVFile(videoInfo, outputFileCSV);
                Video.JSONFile(videoInfo, outputFileJSON);
            }
        }

        static void ICTJob(string searchString, int file)
        {
            string url = "https://www.ictjob.be/en/search-it-jobs?keywords=" + searchString;
            string newestJobElement = "sort-by-date";
            string jobElement = "li.search-item.clearfix";
            string titleLinkElement = "a.job-title.search-item-link";
            string companyElement = "job-company";
            string locationElement = "job-location";
            string keywordsElement = "job-keywords";

            string outputFileCSV = String.Format("{0}\\{1}", path, "Jobs.csv");
            string outputFileJSON = String.Format("{0}\\{1}", path, "Jobs.json");

            int jobCount = 1;

            driver.Navigate().GoToUrl(url);

            Thread.Sleep(5000);

            var newestJobs = driver.FindElement(By.Id(newestJobElement));
            newestJobs.Click();

            Thread.Sleep(11000);

            ReadOnlyCollection<IWebElement> jobs = driver.FindElements(By.CssSelector(jobElement));

            Dictionary<int, List<string>> jobInfo = new Dictionary<int, List<string>>();

            Console.WriteLine("\n");
            Console.WriteLine("We scraped the 5 most recent job offers for " + searchString + ".");

            foreach (IWebElement job in jobs)
            {
                if (jobCount < 6)
                {
                    string jobTitle, jobCompany, jobLocation, jobKeyWords, jobLink;

                    IWebElement titleLink = job.FindElement(By.CssSelector(titleLinkElement));
                    jobTitle = titleLink.Text;
                    jobLink = titleLink.GetAttribute("href");

                    IWebElement company = job.FindElement(By.ClassName(companyElement));
                    jobCompany = company.Text;

                    IWebElement location = job.FindElement(By.ClassName(locationElement));
                    jobLocation = location.Text;

                    IWebElement keywords = job.FindElement(By.ClassName(keywordsElement));
                    jobKeyWords = keywords.Text;

                    Console.WriteLine("Job #" + jobCount);
                    Console.WriteLine("Job title: " + jobTitle);
                    Console.WriteLine("Job keywords: " + jobKeyWords);
                    Console.WriteLine("Company: " + jobCompany);
                    Console.WriteLine("Location: " + jobLocation);
                    Console.WriteLine("Link for extra info: " + jobLink + "\n");

                    List<string> jobData = new List<string>() { jobTitle, jobKeyWords, jobCompany, jobLocation, jobLink };
                    jobInfo.Add(jobCount, jobData);

                    jobCount++;
                }
            }

            if (file == 1)
            {
                Util.CSVFile(jobInfo, outputFileCSV);
            }
            else if (file == 2)
            {
                Job.JSONFile(jobInfo, outputFileJSON);
            }
            else if (file == 3)
            {
                Util.CSVFile(jobInfo, outputFileCSV);
                Job.JSONFile(jobInfo, outputFileJSON);
            }
        }

        static void TheSpruce(string searchString, int file)
        {
            IWebElement plant, plantLink;
            string url = "https://www.thespruce.com/plants-a-to-z-5116344";
            string plantElement = "//div[text() = '" + searchString + "']";
            string plantParentElement = "./parent::*";

            string botanicalNameElement = "tbody > tr:nth-child(2) > td:nth-child(2)";
            string typeElement = "tbody > tr:nth-child(4) > td:nth-child(2)";
            string soilTypeElement = "tbody > tr:nth-child(7) > td:nth-child(2)";
            string sunExposureElement = "tbody > tr:nth-child(6) > td:nth-child(2)";
            string bloomTimeElement = "tbody > tr:nth-child(9) > td:nth-child(2)";
            string nativeAreaElement = "tbody > tr:nth-child(12) > td:nth-child(2)";

            string plantBotanicalName, plantType, plantSunExposure, plantSoilType, plantBloomTime, plantNativeArea;

            string outputFileCSV = String.Format("{0}\\{1}", path, "PlantsInfo.csv");
            string outputFileJSON = String.Format("{0}\\{1}", path, "PlantsInfo.json");

            Console.WriteLine("We scraped information about the plant " + searchString + ".");

            driver.Navigate().GoToUrl(url);
            Thread.Sleep(5000);
            plant = driver.FindElement(By.XPath(plantElement));
            plantLink = plant.FindElement(By.XPath(plantParentElement));
            driver.Navigate().GoToUrl(plantLink.GetAttribute("href"));

            IWebElement botanicalName = driver.FindElement(By.CssSelector(botanicalNameElement));
            plantBotanicalName = botanicalName.Text;

            IWebElement type = driver.FindElement(By.CssSelector(typeElement));
            plantType = type.Text;

            IWebElement sunExposure = driver.FindElement(By.CssSelector(sunExposureElement));
            plantSunExposure = sunExposure.Text;

            IWebElement soilType = driver.FindElement(By.CssSelector(soilTypeElement));
            plantSoilType = soilType.Text;

            IWebElement bloomTime = driver.FindElement(By.CssSelector(bloomTimeElement));
            plantBloomTime = bloomTime.Text;

            IWebElement nativeArea = driver.FindElement(By.CssSelector(nativeAreaElement));
            plantNativeArea = nativeArea.Text;

            Console.WriteLine("Information about the plant: " + searchString);
            Console.WriteLine("Botanical name: " + plantBotanicalName);
            Console.WriteLine("Plant type: " + plantType);
            Console.WriteLine("Sun exposure: " + plantSunExposure);
            Console.WriteLine("Soil type: " + plantSoilType);
            Console.WriteLine("Bloom time: " + plantBloomTime);
            Console.WriteLine("Native area: " + plantNativeArea);

            List<string> plantInfo = new List<string>() { searchString, plantBotanicalName, plantType, plantSunExposure, plantSoilType, plantBloomTime, plantNativeArea };

            if (file == 1)
            {
                Util.CSVFile(plantInfo, outputFileCSV);
            }
            else if (file == 2)
            {
                Plant.JSONFile(plantInfo, outputFileJSON);
            }
            else if (file == 3)
            {
                Util.CSVFile(plantInfo, outputFileCSV);
                Plant.JSONFile(plantInfo, outputFileJSON);
            }
        }
    }
}