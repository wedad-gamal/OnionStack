using System.Linq.Expressions;

namespace Infrastructure.BackgroundJobs
{
    public class HangfireClient : IHangfireClient
    {
        // Immediate
        public string Enqueue<T>(Expression<Action<T>> methodCall)
            => BackgroundJob.Enqueue(methodCall);

        public string Enqueue<T>(Expression<Func<T, Task>> methodCall)
            => BackgroundJob.Enqueue(methodCall);

        // Delayed
        public string Schedule<T>(Expression<Action<T>> methodCall, TimeSpan delay)
            => BackgroundJob.Schedule(methodCall, delay);

        public string Schedule<T>(Expression<Func<T, Task>> methodCall, TimeSpan delay)
            => BackgroundJob.Schedule(methodCall, delay);

        // Recurring
        public void AddOrUpdateRecurring<T>(string jobId, Expression<Action<T>> methodCall, string cronExpression)
            => RecurringJob.AddOrUpdate(jobId, methodCall, cronExpression);

        public void AddOrUpdateRecurring<T>(string jobId, Expression<Func<T, Task>> methodCall, string cronExpression)
            => RecurringJob.AddOrUpdate(jobId, methodCall, cronExpression);

    }
}
