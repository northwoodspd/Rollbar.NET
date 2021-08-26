﻿#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

namespace UnitTest.Rollbar
{
    using global::Rollbar;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;

    [TestClass]
    [TestCategory(nameof(RollbarInfrastructureConfigFixture))]
    public class RollbarInfrastructureConfigFixture
    {
        [TestInitialize]
        public void SetupFixture()
        {
        }

        [TestCleanup]
        public void TearDownFixture()
        {
        }

        [TestMethod]
        public void TestBasics()
        {
            var config = new RollbarInfrastructureConfig();
            Console.WriteLine(config.TraceAsString());

            var results = config.Validate();
            Assert.AreEqual(1, results.Count, "One Validation Rule failed!");
            Console.WriteLine("Validation Results:");
            foreach(var result in results)
            {
                Console.WriteLine($"  {result}");
            }
            Console.WriteLine();
        }

    }

}
