using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Common;
using CymaticLabs.Unity3D.Amqp.SimpleJSON;
using UnityEngine;
using UnityEngine.Events;


namespace Common
{
    public class BehaviourEvent : UnityEvent<BaseBehaviour> { }
    public class FloatEvent : UnityEvent<float> { }
    public class IntEvent : UnityEvent<int> { }
    public class GameObjectEvent : UnityEvent<GameObject> { }
    public class NoArgEvent : UnityEvent { }
    public class TeamEvent : UnityEvent<Team> { }
    public class VectorEvent : UnityEvent<Vector2> { }
    public class ListEvent : UnityEvent<List<int>> { }
    public class LocationEvent : UnityEvent<Dictionary<GameObject, Vector2>> { }
    public class JsonEvent : UnityEvent<JSONNode> { }
    public class VoiceEvent : UnityEvent<VoiceStruct> { }




}
