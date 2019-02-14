using System.Net;
using System.Net.Http;
using System.Web.Http;
using NUnit.Framework;

namespace TodoApp.Api.Tests.Wrappers
{
    internal static class AssertExtended
    {
        public static void IsBadResponseMessage(HttpResponseMessage message, params string[] modelStateKeys)
        {
            message.TryGetContentValue(out HttpError error);
            Assert.Multiple(() =>
            {
                Assert.That(message.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
                Assert.That(error.ModelState.Keys, Is.EquivalentTo(modelStateKeys));
            });
        }
    }
}
