using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System;
using Xunit;

namespace VeeamSeleniumWebDriver
{
    public class CareersVeeamTests : IClassFixture<WebDriverFixture>
    {
        private readonly WebDriverFixture _webDriver;
        private const int WAIT_ELEMENT_TIMEOUT = 15;
        private const string URL_TO_TEST = "https://careers.veeam.com/";
        
        public CareersVeeamTests(WebDriverFixture webDriver)
        {
            _webDriver = webDriver;
        }
        
        [Theory]
        [InlineData(WebDriverType.Chrome, "Romania", "Romania", "English", 39)]
        [InlineData(WebDriverType.Chrome, "Germany", "Germany", "English", 4)]
        [InlineData(WebDriverType.Chrome, "United States - New York", "New York", "English", 1)]
        public void CountVacanciesWithCountryAndLanguage(WebDriverType webDriverType, string countryToSelect, string countryLabel, string languageToSelect, int expectedJobsCount)
        {
            var driver = _webDriver[webDriverType];
            driver.Navigate().GoToUrl(URL_TO_TEST);
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(WAIT_ELEMENT_TIMEOUT));
            var selectedCountry = wait.Until(ExpectedConditions.ElementExists(By.CssSelector("#country-element > div.selecter > span.selecter-selected")));

            if(driver.TryFindElement(By.CssSelector("a.cookie-messaging__button"), out var cookieButton))
            {
                cookieButton.Click();
            }

            selectedCountry.Click();
            driver.FindElement(By.CssSelector($"#country-element > div.selecter span[data-value='{countryToSelect}']")).Click();

            var selectedLanguage = driver.FindElement(By.CssSelector("#language > span.selecter-selected"));
            selectedLanguage.Click();
            var languageSelector = driver.FindElement(By.CssSelector("#language"));
            var languages = languageSelector.FindElements(By.CssSelector("label.controls-checkbox"));
            foreach (var l in languages)
            {
                var input = l.FindElement(By.TagName("input"));
                if((l.Text == languageToSelect && !input.Selected) || (l.Text != languageToSelect && input.Selected))
                {
                    l.Click();
                }
            }
            languageSelector.FindElement(By.CssSelector("a.selecter-fieldset-submit")).Click();

            if (driver.TryFindElement(By.CssSelector("a.load-more-button"), out var loadMoreButton) && loadMoreButton.Displayed)
            { 
                loadMoreButton.Click();
                driver.WaitForAjax();
            }
            
            var vacanciesContainers = driver.FindElements(By.CssSelector("div.vacancies-blocks-container"));
            int realJobsCount = 0;
            foreach (var vc in vacanciesContainers)
            {
                realJobsCount += vc.FindElements(By.CssSelector("div.vacancies-blocks-item")).Count;
            }
            int.TryParse(driver.FindElement(By.CssSelector("div.vacancies-blocks div:nth-child(1) > h3")).Text.Split(' ')[0], out int jobsCountInHeader);

            Assert.Equal(countryLabel, selectedCountry.Text);
            Assert.Equal(languageToSelect, selectedLanguage.Text);
            Assert.Equal(expectedJobsCount, realJobsCount);
            Assert.Equal(expectedJobsCount, jobsCountInHeader);
        }
    }
}
