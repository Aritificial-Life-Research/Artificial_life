using Common;
using UnityEngine;

namespace MessageBus
{

    /// <summary>
    /// BaseBehaviour uses this for communication internal to the
    /// GameObject. It's created implicitly with the first BaseBehaviour
    /// on a given object (see BaseBehaviour).
    /// </summary>
    public class BehaviourMessageBus : MonoBehaviour
    {
        private readonly VoiceEvent _voiceArrived = new VoiceEvent();
        private readonly VectorEvent _impulseRequested = new VectorEvent();
        private readonly BehaviourEvent _onDestroy = new BehaviourEvent();
        private readonly NoArgEvent _fireBullet = new NoArgEvent();
        private readonly IntEvent _damage = new IntEvent();
        private readonly TeamEvent _changeTeam = new TeamEvent();




        public GlobalMessageBus Global
        {
            get
            {
                return GlobalMessageBus.Instance;
            }
        }

        public TeamEvent ChangeTeam
        {
            get { return _changeTeam; }
        }

        public IntEvent Damage
        {
            get { return _damage; }
        }

        public NoArgEvent FireBullet
        {
            get { return _fireBullet; }
        }

        public BehaviourEvent OnDestroy
        {
            get { return _onDestroy; }
        }

        public VectorEvent ImpulseRequested
        {
            get { return _impulseRequested; }
        }

        public VoiceEvent VoiceArrived
        {
            get { return _voiceArrived; }
        }



    }

}
