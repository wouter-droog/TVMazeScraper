using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Polly;

namespace Rtl.TVMazeService.Functions.Middleware
{
    public class PollyPolicyDelegatingHandler : DelegatingHandler
    {
        private readonly IAsyncPolicy<HttpResponseMessage> policy;

        public PollyPolicyDelegatingHandler(IAsyncPolicy<HttpResponseMessage> policy) =>
            this.policy = policy ?? throw new ArgumentNullException(nameof(policy));

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));

            return await this.policy.ExecuteAsync(ct => base.SendAsync(request, ct), cancellationToken);
        }
    }
}
