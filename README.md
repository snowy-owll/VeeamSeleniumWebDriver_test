# Test project for automated site testing
The project contains one parameterized test method ```CountVacanciesWithCountryAndLanguage```. It checks the number of vacancies displayed on the page https://careers.veeam.com/ and compares with the expected.
The method takes the following parameters:

  - ```webDriverType``` - type of web driver (only Chrome is available in this project, but you can add others with minimal code changes)
  - ```countryToSelect``` - name of the country to be selected in the list
  - ```countryLabel``` - how the name of the country is displayed when it is selected. May differ from ```countryToSelect```. For example, when the US state is selected (in this case, only the state name is displayed, whereas in the selection list US + state name)
  - ```languageToSelect``` - name of the language to be selected in the list
  - ```expectedJobsCount``` - the expected number of vacancies for the above parameters

### Requirements:
  - .NET Framework 4.7.1
  - Visual Studio 2019 Community

### Libraries used:
  - [Selenium WebDriver](https://www.selenium.dev) - to automate the browser
  - [xUnit](https://github.com/xunit/xunit) for testing

### Usage
You can run the test using the ```Test Explorer``` in Visual Studio.