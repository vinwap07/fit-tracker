using System.Linq.Expressions;
using Application.Abstractions;
using Hangfire;

namespace Infrastucture.Jobs.Hangfire;

public class HangfireJobService(IBackgroundJobClient backgroundJobClient): IJobService
{
    public void Enqueue<T>(Expression<Action<T>> methodCall)
    {
        backgroundJobClient.Enqueue<T>(methodCall);
    }
}