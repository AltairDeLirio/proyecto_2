using UnityEngine;

public class WayPointChhange : MonoBehaviour
{
    [SerializeField]
    Transform nextWaypoint;

    void OnTriggerEnter(Collider otro)
    {
        PatrolMovement patrol = otro.GetComponent<PatrolMovement>();

        if (patrol != null)
        {
            patrol.objective = nextWaypoint;
        }
    }
}
