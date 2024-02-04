using EDUHUNT_BE.Data;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using SharedClassLibrary.Contracts;
using SharedClassLibrary.DTOs;
using EDUHUNT_BE.Model;
using EDUHUNT_BE.Helper;
using OpenAI_API;
using OpenAI_API.Completions;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Options;
public class ScholarshipRepository : IScholarship
{
    private readonly AppDbContext _dbContext;

    public ScholarshipRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<ScholarshipDTO>> GetScholarships()
    {

        using (var driver = new ChromeDriver())

        {
            SearchGoogle(driver);
            var scholarships = await Scrapingdata(driver); ;

            // Add logic to save scholarships to the database if needed
            // ...

            return scholarships;
        }


        // Add logic to save scholarships to the database if needed
        // ...

    }
    //public async Task<List<ScholarshipDTO>> GetScholarships()
    //{
    //    using (var driver = new ChromeDriver())
    //    {
    //        var scholarships = await _scrapingService.ScrapeScholarships();

    //        // Add logic to save scholarships to the database if needed
    //        // ...

    //        return scholarships;
    //    }
    //}






    public async Task AddScholarship(ScholarshipDTO scholarship)
    {
        // Implement logic to add scholarship to your data source (e.g., database)
        // ...
    }

    private List<ScholarshipDTO> ParseHtml(string html)
    {
        // Implement logic to parse the HTML and extract scholarship information
        // ...

        // For illustration purposes, return an empty list
        return new List<ScholarshipDTO>();
    }


    private async Task<ScholarshipDTO> UseOpenAIPromrt(string html)
    {
        try
        {
            ScholarshipDTO newscholarship = new ScholarshipDTO();
            string prompt = $"read and answer budget(in class inline-block mr-4 text-sm text-gray-600 rounded), title (in class text-3xl font-bold mb-4), location, university, level (Master or Bachelor or phd or ... in class inline-block mr-4 text-sm text-gray-600 rounded), is in site [{html}]";

            var openAI = new OpenAIAPI("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");

            var completions = await openAI.Completions.GetCompletion(prompt);


            Console.WriteLine("==========================this is AI answer==========================");
            Console.WriteLine(completions);

            var budget = "0";
            var title = "";
            var location = "";
            var university = "";
            var level = "";

            if (completions is not null)
            {
                var infoAsArray = completions.Split("\n").ToList();
                title = infoAsArray[0];
                /*for (int i = 0; i < infoAsArray.Count; i++)
                {
                    var infoContent = infoAsArray[i].Split(" ")[1];
                    switch (infoAsArray[i].Split(" ")[0].ToLower()) {
                        case "budget":
                            // content for budget
                            if (!infoContent.Contains("Not specified"))
                            {
                                var regexNum = new Regex("/d");
                                var number = regexNum.Match(infoContent).Groups[0].Value;
                                for(int j = 0; j<infoContent.Split(" ").Length; j++)
                                {
                                    if (infoContent.Split(" ")[j].Contains(number))
                                    {
                                        budget = infoContent.Split(" ")[j];
                                    }
                                }
                            } 
                            break;
                        case "title":
                            // content for title
                            if (!infoContent.Contains("Not specified"))
                            {
                                title = infoContent;
                            }
                            break;
                        case "location":
                            // content for location
                            if (!infoContent.Contains("Not specified"))
                            {
                                location = infoContent;
                            }
                            break;
                        case "university":
                            // content for university
                            if (!infoContent.Contains("Not specified"))
                            {
                                university = infoContent;
                            }
                            break;
                        case "level":
                            // content for level
                            if (!infoContent.Contains("Not specified"))
                            {
                                level = infoContent;
                            }
                            break;
                    }
                }*/
            }

            return new ScholarshipDTO
            {
                Title = title,
                Location = completions,
                School_name = university,
                Level = level
            };
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in UseOpenAIPromrt: {ex.Message}");
            return null; // or handle the error accordingly
        }
    }
    private void SearchGoogle(ChromeDriver driver)
    {

        try
        {
            driver.Navigate().GoToUrl("https://www.google.com/");
            IWebElement textarea = driver.FindElement(By.ClassName("gLFyf"));
            //fill in textarea that " scholarshipportal "
            textarea.SendKeys("scholarshipportal");
            //enter
            textarea.SendKeys(Keys.Enter);
            //wait for 2 seconds
            Thread.Sleep(2000);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
        }

    }
    private async Task<List<ScholarshipDTO>> Scrapingdata(ChromeDriver driver)
    {
        var chromeOptions = new ChromeOptions();
        chromeOptions.AddArgument("--ignore-certificate-errors-spki-list");
        chromeOptions.AddArgument("--ignore-certificate-errors");

        List<ScholarshipDTO> scholarships = new List<ScholarshipDTO>();
        try
        {
            driver.Navigate().GoToUrl("https://www.scholarshipportal.com/");

            await Task.Delay(1000);

            // Find all div elements with class "flex md:w-1/3"
            var scholarshipDivs = driver.FindElements(By.CssSelector("div.flex.md\\:w-1\\/3"));
            foreach (var scholarshipDiv in scholarshipDivs)
            {
                var scholarshipLink = scholarshipDiv.FindElement(By.CssSelector("a"));
                var scholarshipUrl = scholarshipLink.GetAttribute("href");

                ((IJavaScriptExecutor)driver).ExecuteScript($"window.open('{scholarshipUrl}', '_blank');");

                driver.SwitchTo().Window(driver.WindowHandles[^1]);

                var mainElement = driver.FindElement(By.CssSelector("main.p-6.bg-white.rounded-t.shadow.sm\\:p-8.md\\:p-12"));
                //p - 6 bg - white rounded - t shadow sm:p - 8 md: p - 12
                //var mainElement = driver.FindElement(By.CssSelector("main.p-6.bg-white.rounded-t.shadow.sm:p-8.md:p-12"));
                //scholarships.Add(await UseOpenAIPromrt(mainElement.GetAttribute("outerHTML")));

                var title = mainElement.FindElement(By.CssSelector(".text-3xl.font-bold.mb-4")).Text;
                var level = mainElement.FindElement(By.CssSelector("ul > li:nth-child(1) > .inline-block.mr-4.text-sm.text-gray-600.rounded")).Text;
                var budget = mainElement.FindElement(By.CssSelector("ul > li:nth-child(2) > .inline-block.mr-4.text-sm.text-gray-600.rounded")).Text;
                var university = mainElement.FindElement(By.CssSelector(".rounded.bg-gray-100.p-6.flex.my-6 > div.flex-1 > h3 ")).Text;
                var url = mainElement.FindElement(By.CssSelector(".items-center.mt-4.mb-8.sm\\:flex > span > a")).GetAttribute("href");

                scholarships.Add(
                        new ScholarshipDTO
                        {
                            Budget = budget,
                            Title = title,
                            Level = level,
                            School_name = university,
                            Url = url
                        }
                    );

                await Task.Delay(20000);

                driver.Close();

                driver.SwitchTo().Window(driver.WindowHandles[0]);
            }

            // For illustration purposes, return an empty list
            return scholarships;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
            return new List<ScholarshipDTO>();
        }
        finally
        {
            // This will only close the main window, not the additional tabs
            driver.Quit();
        }
    }



}

