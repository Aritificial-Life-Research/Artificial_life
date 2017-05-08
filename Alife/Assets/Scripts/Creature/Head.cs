using System.Collections.Generic;
using System.Linq;
using Common;
using UnityEngine;

namespace Creature
{
    public class Head : BaseBehaviour
    {


        Vector2 _currentMove;
        Vector2 _lookAt;
        Dictionary<GameObject, Vector2> _creatureInsightDict; 

        public override void Awake()
        {
            base.Awake();
            CentralBus.SightChannel.AddListener(updateCreatureInsight);

        }

        // Update is called once per frame
        public override void Update()
        {
            base.Update();
            AutoActions();
        }

        void updateCreatureInsight(Dictionary<GameObject,Vector2> newDict)
        {

            _creatureInsightDict = newDict;
        }

        void AutoActions()
        {
            
            Vector2 fisrtCreaturePosition= FindNearestObject(_creatureInsightDict);
            ChaseTarget(fisrtCreaturePosition);
        }


        Vector2 FindNearestObject(Dictionary<GameObject, Vector2> creatures)
        {
            GameObject FirstCreature = creatures.FirstOrDefault().Key;
            return FirstCreature.transform.position;
        }

        void ChaseTarget(Vector2 targetLoction)
        {
            var lookDirection = targetLoction - (Vector2)transform.position;
            float angle = Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg;
            //Debug.Log(angle);

            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            CentralBus.MotorMove.Invoke(lookDirection);
        }

        void OnDestroy()
        {
            InnerBus.Global.OnPlayerDestroyed.Invoke();
        }

    }

}