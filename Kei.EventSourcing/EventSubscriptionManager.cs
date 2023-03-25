using System;
using System.Collections.Generic;
using Kei.EventSourcing.Interfaces;

namespace Kei.EventSourcing;

public sealed class EventSubscriptionManager
{
    public Dictionary<string, List<Action<Event>>> EventMap { get; private set; } = new();

    public void RegisterEvent<T>(Action<T> action)
        where T : Event
    {
        string eventName = action.GetType().GetGenericArguments()[0].Name;

        if (!EventMap.ContainsKey(eventName))
            EventMap.Add(eventName, new List<Action<Event>>());

        EventMap[eventName].Add(x => action((T)x));
    }

    public void RegisterEvent<T>(IEventAction<T> eventAction)
        where T : Event
    {
        var a = eventAction.Action;

        RegisterEvent(a);
    }
}
