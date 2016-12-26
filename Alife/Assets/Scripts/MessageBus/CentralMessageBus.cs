using UnityEngine;

namespace LD32
{
    public class CentralMessageBus:MonoBehaviour
    {
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


        TeamEvent changeTeam = new TeamEvent();

        public TeamEvent ChangeTeam
        {
            get { return changeTeam; }
        }

        IntEvent _damage = new IntEvent();

        public IntEvent Damage
        {
            get { return _damage; }
        }

        NoArgEvent _weaponFire = new NoArgEvent();

        public NoArgEvent WeaponFire
        {
            get { return _weaponFire; }
        }

        BehaviourEvent _destroyed = new BehaviourEvent();

        public BehaviourEvent OnDestroy
        {
            get { return _destroyed; }
        }

        VectorEvent _impulseRequested = new VectorEvent();

        public VectorEvent ImpulseRequested
        {
            get { return _impulseRequested; }
        }

        VectorEvent _motorMove = new VectorEvent();

        public VectorEvent MotorMove
        {
            get { return _motorMove; }
        }

        VectorEvent _motorRotate = new VectorEvent();

        public VectorEvent MotorRotate
        {
            get { return _motorRotate; }
        }

        LocationEvent _creatureInsight  = new LocationEvent();

        public LocationEvent CreatureInsight
        {
            get { return _creatureInsight; }
        }
    }
}