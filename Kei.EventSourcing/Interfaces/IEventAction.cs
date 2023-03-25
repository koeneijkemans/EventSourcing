namespace Kei.EventSourcing.Interfaces;

public interface IEventAction<T> where T : Event
{
    public void Action(T @event);
}
