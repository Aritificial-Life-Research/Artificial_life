using System.Collections.Generic;
using Common;
using UnityEngine;

namespace Creature
{
    public class Eyes: BaseBehaviour
    {
        private Dictionary<GameObject, Vector2> _creatureDict = new Dictionary<GameObject, Vector2>();
        private Vector2 _selfPosition;
        public float EyeSight;

        void Start()
        {
            base.Start();
        }
       

        // Update is called once per frame
        void Update()
        {
            _selfPosition = transform.position;

            _creatureDict = FindNearbyCreatures(EyeSight);
            CentralBus.SightChannel.Invoke(_creatureDict);

        }


        public  Dictionary<GameObject,Vector2> FindNearbyCreatures(float range)
        {

            Vector2 position = transform.position;
            Collider[] hitColliders = Physics.OverlapSphere(position, range);

            for (int i = 0; i < hitColliders.Length; i++)
            {
                Transform parent = hitColliders[i].transform.parent;
                if (parent)
                {
                    GameObject parentObject = parent.gameObject;
                    if (parentObject && !_creatureDict.ContainsKey(parentObject))
                    {
                        _creatureDict.Add(parentObject ,position);
                    }
                }
            }
            return _creatureDict;
        }


        void OnDestroy()
        {
            InnerBus.Global.OnPlayerDestroyed.Invoke();
        }

    }

}