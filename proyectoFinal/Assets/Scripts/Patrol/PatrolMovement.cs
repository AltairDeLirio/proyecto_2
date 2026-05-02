using UnityEngine;

public class PatrolMovement : MonoBehaviour
{
    [Header ("Moimiento guardia")]

    [SerializeField]
    float velocity;
    [SerializeField]
    float vel_rotation;
    
    public Transform objective;

    [Header("Deteccion Protagonista")]

    [SerializeField]
    Transform player;
    [SerializeField]
    Transform guardianEyes;

    [SerializeField]
    float visionAngle;
    [SerializeField]
    float visionDistance;

    bool detected = false;

    void Start()
    {
        objective = GameObject.Find("WayPoint0").transform;
    }

    void Update()
    {
        //Movimiento guardia
        gameObject.transform.Translate (Vector3.forward * Time.deltaTime * velocity);

        Quaternion rotation = Quaternion.LookRotation(objective.transform.position - gameObject.transform.position);
        gameObject.transform.rotation = Quaternion.Slerp(gameObject.transform.rotation, rotation, Time.deltaTime * vel_rotation);

        //Deteccion prota
        detected = false;
        Vector2 playerVector = player.position - guardianEyes.position;

        if ((Vector3.Angle(playerVector.normalized, guardianEyes.right) < visionAngle * 0.5f) && (playerVector.magnitude < visionDistance)) 
        {
            detected = true;
        }
    }

    private void OnDrawGizmos()
    {
        if (visionAngle <= 0)
        {
            return;
        }

        float halfVisionAngle = visionAngle * 0.5f;

        Vector3 p1, p2;

        p1 = PointForAngle(halfVisionAngle, visionDistance);
        p2 = PointForAngle(-halfVisionAngle, visionDistance);

        if (!detected)
        {
            Gizmos.color = Color.red;
        }
        else
        {
            Gizmos.color = Color.green;
        }

        Gizmos.DrawLine(guardianEyes.position, guardianEyes.position + p1);
        Gizmos.DrawLine(guardianEyes.position, guardianEyes.position + p2);

        Gizmos.DrawRay(guardianEyes.position, guardianEyes.right * 4f);
    }

    Vector3 PointForAngle (float angle, float distance)
    {
        return guardianEyes.TransformDirection(new Vector3(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad)) * distance);
    }
}
