﻿using ChustaSoft.Common.Builders;
using ChustaSoft.Common.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;


namespace ChustaSoft.Common.UnitTest.TestServices
{
    [TestClass]
    public class SelectablePropertiesBuilderHelperUnitTest
    {

        public class TestClass
        {
            public string TestPropertyStr { get; set; }
            public int TestPropertyInt { get; set; }
            public double TestPropertyDouble { get; set; }
            public DateTime ComplexObj { get; set; }
            public IEnumerable<DateTime> TestCollectionDates { get; set; }
        }


        [TestMethod]
        public void Given_Function_WhenSelectPropertyInvoked_ThenISelectablePropertiesBuilderRetrived()
        {
            var testObj = DateTime.Now;

            var testBuilder = testObj.SelectProperty(x => x.Day);

            Assert.AreEqual(testBuilder.Count, 1);
        }

        [TestMethod]
        public void Given_Function_WhenThenSelectPropertyInvoked_ThenISelectablePropertiesBuilderRetrived()
        {
            var testBuilder = SelectablePropertiesBuilder<DateTime>
                .GetProperties()
                .SelectProperty(x => x.Day)
                .ThenSelectProperty(x => x.Month);

            Assert.AreEqual(testBuilder.Count, 2);
        }

        [TestMethod]
        public void Given_Function_WhenFormatSelectionInvoked_ThenStringFormattedRetrived()
        {
            var propertiesFormatted = SelectablePropertiesBuilder<DateTime>
                .GetProperties()
                .SelectProperty(x => x.Day)
                .ThenSelectProperty(x => x.Month)
                .FormatSelection();

            Assert.IsFalse(string.IsNullOrEmpty(propertiesFormatted));
            Assert.IsTrue(propertiesFormatted.LastIndexOf(',') != propertiesFormatted.Length);
        }

        [TestMethod]
        public void Given_ExternalType_WhenFormatSelectionInvoked_ThenStringFormattedRetrived()
        {
            var propertiesFormatted = SelectablePropertiesBuilder<TestClass>
               .GetProperties()
               .SelectProperty(x => x.TestPropertyStr)
               .ThenSelectProperty(x => x.TestPropertyInt)
               .FormatSelection();

            Assert.IsFalse(string.IsNullOrEmpty(propertiesFormatted));
            Assert.IsTrue(propertiesFormatted.LastIndexOf(',') != propertiesFormatted.Length);
        }

        [TestMethod]
        public void Given_ExternalType_WhenFormatSelectionInvokedUsingNestedCollection_ThenStringFormattedRetrived2()
        {
            var propertiesFormatted = SelectablePropertiesBuilderHelper.BackToParent(SelectablePropertiesBuilder<TestClass>
               .GetProperties()
               .SelectProperty(x => x.TestPropertyStr)
               .ThenSelectProperty(x => x.TestPropertyInt)
               .ThenSelectFromCollection(x => x.TestCollectionDates)
                    .ThenSelectProperty(x => x.Minute)
)
               .FormatSelection();

            Assert.IsFalse(string.IsNullOrEmpty(propertiesFormatted));
            Assert.IsTrue(propertiesFormatted.LastIndexOf(',') != propertiesFormatted.Length);
            Assert.IsTrue(propertiesFormatted.Contains("."));
        }

        [TestMethod]
        public void Given_ExternalType_WhenFormatSelectionInvokedUsingNestedCollectionAndComingBackToParentBuilder_ThenStringFormattedRetrived()
        {
            var propertiesFormatted = SelectablePropertiesBuilderHelper.BackToParent(SelectablePropertiesBuilder<TestClass>
               .GetProperties()
               .SelectProperty(x => x.TestPropertyStr)
               .ThenSelectProperty(x => x.TestPropertyInt)
               .ThenSelectFromCollection(x => x.TestCollectionDates)
                    .ThenSelectProperty(x => x.Minute)
)
               .ThenSelectProperty(x => x.TestPropertyDouble)
               .FormatSelection();

            Assert.IsFalse(string.IsNullOrEmpty(propertiesFormatted));
            Assert.IsTrue(propertiesFormatted.LastIndexOf(',') != propertiesFormatted.Length);
            Assert.IsTrue(propertiesFormatted.Contains("."));
            Assert.IsTrue(propertiesFormatted.Contains(nameof(TestClass.TestPropertyDouble)));
        }

        [TestMethod]
        public void Given_ExternalType_WhenFormatSelectionInvokedUsingMultipleNestedCollectionAndComingBackToParentBuilder_ThenStringFormattedRetrived()
        {
            var propertiesFormatted = SelectablePropertiesBuilderHelper.BackToParent(SelectablePropertiesBuilder<TestClass>
               .GetProperties()
               .SelectProperty(x => x.TestPropertyStr)
               .ThenSelectProperty(x => x.TestPropertyInt)
               .ThenSelectFromCollection(x => x.TestCollectionDates)
                    .ThenSelectProperty(x => x.Minute)
                    .ThenSelectProperty(x => x.Month)
)
               .ThenSelectProperty(x => x.TestPropertyDouble)
               .FormatSelection();

            Assert.IsFalse(string.IsNullOrEmpty(propertiesFormatted));
            Assert.IsTrue(propertiesFormatted.LastIndexOf(',') != propertiesFormatted.Length);
            Assert.IsTrue(propertiesFormatted.Contains("."));
            Assert.IsTrue(propertiesFormatted.Contains(nameof(TestClass.TestPropertyDouble)));
        }

        [TestMethod]
        public void Given_ExternalType_WhenGetSelectionInvokedUsingMultipleNestedCollectionAndComplexObjectAndComingBackToParentBuilder_ThenPropertiesListRetrived()
        {
            var propertiesSelected = SelectablePropertiesBuilder<TestClass>
               .GetProperties()
               .SelectProperty(x => x.TestPropertyStr)
               .ThenSelectProperty(x => x.TestPropertyInt)
               .ThenSelectFromCollection(x => x.TestCollectionDates)
                    .ThenSelectProperty(x => x.Minute)
                    .ThenSelectProperty(x => x.Month)
                    .BackToParent()
               .ThenSelectProperty(x => x.TestPropertyDouble)
               .ThenSelectFromProperty(x => x.ComplexObj)
                    .ThenSelectProperty(x => x.Millisecond)
               .GetSelection();

            Assert.AreEqual(propertiesSelected.Count, 6);
        }

    }
}
