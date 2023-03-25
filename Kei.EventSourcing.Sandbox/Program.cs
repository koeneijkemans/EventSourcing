using Kei.EventSourcing;
using Kei.EventSourcing.Interfaces;
using Kei.EventSourcing.Publisher.ServiceBus;
using Kei.EventSourcing.Sandbox.Domain;
using Kei.EventSourcing.Sandbox.EventActions;
using Kei.EventSourcing.Sandbox.Events;
using Kei.EventSourcing.Sandbox.Read;
using Kei.EventSourcing.Store.AzureTable;

Console.WriteLine("Hello, World!");

List<SuperSpecialPersonModel> superSpecialPersons = new();

IEventPublisher serviceBusEventPublisher = new ServiceBusPublisher(new ServiceBusOptions { ConnectionString = "", QueueName = "publishedevents" });
IEventStore eventStore = new AzureTableEventStore(serviceBusEventPublisher, new AzureTableEventStoreOptions { ConnectionString = "", TableName = "events"});

EventSubscriptionManager eventSubscriptionManager = new();
eventSubscriptionManager.RegisterEvent((PersonCreatedEvent @event) => superSpecialPersons.Add(new SuperSpecialPersonModel { Name = @event.Name, Age = @event.Age }));

eventSubscriptionManager.RegisterEvent(new NewSpecialPersonEventAction(superSpecialPersons));
eventSubscriptionManager.RegisterEvent(new SuperSpecialPersonAgeChanged(superSpecialPersons));

StateConnector stateConnector = new(eventStore);

ServiceBusListener serviceBusListener = new(eventSubscriptionManager, new ServiceBusOptions { ConnectionString = "", QueueName = "publishedevents" });
await serviceBusListener.StartListening();

Guid rootId = Guid.NewGuid();

PersonCreatedEvent koenCreated = new () { AggregateRootId = rootId, Name = "Koen", Age = 32 };
AgeChangedEvent koenTo33 = new() { AggregateRootId = rootId, Age = 33 };
NameChangedEvent koenToPiet = new() { AggregateRootId = rootId, Name = "Piet" };
NameChangedEvent pietToKoen = new() { AggregateRootId = rootId, Name = "Koen" };

stateConnector.Save(koenCreated);
stateConnector.Save(koenTo33);
stateConnector.Save(koenToPiet);
stateConnector.Save(pietToKoen);

var person = stateConnector.Get<Person>(rootId);
Console.WriteLine(person.ToString());

Console.WriteLine("---- Super special persons:");

foreach (var p in superSpecialPersons) Console.WriteLine(p.ToString());

Console.ReadLine();
