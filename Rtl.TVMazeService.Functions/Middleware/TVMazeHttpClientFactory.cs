using System;
using System.Net.Http;
using Flurl.Http.Configuration;
using Polly;

namespace Rtl.TVMazeService.Functions.Middleware
{
    public class TVMazeHttpClientFactory : DefaultHttpClientFactory
    {
        public override HttpMessageHandler CreateMessageHandler()
        {
            var policy = Policy
                .Handle<HttpRequestException>()
                .OrResult<HttpResponseMessage>(r => r.StatusCode == System.Net.HttpStatusCode.TooManyRequests)
                .WaitAndRetryAsync(5, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));

            var handler = new PollyPolicyDelegatingHandler(policy)
            {
                InnerHandler = base.CreateMessageHandler()
            };

            return handler;
        }
    }
}
