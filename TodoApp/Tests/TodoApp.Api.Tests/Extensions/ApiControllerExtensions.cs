using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;

namespace TodoApp.Api.Tests.Extensions
{
    public static class ApiControllerExtensions
    {
        public static async Task<HttpResponseMessage> ExecuteAsyncAction<TController>(
            this TController controller,
            Func<TController, Task<IHttpActionResult>> httpAction
        )
            where TController : ApiController
        {
            IHttpActionResult response = await httpAction(controller);
            return await response.ExecuteAsync(CancellationToken.None);
        }
    }
}