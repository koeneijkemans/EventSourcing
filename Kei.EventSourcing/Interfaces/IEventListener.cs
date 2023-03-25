using System.Threading.Tasks;

namespace Kei.EventSourcing.Interfaces;

public interface IEventListener
{
    Task StartListening();
}
