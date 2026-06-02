using UnityEngine;

public class PatrolMovement : MonoBehaviour
{
    [Header("Movimiento guardia")]
    [SerializeField] float velocity = 2f;
    [SerializeField] float velocidadCarrera = 5f;
    [SerializeField] float vel_rotation = 3f; // Reducido para rotación más suave
    
    public Transform objective;

    [Header("Deteccion Protagonista")]
    [SerializeField] Transform player;
    [SerializeField] Transform guardianEyes;
    [SerializeField] float visionAngle = 90f;
    [SerializeField] float visionDistance = 10f;

    [Header("Configuración de Ataque")]
    [SerializeField] float distanciaAtrapar = 1.5f;

    [Header("Interfaz de Game Over")]
    [SerializeField] GameObject panelGameOver; 

    float tiempoInicio = 0f;
    bool detected = false;
    Vector3 direccionMovimiento;

    void Start()
    {
        tiempoInicio = Time.time;

        if (objective == null)
        {
            GameObject waypointInicial = GameObject.Find("WayPoint0");
            if (waypointInicial != null) objective = waypointInicial.transform;
        }

        if (player == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null) player = playerObj.transform;
        }

        if (guardianEyes == null)
        {
            guardianEyes = this.transform;
        }

        direccionMovimiento = transform.forward;
    }

    void Update()
    {
        if (player == null || guardianEyes == null) return;

        // DETECCIÓN
        Vector3 playerVector = player.position - guardianEyes.position;
        float distanciaAlJugador = playerVector.magnitude;
        float angulo = Vector3.Angle(playerVector.normalized, guardianEyes.forward);

        if (angulo < visionAngle * 0.5f && distanciaAlJugador < visionDistance)
        {
            detected = true;
        }
        else
        {
            detected = false;
        }

        // DECIDIR OBJETIVO Y VELOCIDAD
        Transform objetivoActual = objective;
        float velocidadActual = velocity;

        if (detected)
        {
            velocidadActual = velocidadCarrera;
            objetivoActual = player;

            // VERIFICAR ATRAPADO
            if (Time.time > tiempoInicio + 1f && distanciaAlJugador <= distanciaAtrapar)
            {
                EjecutarGameOver();
                return;
            }
        }

        // MOVIMIENTO Y ROTACIÓN SUAVE
        if (objetivoActual != null)
        {
            // Calcular dirección hacia el objetivo
            Vector3 direccionObjetivo = objetivoActual.position - transform.position;
            direccionObjetivo.y = 0; // Mantener horizontal
            
            if (direccionObjetivo.magnitude > 0.1f)
            {
                direccionMovimiento = direccionObjetivo.normalized;
            }
            
            // Rotación SUAVE hacia la dirección de movimiento (no instantánea)
            if (direccionMovimiento != Vector3.zero)
            {
                Quaternion rotacionObjetivo = Quaternion.LookRotation(direccionMovimiento);
                transform.rotation = Quaternion.Slerp(transform.rotation, rotacionObjetivo, Time.deltaTime * vel_rotation);
            }
            
            // Movimiento hacia adelante
            transform.Translate(Vector3.forward * Time.deltaTime * velocidadActual);
        }
    }

    void EjecutarGameOver()
    {
        Debug.Log("El guardia ha atrapado al personaje.");

        if (panelGameOver != null)
        {
            panelGameOver.SetActive(true); 
            Time.timeScale = 0f;          
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    void OnDrawGizmos()
    {
        if (guardianEyes == null || visionAngle <= 0) return;
        
        float halfVisionAngle = visionAngle * 0.5f;
        Vector3 leftBoundary = Quaternion.Euler(0, -halfVisionAngle, 0) * guardianEyes.forward * visionDistance;
        Vector3 rightBoundary = Quaternion.Euler(0, halfVisionAngle, 0) * guardianEyes.forward * visionDistance;
        
        Gizmos.color = detected ? Color.green : Color.red;
        Gizmos.DrawLine(guardianEyes.position, guardianEyes.position + leftBoundary);
        Gizmos.DrawLine(guardianEyes.position, guardianEyes.position + rightBoundary);
        Gizmos.DrawLine(guardianEyes.position, guardianEyes.position + guardianEyes.forward * visionDistance);
        
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, distanciaAtrapar);
    }
}