using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Subject
{

    static List<IObserver> observers = new List<IObserver>();

    public static void Notify(string gameEvent)
    {
        for (int i = 0; i < observers.Count; i++)
        {
            observers[i].OnNotify(gameEvent);
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

}
