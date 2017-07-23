using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Subject
{

    static List<IObserver> observers = new List<IObserver>();
    static List<TransformObserver> transformObservers = new List<TransformObserver>();

    public static void Notify(string gameEvent)
    {
        for (int i = 0; i < observers.Count; i++)
        {
            observers[i].OnNotify(gameEvent);
        }
    }


    public static void Notify(string gameEvent, Vector3 position)
    {
        for (int i = 0; i < observers.Count; i++)
        {
            transformObservers[i].OnNotify(gameEvent, position);
        }
    }

    public static void AddObserver(IObserver observer)
    {
        observers.Add(observer);
    }

    public static void RemoveObserver(IObserver observer)
    {
        observers.Remove(observer);
    }

    public static void AddTransformObserver(TransformObserver observer)
    {
        transformObservers.Add(observer);
    }

    public static void RemoveTransformObserver(TransformObserver observer)
    {
        transformObservers.Remove(observer);
    }
}
