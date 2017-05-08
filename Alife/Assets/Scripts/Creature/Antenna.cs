using Common;
using UnityEngine;
using CymaticLabs.Unity3D.Amqp.SimpleJSON;

namespace CymaticLabs.Unity3D.Amqp
{
    /// <summary>
    /// An example script that shows how to control an object's
    /// position, rotation, and scale using AMQP messages.
    /// </summary>
    public class Antenna : BaseBehaviour
    {
        [Tooltip(
            "An optional ID filter that looks for an 'id' property in the received message. If the ID does not match this value, the message will be ignored."
        )] public string IdFilter;

        [Tooltip("The name of the exchange to subscribe to.")] public string ExchangeName = "NetBrain";

        [Tooltip(
            "The exchange type for the exchange being subscribed to. It is important to get this value correct as the RabbitMQ client will close a connection if you pass the wrong type for an already declared exchange."
        )]
        public AmqpExchangeTypes ExchangeType = AmqpExchangeTypes.Topic;

        public string InputRoutingKey = "NetBrain.Output.#";
        //public string OutputRoutingKey = "NetBrain.Output";


        [Tooltip("When enabled received messages will be logged to the debug console.")]
        public bool DebugLogMessages =true;

        // *Note*: Only interact with the AMQP library in Start(), not Awake() 
        // since the AmqpClient initializes itself in Awake() and won't be ready yet.
        public override void Start()
        {
            // Create a new exchange subscription using the inspector values
            var Input_subscription = new AmqpExchangeSubscription(ExchangeName, ExchangeType, InputRoutingKey,HandleInputMessageReceived);
            //var Output_subscription = new AmqpExchangeSubscription(ExchangeName, ExchangeType, OutputRoutingKey, HandleInputMessageReceived);
            /*
             * Add the subscription to the client. If you are using multiple AmqpClient instances then
             * using the static methods won't work. In that case add a inspector property of type 'AmqpClient'
             * and assigned a reference to the connection you want to work with and call the 'SubscribeToExchange()'
             * non-static method instead.
             */
            AmqpClient.Subscribe(Input_subscription);
            Debug.LogFormat("Anttena subscribed to {0}", Input_subscription);
        }

        //transfer messages heared to brain in update
        public override void  Update()
        {

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

            CentralBus.BrainRadio.Invoke(msg);
            


        }
    }
}


