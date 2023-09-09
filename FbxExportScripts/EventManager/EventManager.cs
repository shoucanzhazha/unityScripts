#define EVENTROUTER_THROWEXCEPTIONS 
#if EVENTROUTER_THROWEXCEPTIONS
#define EVENTROUTER_REQUIRELISTENER
#endif


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

//public struct GameEvent
//{
//    public string EventName;
//    static GameEvent e;
//    public static void Trigger(string newName)
//    {
//        e.EventName = newName;
//        EventManager.TriggerEvent(e);
//    }
//}

[ExecuteAlways]
public class EventManager
{
    private static Dictionary<Type, List<EventListenerBase>> _subscribersList;

    static EventManager()
    {
        _subscribersList = new Dictionary<Type, List<EventListenerBase>>();
    }

    public static void AddListener<Event>(EventListener<Event> listener) where Event : struct
    {
        Type eventType = typeof(Event);
        if (!_subscribersList.ContainsKey(eventType))
        {
            _subscribersList[eventType] = new List<EventListenerBase>();
        }
        if (!SubscriptionExists(eventType, listener))
        {
            _subscribersList[eventType].Add(listener);
        }
    }

	public static void RemoveListener<Event>(EventListener<Event> listener) where Event : struct
    {
        Type eventType = typeof(Event);
        if (!_subscribersList.ContainsKey(eventType))
        {
#if EVENTROUTER_THROWEXCEPTIONS
            throw new ArgumentException(string.Format("Removing listener \"{0}\", but the event type \"{1}\" isn't registered.", listener, eventType.ToString()));
#else
            return;
#endif

        }
        List<EventListenerBase> subscriberList = _subscribersList[eventType];
#if EVENTROUTER_THROWEXCEPTIONS
        bool listenerFound = false;
#endif
        for (int i = 0; i < subscriberList.Count; i++)
        {
            if (subscriberList[i] == listener)
            {
                subscriberList.Remove(subscriberList[i]);
#if EVENTROUTER_THROWEXCEPTIONS
                        listenerFound = true;
#endif
                if (subscriberList.Count == 0)
                {
                    _subscribersList.Remove(eventType);
                }
                return;
            }
        }
#if EVENTROUTER_THROWEXCEPTIONS
        if (!listenerFound)
        {
            throw new ArgumentException(string.Format("Removing listener, but the supplied receiver isn't subscribed to event type \"{0}\".", eventType.ToString()));
        }
#endif

    }

    public static void TriggerEvent<Event>(Event newEvent) where Event : struct
    {
        List<EventListenerBase> list;
        if (!_subscribersList.TryGetValue(typeof(Event), out list))
        {
#if EVENTROUTER_REQUIRELISTENER
            throw new ArgumentException(string.Format("Attempting to send event of type \"{0}\", but no listener for this type has been found. Make sure this.Subscribe<{0}>(EventRouter) has been called, or that all listeners to this event haven't been unsubscribed.", typeof(Event).ToString()));
#else
            return;
#endif
        }
        for (int i = 0; i < list.Count; i++)
        {
            (list[i] as EventListener<Event>).OnEvent(newEvent);
        }
    }

    private static bool SubscriptionExists(Type type, EventListenerBase receiver)
    {
        List<EventListenerBase> receivers;
        if (!_subscribersList.TryGetValue(type, out receivers))
        {
            return false;
        }
        bool exists = false;
        for (int i = 0; i < receivers.Count; i++)
        {
            if (receivers[i] == receiver)
            {
                exists = true;
                break;
            }
        }
        return exists;

    }
}

public static class EventRegister
{
    public static void EventStartListening<EventType>(this EventListener<EventType> caller) where EventType : struct
    {
        EventManager.AddListener<EventType>(caller);
    }

    public static void EventStopListening<EventType>(this EventListener<EventType> caller) where EventType : struct
    {
        EventManager.RemoveListener<EventType>(caller);
    }
}

public interface EventListenerBase { };


public interface EventListener<T> : EventListenerBase
{
    void OnEvent(T eventType);
}



