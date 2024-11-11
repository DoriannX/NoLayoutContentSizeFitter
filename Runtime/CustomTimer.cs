using System;
using UnityEngine;

public class CustomTimer
{
    private readonly float _interval;
    private float _timer;
    private readonly Action _action;
    private bool _started;

    public CustomTimer(float interval, Action action)
    {
        _interval = interval;
        _action = action;
    }

    public void Start()
    {
        _started = true;
    }

    public void Stop()
    {
        _started = false;
    }

    public void Update(float dt)
    {
        if (!_started)
        {
            return;
        }

        if (_timer >= _interval)
        {
            _timer = 0;
            _action();
        }
        else
        {
            _timer += dt;
        }
    }
}