using UnityEngine;


public class ANT_Blackboard : MonoBehaviour
{

    public GameObject targetA;
    public GameObject targetB;
    public GameObject nest;
    public GameObject seed;
    public float stepTime = 10f;
    public float locationDetectableRadius = 20f;
    public float seedDetectableRadius = 15;
    public float placeReachedRadius = 15f;
    public float placeNestRadius = 15f;
    public float predatorDetectableRadius = 30f;

	void Start () {

        if (targetA == null) {
			targetA = GameObject.Find ("LOCATION_A");
			if (targetA == null) {
				Debug.LogError ("no LOCATION_A object found in "+this);
			}
		}

        if (targetB == null) {
			targetB = GameObject.Find ("LOCATION_B");
			if (targetB == null) {
				Debug.LogError ("no LOCATION_B object found in "+this);
			}
		}

        if (nest == null) {
			nest = GameObject.Find ("NEST");
			if (nest == null) {
				Debug.LogError ("no NEST object found in "+this);
			}
		}
    }

}

