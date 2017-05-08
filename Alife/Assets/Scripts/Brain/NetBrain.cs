using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using Assets.Scripts.Common;
using Common;
using RabbitMQ.Client.Framing.Impl;
using UnityEngine;



namespace CymaticLabs.Unity3D.Amqp
{
    /// <summary>
    /// An example script that shows how to control an object's
    /// position, rotation, and scale using AMQP messages.
    /// </summary>
    public class NetBrain : BaseBehaviour
    {
        [Tooltip(
            "An optional ID filter that looks for an 'id' property in the received message. If the ID does not match this value, the message will be ignored."
        )] public string IdFilter;

        public int MyId;

        [Tooltip("The name of the exchange to subscribe to.")] public string ExchangeName = "NetBrain";

        [Tooltip(
            "The exchange type for the exchange being subscribed to. It is important to get this value correct as the RabbitMQ client will close a connection if you pass the wrong type for an already declared exchange."
        )]
        public AmqpExchangeTypes ExchangeType = AmqpExchangeTypes.Topic;

        private  string InputRoutingKey;
        private string OutputRoutingKey = "NetBrain.Output.";
        private List<int> brainState;


        [Serializable]
        private struct InputInfo
        {
            public List<int> HeardMessage;
            public int CurrentHealth;
            public int bulletComing;
            public int ID;

            public InputInfo(List<int> msg, int hp=0,int bc=0, int id=0)
            {
                HeardMessage = msg;
                CurrentHealth = hp;
                bulletComing = bc;
                ID = id;
            }

        }

        InputInfo inputInfo = new InputInfo();

        public int interval = 1;
        float nextTime = 0;


        [Tooltip("When enabled received messages will be logged to the debug console.")]
        public bool DebugLogMessages =true;

        // *Note*: Only interact with the AMQP library in Start(), not Awake() 
        // since the AmqpClient initializes itself in Awake() and won't be ready yet.
        public override void Start()
        {

            InputRoutingKey = ExchangeName + ".Input."+ MyId;
            OutputRoutingKey = ExchangeName + ".Output." + MyId;
            inputInfo.ID = MyId;
            inputInfo.HeardMessage = new List<int>(new int[] { 0, 0 }); ;


            // Create a new exchange subscription using the inspector values
            //var Output_subscription = new AmqpExchangeSubscription(ExchangeName, ExchangeType, OutputRoutingKey, HandleInputMessageReceived);
            /*
             * Add the subscription to the client. If you are using multiple AmqpClient instances then
             * using the static methods won't work. In that case add a inspector property of type 'AmqpClient'
             * and assigned a reference to the connection you want to work with and call the 'SubscribeToExchange()'
             * non-static method instead.
             */
            AmqpClient.Subscribe(new AmqpExchangeSubscription(ExchangeName, ExchangeType, InputRoutingKey, HandleInputMessageReceived));
            //Debug.LogFormat("subscribed to {0}", Input_subscription);

            CentralBus.HeardMessage.AddListener(HeardMessage);
        }

        private void HeardMessage(VoiceStruct msg)
        {
            inputInfo.HeardMessage =msg.voiceMessage;
        }

    

        //transfer messages in update
        public override void Update()
        {
            inputInfo.CurrentHealth = transform.parent.GetComponent<Health>().health;
            // CentralBus.SightChannel.AddListener(updateCreatureInsight);
            //publish out creature info
            List<int> inputArray = WrapInput2Array(inputInfo);
            string json = JsonUtility.ToJson(inputInfo);
            AmqpClient.Publish(ExchangeName, OutputRoutingKey, json);
           

            if (Time.time >= nextTime)
            {

                //mock Recieved output
                MockProcess(inputInfo);
                nextTime += interval;

            }
        }

        private List<int> MockProcess(InputInfo inputInfo)
        {

            Debug.LogFormat("MessageHeard like a threat {0}", inputInfo.HeardMessage.SequenceEqual(new List<int> { 0, 1 })  || inputInfo.HeardMessage.SequenceEqual(new List<int> { 1, 1 }));
            if (inputInfo.CurrentHealth< transform.parent.GetComponent<Health>().maxHealth || inputInfo.HeardMessage.SequenceEqual(new List<int> { 0, 1 }) || inputInfo.HeardMessage.SequenceEqual(new List<int> { 1, 1 }))
            {
                List<int> outputArray= new List<int>(new int[] { 1, 0, 1 });
                CentralBus.WeaponFire.Invoke();

               //Debug.LogFormat("weapon fired at {0}",Time.time);

                //clear thread message
                inputInfo.HeardMessage = new List<int>(new int[] { 0, 0 }); ;

                List<int> talk_message = new List<int>(new int[] {0, 1 });
                //carry ID with it
                VoiceStruct talkStruct= new VoiceStruct(MyId,talk_message);

                CentralBus.TalkMessage.Invoke(talkStruct);

                Debug.LogFormat("talking threat {0}", talkStruct.ToString());
                return outputArray;

            }
            else
            {
                List<int> outputArray = new List<int>(new int[] { 0, 0, 0 });
                return outputArray;
            }

        }

        private List<int> WrapInput2Array(InputInfo inputInfo)
        {

            List<int> inputArray=new List<int>();
            foreach (var i in inputInfo.HeardMessage)
                inputArray.Add(i);
            inputArray.Add(inputInfo.CurrentHealth);
            inputArray.Add(inputInfo.bulletComing);
            return inputArray;
        }

  

        /**
         * Handles messages receieved from this object's subscription based on the exchange name,
         * exchange type, and routing key used. You could also write an anonymous delegate in line
         * when creating the subscription like: (received) => { Debug.Log(received.Message.Body.Length); }
         */

        void HandleInputMessageReceived(AmqpExchangeReceivedMessage received)
        {
            // First convert the message's body, which is a byte array, into a string for parsing the JSON
            var receivedJson = System.Text.Encoding.UTF8.GetString(received.Message.Body);

            // Log if enabled
            if (DebugLogMessages)
            {
                Debug.LogFormat("AMQP message received for {0}{1} => {2}", name,
                    !string.IsNullOrEmpty(IdFilter) ? " id:" + IdFilter : null, receivedJson);
            }

            /**
             *  Parse the JSON message
             *  This example uses the SimpleJSON parser which is included in the AMQP library.
             *  You can find out more about this parser here: http://wiki.unity3d.com/index.php/SimpleJSON
            */
            var msg = CymaticLabs.Unity3D.Amqp.SimpleJSON.JSON.Parse(receivedJson);

            // Get the message ID filter, if any
            var id = msg["id"] != null ? msg["id"].Value : null;

            // If an ID exists but it doesn't match the current ID filter then ignore this message
            if (!string.IsNullOrEmpty(IdFilter) && IdFilter != id)
            {
                if (DebugLogMessages)
                {
                    Debug.LogFormat("AMQP message ignored for {0} id:{1} != {2}", name, IdFilter, id);
                }

                return;
            }


            {
                // If the property exists use its value, otherwise just use the current value
                var objPos =(Vector2)transform.position;
                var posX = msg["posX"] != null ? msg["posX"].AsFloat : objPos.x;
                var posY = msg["posY"] != null ? msg["posY"].AsFloat : objPos.y;
                //var posZ = msg["posZ"] != null ? msg["posZ"].AsFloat : objPos.z;
                Vector2 destVector2=new Vector2(posX,posY);
                var movePulse = destVector2-objPos  ;
                // Update with new values
                CentralBus.MotorMove.Invoke(movePulse);

            
                //transform.position = new Vector3(posX, posY, posZ);
                

            }
        }
    }
}


