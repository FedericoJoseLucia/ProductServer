using System.Collections.Generic;
using System.Linq;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace ProductServer.IntegrationTests.SeedWork
{
    internal class TestNumericOrderer : ITestCaseOrderer
    {
        public const string TypeName = "ProductServer.IntegrationTests.SeedWork.TestNumericOrderer";
        public const string AssemblyName = "ProductServer.IntegrationTests";

        public IEnumerable<TTestCase> OrderTestCases<TTestCase>(IEnumerable<TTestCase> testCases) where TTestCase : ITestCase
        {
            var orderedTestCases = testCases.OrderBy(x => GetTestOrder(x));

            foreach (TTestCase testCase in orderedTestCases)
                yield return testCase;
        }

        private static int GetTestOrder<TTestCase>(TTestCase testCase) where TTestCase : ITestCase
        {
            var attr = testCase.TestMethod.Method.GetCustomAttributes(typeof(TestOrderAttribute).AssemblyQualifiedName).Single();

            return attr.GetNamedArgument<int>(nameof(TestOrderAttribute.Order));
        }
    }
}
