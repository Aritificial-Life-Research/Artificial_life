using System.Collections.Generic;
using System.Linq;
using Common;
using UnityEngine;

namespace Brain
{
    public  class MarkovBrain : BaseBehaviour
    {

        //number of input nodes
        public int nrOfInputs;

        //number of output nodes
        public int nrOfOutputs;

        //number of hidden nodes
        public int nrOfHidden;

        public int nrOfBrainNodes;

        // map input and output address to nodes list,
        List<int> inputNodes;  
        List<int> outputNodes;
		List<int> hiddenNodes;

        List<double> nodes;
        List<double> nextNodes;

        //Todo gate or brain as an independent game object
        //default constructor
        public MarkovBrain(int ins, int outs, int hidden)
        {
            nrOfInputs = ins;
            nrOfOutputs = outs;
            nrOfHidden = hidden;
            
			for (int i = 0; i < nrOfInputs; i++)
            {
                inputNodes.Add(i);
            }
            for (int i = nrOfInputs; i < nrOfInputs + nrOfOutputs; i++)
            {
                outputNodes.Add(i);
            }

			for (int i = nrOfInputs + nrOfOutputs; i < nrOfInputs + nrOfOutputs + nrOfHidden; i++)
			{
				hiddenNodes.Add(i);
			}


            nrOfBrainNodes = ins + outs + hidden;
            nodes.Capacity = nrOfBrainNodes;
            nodes.TrimExcess();
            nextNodes.Capacity = nrOfBrainNodes;

            nextNodes.TrimExcess();

        }






        
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


        //recieve information from eyes

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