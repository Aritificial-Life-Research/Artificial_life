using Common;
using UnityEngine;

namespace MessageBus
{
    public class CentralMessageBus:MonoBehaviour
    {
        private readonly JsonEvent _brainRadio = new JsonEvent();
        private readonly VoiceEvent _heardMessage = new VoiceEvent();
        private readonly VoiceEvent _talkMessage = new VoiceEvent();
        private readonly LocationEvent _sightChannel = new LocationEvent();
        private readonly VectorEvent _motorRotate = new VectorEvent();
        private readonly VectorEvent _motorMove = new VectorEvent();
        private readonly VectorEvent _impulseRequested = new VectorEvent();
        private readonly NoArgEvent _weaponFire = new NoArgEvent();

        public GlobalMessageBus Global
        {
            get { return GlobalMessageBus.Instance; }
        }

/*        static GlobalMessageBus _instance;
        public static GlobalMessageBus Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new GlobalMessageBus();
                }

                return _instance;
            }
        }*/


        //public TeamEvent ChangeTeam { get; } = new TeamEvent();

        //public IntEvent Damage { get; } = new IntEvent();

        public NoArgEvent WeaponFire
        {
            get { return _weaponFire; }
        }

        public VectorEvent ImpulseRequested
        {
            get { return _impulseRequested; }
        }

        public VectorEvent MotorMove
        {
            get { return _motorMove; }
        }

        public VectorEvent MotorRotate
        {
            get { return _motorRotate; }
        }

        //eyes to brain
        public LocationEvent SightChannel
        {
            get { return _sightChannel; }
        }

        //Brain to mouth
        public VoiceEvent TalkMessage
        {
            get { return _talkMessage; }
        }

        //Ear to brain
        public VoiceEvent HeardMessage
        {
            get { return _heardMessage; }
        }

        //anntenna to brain
        public JsonEvent BrainRadio
        {
            get { return _brainRadio; }
        }
    }
}