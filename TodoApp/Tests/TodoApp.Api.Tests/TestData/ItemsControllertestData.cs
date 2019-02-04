using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;

namespace TodoApp.Api.Tests.TestData
{
    public class ItemsControllerTestData : IEnumerable<TestCaseData>
    {
        public IEnumerator<TestCaseData> GetEnumerator()
        {
            yield return new TestCaseData(null);
            yield return new TestCaseData(string.Empty);
            yield return new TestCaseData("          ");
            yield return new TestCaseData("\t");
            yield return new TestCaseData("\n");
            yield return new TestCaseData("\r\n");
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}