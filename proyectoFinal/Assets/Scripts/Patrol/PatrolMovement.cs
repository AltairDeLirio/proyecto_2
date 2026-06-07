using UnityEngine;
using UnityEngine.SceneManagement;

public class PatrolMovement : MonoBehaviour
{
    [Header("Movimiento guardia")]
    [SerializeField] float velocity = 2f;
    [SerializeField] float velocidadCarrera = 5f;
    
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
    bool gameOverActive = false;
    
    private bool movimientoActivado = true; // ← NUEVO

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

        if (panelGameOver != null)
            panelGameOver.SetActive(false);
    }
    
    public void SetGuardMovement(bool canMove) //
    {
        movimientoActivado = canMove;
    }

    void Update()
    {
        if (!movimientoActivado) return; // 
        
        if (player == null || guardianEyes == null) return;
        if (gameOverActive) return;

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

        float velocidadActual = velocity;

        if (detected)
        {
            velocidadActual = velocidadCarrera;
            
            Vector3 direccionAlJugador = player.position - transform.position;
            direccionAlJugador.y = 0; 
            
            if (direccionAlJugador.magnitude > 0.1f)
            {
                direccionAlJugador.Normalize();
                transform.position += direccionAlJugador * velocidadActual * Time.deltaTime;
            }
        }
        else
        {
            if (objective != null)
            {
                Vector3 direccionWaypoint = objective.position - transform.position;
                direccionWaypoint.y = 0;
                
                if (direccionWaypoint.magnitude > 0.1f)
                {
                    direccionWaypoint.Normalize();
                    transform.position += direccionWaypoint * velocity * Time.deltaTime;
                }
            }
        }

        //verificar
        if (Time.time > tiempoInicio + 1f && Vector3.Distance(transform.position, player.position) <= distanciaAtrapar)
        {
            EjecutarGameOver();
        }
    }

    void EjecutarGameOver()
    {
        if (gameOverActive) return;
        gameOverActive = true;
        
        Debug.Log("el jugador ha sido atrapado");

        if (panelGameOver != null)
        {
            panelGameOver.SetActive(true); 
            //Time.timeScale = 0f;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            Debug.LogError("Panel Game Over no asignado en el inspector");
        }
    }

    public void ReiniciarJuego()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
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