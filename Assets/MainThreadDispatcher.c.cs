using System;
using System.Collections.Concurrent;
using UnityEngine;

public class MainThreadDispatcher : MonoBehaviour
{
    private static readonly ConcurrentQueue<Action> executionQueue = new ConcurrentQueue<Action>();

    public static void Enqueue(Action action)
    {
        executionQueue.Enqueue(action);
    }

    void Update()
    {
        while (executionQueue.TryDequeue(out var action))
        {
            action?.Invoke();
        }
    }
}
