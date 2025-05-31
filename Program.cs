using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System.Text.Json;

var options = new ChromeOptions();
options.AddArgument("--headless");

IWebDriver driver = new ChromeDriver(options);


try
{
    driver.Navigate().GoToUrl("https://app.cloudqa.io/home/AutomationPracticeForm");

    var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));

    wait.Until(ExpectedConditions.FrameToBeAvailableAndSwitchToIt(By.Id("iframeId")));

    var form = driver.FindElement(By.XPath("//form"));
    var fname_label = form.FindElement(By.XPath(".//label[contains(text(),'First Name')]"));
    var fname_for = fname_label.GetAttribute("for");
    var Fname = form.FindElement(By.XPath($".//input[@id='{fname_for}']"));
    Fname.SendKeys("Rishab");

    var lname_label = form.FindElement(By.XPath(".//label[contains(text(),'Last Name')]"));
    var lname_for = lname_label.GetAttribute("for");
    var Lname = form.FindElement(By.XPath($".//input[@id='{lname_for}']"));
    Lname.SendKeys("Anand");

    var email_label = form.FindElement(By.XPath(".//label[contains(text(),'Email')]"));
    var email_for = email_label.GetAttribute("for");
    var email = form.FindElement(By.XPath($".//input[@id='{email_for}']"));
    email.SendKeys("rishab.anand@example.com");

    form.Submit();

    var el = wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//div[.//h3[contains(text(),'Submit')and contains(text(),'data')]]")));

    Thread.Sleep(1000);//added this so that trhe email field returns as the same not encoded

    var data = el.FindElement(By.XPath(".//pre")).Text;


    var jsonData = JsonDocument.Parse(data);

    var fname = jsonData.RootElement.GetProperty("First Name").GetString();
    var lname = jsonData.RootElement.GetProperty("Last Name").GetString();
    var emailValue = jsonData.RootElement.GetProperty("Email").GetString();
    Console.WriteLine($"First Name: {fname}");
    Console.WriteLine($"Last Name: {lname}");
    Console.WriteLine($"Email: {emailValue}");

    if (fname == "Rishab" && lname == "Anand" && emailValue == "rishab.anand@example.com")
    {
        Console.WriteLine("Form submitted successfully!");
    }
    else
    {
        Console.WriteLine("Form submission failed.");
    }
}
catch (Exception ex)
{
    Console.WriteLine($"An error occurred: {ex.Message}");
}
finally
{
    driver.Quit();
}
