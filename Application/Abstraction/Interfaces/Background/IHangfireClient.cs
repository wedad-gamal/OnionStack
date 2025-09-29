using System.Linq.Expressions;

namespace Abstraction.Abstraction.Interfaces.Background
{
    public interface IHangfireClient 
    {
        // Immediate Jobs
        string Enqueue<T>(Expression<Action<T>> methodCall);
        string Enqueue<T>(Expression<Func<T, Task>> methodCall);

        // Delayed Jobs
        string Schedule<T>(Expression<Action<T>> methodCall, TimeSpan delay);
        string Schedule<T>(Expression<Func<T, Task>> methodCall, TimeSpan delay);

        // Recurring Jobs
        void AddOrUpdateRecurring<T>(string jobId, Expression<Action<T>> methodCall, string cronExpression);
        void AddOrUpdateRecurring<T>(string jobId, Expression<Func<T, Task>> methodCall, string cronExpression);
    }
}
