using UnityEngine;

public class PatrolMovement : MonoBehaviour
{
    [Header("Movimiento guardia")]
    [SerializeField] float velocity = 2f;         // velocidad base
    [SerializeField] float velocidadCarrera = 5f;  //velocidad cuando persigue al personaje principal
    [SerializeField] float vel_rotation = 5f;
    
    public Transform objective;

    [Header("Deteccion Protagonista")]
    [SerializeField] Transform player;
    [SerializeField] Transform guardianEyes;
    [SerializeField] float visionAngle = 90f;
    [SerializeField] float visionDistance = 10f;

    [Header("Configuración de Ataque")]
    [SerializeField] float distanciaAtrapar = 1.5f; // distancia a la que atrapa al jugador

    bool detected = false;

    void Start()
    {
        // si no hay un objetivo asignado busca el waypoint0 por defecto
        if (objective == null)
        {
            GameObject waypointInicial = GameObject.Find("WayPoint0");
            if (waypointInicial != null) objective = waypointInicial.transform;
        }
    }

void Update()
    {
        if (player == null || guardianEyes == null) return;

        // deteccion
        Vector3 playerVector = player.position - guardianEyes.position;

        // Comprobamos si en ESTE preciso instante estás dentro del cono de visión
        if ((Vector3.Angle(playerVector.normalized, guardianEyes.right) < visionAngle * 0.5f) && (playerVector.magnitude < visionDistance)) 
        {
            detected = true; // Te está viendo: Activa el modo persecución
        }
        else
        {
            detected = false; // ¡Te ha perdido de vista! Vuelve a la normalidad
        }

        // patrullar o perseguir al personaje
        Transform objetivoActual = objective;
        float velocidadActual = velocity; // Por defecto, velocidad de patrulla lenta

        if (detected)
        {
            velocidadActual = velocidadCarrera; // cambia a velocidad rápida
            objetivoActual = player;           // persigue al jugador

            // comprobacion si ha llegado hasta el jugador 
            if (Vector3.Distance(transform.position, player.position) <= distanciaAtrapar)
            {
                EjecutarGameOver();
            }
        }

        // movimiento (hacia el waypoint o jugador)
        if (objetivoActual != null)
        {
            gameObject.transform.Translate(Vector3.forward * Time.deltaTime * velocidadActual);

            Vector3 direccionObjetivo = objetivoActual.position - gameObject.transform.position;
            if (direccionObjetivo != Vector3.zero)
            {
                Quaternion rotation = Quaternion.LookRotation(direccionObjetivo);
                gameObject.transform.rotation = Quaternion.Slerp(gameObject.transform.rotation, rotation, Time.deltaTime * vel_rotation);
            }
        }
    }


    void EjecutarGameOver()
    {
        // CODIGO GAME OVER
        Debug.Log("El guardia ha atrapado al personaje. Aquí se activa el Game Over.");
    }

    private void OnDrawGizmos()
    {
        if (guardianEyes == null || visionAngle <= 0) return;

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

    Vector3 PointForAngle(float angle, float distance)
    {
        return guardianEyes.TransformDirection(new Vector3(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad)) * distance);
    }
}