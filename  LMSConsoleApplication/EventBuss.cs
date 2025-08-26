namespace LMSConsoleApplication;

public class EventBuss:IEventBuss
{
    
    private readonly Dictionary<Type, List<Delegate>> _subscribers = new();

    public void Subscribe<T>(Action<T> handler) where T : class
    {
        var type = typeof(T);
        if (!_subscribers.TryGetValue(type,out var handlersList))
        {
            handlersList = new List<Delegate>();
            _subscribers[type]= handlersList;
        }
        _subscribers[type].Add(handler);
    }

    public void Publish<T>(T @event) where T : class
    {
        var type = typeof(T);
        if (_subscribers.TryGetValue(type, out var handlersList))
        {
            handlersList?.Cast<Action<T>>().ToList().ForEach(handler => handler(@event));
        }
    }
}