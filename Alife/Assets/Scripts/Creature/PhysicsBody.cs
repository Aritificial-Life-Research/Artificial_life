using Common;
using UnityEngine;

namespace Creature
{

    //just a facade for Rigidbody2D
    [RequireComponent(typeof(Rigidbody2D))]
    public class PhysicsBody : BaseBehaviour
    {
        Rigidbody2D body;

        public override void Awake()
        {
            base.Awake();
            body = GetComponent<Rigidbody2D>();

            InnerBus.ImpulseRequested.AddListener(ApplyImpulse);
        }


        void ApplyImpulse(Vector2 impulse)
        {
            // Debug.Log("Impulse: " + impulse);
            body.AddForce(impulse);
        }
    }

}