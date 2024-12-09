using UnityEngine;
using Steerings;

namespace FSM
{
    [RequireComponent(typeof(ANT_Blackboard))]
    [RequireComponent(typeof(WanderAround))]

    public class FSM_ANT_TwoPointsWander : FiniteStateMachine
	{
		public enum State {INITIAL, GOING_TO_A, GOING_TO_B};

		public State currentState = State.INITIAL;

		private WanderAround wander;
		private ANT_Blackboard blackboard;
		public float elapsedTime = 0f;


		void Start ()
		{
            wander = GetComponent<WanderAround>();
            blackboard = GetComponent<ANT_Blackboard>();
            wander.enabled = false;
		}

		public override void Exit () {
            wander.enabled = false;
            base.Exit();
		}

		public override void ReEnter() {
            currentState = State.INITIAL;
            base.ReEnter();
		}

		// Update is called once per frame
		void Update ()
		{

			elapsedTime += Time.deltaTime;

			switch (currentState)
            {
                case State.INITIAL:
                    ChangeState(State.GOING_TO_A);
                    break;
                
				case State.GOING_TO_A:
					if (SensingUtils.DistanceToTarget(gameObject, blackboard.targetA) < blackboard.locationDetectableRadius)
                    {
						if(elapsedTime >= blackboard.stepTime)	
						ChangeState(State.GOING_TO_B);
                    }
                    break;
                
				case State.GOING_TO_B:
                    if (SensingUtils.DistanceToTarget(gameObject, blackboard.targetB) < blackboard.locationDetectableRadius)
                    {
						if(elapsedTime >= blackboard.stepTime)
                        ChangeState(State.GOING_TO_A);
                    }
                    break;
            }
		}

		void ChangeState (State newState) {

			elapsedTime = 0f;

			// exit logic
            switch (currentState)
            {
                case State.GOING_TO_A:
					break;			
                case State.GOING_TO_B:
					break;
            }

			// enter logic
			currentState = newState;
			switch (newState) 
			{
				case State.GOING_TO_A:
					wander.attractor = blackboard.targetA;
					wander.enabled = true;
					break;
				case State.GOING_TO_B:
					wander.attractor = blackboard.targetB;
					wander.enabled = true;
					break;
			}
		}
	}
}
