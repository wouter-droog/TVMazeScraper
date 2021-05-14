using System;
using System.Threading;
using System.Threading.Tasks;

namespace Rtl.TVMazeService.Domain.Utilities
{
    public class TaskLimiter
    {
        private readonly TimeSpan timespan;
        private readonly SemaphoreSlim semaphore;

        public TaskLimiter(int count, TimeSpan timespan)
        {
            this.semaphore = new SemaphoreSlim(count, count);
            this.timespan = timespan;
        }

        public async Task LimitAsync(Func<Task> taskFactory)
        {
            await this.semaphore.WaitAsync().ConfigureAwait(false);
            var task = taskFactory();
            _ = task.ContinueWith(async e =>
              {
                  await Task.Delay(this.timespan);
                  _ = this.semaphore.Release();
              });
            await task;
        }

        public async Task<T> LimitAsync<T>(Func<Task<T>> taskFactory)
        {
            await this.semaphore.WaitAsync().ConfigureAwait(false);
            var task = taskFactory();
            _ = task.ContinueWith(async e =>
              {
                  await Task.Delay(this.timespan);
                  _ = this.semaphore.Release();
              });
            return await task;
        }
    }
}
