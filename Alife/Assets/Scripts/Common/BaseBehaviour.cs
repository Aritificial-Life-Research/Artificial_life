using MessageBus;
using UnityEngine;

namespace Common
{
    public class BaseBehaviour : MonoBehaviour
    {
        public BehaviourMessageBus InnerBus { get; private set; }

        public CentralMessageBus CentralBus { get; set; }

        public float LifeTime =36000;
        public bool DeathUponTime =false;

        private float SpawnTime;

        public virtual void Awake()
        {
            //resolve InnerBus before we do anything else:
            InnerBus = GetComponent<BehaviourMessageBus>();
            SpawnTime = Time.time + LifeTime;

            //get Central bus from root parent object;
            CentralBus = this.transform.root.GetComponent<CentralMessageBus>();

            if (InnerBus == null)
            {
                //we couldn't find a message bus -- we'll have to
                //create one
                InnerBus = gameObject.AddComponent<BehaviourMessageBus>();
            }
        }

        public virtual void Start()
        {

        }

        public virtual void Update()
        {
            if (SpawnTime <= Time.time && DeathUponTime)
            {
                Destroy(this.gameObject);
            }
        }


        void OnDestroy()
        {
            InnerBus.OnDestroy.Invoke(this);
        }

        protected void LookAt2D(Vector2 lookAt)
        {
            var lookDirection = lookAt - (Vector2)transform.position;
            float angle = Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg;
            //Debug.Log(angle);

            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
    }

}