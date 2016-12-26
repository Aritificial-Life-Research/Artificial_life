using UnityEngine;
using System.Collections.Generic;
using UnityEditor;

namespace LD32
{
    public class Eyes: BaseBehaviour
    {
        private Dictionary<GameObject, Vector2> _creatureDict = new Dictionary<GameObject, Vector2>();
        private Vector2 _selfPosition;
        public float EyeSight;

        void satrt()
        {

        }
        void awake()
        {
        }

        // Update is called once per frame
        void Update()
        {
            _selfPosition = transform.position;

            _creatureDict = FindNearbyCreatures(EyeSight);
            CentralBus.CreatureInsight.Invoke(_creatureDict);

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