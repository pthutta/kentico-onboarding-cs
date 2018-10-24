using System.Collections;
using NUnit.Framework;

namespace TodoApp.Api.Tests.TestData
{
    public class ItemsControllerTestData
    {
        public static IEnumerable IncorrectTextTestCases
        {
            get
            {
                yield return new TestCaseData(null);
                yield return new TestCaseData(string.Empty);
                yield return new TestCaseData("          ");
                yield return new TestCaseData("\t");
                yield return new TestCaseData("\n");
                yield return new TestCaseData("\r\n");
            }

        }
    }
}