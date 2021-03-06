﻿using System;
using System.Collections.Generic;
using System.Linq;
using NLog;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace ConseleniumPresentation
{
    [TestFixture]
    public class LocatorIterationTwo
    {
        private int iteration = 1000;
        private IWebDriver driver;
        private Logger logger;

        [SetUp]
        public void Setup()
        {
            var path = System.IO.Path.GetDirectoryName( 
                System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase);
            path = path.Substring(6);
            driver = new ChromeDriver($"{path}\\drivers");
            driver.Navigate().GoToUrl($"{path}\\fakewebsite\\simplecase.html");
            logger = NLog.LogManager.GetCurrentClassLogger();
        }
        
        [Test]
        public void TimeByID()
        {
            var locator = "buttonID";
            var idLocator = By.Id(locator);

            var measuredTime = MeasureTime(idLocator);

            PrintResults(measuredTime, "id", locator);
        }
        
        [Test]
        public void TimeByClass()
        {
            var locator = "button";
            var classNameLocator = By.ClassName(locator);
            
            var measuredTime = MeasureTime(classNameLocator);

            PrintResults(measuredTime, "class", locator);
        }
        
        [Test]
        public void TimeByCss()
        {
            var locator = "#buttonID";
            var classLocator = By.CssSelector(locator);

            var measuredTime = MeasureTime(classLocator);

            PrintResults(measuredTime, "css", locator);
        }
        
        [Test]
        public void TimeByXpath()
        {
            var locator = "//*[@id=\"buttonID\"]";
            var xpathLocator = By.XPath(locator);

            var measuredTime = MeasureTime(xpathLocator);

            PrintResults(measuredTime, "Xpath", locator);
        }
        
        [Test]
        public void TimeByTag()
        {
            var locator = "a";
            var tagLocator = By.TagName(locator);

            var measuredTime = MeasureTime(tagLocator);

            PrintResults(measuredTime, "Xpath", locator);
        }

        [Test]
        public void TimeByLinkText()
        {
            var locator = "Click me";
            var linkText = By.LinkText(locator);

            var measuredTime = MeasureTime(linkText);

            PrintResults(measuredTime, "Link Text", locator);
        }

        [Test]
        public void TimeByPartialLinkText()
        {
            var locator = "me";
            var linkText = By.PartialLinkText(locator);

            var measuredTime = MeasureTime(linkText);

            PrintResults(measuredTime, "Partial Link Text", locator);
        }
        
        [TestCase(1,1,2)]
        public void Calc_Add(int firstNumber, int secondNumber, int expectedResult)
        {
            int actualResult = firstNumber + secondNumber;
            Assert.AreEqual(expectedResult, actualResult);
        }
        
        [TestCase(1,1, ExpectedResult = 2)]
        public int Calc_Add(int firstNumber, int secondNumber)
        {
            return firstNumber + secondNumber;
        }

        private List<long> MeasureTime(By locator)
        {
            var resultsList = new List<long>();
            for (var i = 0; i < iteration; i++)
            {
                var watch = System.Diagnostics.Stopwatch.StartNew();
                driver.FindElement(locator);
                watch.Stop();
                resultsList.Add(watch.ElapsedMilliseconds);
            }

            return resultsList;
        }

        private void PrintResults(List<long> resultsList, string @by, string locatorUsed)
        {
            
            var max = resultsList.Max();
            var min = resultsList.Min();
            var average = resultsList.Average();
            Console.WriteLine($"Locator {by}  averge time is {average}");
            logger.Info("----------AllTime is In Millisecodns-------------------------");
            logger.Info($"LocatorTypeUsed {@by}   Value {locatorUsed}");
            logger.Info($"Samples {iteration} Average {average}  Min {min}  Max {max}");
            logger.Info("-----------------------------------");
            for (var i = 0; i < iteration; i+=10)
            {
                var row = string.Empty;
                for (var j = 0; j < 10; j++)
                {
                    row +=$"{resultsList[i+j]} |";
                }
                logger.Info($"Row {i}|  {row}");
            }
            logger.Info("-----------------------------------");
        }

        [TearDown]
        public void CleanUp()
        {
            driver.Quit();            
        }
    }
}