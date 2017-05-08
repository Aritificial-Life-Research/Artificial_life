using System.Collections.Generic;
using UnityEngine;

public class Dispatcher
{
    Dispatcher _instance;
    Dispatcher instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new Dispatcher();
            }

            return _instance;
        }
    }

    public Dispatcher()
    {

    }

    public void Send(string eventName)
    {

    }
}