namespace LMSConsoleApplication;

public interface IEventBuss
{
    public void Subscribe<T>(Action<T> handler) where T : class;

    public void Publish<T>(T @event) where T : class;
}