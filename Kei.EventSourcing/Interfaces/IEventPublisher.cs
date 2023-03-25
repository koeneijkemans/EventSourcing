using System.Threading.Tasks;

namespace Kei.EventSourcing.Interfaces;

public interface IEventPublisher
{
    Task PublishAsync(Event @event);
}
