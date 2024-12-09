using UnityEngine;
using Steerings;

namespace FSM
{
    [RequireComponent(typeof(ANT_Blackboard))]
    [RequireComponent(typeof(Arrive))]
    [RequireComponent(typeof(FSM_ANT_TwoPointsWander))]

    public class FSM_ANT_Seed_Collecting : FiniteStateMachine
    {
        public enum State { INITIAL, WANDERING, REACHING, TRANSPORTING };
        // REACHING: reaching a seed
        // TRANSPORTING: transporting a seed to the nest

        public State currentState = State.INITIAL;

        private FSM_ANT_TwoPointsWander fsmWander;
        private Arrive arrive;
        private ANT_Blackboard blackboard;

        void Start()
        {
            fsmWander = GetComponent<FSM_ANT_TwoPointsWander>();
            arrive = GetComponent<Arrive>();
            blackboard = GetComponent<ANT_Blackboard>();
            fsmWander.enabled = false;
            arrive.enabled = false;
        }

        public override void Exit()
        {
            fsmWander.enabled = false;
            arrive.enabled = false;
            base.Exit();
        }

        public override void ReEnter()
        {
            currentState = State.INITIAL;
            base.ReEnter();
        }

        // Update is called once per frame
        void Update()
        {
            switch (currentState)
            {
                case State.INITIAL:
                    ChangeState(State.WANDERING);
                    break;

                case State.WANDERING:
                    fsmWander.enabled = true;
                    blackboard.seed = SensingUtils.FindInstanceWithinRadius(gameObject, "SEED", blackboard.seedDetectableRadius);

                    if (blackboard.seed != null) ChangeState(State.REACHING);
                    break;

                case State.REACHING:
                    arrive.enabled = true;
                    arrive.target = blackboard.seed;

                    if (SensingUtils.DistanceToTarget(gameObject, blackboard.seed) < blackboard.placeReachedRadius)
                    {
                        blackboard.seed.transform.parent = gameObject.transform;
                        blackboard.seed.tag = "BEING TRANSPORTED";
                        ChangeState(State.TRANSPORTING);
                    }
                    break;

                case State.TRANSPORTING:
                    arrive.target = blackboard.nest;
                    if (SensingUtils.DistanceToTarget(gameObject, blackboard.nest) < blackboard.placeNestRadius)
                    {
                        blackboard.seed.transform.parent = null;
                        blackboard.seed.tag = "COLLECTED";
                        ChangeState(State.WANDERING);
                    }
                    break;
            }
        }

        public void ChangeState(State newState)
        {
            // exit logic
            switch (currentState)
            {
                case State.WANDERING:
                    fsmWander.enabled = false;
                    break;
                case State.REACHING:
                    arrive.enabled = false;
                    break;
                case State.TRANSPORTING:
                    arrive.enabled = false;
                    break;
            }

            // enter logic
            switch (newState)
            {
                case State.WANDERING:
                    fsmWander.enabled = true;
                    break;
                case State.REACHING:
                    arrive.enabled = true;
                    break;
                case State.TRANSPORTING:
                    arrive.enabled = true;
                    break;
            }

            currentState = newState;
        }
    }
}