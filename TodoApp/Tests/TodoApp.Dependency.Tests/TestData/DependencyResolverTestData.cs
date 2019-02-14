using System.Collections;
using System.Collections.Generic;
using System.Net.Http.Formatting;
using System.Web.Http.Controllers;
using System.Web.Http.Dispatcher;
using System.Web.Http.ExceptionHandling;
using System.Web.Http.Hosting;
using System.Web.Http.Metadata;
using System.Web.Http.Tracing;
using System.Web.Http.Validation;
using Microsoft.Web.Http.Versioning;
using NUnit.Framework;

namespace TodoApp.Dependency.Tests.TestData
{
    public class DependencyResolverTestData : IEnumerable<TestCaseData>
    {
        public IEnumerator<TestCaseData> GetEnumerator()
        {
            yield return new TestCaseData(typeof(IHostBufferPolicySelector));
            yield return new TestCaseData(typeof(IHttpControllerSelector));
            yield return new TestCaseData(typeof(IHttpControllerActivator));
            yield return new TestCaseData(typeof(IHttpActionSelector));
            yield return new TestCaseData(typeof(IHttpActionInvoker));
            yield return new TestCaseData(typeof(IContentNegotiator));
            yield return new TestCaseData(typeof(IExceptionHandler));
            yield return new TestCaseData(typeof(ModelMetadataProvider));
            yield return new TestCaseData(typeof(IModelValidatorCache));
            yield return new TestCaseData(typeof(ITraceWriter));
            yield return new TestCaseData(typeof(IReportApiVersions));
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}