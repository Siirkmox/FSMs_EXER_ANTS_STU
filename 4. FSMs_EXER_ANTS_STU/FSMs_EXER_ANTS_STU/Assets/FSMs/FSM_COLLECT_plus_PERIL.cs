using UnityEngine;
using Steerings;

namespace FSM
{

    [RequireComponent(typeof(ANT_Blackboard))]
    [RequireComponent(typeof(FSM_ANT_Seed_Collecting))]
    [RequireComponent(typeof(Evade))]
    public class FSM_COLLECT_plus_PERIL : FiniteStateMachine
    {
        public enum State { INITIAL, COLLECTING, FLEEING};
        public State currentState = State.INITIAL;
        private ANT_Blackboard blackboard;
        private FSM_ANT_Seed_Collecting fsmSeedCollecting;
        private Evade evade;
        private GameObject predator;
        private GameObject seed;

        void Start()
        {
            blackboard = GetComponent<ANT_Blackboard>();
            fsmSeedCollecting = GetComponent<FSM_ANT_Seed_Collecting>();
            evade = GetComponent<Evade>();
            evade.enabled = false;
            fsmSeedCollecting.enabled = false;
        }
        
        public override void Exit()
        {
            evade.enabled = false;
            fsmSeedCollecting.enabled = false;
            base.Exit();
        }

        public override void ReEnter()
        {
            currentState = State.INITIAL;
            base.ReEnter();
        }

        void Update()
        {
            switch (currentState)
            {
                case State.INITIAL:
                    ChangeState(State.COLLECTING);
                    break;

                case State.COLLECTING:
                    fsmSeedCollecting.enabled = true;
                    predator = SensingUtils.FindInstanceWithinRadius (gameObject, "PREDATOR", blackboard.predatorDetectableRadius); 
                    
                    if (predator != null) ChangeState(State.FLEEING);
                    
                    break;

                case State.FLEEING:
                    evade.enabled = true;
                    evade.target = predator;
                    
                    // Suelta la semilla si la está transportando
                    if (fsmSeedCollecting.currentState == FSM_ANT_Seed_Collecting.State.TRANSPORTING)
                    {
                        if (blackboard.seed != null)
                        {
                            blackboard.seed.transform.parent = null;
                            blackboard.seed.tag = "SEED";
                            blackboard.seed = null;
                            fsmSeedCollecting.ChangeState(FSM_ANT_Seed_Collecting.State.WANDERING);
                        }
                    }

                    if (SensingUtils.DistanceToTarget(gameObject, predator) > blackboard.predatorDetectableRadius)
                        ChangeState(State.COLLECTING);

                    break;
            }
        }

        void ChangeState(State newState)
        {
            // exit logic
			switch (currentState) 
            {
                case State.COLLECTING:
                    fsmSeedCollecting.enabled = false;
                    break;

                case State.FLEEING:
                    evade.enabled = false;
                    break;
			}

			// enter logic
			switch (newState) 
            {
                case State.COLLECTING:
                    fsmSeedCollecting.enabled = true;
                    break;
                case State.FLEEING:
                    evade.enabled = true;
                    break;
			}

			currentState = newState;
        }
    }
}