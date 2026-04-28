using UnityEngine;

public class PatrolMovement : MonoBehaviour
{
    [SerializeField]
    float velocity;
    [SerializeField]
    float vel_rotation;
    
    public Transform objective;

    void Start()
    {
        objective = GameObject.Find("WayPoint0").transform;
    }

    void Update()
    {
        gameObject.transform.Translate (Vector3.forward * Time.deltaTime * velocity);

        Quaternion rotation = Quaternion.LookRotation(objective.transform.position - gameObject.transform.position);
        gameObject.transform.rotation = Quaternion.Slerp(gameObject.transform.rotation, rotation, Time.deltaTime * vel_rotation);
    }
}
