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
            var scholarships = await Scrapingdata();

            // Add logic to save scholarships to the database if needed
            // ...

            return scholarships;
        }
       
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
            var outputobject = "";
            string prompt = $"read and answer budget(in class inline-block mr-4 text-sm text-gray-600 rounded), title (it is h1 or some tag biger than other), location, school name, level (Master or Bachelor or phd or ... in class inline-block mr-4 text-sm text-gray-600 rounded), is in site {html}";




            var openAI = new OpenAIAPI("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");
            
            var completions = await openAI.Completions.GetCompletion(prompt);
           

            Console.WriteLine("==========================this is AI answer==========================");
            Console.WriteLine(completions);

            return new ScholarshipDTO();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in UseOpenAIPromrt: {ex.Message}");
            return null; // or handle the error accordingly
        }
    }

    private async Task<List<ScholarshipDTO>> Scrapingdata()
    {
        using (var driver = new ChromeDriver())
        {
            List<ScholarshipDTO> scholarships = new List<ScholarshipDTO>(); 
            try
            {
                driver.Navigate().GoToUrl("https://www.scholarshipportal.com/");

                await Task.Delay(1000);

                // Find all div elements with class "flex md:w-1/3"
                var scholarshipDivs = driver.FindElements(By.CssSelector("div.flex.md\\:w-1\\/3"));
                var count = 0;
                foreach (var scholarshipDiv in scholarshipDivs)
                {
                    count++;
                    if ( count ==3)
                    {
                        await Task.Delay(20000);
                        count = 0;
                    }
                    var scholarshipLink = scholarshipDiv.FindElement(By.CssSelector("a"));
                    var scholarshipUrl = scholarshipLink.GetAttribute("href");

                    

                    ((IJavaScriptExecutor)driver).ExecuteScript($"window.open('{scholarshipUrl}', '_blank');");

                    driver.SwitchTo().Window(driver.WindowHandles[^1]);

                    var mainElement = driver.FindElement(By.CssSelector("main.p-6.bg-white.rounded-t.shadow.sm\\:p-8.md\\:p-12"));

                    scholarships.Add(await UseOpenAIPromrt(mainElement.GetAttribute("outerHTML")));

                

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
}
