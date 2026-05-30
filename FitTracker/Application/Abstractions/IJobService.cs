using System.Linq.Expressions;

namespace Application.Abstractions;

public interface IJobService
{
    void Enqueue<T>(Expression<Action<T>> methodCall);
}