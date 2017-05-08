using Common;
using MyInput;
using UnityEngine;

namespace Creature
{
    public class Motor : BaseBehaviour

    {
        Rigidbody2D body;
        IInput input;

        public float acceleration;
        public float maxSpeed;



        // Use this for initialization
        public override void Start()
        {
            body = GetComponent<Rigidbody2D>();
        }



        public override void Awake()
        {
            base.Awake();
            CentralBus.MotorMove.AddListener(MotorMove);
            CentralBus.MotorRotate.AddListener(MotorRotate);
        }


        // Update is called once per frame
        public override void Update()
        {
        }

        void MotorMove(Vector2 impulse)
        {
            impulse = impulse.normalized * acceleration * Time.fixedDeltaTime;
            body.AddForce(impulse);
  

            if (body.velocity.magnitude > maxSpeed)
            {
                body.velocity = body.velocity.normalized * maxSpeed;
            }

        }

        void MotorRotate(Vector2 lookAt)
        {
            var lookDirection = lookAt - (Vector2)transform.position;
            float angle = Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg;
            //Debug.Log(angle);

            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        }

        void OnDestroy()
        {
            InnerBus.Global.OnPlayerDestroyed.Invoke();
        }

    }
}