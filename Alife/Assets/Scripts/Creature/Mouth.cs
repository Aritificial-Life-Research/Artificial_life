using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Common;
using Common;
using NetMQ.zmq;
using UnityEngine;

namespace Creature
{
    public class Mouth: BaseBehaviour
    {

        public Vector2 voiceMessage;
        public float voiceRadius=20;
        private Dictionary<GameObject, Vector2> _creatureDict = new Dictionary<GameObject, Vector2>();
        float nextTime = 0;

        void Start()
        {
            base.Start();

            CentralBus.TalkMessage.AddListener(Talk);

        }

        // Update is called once per frame
        void Update()
        {

                base.Update();
                //VoiceReachedCreatures(voiceRadius);

        }

        public Dictionary<GameObject, Vector2> VoiceReachedCreatures(float range)
        {

            Vector2 position = transform.position;
            Collider2D[] hitColliders = Physics2D.OverlapCircleAll(position, range);

            foreach (Collider2D t in hitColliders)
            {
       
                GameObject hitObject = t.transform.gameObject;
                Debug.LogFormat("equals to myself {0}",hitObject != this.transform.parent.gameObject);

                if (hitObject!=this.transform.parent.gameObject && !_creatureDict.ContainsKey(hitObject))
                {
                    _creatureDict.Add(hitObject, position);
                }
            }
            return _creatureDict;
        }

        void Talk(VoiceStruct messageFromBrain)
        {

            InnerBus.Global.VoiceChannel.Invoke(messageFromBrain);
            Debug.LogFormat("sent threat voiceChannel {0}",messageFromBrain.ToString());
            
        }

        void OnDestroy()
        {
            InnerBus.Global.OnPlayerDestroyed.Invoke();
        }

    }

}