using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Common;
using Common;
using MessageBus;
using UnityEngine;

namespace Creature
{
    public class Ear: BaseBehaviour
    {


        public override void Start()
        {
            base.Start();
            List<int> no_message = new List<int> { 0, 0 };
            VoiceStruct noStruct=new VoiceStruct(0,no_message);
            CentralBus.HeardMessage.Invoke(noStruct);
            InnerBus.Global.VoiceChannel.AddListener(MessageHeard);
            this.transform.parent.GetMessageBus().VoiceArrived.AddListener(MessageHeard);
        }



        //pass message to brain through central bus
        private void MessageHeard(VoiceStruct message)
        {
            
            Debug.LogFormat("message from ear {0}",message.ToString());
            CentralBus.HeardMessage.Invoke(message);
               
        }


        // Update is called once per frame
        public override void Update()
        {
            base.Update();

        }



        void OnDestroy()
        {
            InnerBus.Global.OnPlayerDestroyed.Invoke();
        }

    }

}